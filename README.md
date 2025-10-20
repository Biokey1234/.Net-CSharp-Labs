# cst8359Labs

This repository contains a collection of lab projects and an assignment for CST-8359. Each subfolder (Assignment2, Lab2..Lab7) is a standalone .NET project demonstrating different concepts including console programming, ASP.NET Core MVC/Razor Pages/Web API, Entity Framework Core with migrations, Azure integration (App Service, Azure SQL, Blob Storage), and simple seeding/data models.

Below is a concise guide and summary of what was implemented in each lab and the assignment, plus notes on storage (EF Core), migrations, and Azure deployment artifacts included in the repo.

---

## Summary by project

### Assignment2 (folder: `Assignement2`)
- Type: ASP.NET Core MVC web app
- Key features:
  - Uses Entity Framework Core with a `DealsFinderDbContext` (see `Assignement2/Data/DealsFinderDbContext.cs`).
  - Models: `Customer`, `FoodDeliveryService`, `Subscription`, and `Deal` (see `Assignement2/Models`).
  - Many-to-many via join entity `Subscription` (composite key CustomerId + ServiceId).
  - `DbInitializer` seeds sample data for customers, services and subscriptions on startup (`Assignement2/Data/DbInitializer.cs`). The app calls `DbInitializer.Initialize(context)` during startup.
  - `DealsController` supports listing, creating (file upload to wwwroot/images), and deleting deal posts. Uploaded images are stored under `wwwroot/images` and image paths saved in EF.
  - Session state is enabled for the site (see `Assignement2/Program.cs`).
- Migrations:
  - `Migrations/20250327013253_Init` (initial migration placeholder).
  - `Migrations/20250407181249_AddDealModel` — adds the `Deal` table and FK to `FoodDeliveryService`.
- Notes:
  - The DbContext registers the DbSet properties and configures the composite key for `Subscription` in `OnModelCreating`.
  - `DbInitializer.Initialize` now uses `context.Database.Migrate()` and performs seeding after applying migrations (resilient to transient failures).


### Lab2 (folder: `Lab2`)
- Type: Console application
- Implemented algorithms and LINQ examples working on a words list loaded from `Lab2/Words.txt`.
- Features include: file import, bubble sort implementation and timing, LINQ sorting, counting distinct words, taking the first 10 words, reversing words, and a few filtered queries. This is a single-file console project in `Lab2/Program.cs`.


### Lab4 (folder: `Lab4`)
- Type: ASP.NET Core MVC web app (starter)
- Basic MVC wiring with controllers, views, and static files enabled. This lab demonstrates routing and default MVC startup configuration.


### Lab5 (folder: `Lab5`)
- Type: ASP.NET Core MVC web app with EF Core
- Similar setup to `Assignment2`: registers a `DealsFinderDbContext`, seeds data with `DbInitializer`, and uses session state. Program structure matches a typical MVC app scaffold.


### Lab6 (folder: `Lab6`)
- Type: ASP.NET Core Razor Pages app
- Uses Entity Framework Core via `PredictionDataContext` (registered in `Lab6/Program.cs`).
- Adds Azure Blob Storage client support using `Azure.Storage.Blobs.BlobServiceClient` registered as a singleton. The project reads the `AzureBlobStorage` connection string from configuration — indicating the app can upload/read blobs.


### Lab7 (folder: `Lab7`)
- Type: ASP.NET Core Web API
- Uses EF Core with `StudentDbContext` and includes Swagger/OpenAPI configuration (see `Lab7/Program.cs`).
- The project contains Azure deployment artifacts under `Lab7/Properties/PublishProfiles` and `ServiceDependencies` that show it was configured for Azure App Service and Azure SQL (mssql.azure). Example App Service endpoint and publish profile are present; there are also connection strings in publish output (under `obj/Release/.../appsettings.json`) referencing an Azure SQL instance.
- Swagger is configured and XML comments are included to improve API documentation.


## Entity Framework Core / Storage
- This repository uses EF Core (SQL Server provider) in multiple projects (Assignment2, Lab5, Lab6, Lab7). Connection strings are read from `appsettings.json`/`appsettings.Development.json` via `builder.Configuration.GetConnectionString("DefaultConnection")` or similar keys.
- Migration files (for Assignment2) live under `Assignement2/Migrations` and show the schema evolution (initial and AddDealModel migration). The model snapshot `DealsFinderDbContextModelSnapshot.cs` describes the final consolidated model used to generate migrations.
- Seeding approach:
  - `DbInitializer.Initialize` in Assignment2 and Lab5 was updated to call `context.Database.Migrate()` and seed after migrations with a small retry/backoff to be more resilient during deployments.


## Azure deployment and integration notes
- Lab6 and Lab7 demonstrate Azure integration:
  - `Lab6` registers `BlobServiceClient` using an `AzureBlobStorage` connection string — this enables reading/writing blobs in Azure Blob Storage.
  - `Lab7` includes publish profiles and service dependency templates showing a deployment to Azure App Service and an Azure SQL Database (see `Lab7/Properties/PublishProfiles/*` and `Lab7/Properties/ServiceDependencies/*`). The publish profile contains example App Service URLs and msdeploy configuration.
- General steps to deploy to Azure App Service (high-level):
  1. Create an App Service (Windows or Linux) and an Azure SQL Database (or use Managed Identity).
  2. Update the `ConnectionStrings:DefaultConnection` App Setting in the App Service to point to the Azure SQL DB (use secure secrets, do not commit passwords).
  3. If your app uses Blob Storage (Lab6), add an App Setting `AzureBlobStorage` with the storage connection string or use Managed Identity + Azure SDK for authentication.
  4. Use Visual Studio Publish, `dotnet publish` + Azure CLI, or GitHub Actions to deploy the app. The repo contains example publish profiles that were generated by Visual Studio.



