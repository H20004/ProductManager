# Product Manager

Detta är en full stack-applikation byggd med Blazor WebAssembly som frontend och ett .NET Web API som backend.

## Tekniker

- Blazor WebAssembly
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- xUnit
- NSubstitute

## Funktioner

Applikationen har CRUD-funktionalitet för:

- Kategorier
- Produkter

En kategori kan innehålla flera produkter, vilket skapar en en-till-många-relation.

I frontend kan användaren:

- Visa produkter och kategorier
- Skapa nya produkter och kategorier
- Uppdatera befintlig data
- Ta bort data
- Navigera mellan olika sidor
- Se felmeddelanden om API:t inte svarar

## Backend-struktur

Backend använder:

- Controllers
- Services
- Repository pattern
- Generic repository
- Interfaces
- Dependency injection
- DTO:er
- Async/await
- Entity Framework Core

Strukturen följer ungefär detta flöde:

```text
Controller
   ↓
Service
   ↓
Repository
   ↓
Entity Framework Core
   ↓
SQL Server