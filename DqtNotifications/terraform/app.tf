locals {
  auth_rule_name = "SendAndListenSharedAccessKey"
  sku            = var.enable_private_endpoint == true ? "Premium" : var.sku
  capacity       = local.sku == "Premium" && var.capacity <= 0 ? 1 : var.capacity
}


# App Sercie Plan
resource "azurerm_app_service_plan" "app_service_plan" {
  name                = local.app_service_plan_name
  location            = data.azurerm_resource_group.rgsb.location
  resource_group_name = data.azurerm_resource_group.rgsb.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
  

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}


# Function App
resource "azurerm_function_app" "functionAp" {
  name                       = local.azurerm_function_app_name
  location                   = data.azurerm_resource_group.rgsb.location
  resource_group_name        = data.azurerm_resource_group.rgsb.name
  app_service_plan_id        = azurerm_app_service_plan.app_service_plan.id
  storage_account_name       = azurerm_storage_account.st.name
  storage_account_access_key = azurerm_storage_account.st.primary_access_key

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

# Postgress Server
resource "azurerm_postgresql_flexible_server" "postgres-server" {
  name                   = local.postgres_server_name
  location               = data.azurerm_resource_group.rgsb.location
  resource_group_name    = data.azurerm_resource_group.rgsb.name
  version                = "11"
  administrator_login    = local.infrastructure_secrets.POSTGRES_ADMIN_USERNAME
  administrator_password = local.infrastructure_secrets.POSTGRES_ADMIN_PASSWORD
  create_mode            = "Default"
  storage_mb             = var.postgres_flexible_server_storage_mb
  sku_name               = var.postgres_flexible_server_sku
  dynamic "high_availability" {
    for_each = var.enable_postgres_high_availability ? [1] : []
    content {
      mode = "ZoneRedundant"
    }
  }
  lifecycle {
    ignore_changes = [
      tags,
      # Allow Azure to manage deployment zone. Ignore changes.
      zone,
      # Allow Azure to manage primary and standby server on fail-over. Ignore changes.
      high_availability[0].standby_availability_zone
    ]
  }
}

# database

resource "azurerm_postgresql_flexible_server_database" "postgres-database" {
  name      = local.postgres_database_name
  server_id = azurerm_postgresql_flexible_server.postgres-server.id
}

# Servicebus Namespace
resource "azurerm_servicebus_namespace" "servicebus_namespace" {
  name                = local.servicebus_namespace_name
  location            = data.azurerm_resource_group.rgsb.location
  resource_group_name = data.azurerm_resource_group.rgsb.name
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

  listen = true
  send   = true


}

# Servicebus Topic
resource "azurerm_servicebus_topic" "servicebus_topic" {
  name         = local.servicebus_topic_name
  namespace_id = azurerm_servicebus_namespace.servicebus_namespace.id

  enable_partitioning = true


}

#Storage account
resource "azurerm_storage_account" "st" {
  name                              = local.storage_account
  resource_group_name               = data.azurerm_resource_group.rgsb.name
  location                          = data.azurerm_resource_group.rgsb.location
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

#Storage container
resource "azurerm_storage_container" "keys" {
  name                  = local.storage_container_name
  storage_account_name  = azurerm_storage_account.st.name
  container_access_type = "private"
}




