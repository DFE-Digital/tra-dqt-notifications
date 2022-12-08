resource "azurerm_storage_account" "notify" {
  name                              = var.notify_storage_account_name
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
