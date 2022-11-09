output "postgres_server_name" {
  value = azurerm_postgresql_flexible_server.postgres-server.name
}

output "administrator_login" {
  value = var.administrator_login
}

output "administrator_password" {
  value     = var.administrator_password
  sensitive = true
}


/*output "name" {
  value = azurerm_servicebus_namespace.servicebus_namespace.name
}



# secondary connection string for send and listen operations
output "secondary_send_and_listen_connection_string" {
  value = azurerm_servicebus_namespace_authorization_rule.send_listen_auth_rule.secondary_connection_string
}

# primary shared access key with send and listen rights
output "primary_send_and_listen_shared_access_key" {
  value = azurerm_servicebus_namespace_authorization_rule.send_listen_auth_rule.primary_key
}

# secondary shared access key with send and listen rights
output "secondary_send_and_listen_shared_access_key" {
  value = azurerm_servicebus_namespace_authorization_rule.send_listen_auth_rule.secondary_key
}

# sku type
output "sku" {
  value = var.sku
}

output "id" {
  value = azurerm_servicebus_namespace.servicebus_namespace.id
}
*/