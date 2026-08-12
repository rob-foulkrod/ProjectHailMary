# Research: Ingredient Catalog and Keyboard Menu

## Keyboard Input

**Decision**: Read one physical key at a time with `Console.ReadKey(intercept: true)`. Map `UpArrow`, `DownArrow`, `Enter`, and `Delete` from `ConsoleKey`; keep key-to-action mapping separate from rendering.

**Rationale**: `ReadKey` exposes navigation and editing keys directly, while interception prevents escape sequences or key characters from being echoed into the menu. A pure key-to-action mapping can be unit tested without automating a terminal.

**Alternatives considered**:

- `Console.ReadLine`: rejected because line-oriented text input cannot provide immediate arrow-key navigation.
- A third-party console UI library: rejected because the fixed menus need only a small subset of behavior and the repository favors a simple, dependency-light classroom application.

## Menu Control Semantics

**Decision**: Every selectable screen uses Up/Down, Home/End, and Page Up/Page Down to move focus, with Enter activating the focused action. The ingredient screen uses Space to toggle the focused checkbox and Delete to remove it. Enter on an ingredient leaves state unchanged and explains that Space toggles the checkbox. Delete on a non-removable item leaves state unchanged and displays a concise explanation. Focus clamps at the first and last item rather than wrapping.

**Rationale**: One navigation model makes the main, size, shape, crust, ingredient, and confirmation screens predictable. Restricting Delete to removal contexts avoids accidental cancellation or deletion of confirmed orders, which is outside this feature. Clamping makes the bounds of the 15-item catalog apparent.

**Alternatives considered**:

- Enter toggles ingredient selection: rejected because it obscures the explicit Enter-to-add and Delete-to-remove requirement and cannot explain duplicate-add attempts.
- Wrapping focus: rejected because jumping between the first and last entries is less predictable in the longer ingredient catalog.
- Escape as the only cancellation path: rejected because cancellation must remain discoverable; each applicable screen has an explicit Cancel option activated with Enter.

## Rendering and Feedback

**Decision**: Redraw the complete menu frame after every handled key. Use ASCII markers (`>` for focus and `[x]`/`[ ]` for selected state), visible ingredient numbers, names, US-dollar prices, the current total, a one-line key hint, and the most recent feedback message. Color may enhance the display but is never the only signal.

**Rationale**: Full redraws are simple and reliable for a catalog of 15 entries. Text markers preserve meaning in terminals with limited color support and satisfy the requirement to distinguish selected ingredients. Immediate feedback makes duplicate add, invalid remove, and unsupported-key behavior observable.

**Alternatives considered**:

- Color-only highlighting: rejected because selected and focused states would be ambiguous in monochrome or inaccessible terminals.
- Incrementally rewriting individual rows: rejected because cursor bookkeeping adds complexity without a meaningful performance benefit at this scale.
- Treating catalog menu numbers as typed commands: rejected because numbers remain stable visual identifiers while arrows are the required selection mechanism.

## Interactive Terminal Boundary

**Decision**: Treat an interactive terminal as a runtime prerequisite. Before entering a key-driven menu, detect redirected input or output and stop with a clear message rather than calling cursor or key APIs in an unsupported context.

**Rationale**: `Console.ReadKey` and screen-clearing behavior are terminal-oriented and can fail or behave unexpectedly when streams are redirected. Explicit failure is deterministic and keeps automated tests focused on state and calculations.

**Alternatives considered**:

- A second line-input fallback protocol: rejected because it would create an unrequested interaction contract and duplicate all menu behavior.
- Silently continuing under redirection: rejected because users would receive incomplete rendering or an exception without an actionable explanation.

## Catalog and Selection State

**Decision**: Represent each ingredient as an immutable value with stable menu number, unique name, and `decimal` price. Keep the catalog in one immutable ordered collection. Track active selections by menu number in a set, and derive displayed/confirmed ingredients by filtering catalog order.

**Rationale**: A set enforces at-most-once selection, while catalog-order projection provides deterministic display and review. `decimal` represents currency exactly for the specified prices and avoids binary floating-point drift.

**Alternatives considered**:

- Store free-form ingredient strings: rejected because names cannot reliably enforce catalog membership, uniqueness, or pricing.
- Store a mutable total and increment/decrement it: rejected because recomputing from fixed charges and current selections avoids drift and stale totals.
- Use a dictionary as the public catalog shape: rejected because explicit ordered entries better preserve stable menu order.

## Confirmation and Order Review

**Decision**: Compute the active total from size, shape, crust, and selected ingredient prices. On confirmation, copy the selected ingredients and total into an immutable pizza-order snapshot; cancellation discards only the active customization.

**Rationale**: A snapshot ensures later review matches exactly what the customer approved and prevents subsequent editing state from changing confirmed orders.

**Alternatives considered**:

- Store a reference to mutable customization state: rejected because later edits could alter a confirmed order.
- Recalculate confirmed totals during review: rejected because review must retain the confirmed ingredient list and total unchanged.

## Validation Strategy

**Decision**: Add one xUnit test project for catalog invariants, add/remove outcomes, price calculation, confirmation snapshots, and clamped navigation state. Validate physical keys, redraws, and complete customer flows with the interactive quickstart.

**Rationale**: Domain and navigation state are deterministic and inexpensive to automate. Actual `Console.ReadKey` and terminal rendering are environment-dependent, so a small manual acceptance layer is more reliable than a heavyweight terminal harness for this classroom app.

**Alternatives considered**:

- Manual testing only: rejected because duplicate prevention and pricing have compact, high-value regression tests.
- End-to-end terminal automation: rejected because it adds platform-specific infrastructure disproportionate to a single-process demonstration app.