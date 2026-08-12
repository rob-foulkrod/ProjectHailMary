# Quickstart: Validate the Ingredient Catalog and Keyboard Menu

## Prerequisites

- .NET 10 SDK
- An interactive terminal with standard input and output attached
- Repository root as the working directory

## Build and Test

```powershell
dotnet build src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj
dotnet test tests/PizzaSales.ConsoleApp.Tests/PizzaSales.ConsoleApp.Tests.csproj
```

Expected result: both commands succeed, and tests verify catalog invariants, responsive viewport layout, line clipping, clamped menu navigation, checkbox toggling, removals, totals, and immutable confirmed snapshots.

## Run

```powershell
dotnet run --project src/PizzaSales.ConsoleApp/PizzaSales.ConsoleApp.csproj
```

Do not pipe input or output; the application should report that an interactive terminal is required when either stream is redirected.

## Scenario 1: Navigate Every Menu with Keys

1. On the main menu, use Up/Down and verify focus clamps at the first and last rows.
2. Focus Add pizza and press Enter.
3. Choose small, round, and regular crust using only Up/Down and Enter.
4. Verify the ingredient screen provides all 15 ingredients in the order defined by [spec.md](spec.md), followed by Continue to confirmation and Cancel pizza. If the terminal is short, use arrows or page keys and verify the visible range changes.
5. Verify the focused row uses `>`, selected rows use `[x]`, and unselected rows use `[ ]`.

Expected result: no typed number or line input is needed on any selectable screen, and menu boundaries do not wrap.

## Scenario 2: Toggle Ingredients

Starting with a small, round, regular-crust pizza whose base total is $8.00:

1. Focus Cheese and press Space. Verify Cheese becomes selected and the total is $9.00.
2. Press Space again without moving focus. Verify Cheese becomes unselected and the total returns to $8.00.
3. Press Space to reselect Cheese, move to Pepperoni, and press Space. Verify both ingredients are selected and the total is $10.50.
4. Press an unsupported key. Verify the corrective key hint appears and selections and total remain $10.50.

Expected result: Space behaves like a checkbox toggle, each selected ingredient contributes its catalog price once, and every key redraws the current state immediately.

## Scenario 3: Remove Ingredients and Block Empty Confirmation

Continue with Cheese and Pepperoni selected:

1. Move focus to Cheese and press Delete. Verify only Pepperoni remains and the total becomes $9.50.
2. Press Delete again. Verify the message says Cheese is not selected and the total remains $9.50.
3. Move to Pepperoni and press Delete. Verify no ingredients remain and the total returns to $8.00.
4. Focus Continue to confirmation and press Enter.

Expected result: the menu remains open and displays `Select at least one ingredient before continuing.`

## Scenario 4: Confirm and Review a Pizza

1. Toggle Cheese and Sausage on with Space for the small, round, regular-crust pizza.
2. Verify the ingredient-screen total is $10.75.
3. Activate Continue to confirmation.
4. Verify confirmation shows Cheese, Sausage, and $10.75; then activate Confirm pizza.
5. From the main menu, activate Review order.

Expected result: review shows exactly the confirmed ingredients and $10.75, and the order total is $10.75.

## Scenario 5: Return to Editing and Cancel

1. Start another pizza, choose its base options, and add any ingredient.
2. Continue to confirmation, then activate Back to ingredients.
3. Verify the prior selection and total are unchanged.
4. Activate Cancel pizza and return to Review order.

Expected result: the cancelled pizza does not appear, and the pizza confirmed in Scenario 4 remains unchanged.

## Scenario 6: Delete Outside the Ingredient Screen

1. Press Delete on the main, setup, confirmation, and review menus.
2. Verify each screen remains active and no order or customization data is removed.

Expected result: Delete removes only the focused selected ingredient while the ingredient editor is active.