# Feature Specification: Ingredient Catalog and Pricing

**Feature Branch**: Not created (no branch hook configured)

**Created**: 2026-08-11

**Status**: Draft

**Input**: User description: "The pizza ordering application will offer a predefined catalog of 15
ingredients and prices. Ingredients will include cheese for $1.00, pepperoni for $1.50, mushrooms
for $0.75, olives for $0.75, and sausage for $1.75. Customers will use a menu system to view all
available ingredients. The menu will allow customers to add or remove ingredients from their pizza.
The application will automatically calculate and display the total price."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - View the Ingredient Catalog (Priority: P1)

As a customer building a pizza, I want to see every available ingredient and its price so that I
can make an informed choice before adding ingredients.

**Why this priority**: Customers cannot customize or verify a pizza price without first knowing the
available choices and their costs.

**Independent Test**: Start a new pizza and open the ingredient menu. Verify that exactly 15
ingredients appear in the defined order and that each has its correct dollar price.

**Acceptance Scenarios**:

1. **Given** a customer is building a pizza, **When** the ingredient menu is displayed, **Then** all
   15 catalog ingredients appear with a stable menu number, name, and price.
2. **Given** ingredients have already been selected, **When** the menu is displayed again, **Then**
   the customer can distinguish selected ingredients from ingredients still available to add.

---

### User Story 2 - Add Ingredients and See the Total (Priority: P1)

As a customer, I want to add catalog ingredients and immediately see the updated pizza total so that
I understand the cost of my choices.

**Why this priority**: Selecting ingredients and understanding their price impact is the feature's
primary customer value.

**Independent Test**: Record a pizza's total, add cheese and pepperoni, and verify that both are
selected and the displayed total increases by exactly $2.50.

**Acceptance Scenarios**:

1. **Given** cheese is not selected, **When** the customer adds cheese, **Then** cheese appears in
   the selected ingredients and the pizza total increases by $1.00.
2. **Given** cheese is already selected, **When** the customer attempts to add cheese again, **Then**
   the selection and total remain unchanged and the customer receives a clear explanation.
3. **Given** the customer has selected multiple ingredients, **When** any selection changes, **Then**
   the displayed total equals all current non-ingredient charges plus each selected ingredient price.

---

### User Story 3 - Remove Ingredients and Correct the Total (Priority: P2)

As a customer, I want to remove a selected ingredient and immediately see the corrected total so that
I can revise my pizza before confirming it.

**Why this priority**: Customers need a safe correction path, but it depends on ingredient selection
and pricing already being available.

**Independent Test**: Add cheese and sausage, remove cheese, and verify that sausage remains selected
and the displayed total decreases by exactly $1.00.

**Acceptance Scenarios**:

1. **Given** cheese and sausage are selected, **When** the customer removes cheese, **Then** only
   sausage remains and the pizza total decreases by $1.00.
2. **Given** olives are not selected, **When** the customer attempts to remove olives, **Then** the
   selected ingredients and total remain unchanged and the customer receives a clear explanation.
3. **Given** one ingredient remains selected, **When** the customer removes it, **Then** the selected
   list becomes empty, the ingredient portion of the total becomes $0.00, and confirmation remains
   unavailable until an ingredient is added.

---

### User Story 4 - Confirm the Calculated Pizza (Priority: P2)

As a customer, I want the confirmed and reviewed pizza to retain the ingredients and total I approved
so that I can trust the order summary.

**Why this priority**: A correct running total has value only if the confirmed order preserves it.

**Independent Test**: Select two ingredients, note the displayed total, confirm the pizza, and verify
that review shows the same two ingredients and total.

**Acceptance Scenarios**:

1. **Given** at least one ingredient is selected and the current total is displayed, **When** the
   customer confirms the pizza, **Then** the pizza is added with exactly those ingredients and total.
2. **Given** the customer cancels before confirmation, **When** the order is reviewed, **Then** no
   pizza or charge from the cancelled customization appears.

### Edge Cases

- A blank, non-numeric, or out-of-range menu choice leaves selections and total unchanged and allows
  another attempt.
- Adding an already selected ingredient never creates a duplicate or duplicate charge.
- Removing an ingredient that is not selected never changes the total.
- Removing and re-adding an ingredient repeatedly returns the same exact total without rounding drift.
- Ingredients with the same price remain separate menu choices and can be selected independently.
- Removing the final ingredient is allowed during editing, but a pizza with no ingredients cannot be
  confirmed.
- Cancelling customization leaves previously confirmed pizzas and their totals unchanged.

## Requirements *(mandatory)*

### Ingredient Catalog

The catalog MUST contain exactly these entries in this stable menu order:

| Menu Number | Ingredient | Price |
|-------------|------------|------:|
| 1 | Cheese | $1.00 |
| 2 | Pepperoni | $1.50 |
| 3 | Mushrooms | $0.75 |
| 4 | Olives | $0.75 |
| 5 | Sausage | $1.75 |
| 6 | Onions | $0.50 |
| 7 | Green peppers | $0.75 |
| 8 | Bacon | $1.75 |
| 9 | Ham | $1.50 |
| 10 | Pineapple | $1.00 |
| 11 | Tomatoes | $0.75 |
| 12 | Spinach | $0.75 |
| 13 | Jalapenos | $0.75 |
| 14 | Chicken | $2.00 |
| 15 | Anchovies | $1.50 |

### Functional Requirements

- **FR-001**: The system MUST provide an ingredient menu while a customer builds a pizza.
- **FR-002**: The ingredient menu MUST show exactly 15 ingredients, each with its stable menu number,
  name, and dollar price from the defined catalog.
- **FR-003**: The menu MUST identify which catalog ingredients are currently selected.
- **FR-004**: A customer MUST be able to add any unselected catalog ingredient to the current pizza.
- **FR-005**: Each catalog ingredient MUST appear at most once on a pizza and contribute its listed
  price at most once.
- **FR-006**: A customer MUST be able to remove any selected ingredient before confirming the pizza.
- **FR-007**: Adding, removing, or attempting an invalid operation MUST immediately redisplay the
  current selections and pizza total.
- **FR-008**: The pizza total MUST equal existing size, shape, and crust charges plus the sum of all
  currently selected ingredient prices.
- **FR-009**: Ingredient prices and pizza totals MUST be displayed in US dollars with two decimal
  places.
- **FR-010**: Blank, malformed, and out-of-range menu choices MUST display a corrective message,
  preserve the current customization, and allow another choice.
- **FR-011**: Attempts to add a selected ingredient or remove an unselected ingredient MUST explain
  why no change occurred and MUST NOT change the total.
- **FR-012**: The customer MUST be able to remove all ingredients while editing, but MUST select at
  least one ingredient before confirming the pizza.
- **FR-013**: Confirmation MUST show the selected ingredients and calculated total before adding the
  pizza to the order.
- **FR-014**: A confirmed pizza's ingredient list and total MUST appear unchanged during order review.
- **FR-015**: Cancelling an unconfirmed pizza MUST NOT change any previously confirmed order or total.

### Key Entities

- **Ingredient**: A catalog choice with a stable menu number, unique name, and fixed per-pizza price.
- **Ingredient Catalog**: The ordered collection of exactly 15 ingredients available for every pizza.
- **Pizza Customization**: The current pizza's existing size, shape, and crust choices together with
  its selected catalog ingredients and calculated total.
- **Confirmed Pizza**: An approved customization whose selected ingredients and total are retained for
  order review.

### Out of Scope

- Ingredient quantities, double portions, substitutions, and customer-created ingredients.
- Catalog administration or price changes during a running session.
- Taxes, discounts, coupons, delivery charges, and payment processing.
- Editing a pizza after it has been confirmed.
- Saving the catalog, customizations, or orders between application sessions.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Every catalog view shows all 15 ingredients with the specified names and prices, with no
  missing or duplicate menu numbers.
- **SC-002**: In all acceptance scenarios, each valid add or remove action produces the mathematically
  correct total and each invalid action leaves the total unchanged.
- **SC-003**: Customers see the updated selections and total within 1 second after entering a valid or
  invalid ingredient-menu choice.
- **SC-004**: A customer can add two ingredients, remove one, and identify the final selected ingredient
  and total in under 2 minutes without restarting the ordering flow.
- **SC-005**: At least 9 of 10 first-time users in a classroom usability check can view the catalog,
  customize a pizza, and identify its total without assistance.
- **SC-006**: Confirmed pizza reviews match the ingredients and totals shown at confirmation in 100% of
  acceptance-test cases.

## Assumptions

- Prices are in US dollars and are fixed for the duration of a session.
- The ten ingredients not explicitly named in the request use the common demo defaults listed in the
  Ingredient Catalog table.
- Existing size, shape, and crust options and charges remain unchanged and are included in the total.
- A new pizza starts with no selected ingredients; each catalog ingredient is an optional paid addition.
- Each ingredient has a quantity of one when selected; extra portions require a separate future feature.
- The existing add, confirm, review, cancel, and quit flows remain available.