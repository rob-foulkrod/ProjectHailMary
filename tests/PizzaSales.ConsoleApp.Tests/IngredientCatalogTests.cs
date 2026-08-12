using PizzaSales.ConsoleApp;
using Xunit;

namespace PizzaSales.ConsoleApp.Tests;

public class IngredientCatalogTests
{
    [Fact]
    public void Catalog_has_fifteen_entries_in_expected_order_and_prices()
    {
        var ingredients = IngredientCatalog.All;

        Assert.Equal(15, ingredients.Count);
        Assert.Equal("Cheese", ingredients[0].Name);
        Assert.Equal(1.00m, ingredients[0].Price);
        Assert.Equal("Pepperoni", ingredients[1].Name);
        Assert.Equal(1.50m, ingredients[1].Price);
        Assert.Equal("Anchovies", ingredients[14].Name);
        Assert.Equal(1.50m, ingredients[14].Price);
    }

    [Fact]
    public void Catalog_cannot_be_changed_through_the_public_collection()
    {
        var ingredients = Assert.IsAssignableFrom<IList<Ingredient>>(IngredientCatalog.All);

        Assert.Throws<NotSupportedException>(() =>
            ingredients[0] = new Ingredient(1, "Changed", 99m));
        Assert.Equal("Cheese", IngredientCatalog.All[0].Name);
    }
}
