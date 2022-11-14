variable "project" {
  default     = ""
  description = "Project name (required for Premium SKU) - sds or cft. "
}

variable "environment_name" {
  type = string
}

variable "resource_prefix"{
  type    = string
  default = ""
}
variable "namespace_prefix" {
  type    = string
  default = ""
}

variable "mssql_server" {
  type    = string
  default = ""
}

variable "topic_prefix" {
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

variable "plan_prefix" {
  type    = string
  default = ""
}

variable "azure_sp_credentials_json" {
  type    = string
  default = null
}

variable "resource_group_name" {
type        = string
}

variable "sku" {
  type        = string
  default     = "Standard"
  description = "SKU type (Basic, Standard and Premium)"
}

variable "capacity" {
  type        = number
  default     = 0
  description = "Specifies the capacity"
}

variable "function_app_name" {
  type    = string
  default = ""
}

variable "storage_account_name" {
  type = string
}

variable "zone_redundant" {
  type        = bool
  default     = false
  description = "Whether or not this resource is zone redundant (true or false)"
}

variable "enable_public_access" {
  type        = bool
  default     = false
  description = "Enable public access (should only be enabled for a migration when using the Premium SKU and a private endpoint connection)"
}

variable "enable_private_endpoint" {
  default     = false
  description = "Enable Private endpoint? Only available with the Premium SKU, if set to true a Premium type Service Bus Namespace will be deployed automatically"
}

variable "data_protection_container_delete_retention_days" {
  default = 7
  type    = number
}

variable "administrator_login" {
  description = "The Administrator Login for the MSSQL Server."
  type        = string
  default     = "MSsqladm"
}

variable "administrator_login_password" {
  description = "The Password associated for the MSSQL server."
  type        = string
  default     = "Testing456"
  sensitive   = true
}

variable "mssql_max_size_gb" {
  description = "The max size of the database in gigabytes"
  type        = number
  default     = 2
}

variable "mssql_database" {
  description = "The name of the MSSQL database to create. Must be set if `enable_mssql_database` is true"
  type        = string
  default     = ""
}
variable "mssql_sku_name" {
  description = "Specifies the name of the SKU used by the database"
  type        = string
  default     = "Basic"
}

locals {
  hosting_environment         = var.environment_name
  servicebus_namespace_name   = "${var.namespace_prefix}dqtnoti-${var.environment_name}-sbn"
  servicebus_topic_name       = "${var.topic_prefix}dqtnoti-${var.environment_name}-sbt"
  storage_account             = "${var.storage_account}dqtnoti${var.environment_name}sg"
  app_service_plan_name       = "${var.plan_prefix}dqtnoti-${var.environment_name}-spl"
  azurerm_function_app_name   = "${var.function_app_name}dqtnoti-${var.environment_name}-fapp"
  mssql_server                = "${var.mssql_server}dqtnoti-${var.environment_name}-mssql"
  mssql_max_size_gb           = var.mssql_max_size_gb
  mssql_sku_name              = var.mssql_sku_name
  mssql_database              = "${var.mssql_database}dqtnoti-${var.environment_name}-sqldb"
}
