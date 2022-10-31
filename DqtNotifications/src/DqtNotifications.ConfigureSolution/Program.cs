using System.CommandLine;
using System.Text.Json.Nodes;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

const string solutionName = "DqtNotifications";
const string serviceEndpointName = "DqtNotifications Service Bus Topic";

var crmUrlOption = new Option<string>("--crm-url") { IsRequired = true };
var crmClientIdOption = new Option<string>("--client-id") { IsRequired = true };
var crmClientSecretOption = new Option<string>("--client-secret") { IsRequired = true };
var skipVersionIncrementOption = new Option<bool>("--skip-version-increment");

var rootCommand = new RootCommand(description: $"Configures the {solutionName} solution in CRM.");
rootCommand.AddOption(crmUrlOption);
rootCommand.AddOption(crmClientIdOption);
rootCommand.AddOption(crmClientSecretOption);
rootCommand.AddOption(skipVersionIncrementOption);

rootCommand.SetHandler(
    async (string crmUrl, string clientId, string clientSecret, bool skipVersionIncrement) =>
    {
        var serviceClient = new ServiceClient(new Uri(crmUrl), clientId, clientSecret, useUniqueInstance: false);

        var solution = await FindEntity("solution", D("uniquename", solutionName));

        var serviceEndpoint = await FindEntity("serviceendpoint", D("name", serviceEndpointName));

        Task<Entity> GetMessage(string messageName) => FindEntity("sdkmessage", D("name", messageName));
        var messages = new[] { await GetMessage("create"), await GetMessage("update"), await GetMessage("delete") };

        var reportingConfig = JsonNode.Parse(File.ReadAllText(Path.Combine(System.AppContext.BaseDirectory, "config.json")))!;

        foreach (var (entity, entityConfig) in reportingConfig["entities"]!.AsObject())
        {
            Console.WriteLine($"Configuring {entity} entity...");

            var entityMetadata = serviceClient.GetEntityMetadata(entity);
            var entityDisplayName = entityMetadata.DisplayName.LocalizedLabels.First().Label;

            foreach (var message in messages)
            {
                var messageFilter = await FindEntity(
                    "sdkmessagefilter",
                    D("sdkmessageid", message.Id, "primaryobjecttypecode", entity, "secondaryobjecttypecode", "none"));

                async Task<Guid?> FindSdkMessageProcessingStep()
                {
                    // Can't use FindEntity here since I can't figure out how to add a 'eventhandler' predicate to the query :-/

                    var query = new QueryExpression("sdkmessageprocessingstep")
                    {
                        Criteria = new FilterExpression(LogicalOperator.And),
                        ColumnSet = new(allColumns: true)
                    };

                    query.Criteria.AddCondition("sdkmessagefilterid", ConditionOperator.Equal, messageFilter.Id);
                    query.Criteria.AddCondition("sdkmessageid", ConditionOperator.Equal, message.Id);
                    query.Criteria.AddCondition("solutionid", ConditionOperator.Equal, serviceEndpoint.GetAttributeValue<Guid>("solutionid"));

                    var result = await serviceClient.RetrieveMultipleAsync(query);

                    return result.Entities.SingleOrDefault(e => e.GetAttributeValue<EntityReference>("eventhandler")?.Id == serviceEndpoint.Id)?.Id;
                }

                var stepId = await FindSdkMessageProcessingStep();

                var messageName = message.GetAttributeValue<string>("name");
                var stepName = $"{serviceEndpoint.GetAttributeValue<string>("name")}: {messageName} of {entityDisplayName}";
                var stepDescription = $"{messageName} of {entityDisplayName}";

                var stepEntity = new Entity()
                {
                    LogicalName = "sdkmessageprocessingstep",
                    Attributes = new()
                    {
                        { "sdkmessageid", new EntityReference("sdkmessage", message.Id) },
                        { "sdkmessagefilterid", new EntityReference("sdkmessagefilter", messageFilter.Id) },
                        { "eventhandler", new EntityReference("serviceendpoint", serviceEndpoint.Id) },
                        { "name", stepName },
                        { "description", stepDescription },
                        { "mode", new OptionSetValue(1) },  // Asynchronous
                        { "rank", 1 },
                        { "stage", new OptionSetValue(40) },  // PostOperation
                        { "supporteddeployment", new OptionSetValue(0) },  // ServerOnly
                        { "asyncautodelete", true }
                    }
                };

                if (stepId is null)
                {
                    stepId = await serviceClient.CreateAsync(stepEntity);
                    Console.WriteLine($"  Created step for {messageName}");
                }
                else
                {
                    stepEntity.Id = stepId.Value;
                    await serviceClient.UpdateAsync(stepEntity);
                    Console.WriteLine($"  Updated step for {messageName}");
                }

                await serviceClient.ExecuteAsync(new AddSolutionComponentRequest()
                {
                    ComponentType = 92,  // SDKMessageProcessingStep - reference https://github.com/microsoft/PowerApps-Samples/blob/1adb4891a312555a2c36cfe7b99c0a225a934a0d/cds/webapi/C%23/MetadataOperations/MetadataTypes/SolutionComponentType.cs
                    ComponentId = stepId.Value,
                    SolutionUniqueName = solutionName
                });
            }

            Console.WriteLine();
        }

        // TODO Tear down any entities that have been removed from config

        // Bump solution version
        if (!skipVersionIncrement)
        {
            var currentVersion = new Version(solution.GetAttributeValue<string>("version"));
            var newVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build, currentVersion.Revision + 1);

            var updatedSolution = new Entity()
            {
                LogicalName = "solution",
                Id = solution.Id,
            };
            updatedSolution.Attributes["version"] = newVersion.ToString();

            await serviceClient.UpdateAsync(updatedSolution);
        }

        Console.WriteLine("Done");

        async Task<Entity> FindEntity(string entityName, IDictionary<string, object> matchAttributes)
        {
            var query = new QueryExpression(entityName)
            {
                Criteria = new FilterExpression(LogicalOperator.And),
                ColumnSet = new(allColumns: true)
            };

            foreach (var attr in matchAttributes)
            {
                query.Criteria.AddCondition(attr.Key, ConditionOperator.Equal, attr.Value);
            }

            var result = await serviceClient.RetrieveMultipleAsync(query);

            if (result.Entities.Count > 1)
            {
                throw new Exception($"Multiple {entityName} entities found matching criteria.");
            }

            if (result.Entities.Count == 0)
            {
                throw new Exception($"Could not find matching {entityName} entity.");
            }

            return result.Entities.Single();
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{string, object}"/> given pairs of keys and values.
        /// </summary>
        static Dictionary<string, object> D(params object[] keysWithValues)
        {
            if (keysWithValues.Length % 2 != 0)
            {
                throw new ArgumentException("Mis-matched number of keys and values.", nameof(keysWithValues));
            }

            var result = new Dictionary<string, object>();

            for (var i = 0; i < keysWithValues.Length; i += 2)
            {
                result.Add((string)keysWithValues[i], keysWithValues[i + 1]);
            }

            return result;
        }
    },
    crmUrlOption,
    crmClientIdOption,
    crmClientSecretOption,
    skipVersionIncrementOption);

return await rootCommand.InvokeAsync(args);
