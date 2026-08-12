using System.Globalization;

namespace PizzaSales.ConsoleApp;

internal static class CurrencyFormatter
{
    private static readonly CultureInfo UnitedStatesCulture =
        CultureInfo.GetCultureInfo("en-US");

    public static string Format(decimal value)
    {
        return value.ToString("C2", UnitedStatesCulture);
    }
}
