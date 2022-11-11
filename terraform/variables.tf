variable "project" {
  default     = ""
  description = "Project name (required for Premium SKU) - sds or cft. "
}

variable "environment_name" {
  type = string
}

variable "resource_prefix"{
  type = string
  default = ""
}
variable "namespace_prefix" {
  type    = string
  default = ""
}

variable "topic_prefix" {
  type    = string
  default = ""
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
  description = "Specifies the capacity. Defaults to 1 when using Premium SKU."
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


variable "sql_administrator_login" {
  description = "The Administrator Login for the PostgreSQL Server. Changing this forces a new resource to be created."
  type        = string
  default = "psqladmin"
}

variable "administrator_password" {
  description = "The Password associated with the administrator_login for the PostgreSQL Server."
  type        = string
  default = "Password1234"
}

locals {
  hosting_environment       = var.environment_name
  servicebus_namespace_name = "${var.namespace_prefix}dqtnoti-${var.environment_name}-sbn"
  servicebus_topic_name     = "${var.topic_prefix}dqtnoti-${var.environment_name}-sbt"
  storage_account           = "${var.storage_account}dqtnoti${var.environment_name}sg"
  app_service_plan_name     = "${var.plan_prefix}dqtnoti-${var.environment_name}-spl"
  azurerm_function_app_name = "${var.function_app_name}dqtnoti-${var.environment_name}-fapp"
}