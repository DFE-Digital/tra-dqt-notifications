terraform {
  required_version = ">= 1.3.1"

  backend "azurerm" {
    container_name = "dqtnoti-tfstate"
  }

   required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.27.0"
    }
    }
  }
}

