# Console Menu Contract

## Runtime Boundary

The application requires an interactive console for key-driven menus. If standard input or output is redirected, it exits the interactive flow with a clear terminal-required message rather than attempting to read keys or reposition the display.

## Global Key Contract

| Key | Behavior |
|-----|----------|
| Up Arrow | Move focus to the previous item; remain on the first item at the upper boundary |
| Down Arrow | Move focus to the next item; remain on the final item at the lower boundary |
| Home / End | Move focus to the first / final item |
| Page Up / Page Down | Move focus by one visible page and clamp at the boundary |
| Space | Toggle the focused ingredient checkbox; otherwise explain that the row is not toggleable |
| Enter | Activate the focused action; on an ingredient row, explain that Space toggles the checkbox |
| Delete | Remove the focused ingredient on the ingredient screen; otherwise explain that nothing can be removed |
| Escape | Cancel the active ingredient customization; ignored by general menus |
| Any other key | Preserve state and display the available-key hint |

After every key, the active screen redraws within one second and shows any resulting feedback. Focus never wraps between the first and final item.

## Visual Contract

- `>` marks the focused row; every other row begins with a space.
- Ingredient rows show `[x]` when selected and `[ ]` when unselected.
- Focus and selection remain distinguishable without color.
- Ingredient rows retain their stable number, exact catalog name, and US-dollar price with two decimal places.
- The active pizza total appears on every ingredient-screen redraw.
- If terminal height requires scrolling, a visible range identifies the currently rendered rows.
- The final line states the available keys for the current screen.

Example ingredient rows:

```text
> [ ]  1. Cheese          $1.00
  [x]  2. Pepperoni       $1.50
```

Menu numbers are visual catalog identifiers, not typed commands.

## Main Menu

Items, in order:

1. Add pizza
2. Review order
3. Quit

Up/Down changes focus and Enter opens the focused action. Delete and unsupported keys do not change the order.

## Pizza Setup Menus

Size items appear as small, medium, large, extra large, and Cancel pizza. Shape items appear as round, square, and Cancel pizza. Crust items appear as regular crust, stuffed crust, and Cancel pizza.

Up/Down changes focus and Enter chooses the focused value. Choosing Cancel pizza discards the active customization and returns to the main menu without changing confirmed pizzas.

## Ingredient Menu

The screen lists the 15 ingredient rows in specification order, followed by Continue to confirmation and Cancel pizza.

### Space on an Ingredient

- If unselected, add it, keep focus on its row, recalculate the total, and display `{Name} added.`
- If selected, remove it, keep focus on its row, recalculate the total, and display `{Name} removed.`

### Enter on an Ingredient

- Preserve state and total and display `Use Space to toggle the focused checkbox.`

### Delete on an Ingredient

- If selected, remove it, keep focus on its row, recalculate the total, and display `{Name} removed.`
- If unselected, preserve state and total and display `{Name} is not selected.`

### Enter on an Action

- Continue to confirmation proceeds only when at least one ingredient is selected. Otherwise it displays `Select at least one ingredient before continuing.`
- Cancel pizza discards the active customization and returns to the main menu.
- Delete on either action preserves state and displays `Nothing can be removed from this row.`

## Confirmation Menu

The screen displays the chosen size, shape, crust, all selected ingredients in catalog order, and the same total last shown by the ingredient menu.

Items, in order:

1. Confirm pizza
2. Back to ingredients
3. Cancel pizza

Confirm pizza creates one immutable confirmed-pizza snapshot and returns to the main menu. Back to ingredients resumes the same customization. Cancel pizza discards it. Delete does not remove ingredients or confirmed pizzas on this screen.

## Review Screen

Review displays every confirmed pizza with its snapshotted size, shape, crust, ingredient names, and price, followed by the total order value. An Enter-activated Back item returns to the main menu. Reviewing never mutates an order.
