# Project Hail Mary

> A hands-on GitHub Copilot classroom workspace for building, learning, and shipping with confidence.

Project Hail Mary is a GH-300 demonstration repository that brings together a deliberately approachable pizza-ordering application, practical GitHub Copilot exercises, and a delivery-management dashboard. It is designed to make experimentation easy while keeping the path from idea to implementation visible.

## What is inside?

### Pizza Ordering System

`src/PizzaSales.ConsoleApp` is a .NET 10 console application that provides the starting point for a pizza-ordering experience:

- Menu options for adding and reviewing orders
- Friendly prompts and validation for menu selection
- A clear exit flow

The application is intentionally small and incomplete so learners can use GitHub Copilot to explore requirements, implementation, testing, and iteration.

### Copilot learning paths

[`labs.md`](labs.md) collects guided exercises covering:

- GitHub Copilot fundamentals
- Copilot in Visual Studio Code
- Copilot cloud agent
- Copilot CLI
- JavaScript and Python development
- Unit testing
- Custom instructions, MCP, code review, and modernization

### Delivery-management demo

The [`programmgmt`](programmgmt) folder contains:

- [`mgmt.md`](programmgmt/mgmt.md): scenario notes, seed data, and an executive walkthrough
- [`milestone-control-room.html`](programmgmt/milestone-control-room.html): a dependency-free delivery dashboard that can be opened directly in a browser

The dashboard uses GitHub Issues and Milestones as its source of truth for snapshot data.

## Quick start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows, macOS, or Linux

### Build

```powershell
dotnet build src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj
```

### Run

```powershell
dotnet run --project src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj
```

### Open the dashboard

Open `programmgmt/milestone-control-room.html` directly in a browser. No server, package installation, or build step is required.

## Repository layout

```text
.
├── .github/                         Copilot instructions and reusable skills
├── programmgmt/                     Delivery-management demo assets
├── src/PizzaSales.ConsoleApp/       .NET 10 console application
├── labs.md                          GH-300 exercise catalog
└── README.md                        Project overview and quick start
```

## Learning by doing

This repository is intentionally optimized for small, focused changes. Start with the console app, choose an exercise from `labs.md`, and use Copilot to help investigate, implement, test, and explain each step. The goal is not just to finish the code—it is to build durable engineering habits along the way.

## License

This project is available under the [MIT License](LICENSE).
