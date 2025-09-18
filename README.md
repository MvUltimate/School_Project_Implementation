# School Project Implementation

## Table of Contents

* [Overview](#overview)
* [Architecture](#architecture)
* [Projects / Structure](#projects--structure)
* [Getting Started](#getting-started)
* [Requirements & Dependencies](#requirements--dependencies)
* [Usage](#usage)

---

## Overview

This repository contains the implementation of a school project composed of multiple subsystems:

* **DAL** – Data Access Layer for database operations.
* **MVC\_SchoolProject** – Web application using the MVC pattern.
* **WebApi\_SchoolProject** – REST API project.
* **SeederTest** – Seeder or test data project.

The goal is to provide a complete full‑stack example including frontend, backend, API, and database layers, applying best practices such as separation of concerns and database seeding.

---

## Architecture

High-level overview of the interactions:

```text
[User / Web Client]
     ↓ HTTP / Browser
[MVC_SchoolProject] ↔ [WebApi_SchoolProject]
     ↑                    ↑
     └── Data Access Layer ──┘
         (DAL handles ORM / database operations)
```

* **MVC\_SchoolProject**: manages UI and views.
* **WebApi\_SchoolProject**: provides endpoints for data operations.
* **DAL**: central data persistence and models.
* **SeederTest**: populates development/test data.

---

## Projects / Structure

| Project                             | Purpose                                           |
| ----------------------------------- | ------------------------------------------------- |
| `DAL`                               | ORM models, database context, repository patterns |
| `MVC_SchoolProject`                 | User interface, controllers, views                |
| `WebApi_SchoolProject`              | REST API endpoints, JSON responses                |
| `SeederTest`                        | Populate the database with sample data            |
| `School_Project_Implementation.sln` | Solution file for Visual Studio                   |

---

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/MvUltimate/School_Project_Implementation.git
   ```
2. Open `School_Project_Implementation.sln` in Visual Studio or compatible IDE.
3. Configure the DAL connection string for your database.
4. Build all projects.
5. Run `SeederTest` (if applicable) to populate sample data.
6. Start the Web API project.
7. Run or debug the MVC project (which consumes the API).

---

## Requirements & Dependencies

* .NET version (e.g., .NET 6 / .NET Core / .NET Framework)
* Entity Framework or chosen ORM
* SQL Server (or other compatible database)
* Visual Studio / VS Code or other .NET IDE
* NuGet dependencies as listed in each project

---

## Usage

* Use the MVC app for user-facing interactions.
* Access the Web API via HTTP clients (Postman, curl) or AJAX from the frontend.
* Extend the DAL to support new entities or database tables.
* Run the seeder to reset or populate test data.
