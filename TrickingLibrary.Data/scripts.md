### migrations
dotnet ef migrations add <name> -c AppDbContext -s ..\TrickingLibrary.Api -o .\Migrations

### migration script
dotnet ef migrations script -i -c AppDbContext -s ..\TrickingLibrary.Api -o script.sql

### identity migrations & update
dotnet ef migrations add <name> -c ApiIdentityDbContext -o .\IdentityMigrations
dotnet ef migrations script -i -c ApiIdentityDbContext -o script.sql
dotnet ef database update -c ApiIdentityDbContext