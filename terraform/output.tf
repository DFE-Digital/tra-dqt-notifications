output "mssql_server" {
  value = azurerm_mssql_server.mssql_server.name
}

output "functionapp_name" {
  value = azurerm_linux_function_app.function_app.name
}
