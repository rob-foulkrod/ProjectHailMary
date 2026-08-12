namespace PizzaSales.ConsoleApp;

public sealed class PizzaCustomization
{
    private readonly HashSet<int> _selectedMenuNumbers = new();

    public PizzaCustomization(decimal basePrice)
        : this(basePrice, "small", "round", false)
    {
    }

    public PizzaCustomization(decimal basePrice, string size, string shape, bool stuffedCrust)
    {
        BasePrice = basePrice;
        Size = size;
        Shape = shape;
        StuffedCrust = stuffedCrust;
    }

    public decimal BasePrice { get; }
    public string Size { get; }
    public string Shape { get; }
    public bool StuffedCrust { get; }

    public IReadOnlySet<int> SelectedIngredients => new HashSet<int>(_selectedMenuNumbers);

    public IReadOnlyList<Ingredient> SelectedIngredientDetails =>
        Array.AsReadOnly(
            IngredientCatalog.All
                .Where(ingredient => _selectedMenuNumbers.Contains(ingredient.MenuNumber))
                .OrderBy(ingredient => ingredient.MenuNumber)
                .ToArray());

    public decimal TotalPrice => BasePrice + SelectedIngredientDetails.Sum(ingredient => ingredient.Price);

    public bool CanConfirm => _selectedMenuNumbers.Count > 0;

    public bool AddIngredient(int menuNumber)
    {
        if (!IngredientCatalog.TryGetByMenuNumber(menuNumber, out _))
        {
            return false;
        }

        return _selectedMenuNumbers.Add(menuNumber);
    }

    public bool RemoveIngredient(int menuNumber)
    {
        return _selectedMenuNumbers.Remove(menuNumber);
    }
}
