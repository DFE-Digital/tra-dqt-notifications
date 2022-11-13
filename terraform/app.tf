locals {
  auth_rule_name = "SendAndListenSharedAccessKey"
  sku            = var.enable_private_endpoint == true ? "Standard" : var.sku
  capacity       = local.sku == "Standard" && var.capacity <= 0 ? 1 : var.capacity
       }

# App Service Plan
resource "azurerm_app_service_plan" "app_service_plan" {
  name                   = local.app_service_plan_name
  location               = data.azurerm_resource_group.resource_group.location
  resource_group_name    = data.azurerm_resource_group.resource_group.name
  os_type                = "Linux"
  zone_balancing_enabled = var.worker_count != null ? true : false
  worker_count           = var.worker_count

  sku {
    tier = var.app_service_plan_sku_tier
    size = var.app_service_plan_sku_size
  }

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

# Function App
resource "azurerm_function_app" "function_app" {
  name                       = local.azurerm_function_app_name
  location                   = data.azurerm_resource_group.resource_group.location
  resource_group_name        = data.azurerm_resource_group.resource_group.name
  app_service_plan_id        = azurerm_app_service_plan.app_service_plan.id
  storage_account_name       = azurerm_storage_account.st.name
  storage_account_access_key = azurerm_storage_account.st.primary_access_key

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
  sku                 = local.sku
  zone_redundant      = var.zone_redundant
  capacity            = local.capacity

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

# Servicebus Auth Rule
resource "azurerm_servicebus_namespace_authorization_rule" "send_listen_auth_rule" {
  name         = local.auth_rule_name
  namespace_id = azurerm_servicebus_namespace.servicebus_namespace.id
  listen       = true
  send         = true
}

# Servicebus Topic
resource "azurerm_servicebus_topic" "servicebus_topic" {
  name                = local.servicebus_topic_name
  namespace_id        = azurerm_servicebus_namespace.servicebus_namespace.id
  enable_partitioning = true
}

#Storage account
resource "azurerm_storage_account" "st" {
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

resource "azurerm_mssql_server" "default" {
  name                          = local.mssql_prefix
  resource_group_name           = data.azurerm_resource_group.resource_group.name
  location                      = data.azurerm_resource_group.resource_group.location
  version                       = "12.0"
  administrator_login           = var.mssql_administrator_login
  administrator_password        = local.mssql_server_admin_password
  public_network_access_enabled = false
  minimum_tls_version           = "1.2"

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_mssql_database" "default" {
  name        = local.mssql_database_name
  server_id   = azurerm_mssql_server.default[0].id
  collation   = "SQL_Latin1_General_CP1_CI_AS"
  sku_name    = local.mssql_sku_name
  max_size_gb = local.mssql_max_size_gb
 }
