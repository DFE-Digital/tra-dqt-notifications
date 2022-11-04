output "name" {
  value = azurerm_servicebus_namespace.servicebus_namespace.name
}

# sku type
output "sku" {
  value = var.sku
}

output "id" {
  value = azurerm_servicebus_namespace.servicebus_namespace.id
}