using PizzaSales.ConsoleApp;
using Xunit;

namespace PizzaSales.ConsoleApp.Tests;

public class PizzaOrderTests
{
    [Fact]
    public void Confirmed_order_keeps_selected_ingredients_and_total_unchanged()
    {
        var customization = new PizzaCustomization(8.00m);
        customization.AddIngredient(1);
        customization.AddIngredient(2);

        var order = PizzaOrder.FromCustomization(customization);

        Assert.Equal(2, order.Ingredients.Count);
        Assert.Equal(10.50m, order.Price);
        Assert.Equal("Cheese", order.Ingredients[0].Name);
        Assert.Equal("Pepperoni", order.Ingredients[1].Name);
    }

    [Fact]
    public void Confirmed_order_ingredient_snapshot_is_read_only()
    {
        var customization = new PizzaCustomization(8.00m);
        customization.AddIngredient(1);
        var order = PizzaOrder.FromCustomization(customization);
        var ingredients = Assert.IsAssignableFrom<IList<Ingredient>>(order.Ingredients);

        Assert.Throws<NotSupportedException>(() =>
            ingredients.Add(new Ingredient(2, "Pepperoni", 1.50m)));
        Assert.Single(order.Ingredients);
        Assert.Equal(9.00m, order.Price);
    }
}
