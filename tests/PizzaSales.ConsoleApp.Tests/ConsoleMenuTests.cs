using System.Text;
using PizzaSales.ConsoleApp;
using Xunit;

namespace PizzaSales.ConsoleApp.Tests;

public class ConsoleMenuTests
{
    [Fact]
    public void Viewport_uses_available_terminal_height_and_keeps_focus_visible()
    {
        MenuViewport initial = ConsoleMenuLayout.CalculateViewport(
            selectedIndex: 0,
            viewportStart: 0,
            itemCount: 17,
            windowHeight: 12,
            fixedRowCount: 3);

        MenuViewport moved = ConsoleMenuLayout.CalculateViewport(
            selectedIndex: 12,
            viewportStart: initial.Start,
            itemCount: 17,
            windowHeight: 12,
            fixedRowCount: 3);

        Assert.Equal(new MenuViewport(0, 9), initial);
        Assert.Equal(new MenuViewport(4, 9), moved);
    }

    [Fact]
    public void Lines_are_clipped_before_the_terminal_wrap_column()
    {
        string line = ConsoleMenuLayout.FitLine("123456789012345", windowWidth: 10);

        Assert.Equal("123456789", line);
    }

    [Fact]
    public void Navigation_clamps_and_supports_home_end_and_pages()
    {
        Assert.Equal(0, MenuNavigation.Move(0, 17, ConsoleKey.UpArrow, 5));
        Assert.Equal(16, MenuNavigation.Move(16, 17, ConsoleKey.DownArrow, 5));
        Assert.Equal(16, MenuNavigation.Move(3, 17, ConsoleKey.End, 5));
        Assert.Equal(0, MenuNavigation.Move(10, 17, ConsoleKey.Home, 5));
        Assert.Equal(15, MenuNavigation.Move(10, 17, ConsoleKey.PageDown, 5));
        Assert.Equal(5, MenuNavigation.Move(10, 17, ConsoleKey.PageUp, 5));
    }

    [Fact]
    public void Space_toggles_checkbox_and_enter_on_continue_accepts_selection()
    {
        var console = new FakeConsoleAdapter(
            windowWidth: 40,
            windowHeight: 9,
            ConsoleKey.Spacebar,
            ConsoleKey.End,
            ConsoleKey.UpArrow,
            ConsoleKey.Enter);
        var customization = new PizzaCustomization(8.00m, "small", "round", false);

        bool accepted = ConsoleMenu.EditIngredients(
            "small",
            "round",
            false,
            customization,
            console);

        Assert.True(accepted);
        Assert.Contains(1, customization.SelectedIngredients);
        Assert.Contains(console.Frames, frame => frame.Contains("[x] 1. Cheese"));
        Assert.All(
            console.Frames.SelectMany(frame => frame.Split('\n'))
                .Where(line => line.Length > 0),
            line => Assert.True(line.TrimEnd('\r').Length < console.WindowWidth));
    }

    private sealed class FakeConsoleAdapter : IConsoleAdapter
    {
        private readonly Queue<ConsoleKeyInfo> _keys;
        private StringBuilder _currentFrame = new();

        public FakeConsoleAdapter(int windowWidth, int windowHeight, params ConsoleKey[] keys)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            _keys = new Queue<ConsoleKeyInfo>(keys.Select(CreateKeyInfo));
        }

        public bool IsInputRedirected => false;
        public bool IsOutputRedirected => false;
        public int WindowWidth { get; }
        public int WindowHeight { get; }
        public List<string> Frames { get; } = [];

        public void Clear()
        {
            if (_currentFrame.Length > 0)
            {
                Frames.Add(_currentFrame.ToString());
            }

            _currentFrame = new StringBuilder();
        }

        public void Write(string value) => _currentFrame.Append(value);

        public void WriteLine(string value) => _currentFrame.AppendLine(value);

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (_keys.Count == 0)
            {
                throw new InvalidOperationException("The test did not provide enough keys.");
            }

            ConsoleKeyInfo key = _keys.Dequeue();
            if (_keys.Count == 0)
            {
                Frames.Add(_currentFrame.ToString());
            }

            return key;
        }

        private static ConsoleKeyInfo CreateKeyInfo(ConsoleKey key)
        {
            char keyChar = key == ConsoleKey.Spacebar ? ' ' : '\0';
            return new ConsoleKeyInfo(keyChar, key, false, false, false);
        }
    }
}
