# Papi's Power Practice MVC

Live demo
---------
https://papispowerpracticemvc.azurewebsites.net

Project description
-------------------
Papi's Power Practice MVC is a Razor Pages web application designed to support people who enjoy going to the gym. The app allows users to:
- Log and track training sessions (workouts).
- Get suggested exercises tailored to their goals.
- View guidance and instructions on how to perform exercises safely and effectively.

Summary
-------
This repository is a Razor Pages web application targeting .NET 8 (project name: `PapisPowerPracticeMvc`). The README below documents setup, run, and troubleshooting steps, plus a short analysis and assumptions because repository files were not inspected directly during generation.

Assumptions from workspace context
- Project targets .NET 8.
- Source repository: `https://github.com/onni82/PapisPowerPracticeMvc` (branch `master`).

Prerequisites
-------------
- .NET 8 SDK installed: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- Visual Studio 2022 (17.7+) with the ASP.NET and web development workload
- Git (for cloning and working with the repo)

Quickstart — Visual Studio
--------------------------
1. Open Visual Studio 2022.
2. Open the solution via __File > Open > Project/Solution__ or double-click the `.sln`.
3. Ensure the project is using .NET 8 in __Solution Explorer__ → right-click project → __Project > Properties__ → __Target Framework__ (should show `.NET 8`).
4. Set the startup project (right-click project → __Set as Startup Project__).
5. Start debugging: __Debug > Start Debugging__ (F5) or __Debug > Start Without Debugging__ (Ctrl+F5).

Quickstart — CLI
----------------
From repository root:
- Restore, build and run: `dotnet restore dotnet build -c Release dotnet run --project ./PapisPowerPracticeMvc`
- To publish: `dotnet publish -c Release -o ./publish`

If the project uses EF Core (optional)
-------------------------------------
If the app uses Entity Framework Core, run migrations and update the database:
`dotnet tool restore dotnet ef database update --project ./PapisPowerPracticeMvc`

(If `dotnet ef` is not available: `dotnet tool install --global dotnet-ef`)

Recommended project structure (Razor Pages)
------------------------------------------
- `PapisPowerPracticeMvc/` (project root)
  - `wwwroot/` — static files (css, js, images)
  - `appsettings.json` — configuration (connection strings, logging)
  - `Program.cs` — minimal hosting (service registration, routing)
  - `PapisPowerPracticeMvc.csproj` — project file (TargetFramework net8.0)

Configuration notes
-------------------
- appsettings.json: review logging and connection strings.
- Secrets: never commit production secrets. Use `dotnet user-secrets` or environment variables for sensitive data.
- Environment: set `ASPNETCORE_ENVIRONMENT` to `Development` locally for developer-friendly error pages.

Development tips
----------------
- Use `dotnet watch run` for fast edit-and-refresh during development.
- Use the Browser Link / Live Reload in ASP.NET Core if available for CSS/HTML refresh.
- In Visual Studio, use __Tools > Options__ → search for "Web" or "Browser" to adjust debugging and web server settings.

Common commands
---------------
- Build: `dotnet build`
- Run: `dotnet run`
- Test (if tests exist): `dotnet test`
- Restore: `dotnet restore`

Troubleshooting
---------------
- If the app fails to start, check the Output window in Visual Studio: __View > Output__ and select the appropriate pane.
- Check logs in `appsettings.Development.json` or console output.
- If port conflicts occur, change launch settings in `Properties/launchSettings.json` or via project debug profile in Visual Studio.

Contributing
------------
- Create a branch for your work: `git checkout -b feature/your-feature`
- Commit with clear messages, push to your fork or remote.
- Open a pull request against `master` branch.

License & Code of Conduct
-------------------------
- Add your license file (e.g., `LICENSE`) and a `CODE_OF_CONDUCT.md` if required.
- If you intend a specific open-source license, add it in the repository root.

Repository analysis summary
---------------------------
- The repo is a .NET 8 Razor Pages site. This README focuses on Razor Pages workflows (build/run, config, EF Core optional steps).
- If you want, I can:
  - Inspect specific files (`Program.cs`, `Pages/_Layout.cshtml`, `appsettings.json`) and update this README with concrete details (routes, authentication, DB).
  - Add sample launch profiles or CI workflow for GitHub Actions.

Next steps I can take (pick one)
- Create or update `README.md` in the repo with the contents above.
- Inspect specific project files and produce a targeted README with exact build/run instructions and detected dependencies.