locals {
  dqtnoti_env_vars = merge(local.infrastructure_secrets,
    {
      APPLICATIONINSIGHTS_CONNECTION_STRING = azurerm_application_insights.function_app_insights.connection_string
    }
  )
}

resource "azurerm_service_plan" "app_service_plan" {
  name                   = local.app_service_plan_name
  location               = data.azurerm_resource_group.resource_group.location
  resource_group_name    = data.azurerm_resource_group.resource_group.name
  os_type                = "Linux"
  sku_name               = var.app_service_plan_sku_size
  zone_balancing_enabled = var.worker_count != null ? true : false
  worker_count           = var.worker_count
  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_linux_function_app" "function_app" {
  name                       = local.linux_function_app_name
  location                   = data.azurerm_resource_group.resource_group.location
  resource_group_name        = data.azurerm_resource_group.resource_group.name
  service_plan_id            = azurerm_service_plan.app_service_plan.id
  storage_account_name       = azurerm_storage_account.notify.name
  storage_account_access_key = azurerm_storage_account.notify.primary_access_key

  app_settings = local.dqtnoti_env_vars

  connection_string {
    name  = "ReportingDbListener"
    type  = "ServiceBus"
    value = replace(azurerm_servicebus_topic_authorization_rule.reporting_db_listener.primary_connection_string, ";EntityPath=${azurerm_servicebus_topic.servicebus_topic.name}", "")
  }

  site_config {
    application_stack {
      dotnet_version = "7.0"
    }
  }

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

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

resource "azurerm_servicebus_namespace_authorization_rule" "send_listen_auth_rule" {
  name         = "SendAndListenSharedAccessKey"
  namespace_id = azurerm_servicebus_namespace.servicebus_namespace.id
  listen       = true
  send         = true
}

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

resource "azurerm_application_insights" "function_app_insights" {
  name                 = local.app_insights_name
  resource_group_name  = data.azurerm_resource_group.resource_group.name
  location             = data.azurerm_resource_group.resource_group.location
  application_type     = "web"
  daily_data_cap_in_gb = var.application_insights_daily_data_cap_gb
  retention_in_days    = var.application_insights_retention_days

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}
