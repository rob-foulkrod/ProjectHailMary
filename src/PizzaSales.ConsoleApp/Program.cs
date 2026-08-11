

Console.WriteLine("Welcome to the Pizza Ordering System!");

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
			Add();
			break;
		case "2":
			Review();
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

static void Add()
{
	Console.WriteLine("Add pizza selected.");
	// TODO: Add pizza ordering logic.
}

static void Review()
{
	Console.WriteLine("Review orders selected.");
	// TODO: Add order review logic.
}

static void Quit()
{
	Console.WriteLine("Thank you for using the Pizza Ordering System!");
}

