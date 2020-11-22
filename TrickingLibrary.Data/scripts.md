### migrations
dotnet ef migrations add <name> -c AppDbContext -s ..\TrickingLibrary.Api -o .\Migrations

### migration script
dotnet ef migrations script -i -c AppDbContext -s ..\TrickingLibrary.Api -o script.sql

### identity migrations & update
dotnet ef migrations add <name> -c IdentityDbContext -o .\IdentityMigrations
dotnet ef migrations script -i -c IdentityDbContext -o script.sql
dotnet ef database update -c IdentityDbContext