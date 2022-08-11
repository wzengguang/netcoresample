# Migration
CMD: cd sample.web
## add
dotnet ef migrations add 22070811.1 --project ../Sample.Migrations
## rebuild project
## update
dotnet ef database update --project ../Sample.Migrations
## remove 
dotnet ef migrations remove [migration]
