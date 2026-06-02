# Computer Store API

Computer Store API is a RESTful web application built with ASP.NET Core Web API.
The project manages computer store products, categories, stock import and basket discount calculation.

## Project Overview

This project was developed for the Internet Services course.
It demonstrates a layered architecture using ASP.NET Core, Entity Framework Core, SQL Server, AutoMapper and unit/integration testing.

## Main Features

* Create, read, update, and delete categories
* Create, read, update, and delete products
* Assign products to categories
* Prevent deleting categories that are already used by products
* Import product stock
* Calculate basket discounts
* Friendly error handling
* Swagger support for testing endpoints
* Unit and integration tests

## Technologies Used

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* AutoMapper
* Swagger / OpenAPI
* xUnit
* Visual Studio

## Project Structure

```text
ComputerStoreApi/                  - Main Web API project
ComputerStoreApi.BLL/              - Business logic layer
ComputerStoreApi.DAL/              - Data access layer
ComputerStoreAPi.Tests/            - Unit tests
ComputerStoreApi.IntegrationTests/ - Integration tests
IS_Project.slnx                    - Solution file
```

## Architecture

The project uses a layered architecture:

* **API Layer**: Handles HTTP requests and responses through controllers.
* **BLL Layer**: Contains business logic and service classes.
* **DAL Layer**: Handles database access using Entity Framework Core.
* **Tests**: Includes unit and integration tests for validating functionality.

## How to Run the Project

1. Open the solution in Visual Studio.
2. Make sure SQL Server is running.
3. Check the connection string in `appsettings.json`.
4. Run the API project.
5. Open Swagger in the browser to test the endpoints.

Example local URL:

```text
http://localhost:5120/swagger
```

## Example Endpoints

```http
GET /api/Categories
POST /api/Categories
PUT /api/Categories/{id}
DELETE /api/Categories/{id}

GET /api/Products
POST /api/Products
PUT /api/Products/{id}
DELETE /api/Products/{id}

POST /api/Stock/import
POST /api/Basket/calculate-discount
```

## Testing

The project includes both unit tests and integration tests.

To run the tests:

1. Open the solution in Visual Studio.
2. Go to Test Explorer.
3. Click Run All Tests.

## Author

Matej Mitevski
Computer Science Student
UACS
