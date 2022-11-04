
variable "resource_group_name" {
  type        = string
  description = "Resource group in which the Service Bus namespace should exist"
}

variable "environment_name" {
  type = string
}
variable "sku" {
  type        = string
  default     = "Standard"
  description = "SKU type (Basic, Standard and Premium)"
}

variable "resource_prefix" {
  type    = string
  default = ""
}

variable "app_suffix" {
  type    = string
  default = ""
}
locals {
  hosting_environment = var.environment_name
  service_bus_name    = "${var.resource_prefix}rsm-${var.environment_name}${var.app_suffix}-srbsnsp"
}