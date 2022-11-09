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

# primary connection string for send and listen operations
output "primary_send_and_listen_connection_string" {
  value = azurerm_servicebus_namespace_authorization_rule.send_listen_auth_rule.primary_connection_string
}

output "id" {
  value = azurerm_servicebus_namespace.servicebus_namespace.id
}
*/