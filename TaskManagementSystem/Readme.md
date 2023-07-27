# Task Management System

## Create Database:


* 1-Add your connectionString in Appsettings.Json and in the nlog.cofig
* 2-Create the LogTable in the database using this query:

CREATE TABLE [dbo].[Logs] (
    [Date]  NVARCHAR(MAX),
    [Level] NVARCHAR(50),
    [RequestPath] NVARCHAR(1000),
    [RequestQueryString] NVARCHAR(1000),
    [Source] NVARCHAR(100),
    [Message] NVARCHAR(MAX),
    [StackTrace] NVARCHAR(MAX),
    [InnerExceptionMessage] NVARCHAR(MAX),
    [InnerExceptionStackTrace] NVARCHAR(MAX)
);

* 3-In Package Manager Console -> set Default Project: TaskManagementSystem.DataAccess -> run command: Update-Database
       

###### Finally Run the Project and utilise the TaskControllerTests for testing.