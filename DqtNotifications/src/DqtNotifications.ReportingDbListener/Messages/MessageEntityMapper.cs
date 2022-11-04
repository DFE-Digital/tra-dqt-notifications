using System.Globalization;
using System.Text.RegularExpressions;

namespace DqtNotifications.ReportingDbListener.Messages;

public class MessageEntityMapper
{
    private static readonly Regex _datePattern = new(@"^\/Date\((\d+)(\+(\d{4}))?\)\/$", RegexOptions.Compiled);

    public EntityDelta MapMessage(IMessage message) => message switch
    {
        CreateMessage createMessage => EntityDelta.Create(
            createMessage.Target.LogicalName,
            createMessage.Target.Id,
            TranslateAttributes(createMessage.Target)),
        UpdateMessage updateMessage => EntityDelta.Update(
            updateMessage.Target.LogicalName,
            updateMessage.Target.Id,
            TranslateAttributes(updateMessage.Target)),
        DeleteMessage deleteMessage => EntityDelta.Delete(
            deleteMessage.Target.LogicalName,
            deleteMessage.Target.Id),
        _ => throw new NotSupportedException($"Unknown {nameof(message)} type: '{message.GetType().FullName}.")
    };

    private IReadOnlyDictionary<string, object?> TranslateAttributes(Entity entity)
    {
        return entity.Attributes.ToDictionary(
            kvp => kvp.Key,
            kvp =>
            {
                var attributeName = kvp.Key;

                var value = kvp.Value switch
                {
                    string valueString when _datePattern.IsMatch(valueString) => ConvertDateTime(entity, attributeName),
                    var v when v is string || v is int || v is bool || v is null => v,
                    OptionSetValue optionSetValue => optionSetValue.Value,
                    EntityReference entityReference => new ReportingDbListener.EntityReference(entityReference.LogicalName, entityReference.Id),
                    _ => throw new NotSupportedException($"Unsupported type: '{kvp.Value.GetType().FullName}' for attribute '{attributeName}'.")
                };

                return value;
            });
    }

    private DateTime ConvertDateTime(Entity entity, string attributeName)
    {
        var formattedValue = entity.FormattedValues[attributeName];
        return DateTime.Parse(formattedValue, provider: null, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
    }
}
