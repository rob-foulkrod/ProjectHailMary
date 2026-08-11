

Console.WriteLine("Welcome to the Pizza Ordering System!");

var orders = new List<PizzaOrder>();
bool isRunning = true;

while (isRunning)
{
	Console.WriteLine("\nPlease select an option:");
	Console.WriteLine("1. Add");
	Console.WriteLine("2. Review");
	Console.WriteLine("3. Quit");
	Console.Write("Choice: ");

	switch (Console.ReadLine())
	{
		case "1":
			Add(orders);
			break;
		case "2":
			Review(orders);
			break;
		case "3":
			Quit();
			isRunning = false;
			break;
		default:
			Console.WriteLine("Invalid option. Please enter 1, 2, or 3.");
			break;
	}
}

static void Add(List<PizzaOrder> orders)
{
	Console.WriteLine("Add pizza selected.");

	Console.WriteLine("Enter the ingredients for the pizza (comma-separated):");
	List<string> ingredients;
	while (true)
	{
		string? ingredientInput = Console.ReadLine();
		ingredients = ParseIngredients(ingredientInput);
		if (ingredients.Count > 0)
		{
			break;
		}

		Console.WriteLine("Please enter at least one ingredient.");
	}

	string size = PromptForSelection(
		"Choose a size:",
		new[] { "small", "medium", "large", "extra large" });

	string shape = PromptForSelection(
		"Choose a shape:",
		new[] { "round", "square" });

	bool stuffedCrust = PromptForYesNo("Would you like stuffed crust? (y/n): ");

	decimal price = EstimatePrice(ingredients, size, shape, stuffedCrust);
	Console.WriteLine($"Estimated price: {price:C}");

	if (!PromptForYesNo("Confirm this pizza? (y/n): "))
	{
		Console.WriteLine("Pizza cancelled.");
		return;
	}

	orders.Add(new PizzaOrder(size, shape, stuffedCrust, ingredients, price));
	Console.WriteLine("Pizza added to your order.");
}

static void Review(List<PizzaOrder> orders)
{
	Console.WriteLine("Review orders selected.");
	if (orders.Count == 0)
	{
		Console.WriteLine("No pizzas have been added yet.");
		return;
	}

	decimal total = 0m;
	for (int i = 0; i < orders.Count; i++)
	{
		PizzaOrder order = orders[i];
		Console.WriteLine($"{i + 1}. {order}");
		total += order.Price;
	}

	Console.WriteLine($"Total order value: {total:C}");
}

static void Quit()
{
	Console.WriteLine("Thank you for using the Pizza Ordering System!");
}

static List<string> ParseIngredients(string? ingredientInput)
{
	if (string.IsNullOrWhiteSpace(ingredientInput))
	{
		return new List<string>();
	}

	return ingredientInput
		.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
		.Select(ingredient => ingredient.Trim())
		.Where(ingredient => ingredient.Length > 0)
		.ToList();
}

static string PromptForSelection(string prompt, IReadOnlyList<string> options)
{
	while (true)
	{
		Console.WriteLine(prompt);
		for (int i = 0; i < options.Count; i++)
		{
			Console.WriteLine($"{i + 1}. {options[i]}");
		}

		Console.Write("Choice: ");
		string? response = Console.ReadLine();
		if (int.TryParse(response, out int index) && index >= 1 && index <= options.Count)
		{
			return options[index - 1];
		}

		string normalized = response?.Trim().ToLowerInvariant() ?? string.Empty;
		for (int i = 0; i < options.Count; i++)
		{
			if (string.Equals(options[i], normalized, StringComparison.OrdinalIgnoreCase))
			{
				return options[i];
			}
		}

		Console.WriteLine("Invalid selection. Please try again.");
	}
}

static bool PromptForYesNo(string prompt)
{
	while (true)
	{
		Console.Write(prompt);
		string? response = Console.ReadLine();
		if (string.Equals(response, "y", StringComparison.OrdinalIgnoreCase) ||
			string.Equals(response, "yes", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		if (string.Equals(response, "n", StringComparison.OrdinalIgnoreCase) ||
			string.Equals(response, "no", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}

		Console.WriteLine("Please enter y or n.");
	}
}

static decimal EstimatePrice(IReadOnlyList<string> ingredients, string size, string shape, bool stuffedCrust)
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
	decimal ingredientPrice = ingredients.Count * 0.75m;

	return basePrice + shapePrice + crustPrice + ingredientPrice;
}

sealed class PizzaOrder
{
	public PizzaOrder(string size, string shape, bool stuffedCrust, IReadOnlyList<string> ingredients, decimal price)
	{
		Size = size;
		Shape = shape;
		StuffedCrust = stuffedCrust;
		Ingredients = ingredients;
		Price = price;
	}

	public string Size { get; }
	public string Shape { get; }
	public bool StuffedCrust { get; }
	public IReadOnlyList<string> Ingredients { get; }
	public decimal Price { get; }

	public override string ToString()
	{
		string crustDescription = StuffedCrust ? "stuffed crust" : "regular crust";
		return $"{Size} {Shape} pizza with {string.Join(", ", Ingredients)} ({crustDescription}) - {Price:C}";
	}
}
