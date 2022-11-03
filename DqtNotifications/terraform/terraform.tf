terraform {
  required_version = "~> 1.0"

  backend "azurerm" {
    container_name = "dqt-notifications-tfstate"
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.84"
    }
  }
}

