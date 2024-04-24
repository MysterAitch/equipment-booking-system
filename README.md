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
