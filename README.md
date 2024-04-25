# equipment-booking-system



## Pre-requisites

- AZ CLI
  - https://learn.microsoft.com/en-us/cli/azure/install-azure-cli

- Application registration
  - Used to authenticate the application with Azure AD/Entra
  - Note: auth with SingleOrg requires all users to be in that directory,
    and allowing multi-org / personal accounts requires publisher verification.
    - https://learn.microsoft.com/en-gb/entra/identity-platform/publisher-verification-overview
  - Enable sign-up experience (NOTE: Attempted, but not gotten working yet)
    - https://github.com/AzureAD/microsoft-identity-web/wiki/web-apps
    - https://learn.microsoft.com/en-gb/entra/external-id/self-service-sign-up-user-flow

- SQL Server of some kind with a database and relevant credentials for connection string
  - Azure SQL Database
  - Local SQL Server



## Create a new project
```shell
dotnet new webapp --auth SingleOrg -o EquipmentBookingSystem
```

Setup application secrets for authentication
```shell
dotnet user-secrets init
dotnet user-secrets set "AzureAd:Domain" "your-client-id"
dotnet user-secrets set "AzureAd:ClientId" "your-client-id"
dotnet user-secrets set "AzureAd:ClientSecret" "your-client-secret"
dotnet user-secrets set "AzureAd:TenantId" "your-tenant-id"
```


## Add Entity Framework Core
```shell
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

## Scaffold a new razor page
```shell
dotnet aspnet-codegenerator razorpage -m Item --dbContext EquipmentBookingSystem.Website.Data.WebsiteDbContext -udl -outDir Pages/Items --databaseProvider SqlServer
```


## Add a new migration
```shell
dotnet ef migrations add InitialCreate
```

## Update the database

```shell
dotnet user-secrets set "ConnectionStrings:WebsiteDbContext" "Server=tcp:equipment-booking-system.database.windows.net,1433;Initial Catalog=equipment-booking-system-database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';"
```

Note, requires az cli if using Azure AD authentication (e.g., locally)

```shell
dotnet ef database update
```

