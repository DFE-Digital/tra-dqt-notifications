
# App Service Plan
resource "azurerm_service_plan" "app_service_plan" {
  name                = local.app_service_plan_name
  location            = data.azurerm_resource_group.resource_group.location
  resource_group_name = data.azurerm_resource_group.resource_group.name
  os_type             = "Linux"
  sku_name            = "B1"
  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

# Function App
resource "azurerm_linux_function_app" "function_app" {
  name                       = local.linux_function_app_name
  location                   = data.azurerm_resource_group.resource_group.location
  resource_group_name        = data.azurerm_resource_group.resource_group.name
  service_plan_id            = azurerm_service_plan.app_service_plan.id
  storage_account_name       = azurerm_storage_account.storage_account.name
  storage_account_access_key = azurerm_storage_account.storage_account.primary_access_key

  connection_string {
    name  = "ReportingDbListener"
    type  = "ServiceBus"
    value = replace(azurerm_servicebus_topic_authorization_rule.reporting_db_listener.primary_connection_string, ";EntityPath=${azurerm_servicebus_topic.servicebus_topic.name}", "")
  }

  site_config {}

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

# Servicebus Namespace
resource "azurerm_servicebus_namespace" "servicebus_namespace" {
  name                = local.servicebus_namespace_name
  location            = data.azurerm_resource_group.resource_group.location
  resource_group_name = data.azurerm_resource_group.resource_group.name
  sku                 = "Standard"
  zone_redundant      = var.service_bus_zone_redundant
  capacity            = 0

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

# Servicebus Topic
resource "azurerm_servicebus_topic" "servicebus_topic" {
  name                = "crm-messages"
  namespace_id        = azurerm_servicebus_namespace.servicebus_namespace.id
  enable_partitioning = true
}

resource "azurerm_servicebus_topic_authorization_rule" "dqt_sender" {
  name     = "DqtSender"
  topic_id = azurerm_servicebus_topic.servicebus_topic.id
  listen   = false
  send     = true
  manage   = false
}

resource "azurerm_servicebus_topic_authorization_rule" "reporting_db_listener" {
  name     = "ReportingDbListener"
  topic_id = azurerm_servicebus_topic.servicebus_topic.id
  listen   = true
  send     = false
  manage   = false
}

resource "azurerm_servicebus_subscription" "reporting_db_listener" {
  name                                 = "ReportingDbListener"
  topic_id                             = azurerm_servicebus_topic.servicebus_topic.id
  max_delivery_count                   = 20
  lock_duration                        = "PT2M" # 2 minutes
  dead_lettering_on_message_expiration = true
  enable_batched_operations            = true
  requires_session                     = true
}

#Storage account
resource "azurerm_storage_account" "storage_account" {
  name                              = local.storage_account
  resource_group_name               = data.azurerm_resource_group.resource_group.name
  location                          = data.azurerm_resource_group.resource_group.location
  account_tier                      = "Standard"
  account_replication_type          = var.environment_name != "dev" ? "LRS" : "GRS"
  account_kind                      = "StorageV2"
  min_tls_version                   = "TLS1_2"
  infrastructure_encryption_enabled = true

  blob_properties {
    last_access_time_enabled = true

    container_delete_retention_policy {
      days = var.data_protection_container_delete_retention_days
    }
  }

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

#MSSQL Server
resource "azurerm_mssql_server" "mssql_server" {
  name                         = local.mssql_server
  resource_group_name          = data.azurerm_resource_group.resource_group.name
  location                     = data.azurerm_resource_group.resource_group.location
  version                      = "12.0"
  administrator_login          = local.infrastructure_secrets.SQL_ADMIN_USERNAME
  administrator_login_password = local.infrastructure_secrets.SQL_ADMIN_PASSWORD

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

#MSSQL Database
resource "azurerm_mssql_database" "mssql_database" {
  name        = local.mssql_database
  server_id   = azurerm_mssql_server.mssql_server.id
  collation   = "SQL_Latin1_General_CP1_CI_AS"
  sku_name    = local.mssql_sku_name
  max_size_gb = local.mssql_max_size_gb

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}
