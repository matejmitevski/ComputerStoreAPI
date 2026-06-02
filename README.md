# Computer Store API

A RESTful Web API for managing a computer store system, built with **ASP.NET Core Web API**, **Entity Framework Core** and **SQL Server**.

The project includes product and category management, stock import functionality, basket discount calculation, validation, error handling, and automated tests. It was developed as part of an Internet Services course project, with a focus on clean structure, layered architecture and practical backend development.

---

## Project Purpose

The goal of this project is to demonstrate the development of a structured backend application using modern C# and .NET technologies.

The API simulates a basic computer store system where products can be managed, assigned to categories, updated in stock and used in basket discount calculations.

This project demonstrates:

* REST API development with ASP.NET Core
* Layered application architecture
* Entity Framework Core database integration
* SQL Server usage
* DTOs and AutoMapper
* Business logic separation
* Unit and integration testing
* Swagger API documentation
* Error handling and validation

---

## Technologies Used

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **AutoMapper**
* **Swagger / OpenAPI**
* **xUnit**
* **Visual Studio**
* **Postman**

---

## Main Features

### Category Management

* Create new categories
* View all categories
* Update existing categories
* Delete categories
* Prevent deletion of categories that are already assigned to products

### Product Management

* Create new products
* View all products
* Update product details
* Delete products
* Assign products to one or more categories

### Stock Import

* Import and update product stock
* Process stock data through a dedicated endpoint
* Keep product quantity information updated

### Basket Discount Calculation

* Calculate basket discounts based on selected products
* Apply business logic through the service layer
* Return calculated discount results through the API

### Testing

* Unit tests for business logic
* Integration tests for API behavior
* Test coverage for important project functionality

---

## Project Structure

```text
ComputerStoreApi/
│
├── ComputerStoreApi/                  # API layer
│   ├── Controllers/                   # API endpoints
│   ├── Program.cs                     # Application startup
│   └── appsettings.json               # Configuration
│
├── ComputerStoreApi.BLL/              # Business Logic Layer
│   ├── Services/                      # Business logic services
│   ├── DTOs/                          # Data Transfer Objects
│   └── Mapping/                       # AutoMapper profiles
│
├── ComputerStoreApi.DAL/              # Data Access Layer
│   ├── Data/                          # Database context
│   ├── Models/                        # Entity models
│   ├── Repositories/                  # Data access logic
│   └── Migrations/                    # EF Core migrations
│
├── ComputerStoreAPi.Tests/            # Unit tests
├── ComputerStoreApi.IntegrationTests/ # Integration tests
├── IS_Project.slnx                    # Solution file
└── README.md
```

---

## Architecture

The project follows a layered architecture:

### API Layer

The API layer contains the controllers and handles HTTP requests and responses. It exposes endpoints for products, categories, stock import and basket discount calculation.

### Business Logic Layer

The BLL contains the main application logic. It processes requests, applies business rules, validates operations and communicates with the data access layer.

### Data Access Layer

The DAL handles database communication using Entity Framework Core and SQL Server. It contains the database context, entity models, repositories and migrations.

### Test Projects

The solution includes both unit tests and integration tests to verify that the application works correctly and that the API endpoints behave as expected.

---

## API Endpoints

### Categories

```http
GET /api/Categories
POST /api/Categories
PUT /api/Categories/{id}
DELETE /api/Categories/{id}
```

### Products

```http
GET /api/Products
POST /api/Products
PUT /api/Products/{id}
DELETE /api/Products/{id}
```

### Stock

```http
POST /api/Stock/import
```

### Basket

```http
POST /api/Basket/calculate-discount
```

---

## What I Learned

Through this project, I gained practical experience with:

* Building RESTful APIs using ASP.NET Core
* Structuring a backend project into multiple layers
* Working with Entity Framework Core and SQL Server
* Creating and using DTOs
* Applying AutoMapper for object mapping
* Writing unit and integration tests
* Testing APIs with Swagger and Postman
* Handling errors and validation in a backend application

---

## Author

**Matej Mitevski**
Computer Science Student
UACS - University American College Skopje
