
resource "azurerm_servicebus_namespace" "servicebus_namespace" {
  name                = local.service_bus_name
  location            = data.azurerm_resource_group.group.location
  resource_group_name = data.azurerm_resource_group.group.name
  sku                 = var.sku
  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}