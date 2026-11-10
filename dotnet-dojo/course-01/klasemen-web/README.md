# Klasemen Web — MVC practice

A small ASP.NET Core MVC exercise using fictional sample data. The code was prepared with AI assistance for guided learning.

- `Program.cs`: registers MVC and configures the default route.
- `Models/Klub.cs`: defines club data and validation rules.
- `Controllers/KlasemenController.cs`: handles GET and POST, checks validation, and returns the view.
- `Views/Klasemen/Index.cshtml`: Razor view displaying the form and validation messages.

Run from this folder with `dotnet run`, then open the local URL printed in the terminal. The form demonstrates validation only: no data is saved and this is not a complete league table. The 0–100 points range is an example, not a real competition rule. The project uses the shared ASP.NET Core framework and adds no external NuGet packages.
