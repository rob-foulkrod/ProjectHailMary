using PizzaSales.ConsoleApp;
using Xunit;

namespace PizzaSales.ConsoleApp.Tests;

public class PizzaCustomizationTests
{
    [Fact]
    public void AddIngredient_increments_total_once_for_each_selected_item()
    {
        var customization = new PizzaCustomization(8.00m);

        customization.AddIngredient(1);
        customization.AddIngredient(2);

        Assert.Equal(2, customization.SelectedIngredients.Count);
        Assert.Equal(10.50m, customization.TotalPrice);
    }

    [Fact]
    public void AddIngredient_twice_is_noop_and_keeps_total_stable()
    {
        var customization = new PizzaCustomization(8.00m);

        customization.AddIngredient(1);
        customization.AddIngredient(1);

        Assert.Single(customization.SelectedIngredients);
        Assert.Equal(9.00m, customization.TotalPrice);
    }

    [Fact]
    public void RemoveIngredient_removes_selection_and_updates_total()
    {
        var customization = new PizzaCustomization(8.00m);
        customization.AddIngredient(1);
        customization.AddIngredient(5);

        customization.RemoveIngredient(1);

        Assert.Single(customization.SelectedIngredients);
        Assert.Equal(9.75m, customization.TotalPrice);
    }

    [Fact]
    public void Selected_ingredient_collection_cannot_mutate_customization_state()
    {
        var customization = new PizzaCustomization(8.00m);
        customization.AddIngredient(1);

        var exposedSelection = Assert.IsAssignableFrom<ISet<int>>(customization.SelectedIngredients);
        exposedSelection.Add(2);

        Assert.Single(customization.SelectedIngredients);
        Assert.Contains(1, customization.SelectedIngredients);
        Assert.Equal(9.00m, customization.TotalPrice);
    }
}
