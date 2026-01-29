# About Sale Module

This feature comes with the implementation of a RESTful API for sales management, following the principles of **Domain-Driven Design (DDD)**, **Clean Architecture** e **SOLID**.

---

## Features implemented

* **Complete CRUD:** Create Sale (`POST`), Read Sale (`GET`), Update Sale (`PUT`), Delete Sale (`DELETE`) and Cancel Sale (`PATCH`)
* **Automatic Discount Rules** Applied to Domain Layer Entities:
  * **< 4 items:** No discount.
  * **4 a 9 item:** 10% discount
  * **10 a 20 items:** 20% discount
  * **> 20 items:** Do not allow sales (Business Rule).


* [`TO-DO`] **Domain Events:** Event triggering (`SaleCreated`, `SaleModified`, `SaleCancelled`) for asynchronous integration.
* **Sale Soft Delete:** Cancellation does not delete the record, it only changes its status. (`IsCancelled`).
* **Observability:** Tracking changes by `ILogger` in all handlers.

### Directory Architecture and Structure

The solution was structured in logical layers to ensure separation of responsibilities:

```text
src/
├── Ambev.DeveloperEvaluation.Domain/         # Central Layer (Entities, Rules, Interfaces)
│   ├── Entities/Sale/                        # Entities Sale and SaleItem
│   ├── Events/                               
│   └── Repositories/                         # Interfaces (ISaleRepository)
│
├── Ambev.DeveloperEvaluation.Application/    # Use Cases (CQRS)
│   ├── Sales/
│   │   ├── CreateSale/                       # Commands, Handlers, Validators and some Profiles (Command <-> Entity)
│   │   ├── GetSale/
│   │   ├── UpdateSale/
│   │   └── CancelSale/
│
├── Ambev.DeveloperEvaluation.ORM/            # Data Infrastructure
│   ├── Mapping/                              # EF Core Configuration (Tables 'sales', 'sale_items')
│   ├── Repositories/                         # Implementation (SaleRepository)
│   └── Migrations/                           # History of changes at the Bank
│
├── Ambev.DeveloperEvaluation.WebApi/         # Input Layer (API)
│   ├── Features/Sales/                       # SalesController, DTOs, Request/Response
│   └── Mappings/                             # AutoMapper Profiles (Request -> Command)

```

---

## Ambiente de Desenvolvimento (Local)

To run this local project using the **VS Code**, make sure you have the following tools installed.:

### Requirements

1. **[VS Code](https://code.visualstudio.com/)** (latestl)
* *Recommended Extensions:* C# Dev Kit, Docker, SQLTools/pgAdmin/DBeaver


2. **[.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**
3. **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** 
4. **[Git](https://git-scm.com/)**

---

## How to Run the Project?

Follow the steps below to attach the foot applicator to your machine..

### 1. Clone the Repository

Open the VS Code terminal. (Ctrl+`) and execute:

```bash
git clone https://github.com/andrefpcunha/Ambev.DeveloperEvaluation.git
cd Ambev.DeveloperEvaluation

```

### 2. Up the Infrastructure (Docker)

In your terminal, access the `\template\backend` directory and run the command:

```bash
docker-compose up -d

```

### 3. Aply Migrations (Data base)

Before running the API, you need to create the `Sales` and `SaleItems` tables in the database. In the terminal, execute:

```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
```
<br>

> **Note:** If you don't have the `dotnet-ef` tool installed globally, install it with the command: 
> 
> `dotnet tool install --global dotnet-ef`

### 4. Run Application

You can run it via the terminal or through the VS Code Debugger. (`F5`).

**In Terminal:**

- access the `\template\backend` directory and run the command:

```bash
docker-compose up --build

```

- To access Swagger: `http://localhost:5555/swagger`

---

## Testing and Quality Assurance

The project has unit test coverage focused on Handlers and Entities..

### Generate Coverage Report

To view the code coverage percentage:

```bash
# Install report generator (first time only)
dotnet tool install -g dotnet-reportgenerator-globaltool
```

### Run Unit Tests

```bash
# To run all the tests for the solution with data collection (XPlat Code Coverage):
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

```

To view the test coverage report, open the `coveragereport/index.html` file in your browser to see the details..

---