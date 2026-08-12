# Tasks: Ingredient Catalog and Keyboard Menu

**Input**: Design documents from `/specs/001-ingredient-catalog/`

**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create the minimal project structure for the console app and the first test project.

- [X] T001 Create the feature structure under `src/PizzaSales.ConsoleApp/` and `tests/PizzaSales.ConsoleApp.Tests/` per the implementation plan
- [X] T002 Add the .NET 10 console app and xUnit test project scaffold in `src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj` and `tests/PizzaSales.ConsoleApp.Tests/PizzaSales.ConsoleApp.Tests.csproj`
- [X] T003 [P] Configure the solution-level test project references and restore dependencies for the console app and test project

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core data model and shared catalog behavior that every user story depends on.

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel.

- [X] T004 Define the immutable `Ingredient` model in `src/PizzaSales.ConsoleApp/Ingredient.cs`
- [X] T005 Implement the fixed 15-item catalog and catalog-order lookup in `src/PizzaSales.ConsoleApp/IngredientCatalog.cs`
- [X] T006 Implement pizza customization state, selected ingredient tracking, and total calculation in `src/PizzaSales.ConsoleApp/PizzaCustomization.cs`
- [X] T007 Implement the confirmed pizza snapshot in `src/PizzaSales.ConsoleApp/PizzaOrder.cs`
- [X] T008 [P] Add a shared menu-state abstraction for focus tracking and feedback in `src/PizzaSales.ConsoleApp/ConsoleMenu.cs`
- [X] T009 [P] Add validation helpers for currency formatting and catalog invariants used across the app in `src/PizzaSales.ConsoleApp/Program.cs` and supporting model files

---

## Phase 3: User Story 1 - View the Ingredient Catalog (Priority: P1) 🎯 MVP

**Goal**: Display every ingredient in stable order with its price and selected indicator.

**Independent Test**: Start a new pizza and open the ingredient menu; verify exactly 15 ingredients appear in the defined order and each has the correct dollar price.

### Tests for User Story 1

- [X] T010 [P] [US1] Add catalog-order and pricing tests in `tests/PizzaSales.ConsoleApp.Tests/IngredientCatalogTests.cs`
- [X] T011 [P] [US1] Add selected-state and total tests in `tests/PizzaSales.ConsoleApp.Tests/PizzaCustomizationTests.cs`

### Implementation for User Story 1

- [X] T012 [P] [US1] Render the ingredient screen with menu numbers, names, prices, and selected markers in `src/PizzaSales.ConsoleApp/Program.cs`
- [X] T013 [US1] Wire `Console.ReadKey(intercept: true)` processing for Up/Down focus movement in `src/PizzaSales.ConsoleApp/ConsoleMenu.cs`
- [X] T014 [US1] Show a visible focus marker and selected-state markers without relying on color alone in `src/PizzaSales.ConsoleApp/ConsoleMenu.cs`
- [X] T015 [US1] Ensure the ingredient menu displays all 15 entries in order and keeps the selected state synchronized with the active customization in `src/PizzaSales.ConsoleApp/Program.cs`

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently.

---

## Phase 4: User Story 2 - Add Ingredients and See the Total (Priority: P1)

**Goal**: Add selected ingredients from the keyboard menu and keep the running total accurate.

**Independent Test**: Record the current total, add cheese and pepperoni, and confirm both are selected and the total increases by exactly $2.50.

### Tests for User Story 2

- [X] T016 [P] [US2] Add duplicate-prevention tests in `tests/PizzaSales.ConsoleApp.Tests/PizzaCustomizationTests.cs`
- [X] T017 [P] [US2] Add total-calculation validation tests in `tests/PizzaSales.ConsoleApp.Tests/PizzaCustomizationTests.cs`

### Implementation for User Story 2

- [X] T018 [US2] Implement Enter-to-add behavior for focused ingredients in `src/PizzaSales.ConsoleApp/ConsoleMenu.cs`
- [X] T019 [US2] Enforce at-most-once ingredient selection and preserve the total on duplicate attempts in `src/PizzaSales.ConsoleApp/PizzaCustomization.cs`
- [X] T020 [US2] Recalculate and display the ingredient subtotal immediately after each valid add in `src/PizzaSales.ConsoleApp/Program.cs`
- [X] T021 [US2] Add a corrective feedback message for already-selected ingredients in `src/PizzaSales.ConsoleApp/Program.cs`

**Checkpoint**: At this point, User Stories 1 and 2 should both work independently.

---

## Phase 5: User Story 3 - Remove Ingredients and Correct the Total (Priority: P2)

**Goal**: Remove selected ingredients with Delete and keep the total synchronized.

**Independent Test**: Add cheese and sausage, remove cheese, and verify the displayed total decreases by exactly $1.00.

### Tests for User Story 3

- [X] T022 [P] [US3] Add remove-selected and no-op-remove tests in `tests/PizzaSales.ConsoleApp.Tests/PizzaCustomizationTests.cs`
- [X] T023 [P] [US3] Add removal feedback and total-consistency regression tests in `tests/PizzaSales.ConsoleApp.Tests/PizzaCustomizationTests.cs`

### Implementation for User Story 3

- [X] T024 [US3] Implement Delete-to-remove behavior for the focused ingredient in `src/PizzaSales.ConsoleApp/ConsoleMenu.cs`
- [X] T025 [US3] Ensure removing a selected ingredient updates the running total and preserves other selected ingredients in `src/PizzaSales.ConsoleApp/PizzaCustomization.cs`
- [X] T026 [US3] Display a clear message when Delete is pressed on an unselected ingredient and do not change totals in `src/PizzaSales.ConsoleApp/Program.cs`
- [X] T027 [US3] Allow removing the final ingredient while keeping the customization editable until confirmation in `src/PizzaSales.ConsoleApp/Program.cs`

**Checkpoint**: At this point, the ingredient editor should handle add-and-remove semantics safely.

---

## Phase 6: User Story 4 - Confirm the Calculated Pizza (Priority: P2)

**Goal**: Confirm the customization only when valid, preserve approved totals, and keep review exact.

**Independent Test**: Select two ingredients, note the displayed total, confirm, and verify the review shows the same two ingredients and total.

### Tests for User Story 4

- [X] T028 [P] [US4] Add confirmation and cancellation tests in `tests/PizzaSales.ConsoleApp.Tests/PizzaOrderTests.cs`
- [X] T029 [P] [US4] Add review-order snapshot tests in `tests/PizzaSales.ConsoleApp.Tests/PizzaOrderTests.cs`

### Implementation for User Story 4

- [X] T030 [US4] Add Continue-to-confirmation and cancel actions to the ingredient menu in `src/PizzaSales.ConsoleApp/Program.cs`
- [X] T031 [US4] Block confirmation until at least one ingredient is selected in `src/PizzaSales.ConsoleApp/PizzaCustomization.cs`
- [X] T032 [US4] Copy the selected ingredients and total into an immutable order snapshot when confirmed in `src/PizzaSales.ConsoleApp/PizzaOrder.cs`
- [X] T033 [US4] Render the confirmation screen with the selected ingredients and confirmed total in `src/PizzaSales.ConsoleApp/Program.cs`
- [X] T034 [US4] Implement order review so confirmed pizzas are displayed unchanged and cancellation skips mutation in `src/PizzaSales.ConsoleApp/Program.cs`

**Checkpoint**: At this point, the full pizza-selection flow is functional and independently testable.

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Final integration cleanup, manual validation, and UX quality pass.

- [X] T035 [P] Run the quickstart validation scenarios from `specs/001-ingredient-catalog/quickstart.md` against the console app
- [X] T036 [P] Review `Program.cs` for single-responsibility boundaries and reduce duplicate menu rendering logic
- [X] T037 [P] Validate that `Console.ReadKey` logic handles redirected input/output gracefully and user-facing messages remain clear
- [X] T038 Finalize all ingredient pricing and menu wording to match the product specification in `src/PizzaSales.ConsoleApp/Program.cs` and `src/PizzaSales.ConsoleApp/IngredientCatalog.cs`
- [X] T039 [P] Run `dotnet build` and `dotnet test` for the console app and test project as the final verification gate

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion and blocks all user stories
- **User Story Phases (3-6)**: All depend on the foundational phase
- **Polish (Phase 7)**: Depends on all desired stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Phase 2; no dependencies on other stories
- **User Story 2 (P1)**: Can start after Phase 2; should be independently testable once basic catalog and selection behavior exist
- **User Story 3 (P2)**: Can start after Phase 2; depends on the ingredient selection state from the foundational work
- **User Story 4 (P2)**: Can start after Phase 2 and is built on the same selection model

### Parallel Opportunities

- Setup tasks T001-T003 can run in parallel once the structure is agreed
- Foundational tasks T004-T009 can proceed in parallel across model and catalog files
- Story tests for each user story can be authored in parallel with the story implementation
- Final validation tasks T035-T039 can run in parallel as a last pass when the stories are complete

---

## Parallel Example: User Story 1

```bash
# Example parallel work for User Story 1
# Catalog and state model tasks
Task: "Implement Ingredient model in src/PizzaSales.ConsoleApp/Ingredient.cs"
Task: "Implement fixed catalog in src/PizzaSales.ConsoleApp/IngredientCatalog.cs"

# UI + validation tasks
Task: "Render ingredient menu in src/PizzaSales.ConsoleApp/Program.cs"
Task: "Add console key navigation in src/PizzaSales.ConsoleApp/ConsoleMenu.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate the ingredient catalog independently
5. Iterate to add selection and confirmation behavior

### Incremental Delivery

1. Setup + foundational model work
2. Add the ingredient catalog and view
3. Add add/remove semantics and running totals
4. Add confirmation and review behavior
5. Run final validation and polish

### Parallel Team Strategy

With multiple developers:

1. Shared setup and foundational model work together
2. One developer handles the ingredient display and menu navigation
3. One developer handles add/remove logic and totals
4. One developer handles confirmation, review, and final validation
