namespace PizzaSales.ConsoleApp;

public sealed class PizzaOrder
{
    private PizzaOrder(
        string size,
        string shape,
        bool stuffedCrust,
        IReadOnlyList<Ingredient> ingredients,
        decimal price)
    {
        Size = size;
        Shape = shape;
        StuffedCrust = stuffedCrust;
        Ingredients = Array.AsReadOnly(ingredients.ToArray());
        Price = price;
    }

    public string Size { get; }
    public string Shape { get; }
    public bool StuffedCrust { get; }
    public IReadOnlyList<Ingredient> Ingredients { get; }
    public decimal Price { get; }

    public static PizzaOrder FromCustomization(PizzaCustomization customization)
    {
        ArgumentNullException.ThrowIfNull(customization);

        if (!customization.CanConfirm)
        {
            throw new InvalidOperationException(
                "A pizza must contain at least one ingredient before confirmation.");
        }

        return new PizzaOrder(
            customization.Size,
            customization.Shape,
            customization.StuffedCrust,
            customization.SelectedIngredientDetails,
            customization.TotalPrice);
    }

    public override string ToString()
    {
        string crustDescription = StuffedCrust ? "stuffed crust" : "regular crust";
        return $"{Size} {Shape} pizza with {string.Join(", ", Ingredients.Select(ingredient => ingredient.Name))} ({crustDescription}) - {CurrencyFormatter.Format(Price)}";
    }
}
