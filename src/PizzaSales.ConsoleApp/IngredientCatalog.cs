namespace PizzaSales.ConsoleApp;

public static class IngredientCatalog
{
    public static IReadOnlyList<Ingredient> All { get; } = Array.AsReadOnly(
    new Ingredient[]
    {
        new Ingredient(1, "Cheese", 1.00m),
        new Ingredient(2, "Pepperoni", 1.50m),
        new Ingredient(3, "Mushrooms", 0.75m),
        new Ingredient(4, "Olives", 0.75m),
        new Ingredient(5, "Sausage", 1.75m),
        new Ingredient(6, "Onions", 0.50m),
        new Ingredient(7, "Green peppers", 0.75m),
        new Ingredient(8, "Bacon", 1.75m),
        new Ingredient(9, "Ham", 1.50m),
        new Ingredient(10, "Pineapple", 1.00m),
        new Ingredient(11, "Tomatoes", 0.75m),
        new Ingredient(12, "Spinach", 0.75m),
        new Ingredient(13, "Jalapenos", 0.75m),
        new Ingredient(14, "Chicken", 2.00m),
        new Ingredient(15, "Anchovies", 1.50m)
    });

    public static bool TryGetByMenuNumber(int menuNumber, out Ingredient ingredient)
    {
        Ingredient match = All.FirstOrDefault(item => item.MenuNumber == menuNumber);
        if (match.Name is not null)
        {
            ingredient = match;
            return true;
        }

        ingredient = default;
        return false;
    }
}
