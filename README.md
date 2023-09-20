## About this web application

![build and test](https://github.com/WebWat/certificate-MVC/actions/workflows/dotnet.yml/badge.svg)
![GitHub Workflow Status](https://img.shields.io/github/workflow/status/webwat/certificate-MVC/Publish%20Docker%20image?label=Publish%20Docker%20image)
[![CodeFactor](https://www.codefactor.io/repository/github/webwat/certificate-mvc/badge)](https://www.codefactor.io/repository/github/webwat/certificate-mvc)

This web application helps you easily create, store and share your personal achievements such as diplomas or certificates. 

## What it consists of

Website was written using the [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb) template and includes technologies, tools such as 
  - [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (for database operations) 
  - [Identity API](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio) (authentication with mail confirmation, password change, etc.)
  - Testing with [Coyote](https://github.com/microsoft/coyote)
  - Testing with EdgeWebDriver
  - [Bootstrap](https://getbootstrap.com/) (no way without him)
  - Docker Compose deploy
  
There is also a [separate branch](https://github.com/WebWat/certificate-MVC/tree/cosmos-db) for connecting to CosmosDB.

## What it looks like

Here are a few screenshots for an overview.

### Index page
![main](https://github.com/WebWat/certificate-MVC/blob/dev/imgs/main.png)

### Work page
![index](https://github.com/WebWat/certificate-MVC/blob/dev/imgs/index.png)

### Certificate details
![details](https://github.com/WebWat/certificate-MVC/blob/dev/imgs/details.png)

### Account management
![manage](https://github.com/WebWat/certificate-MVC/blob/dev/imgs/manage.png)

### Public link (share with someone)
![public](https://github.com/WebWat/certificate-MVC/blob/dev/imgs/public.png)
