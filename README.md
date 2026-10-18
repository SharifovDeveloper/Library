# 📚 Library Management System

An ASP.NET Core MVC web application for managing a library catalog: books, authors and categories.

![.NET](https://img.shields.io/badge/.NET-7.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?logo=bootstrap&logoColor=white)

## ✨ Features

- **Books, authors and categories**: create, view, edit and delete through server-rendered pages
- Relations between books, their authors and categories
- **Fake data seeding** with Bogus, so the library is filled with sample books on first run
- Database is migrated automatically on startup

## 🏗️ Project structure

```
Library/
├── Controllers/   → Books, Authors, Categories, Home
├── Models/        → Book, Author, Category
├── ViewModels/    → BookViewModel
├── Views/         → Razor views for each entity
├── Data/          → LibraryDbContext
└── Extensions/    → DatabaseSeeder
```

## 🚀 Getting started

**Requirements:** .NET 7 SDK, SQL Server

1. Clone the repository:
   ```bash
   git clone https://github.com/SharifovDeveloper/Library.git
   cd Library
   ```
2. Set your SQL Server connection string in `Library/appsettings.json` → `ConnectionStrings:LibraryConnection`.
3. Run the app (the database is created and seeded automatically):
   ```bash
   dotnet run --project Library
   ```

## 🛠️ Tech stack

ASP.NET Core MVC · Entity Framework Core · SQL Server · Bogus · Bootstrap
