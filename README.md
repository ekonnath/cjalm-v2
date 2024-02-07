# C-JALM

## Introduction
The Corporate Jargon and Language Manager (C-JALM) was created to familarize myself with `EntityFrameworkCore` and to get more comfortable working with `SQL` and Microsoft's `SSMS`. 

The manager automatically creates a local database that can be used to store corporate jargon and language, while also being able to track how much each word/phrase is used. Built within a C# console application, users have the ability to perform basic CRUD operations (create, read, update, delete) to a limited capacity. All operations are configured through `EntityFramework Core v8.0.1`

## Getting Started
Navigate to the path `.../cjalm-v2/cjalm-v2` through a console or terminal. To run the program, run the following command within this folder:
```
dotnet run
```
This project runs on `.NET v8.0`.

## Project Structure
The parent directory for `cjalm-v2` has the following children folders that are each their own respective project.

### `cjalm-v2`
Contains the `Program.cs` file and the main logic of the program. 

### `cjalm-v2.data`
Contains the database context class that configures the localdb SQLServer that is used for the database. Here the connection is configured to `CJALM-v2.Database`.

### `cjalm-v2.domain`
Contains the `Entry.cs` model class that is used by EFCore to interact with the database.

## CJALM-v2.Database
The database can be viewed through the `SQL Server Object Explorer` or `Microsoft SQL Server Management Studio (SSMS)` once the program has been run at least once. Within `SSMS` you will connect to the Server name `(localdb)\MSSQLLocalDB` to see the database and perform SQL queries on it. 