# TagsChallenge
Instruction

1. Open in Visual Studio 
2  Project TagsChallenge.API must be Startup project (right click on TagsChallenge.API, select Set As Startup Project)
3. Restore NuGet packages for the solution (right click Solution explorer, select Restore NuGet Packages)
4. In Package Manager Console navigate to TagsChallenge.DAL directory
5. To create database run command in Package Manager Console: dotnet ef database update -s ..\TagsChallenge.API\ --verbose 
6. Database connection string can be modified in TagsChallenge.API\appsettings.json file
7. Run application
8. Use Swagger to register (https://localhost:44321/swagger/index.html)
9. For Auth token execute Login request in Swagger 
10. To authorize in Swagger add Bearer + returned Auth token 
