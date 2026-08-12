# Implementation Plan: Ingredient Catalog and Keyboard Menu

**Branch**: `001-ingredient-catalog` | **Date**: 2026-08-11 | **Spec**: [spec.md](spec.md)

**Input**: Feature specification from `/specs/001-ingredient-catalog/spec.md` plus the interaction constraint that menus use arrow keys, Enter, and Delete.

**Note**: This template is filled in by the `/speckit-plan` command; its definition describes the execution workflow.

## Summary

Add the fixed 15-item ingredient catalog, per-ingredient pricing, selection state, and confirmed-order snapshots to the existing pizza console application. Replace typed menu choices with a reusable `Console.ReadKey(intercept: true)` menu: Up/Down moves a clamped focus, Enter activates a general menu option or adds the focused ingredient, and Delete removes the focused ingredient. Every redraw shows focus, selected state, feedback, and the current total without relying on color alone.

## Technical Context

**Language/Version**: C# 14 / .NET 10 (`net10.0`)

**Primary Dependencies**: .NET Base Class Library (`System.Console`, generic collections); no external runtime packages

**Storage**: N/A; catalog, active customization, and confirmed orders remain in memory for one process session

**Testing**: xUnit test project for catalog, pricing, selection, and navigation state; interactive quickstart scenarios for terminal rendering and physical key behavior

**Target Platform**: Interactive terminal on any platform supported by .NET 10; redirected standard input is not an interactive-menu target

**Project Type**: Single console application with one companion test project

**Performance Goals**: Redraw selections and the recalculated total within 1 second of each key press; all operations are bounded by the 15-item catalog

**Constraints**: Preserve top-level statement orchestration; use `decimal` for prices; prevent duplicate ingredients; keep selected output in catalog order; show focus/selection without color-only cues; do not edit generated `bin/` or `obj/` content

**Scale/Scope**: One local user, one active pizza customization, exactly 15 catalog entries, and an in-memory list of confirmed pizzas

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

The constitution is still an unratified placeholder and contains no enforceable project principles. Gate status: **PASS (provisional)** with no violations to justify.

Repository guidance adds these checks:

- **PASS**: The design retains one simple top-level console application and introduces only focused domain/menu types.
- **PASS**: The implementation uses only the .NET runtime and keeps all state in memory.
- **PASS**: Generated `bin/` and `obj/` content remains untouched.
- **PASS**: Automated tests cover deterministic state and pricing; terminal-specific rendering is validated through the quickstart.

Post-design re-check: **PASS**. The Phase 1 model and console contract do not introduce persistence, services, or additional production projects.

## Project Structure

### Documentation (this feature)

```text
specs/001-ingredient-catalog/
├── plan.md              # This file (/speckit-plan command output)
├── research.md          # Phase 0 output (/speckit-plan command)
├── data-model.md        # Phase 1 output (/speckit-plan command)
├── quickstart.md        # Phase 1 output (/speckit-plan command)
├── contracts/
│   └── console-menu.md  # Key, rendering, and screen behavior contract
└── tasks.md             # Phase 2 output (/speckit-tasks command - NOT created by /speckit-plan)
```

### Source Code (repository root)

```text
src/PizzaSales.ConsoleApp/
├── PizzaSales.ConsoleApp.csproj
├── Program.cs                    # Top-level order workflow and screen transitions
├── ConsoleMenu.cs                # Key mapping, focused-index state, and rendering
├── Ingredient.cs                 # Immutable catalog value
├── IngredientCatalog.cs          # Ordered fixed catalog
├── PizzaCustomization.cs         # Editable selections and computed total
└── PizzaOrder.cs                 # Confirmed immutable snapshot

tests/PizzaSales.ConsoleApp.Tests/
├── PizzaSales.ConsoleApp.Tests.csproj
├── ConsoleMenuStateTests.cs
├── IngredientCatalogTests.cs
├── PizzaCustomizationTests.cs
└── PizzaOrderTests.cs
```

**Structure Decision**: Keep `Program.cs` as the top-level application coordinator while moving deterministic catalog, pricing, menu-navigation state, and confirmed-order data into small sibling types. Add one xUnit project under `tests/` so domain behavior can be validated without trying to automate `Console.ReadKey` or cursor rendering.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|

No constitution violations require justification.
