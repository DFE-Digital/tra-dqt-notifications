variable "environment_name" {
  type = string
}

variable "resource_prefix" {
  type    = string
  default = ""
}
variable "app_service_plan_sku_tier" {
  type    = string
  default = "Basic"
}

variable "app_service_plan_sku_size" {
  type    = string
  default = "B1"
}

variable "storage_account" {
  type    = string
  default = ""
}

variable "key_vault_name" {
  type = string
}

variable "azure_sp_credentials_json" {
  type    = string
  default = null
}

variable "resource_group_name" {
  type = string
}

variable "service_bus_zone_redundant" {
  type        = bool
  default     = false
  description = "Whether or not this resource is zone redundant - servicebus-namespace (true or false)"
}

variable "data_protection_container_delete_retention_days" {
  default = 7
  type    = number
}
variable "mssql_max_size_gb" {
  description = "The max size of the database in gigabytes"
  type        = number
  default     = 2
}

variable "mssql_sku_name" {
  description = "Specifies the name of the SKU used by the database"
  type        = string
  default     = "Basic"
}

variable "application_insights_daily_data_cap_gb" {
  type    = string
  default = "0.033"
}

variable "application_insights_retention_days" {
  type    = number
  default = 30
}

locals {
  hosting_environment       = var.environment_name
  servicebus_namespace_name = "${var.resource_prefix}-dqtnoti-${var.environment_name}-sbn"
  servicebus_topic_name     = "${var.resource_prefix}-dqtnoti-${var.environment_name}-sbt"
  storage_account           = "${var.resource_prefix}dqtnoti${var.environment_name}sg"
  app_service_plan_name     = "${var.resource_prefix}-dqtnoti-${var.environment_name}-spl"
  linux_function_app_name   = "${var.resource_prefix}-dqtnoti-${var.environment_name}-fapp"
  mssql_server              = "${var.resource_prefix}-dqtnoti-${var.environment_name}-mssql"
  mssql_database            = "${var.resource_prefix}-dqtnoti-${var.environment_name}-sqldb"
  app_insights_name         = "${var.resource_prefix}-dqtnoti-${var.environment_name}-appi"
  mssql_max_size_gb         = var.mssql_max_size_gb
  mssql_sku_name            = var.mssql_sku_name
}
