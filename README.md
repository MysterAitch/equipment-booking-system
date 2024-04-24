# equipment-booking-system




## Create a new project
```
dotnet new webapp --auth SingleOrg -o EquipmentBookingSystem
```

Setup application secrets for authentication
```
dotnet user-secrets init
dotnet user-secrets set "AzureAd:Domain" "your-client-id"
dotnet user-secrets set "AzureAd:ClientId" "your-client-id"
dotnet user-secrets set "AzureAd:ClientSecret" "your-client-secret"
dotnet user-secrets set "AzureAd:TenantId" "your-tenant-id"
```


## Add Entity Framework Core
```
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

## Scaffold a new razor page
```
dotnet aspnet-codegenerator razorpage -m Item -dc EquipmentBookingSystem.Website.Data.EquipmentBookingSystem.Website.ItemContext -udl -outDir Pages/Items --referenceScriptLibraries --databaseProvider SqlServer
```
