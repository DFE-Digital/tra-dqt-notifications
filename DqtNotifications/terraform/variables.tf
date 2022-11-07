
variable "project" {
  default     = ""
  description = "Project name (required for Premium SKU) - sds or cft. "
}

variable "environment_name" {
  type = string
}

variable "namespace_prefix" {
  type    = string
  default = ""
}

variable "topic_prefix" {
  type    = string
  default = ""
}

variable "storage_prefix" {
  type    = string
  default = ""

}

variable "plan_prefix" {
  type    = string
  default = ""
}
#variable "resource_group_name" {
# type        = string
#description = "Resource group in which the Service Bus namespace should exist"
#}


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

variable "storage_container_name" {
  type    = string
  default = ""

}

variable "function_app_name" {
  type    = string
  default = ""

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


locals {
  hosting_environment       = var.environment_name
  servicebus_namespace_name = "${var.namespace_prefix}dqtnoti-${var.environment_name}-sbn"
  servicebus_topic_name     = "${var.topic_prefix}dqtnoti-${var.environment_name}-sbt"
  storage_account_name      = "${var.storage_prefix}dqtnoti${var.environment_name}stg1"
  storage_container_name    = "${var.storage_container_name}dqtnoti-${var.environment_name}-sck"
  app_service_plan_name     = "${var.plan_prefix}dqtnoti-${var.environment_name}-spl"
  azurerm_function_app_name = "${var.function_app_name}dqtnoti-${var.environment_name}-fapp"
}

