
provider "azurerm" {
  subscription_id            = try(local.azure_credentials.subscriptionId, null)
  client_id                  = try(local.azure_credentials.clientId, null)
  client_secret              = try(local.azure_credentials.clientSecret, null)
  tenant_id                  = try(local.azure_credentials.tenantId, null)
  skip_provider_registration = true

  features {}
}

#provider "statuscake" {
 # api_token = local.monitoring_secrets.STATUSCAKE_PASSWORD
#}