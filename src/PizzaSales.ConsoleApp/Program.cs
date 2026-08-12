using PizzaSales.ConsoleApp;

if (Console.IsInputRedirected || Console.IsOutputRedirected)
{
    Console.Error.WriteLine(ConsoleMenu.TerminalRequiredMessage);
    Environment.ExitCode = 1;
    return;
}

try
{
    RunOrderingApplication();
}
catch (InvalidOperationException exception)
    when (exception.Message == ConsoleMenu.TerminalRequiredMessage)
{
    Console.Error.WriteLine(ConsoleMenu.TerminalRequiredMessage);
    Environment.ExitCode = 1;
}

static void RunOrderingApplication()
{
    Console.WriteLine("Welcome to the Pizza Ordering System!");

    var orders = new List<PizzaOrder>();
    while (true)
    {
        string choice = ConsoleMenu.ShowMenu(
            "Please select an option:",
            ["Add pizza", "Review order", "Quit"]);

        switch (choice)
        {
            case "Add pizza":
                AddPizza(orders);
                break;
            case "Review order":
                ReviewOrders(orders);
                break;
            case "Quit":
                Console.Clear();
                Console.WriteLine("Thank you for using the Pizza Ordering System!");
                return;
        }
    }
}

static void AddPizza(List<PizzaOrder> orders)
{
    string size = ConsoleMenu.ShowMenu(
        "Choose a size:",
        ["small", "medium", "large", "extra large", "Cancel pizza"]);

    if (size == "Cancel pizza")
    {
        ShowStatus("Pizza cancelled.");
        return;
    }

    string shape = ConsoleMenu.ShowMenu(
        "Choose a shape:",
        ["round", "square", "Cancel pizza"]);

    if (shape == "Cancel pizza")
    {
        ShowStatus("Pizza cancelled.");
        return;
    }

    string crustChoice = ConsoleMenu.ShowMenu(
        "Choose crust:",
        ["regular crust", "stuffed crust", "Cancel pizza"]);

    if (crustChoice == "Cancel pizza")
    {
        ShowStatus("Pizza cancelled.");
        return;
    }

    bool stuffedCrust = string.Equals(crustChoice, "stuffed crust", StringComparison.OrdinalIgnoreCase);
    decimal basePrice = GetBasePrice(size, shape, stuffedCrust);
    var customization = new PizzaCustomization(basePrice, size, shape, stuffedCrust);

    while (true)
    {
        bool readyForConfirmation = ConsoleMenu.EditIngredients(
            size,
            shape,
            stuffedCrust,
            customization);

        if (!readyForConfirmation)
        {
            ShowStatus("Pizza cancelled.");
            return;
        }

        string confirmationChoice = ConsoleMenu.ShowMenu(
            BuildConfirmationHeader(customization),
            ["Confirm pizza", "Back to ingredients", "Cancel pizza"]);

        if (confirmationChoice == "Back to ingredients")
        {
            continue;
        }

        if (confirmationChoice == "Cancel pizza")
        {
            ShowStatus("Pizza cancelled.");
            return;
        }

        orders.Add(PizzaOrder.FromCustomization(customization));
        ShowStatus("Pizza added to your order.");
        return;
    }
}

static void ReviewOrders(List<PizzaOrder> orders)
{
    var headerLines = new List<string> { "Review order" };

    if (orders.Count == 0)
    {
        headerLines.Add("No pizzas have been added yet.");
        headerLines.Add($"Total order value: {CurrencyFormatter.Format(0m)}");
        ConsoleMenu.ShowMenu(headerLines, ["Back"]);
        return;
    }

    decimal total = 0m;
    for (int i = 0; i < orders.Count; i++)
    {
        PizzaOrder order = orders[i];
        headerLines.Add($"{i + 1}. {order}");
        total += order.Price;
    }

    headerLines.Add($"Total order value: {CurrencyFormatter.Format(total)}");
    ConsoleMenu.ShowMenu(headerLines, ["Back"]);
}

static IReadOnlyList<string> BuildConfirmationHeader(PizzaCustomization customization)
{
    string crust = customization.StuffedCrust ? "stuffed crust" : "regular crust";
    var lines = new List<string>
    {
        "Confirm pizza",
        $"{customization.Size} {customization.Shape} pizza ({crust})",
        "Ingredients:"
    };

    lines.AddRange(
        customization.SelectedIngredientDetails.Select(
            ingredient => $"  {ingredient.MenuNumber}. {ingredient.Name} - {CurrencyFormatter.Format(ingredient.Price)}"));
    lines.Add($"Total: {CurrencyFormatter.Format(customization.TotalPrice)}");
    return lines;
}

static void ShowStatus(string message)
{
    Console.Clear();
    Console.WriteLine(message);
    Console.WriteLine("Press any key to continue.");
    Console.ReadKey(intercept: true);
}

static decimal GetBasePrice(string size, string shape, bool stuffedCrust)
{
    decimal basePrice = size.ToLowerInvariant() switch
    {
        "small" => 8m,
        "medium" => 10m,
        "large" => 12m,
        "extra large" => 15m,
        _ => 8m
    };

    decimal shapePrice = string.Equals(shape, "square", StringComparison.OrdinalIgnoreCase) ? 1.5m : 0m;
    decimal crustPrice = stuffedCrust ? 2.5m : 0m;
    return basePrice + shapePrice + crustPrice;
}
