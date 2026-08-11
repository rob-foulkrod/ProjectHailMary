# Project Guidelines

## Purpose

- This repository is a GH-300 GitHub Copilot classroom and demonstration workspace.
- The pizza-ordering application is intentionally incomplete starter code for exercises, not a production system.
- Use [labs.md](../labs.md) for the exercise catalog and [programmgmt/mgmt.md](../programmgmt/mgmt.md) for the delivery-management demo scenario.

## Key Surfaces

- `src/PizzaSales.ConsoleApp/` is a .NET 10 console application using top-level statements. Preserve its simple structure unless an exercise requires broader design changes.
- `milestone-control-room.html` is a self-contained static delivery dashboard. GitHub Issues and Milestones are the source of truth for its snapshot data.
- Follow `.github/skills/refresh-milestone-dashboard/SKILL.md` when refreshing dashboard data. During a refresh, edit only the marked JavaScript data block and preserve the fixed reporting date unless the user requests a new date.

## Working Conventions

- Keep changes small, exercise-focused, and consistent with nearby code.
- Do not edit generated `bin/` or `obj/` content.
- Do not invent live GitHub status, blocker, or milestone facts; query GitHub when current data matters.
- Preserve the dashboard's single-file, dependency-free design.

## Build and Run

```powershell
dotnet build src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj
dotnet run --project src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj
```

There is currently no automated test project. The dashboard requires no build step and can be opened directly in a browser.