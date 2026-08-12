using System.IO;

namespace PizzaSales.ConsoleApp;

internal interface IConsoleAdapter
{
    bool IsInputRedirected { get; }
    bool IsOutputRedirected { get; }
    int WindowWidth { get; }
    int WindowHeight { get; }
    void Clear();
    void Write(string value);
    void WriteLine(string value);
    ConsoleKeyInfo ReadKey(bool intercept);
}

internal sealed class SystemConsoleAdapter : IConsoleAdapter
{
    public static SystemConsoleAdapter Instance { get; } = new();

    private SystemConsoleAdapter()
    {
    }

    public bool IsInputRedirected => Console.IsInputRedirected;
    public bool IsOutputRedirected => Console.IsOutputRedirected;
    public int WindowWidth => GetDimension(() => Console.WindowWidth, 80);
    public int WindowHeight => GetDimension(() => Console.WindowHeight, 25);

    public void Clear() => Console.Clear();
    public void Write(string value) => Console.Write(value);
    public void WriteLine(string value) => Console.WriteLine(value);
    public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);

    private static int GetDimension(Func<int> readDimension, int fallback)
    {
        try
        {
            return Math.Max(1, readDimension());
        }
        catch (IOException)
        {
            return fallback;
        }
        catch (PlatformNotSupportedException)
        {
            return fallback;
        }
    }
}

internal readonly record struct MenuViewport(int Start, int Count);

internal static class ConsoleMenuLayout
{
    public static MenuViewport CalculateViewport(
        int selectedIndex,
        int viewportStart,
        int itemCount,
        int windowHeight,
        int fixedRowCount)
    {
        if (itemCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(itemCount));
        }

        selectedIndex = Math.Clamp(selectedIndex, 0, itemCount - 1);
        int availableRows = Math.Max(1, windowHeight - fixedRowCount);
        int visibleCount = Math.Min(itemCount, availableRows);
        int maximumStart = Math.Max(0, itemCount - visibleCount);
        int start = Math.Clamp(viewportStart, 0, maximumStart);

        if (selectedIndex < start)
        {
            start = selectedIndex;
        }
        else if (selectedIndex >= start + visibleCount)
        {
            start = selectedIndex - visibleCount + 1;
        }

        return new MenuViewport(Math.Clamp(start, 0, maximumStart), visibleCount);
    }

    public static string FitLine(string value, int windowWidth)
    {
        ArgumentNullException.ThrowIfNull(value);

        string singleLine = value.Replace('\r', ' ').Replace('\n', ' ');
        int usableWidth = Math.Max(1, windowWidth - 1);
        return singleLine.Length <= usableWidth
            ? singleLine
            : singleLine[..usableWidth];
    }
}

internal static class MenuNavigation
{
    public static int Move(
        int selectedIndex,
        int itemCount,
        ConsoleKey key,
        int pageSize)
    {
        if (itemCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(itemCount));
        }

        int lastIndex = itemCount - 1;
        int clampedIndex = Math.Clamp(selectedIndex, 0, lastIndex);
        int page = Math.Max(1, pageSize);

        return key switch
        {
            ConsoleKey.UpArrow => Math.Max(0, clampedIndex - 1),
            ConsoleKey.DownArrow => Math.Min(lastIndex, clampedIndex + 1),
            ConsoleKey.Home => 0,
            ConsoleKey.End => lastIndex,
            ConsoleKey.PageUp => Math.Max(0, clampedIndex - page),
            ConsoleKey.PageDown => Math.Min(lastIndex, clampedIndex + page),
            _ => clampedIndex
        };
    }

    public static bool IsNavigationKey(ConsoleKey key)
    {
        return key is ConsoleKey.UpArrow
            or ConsoleKey.DownArrow
            or ConsoleKey.Home
            or ConsoleKey.End
            or ConsoleKey.PageUp
            or ConsoleKey.PageDown;
    }
}

public static class ConsoleMenu
{
    internal const string TerminalRequiredMessage =
        "This application requires an interactive terminal; redirected input or output is not supported.";

    public static string ShowMenu(string title, IReadOnlyList<string> options)
    {
        return ShowMenu([title], options, SystemConsoleAdapter.Instance);
    }

    public static string ShowMenu(
        IReadOnlyList<string> headerLines,
        IReadOnlyList<string> options)
    {
        return ShowMenu(headerLines, options, SystemConsoleAdapter.Instance);
    }

    internal static string ShowMenu(
        IReadOnlyList<string> headerLines,
        IReadOnlyList<string> options,
        IConsoleAdapter console)
    {
        ArgumentNullException.ThrowIfNull(headerLines);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(console);

        if (headerLines.Count == 0)
        {
            throw new ArgumentException("At least one header line is required.", nameof(headerLines));
        }

        if (options.Count == 0)
        {
            throw new ArgumentException("At least one option is required.", nameof(options));
        }

        EnsureInteractiveTerminal(console);

        int selectedIndex = 0;
        int viewportStart = 0;

        while (true)
        {
            MenuViewport viewport = ConsoleMenuLayout.CalculateViewport(
                selectedIndex,
                viewportStart,
                options.Count,
                console.WindowHeight,
                headerLines.Count + 1);
            viewportStart = viewport.Start;

            console.Clear();
            string range = FormatViewportRange(viewport, options.Count);
            for (int index = 0; index < headerLines.Count; index++)
            {
                string headerLine = index == 0
                    ? $"{range}{headerLines[index]}"
                    : headerLines[index];
                WriteDisplayLine(console, headerLine);
            }

            for (int index = viewport.Start; index < viewport.Start + viewport.Count; index++)
            {
                string marker = index == selectedIndex ? ">" : " ";
                WriteDisplayLine(console, $"{marker} {options[index]}");
            }

            WriteDisplayLine(
                console,
                "Arrows move | Home/End | PgUp/PgDn | Enter selects",
                endLine: false);

            ConsoleKey key = console.ReadKey(intercept: true).Key;
            if (MenuNavigation.IsNavigationKey(key))
            {
                selectedIndex = MenuNavigation.Move(
                    selectedIndex,
                    options.Count,
                    key,
                    viewport.Count);
                continue;
            }

            if (key == ConsoleKey.Enter)
            {
                return options[selectedIndex];
            }
        }
    }

    public static bool EditIngredients(
        string size,
        string shape,
        bool stuffedCrust,
        PizzaCustomization customization)
    {
        return EditIngredients(
            size,
            shape,
            stuffedCrust,
            customization,
            SystemConsoleAdapter.Instance);
    }

    internal static bool EditIngredients(
        string size,
        string shape,
        bool stuffedCrust,
        PizzaCustomization customization,
        IConsoleAdapter console)
    {
        ArgumentNullException.ThrowIfNull(customization);
        ArgumentNullException.ThrowIfNull(console);
        EnsureInteractiveTerminal(console);

        const int actionCount = 2;
        const int fixedRowCount = 3;
        int itemCount = IngredientCatalog.All.Count + actionCount;
        int selectedIndex = 0;
        int viewportStart = 0;
        string feedback = string.Empty;

        while (true)
        {
            MenuViewport viewport = ConsoleMenuLayout.CalculateViewport(
                selectedIndex,
                viewportStart,
                itemCount,
                console.WindowHeight,
                fixedRowCount);
            viewportStart = viewport.Start;

            console.Clear();
            string crust = stuffedCrust ? "stuffed crust" : "regular crust";
            WriteDisplayLine(
                console,
                $"{FormatViewportRange(viewport, itemCount)}Customize: {size} {shape} ({crust}) | Total: {CurrencyFormatter.Format(customization.TotalPrice)}");

            IReadOnlySet<int> selectedIngredients = customization.SelectedIngredients;
            for (int index = viewport.Start; index < viewport.Start + viewport.Count; index++)
            {
                string marker = index == selectedIndex ? ">" : " ";
                if (index < IngredientCatalog.All.Count)
                {
                    Ingredient ingredient = IngredientCatalog.All[index];
                    string checkbox = selectedIngredients.Contains(ingredient.MenuNumber) ? "[x]" : "[ ]";
                    WriteDisplayLine(
                        console,
                        $"{marker} {checkbox} {ingredient.MenuNumber}. {ingredient.Name,-16} {CurrencyFormatter.Format(ingredient.Price),8}");
                }
                else
                {
                    string action = index == IngredientCatalog.All.Count
                        ? "Continue to confirmation"
                        : "Cancel pizza";
                    WriteDisplayLine(console, $"{marker}     {action}");
                }
            }

            WriteDisplayLine(console, feedback);
            WriteDisplayLine(
                console,
                "Arrows move | Space toggles | Del removes | Enter chooses",
                endLine: false);

            ConsoleKey key = console.ReadKey(intercept: true).Key;
            if (MenuNavigation.IsNavigationKey(key))
            {
                selectedIndex = MenuNavigation.Move(
                    selectedIndex,
                    itemCount,
                    key,
                    viewport.Count);
                feedback = string.Empty;
                continue;
            }

            if (key == ConsoleKey.Spacebar)
            {
                if (selectedIndex >= IngredientCatalog.All.Count)
                {
                    feedback = "Choose an ingredient row to toggle.";
                    continue;
                }

                Ingredient ingredient = IngredientCatalog.All[selectedIndex];
                if (selectedIngredients.Contains(ingredient.MenuNumber))
                {
                    customization.RemoveIngredient(ingredient.MenuNumber);
                    feedback = $"{ingredient.Name} removed.";
                }
                else
                {
                    customization.AddIngredient(ingredient.MenuNumber);
                    feedback = $"{ingredient.Name} added.";
                }

                continue;
            }

            if (key == ConsoleKey.Delete)
            {
                if (selectedIndex >= IngredientCatalog.All.Count)
                {
                    feedback = "Nothing can be removed from this row.";
                    continue;
                }

                Ingredient ingredient = IngredientCatalog.All[selectedIndex];
                feedback = customization.RemoveIngredient(ingredient.MenuNumber)
                    ? $"{ingredient.Name} removed."
                    : $"{ingredient.Name} is not selected.";
                continue;
            }

            if (key == ConsoleKey.Enter)
            {
                if (selectedIndex < IngredientCatalog.All.Count)
                {
                    feedback = "Use Space to toggle the focused checkbox.";
                    continue;
                }

                if (selectedIndex == IngredientCatalog.All.Count)
                {
                    if (customization.CanConfirm)
                    {
                        return true;
                    }

                    feedback = "Select at least one ingredient before continuing.";
                    continue;
                }

                return false;
            }

            if (key == ConsoleKey.Escape)
            {
                return false;
            }

            feedback = "Use arrows to move, Space to toggle, Delete to remove, or Enter on an action.";
        }
    }

    private static void EnsureInteractiveTerminal(IConsoleAdapter console)
    {
        if (console.IsInputRedirected || console.IsOutputRedirected)
        {
            throw new InvalidOperationException(TerminalRequiredMessage);
        }
    }

    private static string FormatViewportRange(MenuViewport viewport, int itemCount)
    {
        return viewport.Count < itemCount
            ? $"[{viewport.Start + 1}-{viewport.Start + viewport.Count} of {itemCount}] "
            : string.Empty;
    }

    private static void WriteDisplayLine(
        IConsoleAdapter console,
        string value,
        bool endLine = true)
    {
        string fittedLine = ConsoleMenuLayout.FitLine(value, console.WindowWidth);
        if (endLine)
        {
            console.WriteLine(fittedLine);
        }
        else
        {
            console.Write(fittedLine);
        }
    }
}
