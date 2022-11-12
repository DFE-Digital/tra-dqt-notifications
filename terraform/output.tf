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