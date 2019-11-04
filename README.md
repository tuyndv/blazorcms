# Base Cms

Pl base cms. 

Project struct

	1.Representation
		Pl.Cms => a web app to management content

	2.Core
		Pl.Core => contains all business, interface, constants, and settings class of system

	3.Infrastructure
		Pl.Caching => all cache provider that support in system, and some caching helper
		Pl.Data => all data implement classes, use entity framework core 3.0
		Pl.Identity => Use asp.net identity 3.0 to authentication user in system
		Pl.Logging => all logging provider in system, error and user activity use db log, extend use serilog
		Pl.WebFramework => contains tagheler, razor helper, filter, extentions vvv. all support for web app

	4.Tests
		Pl.FuntionalTest => funtion test for all project
		Pl.IntegrationTest => integration test for all project
		Pl.UnitTest => a unit test for all project

	5.SolutionItems
		CHANGELOG => app version info
		CommandTemplate => all common command template
		CONTRIBUTING => code rule, things rule
		README => about introduce for system

System architectures 
	see => https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures

Code conventions
	see => https://github.com/ktaranov/naming-convention/blob/master/C%23%20Coding%20Standards%20and%20Naming%20Conventions.md