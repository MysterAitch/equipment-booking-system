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
dotnet aspnet-codegenerator razorpage -m Item -dc EquipmentBookingSystem.Website.Data.WebsiteDbContext -udl -outDir Pages/Items --databaseProvider SqlServer
```

```shell
dotnet aspnet-codegenerator razorpage -m Booking -dc EquipmentBookingSystem.Website.Data.WebsiteDbContext -udl -outDir Pages/Bookings --databaseProvider SqlServer
```

```shell
dotnet aspnet-codegenerator razorpage -m Event -dc EquipmentBookingSystem.Website.Data.WebsiteDbContext -udl -outDir Pages/Events --databaseProvider SqlServer
```

## Add a new migration

```shell
dotnet ef migrations add InitialCreate
```
```shell
dotnet ef migrations add UpdateModel
```
```shell
dotnet ef migrations add UpdateModel2
```
```shell
dotnet ef migrations add AddBookings
```
```shell
dotnet ef migrations add AddAuditTable
```
```shell
dotnet ef migrations add AddAuditTableEntityId
```
```shell
dotnet ef migrations add AddAuditTableEntityIdNullable
```
```shell
dotnet ef migrations add AddItemIdentifiers
```
```shell
dotnet ef migrations add RemoveItemName
```
```shell
dotnet ef migrations add AddItemNotes
```
```shell
dotnet ef migrations add AddBookingNotes
```
```shell
dotnet ef migrations add AddBasicEvent
```
```shell
dotnet ef migrations add AddBasicEvent2
```
```shell
dotnet ef migrations add AddBasicEvent3
```

## Update the database

```shell
dotnet user-secrets set "ConnectionStrings:WebsiteDbContext" "Server=tcp:equipment-booking-system.database.windows.net,1433;Initial Catalog=equipment-booking-system-database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';"
```
```shell
dotnet user-secrets set "ConnectionStrings:WebsiteDbContext" "Server=tcp:equipment-booking-system.database.windows.net,1433;Initial Catalog=equipment-booking-system-database-dev;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';"
```

Note, requires az cli if using Azure AD authentication (e.g., locally)

```shell
dotnet ef database update
```

Check for pending updates -- TODO: Include this in a deployment pipeline (note: would need database credentials adding to the action)
    
```shell
dotnet ef migrations list | grep "(Pending)"
```

```shell
dotnet ef migrations list | Select-String "(Pending)"
```

```shell
try {
    $matches = dotnet ef migrations list | Select-String "(Pending)$"
    if ($matches.Count -gt 0) {
        Write-Host "There are pending migrations"
        exit 1
    } else {
        Write-Host "There are no pending migrations"
        exit 0
    }
}
catch {
    Write-Host "An error occurred executing 'dotnet ef migrations list'. Please check your environment"
    exit 2
}
```



## Azure Web App Configuration

App Settings:
- Used to authenticate web app to database(?)
  - `AZURE_CLIENT_ID`
  - `AZURE_CLIENT_SECRET`
  - `AZURE_TENANT_ID`
- Used to authenticate user to web app (note double underscore `__` used to represent nested values)
  - `AzureAd__Domain`
  - `AzureAd__ClientId`
  - `AzureAd__ClientSecret`
  - `AzureAd__TenantId`


Enable Managed Identity for Azure SQL Database

- https://learn.microsoft.com/en-us/azure/app-service/tutorial-connect-msi-sql-database?tabs=windowsclient%2Cefcore%2Cdotnet

```sql
-- web-app name: equipment-booking-system
-- TODO: Minimise permissions
-- TODO: Consider a user group, instead of specific/individual users
-- Server=tcp:equipment-booking-system.database.windows.net,1433;Initial Catalog=equipment-booking-system-database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';

CREATE USER [equipment-booking-system] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [equipment-booking-system];
ALTER ROLE db_datawriter ADD MEMBER [equipment-booking-system];
ALTER ROLE db_ddladmin ADD MEMBER [equipment-booking-system];
GO
```

Configure connection string in web app configuration to use managed identity

```
-- NOTE: AD Default _should_ work, but in practice it seemed to require explicitly specifying AD Managed Identity...?
Server=tcp:equipment-booking-system.database.windows.net,1433;Initial Catalog=equipment-booking-system-database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default';
Server=tcp:equipment-booking-system.database.windows.net,1433;Initial Catalog=equipment-booking-system-database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Managed Identity';
```

