# Data Model: Ingredient Catalog and Keyboard Menu

## Ingredient

An immutable catalog value available to every pizza customization.

| Field | Type | Rules |
|-------|------|-------|
| `MenuNumber` | `int` | Required; unique; inclusive range 1-15; also defines catalog order |
| `Name` | `string` | Required; nonblank; unique using ordinal case-insensitive comparison |
| `Price` | `decimal` | Required; greater than zero; fixed to the value in the feature specification |

Validation rules:

- The catalog contains exactly the 15 specified ingredients.
- Menu numbers are contiguous from 1 through 15 and list entries appear in that order.
- Prices are represented as `decimal` and rendered using US currency with two decimal places.
- Ingredient values do not change during an application session.

## Ingredient Catalog

The single read-only ordered collection used to render menus, resolve selection identifiers, calculate prices, and create confirmed snapshots.

| Field | Type | Rules |
|-------|------|-------|
| `Ingredients` | `IReadOnlyList<Ingredient>` | Exactly 15 immutable entries in `MenuNumber` order |

Relationships:

- One catalog supplies zero or more pizza customizations during a session.
- A customization can select zero to 15 catalog ingredients.
- No catalog administration or persistence is supported.

## Pizza Customization

Mutable state for the one pizza currently being built.

| Field | Type | Rules |
|-------|------|-------|
| `Size` | size option | One of small, medium, large, or extra large |
| `Shape` | shape option | Round or square |
| `StuffedCrust` | `bool` | `true` for stuffed crust; otherwise regular crust |
| `SelectedMenuNumbers` | set of `int` | Each value must resolve to the catalog; duplicates are impossible |
| `BasePrice` | derived `decimal` | Existing size price plus existing shape and crust charges |
| `IngredientPrice` | derived `decimal` | Sum of prices for selected catalog entries |
| `TotalPrice` | derived `decimal` | `BasePrice + IngredientPrice`; never incrementally stored |
| `CanConfirm` | derived `bool` | `true` only when at least one ingredient is selected |

Selection operations return explicit outcomes so the menu can display feedback:

| Operation | Success outcome | No-change outcome |
|-----------|-----------------|-------------------|
| Add focused ingredient | `Added` | `AlreadySelected` |
| Remove focused ingredient | `Removed` | `NotSelected` |

Invariants:

- A selected ingredient contributes its price exactly once.
- Selected ingredients are displayed and copied in catalog order, independent of selection order.
- Failed add/remove operations do not change selections or totals.
- Removing the last ingredient is valid, but confirmation remains blocked.

### Customization State Transitions

```text
ChoosingBaseOptions -> EditingIngredients
EditingIngredients -- Space ingredient --> EditingIngredients (toggle selection)
EditingIngredients -- Delete ingredient --> EditingIngredients (remove or no change)
EditingIngredients -- Continue, empty --> EditingIngredients (validation message)
EditingIngredients -- Continue, nonempty --> AwaitingConfirmation
AwaitingConfirmation -- Back --> EditingIngredients
AwaitingConfirmation -- Confirm --> Confirmed
ChoosingBaseOptions | EditingIngredients | AwaitingConfirmation -- Cancel --> Cancelled
```

`Confirmed` and `Cancelled` are terminal states for the active customization. Cancellation does not mutate previously confirmed pizzas.

## Confirmed Pizza

An immutable snapshot stored in the current order after confirmation.

| Field | Type | Rules |
|-------|------|-------|
| `Size` | size option | Copied from the active customization |
| `Shape` | shape option | Copied from the active customization |
| `StuffedCrust` | `bool` | Copied from the active customization |
| `Ingredients` | `IReadOnlyList<Ingredient>` | Nonempty copy in catalog order; callers cannot mutate it |
| `Price` | `decimal` | Exact total displayed at confirmation |

Relationships:

- A session order contains zero or more confirmed pizzas.
- Review reads only confirmed snapshots and sums their stored prices.
- An active customization has no relationship to the order until confirmation succeeds.

## Menu State

Transient presentation state shared by all selectable screens.

| Field | Type | Rules |
|-------|------|-------|
| `Items` | ordered menu items | At least one item; order remains stable while the screen is active |
| `FocusedIndex` | `int` | Inclusive range 0 through `Items.Count - 1` |
| `Feedback` | nullable `string` | Most recent operation result; does not encode domain state |
| `ScreenKind` | menu kind | Determines what Enter and Delete mean |

Navigation rules:

- Up decrements `FocusedIndex` and clamps at zero.
- Down increments `FocusedIndex` and clamps at the final item.
- Home and End move to the first and final item.
- Page Up and Page Down move by the visible viewport size and clamp at a boundary.
- Space toggles the focused ingredient selection.
- Enter activates actions and leaves ingredient state unchanged.
- Delete requests removal only when an ingredient row is focused.
- Unsupported keys and inapplicable actions preserve all domain and navigation state except feedback.
