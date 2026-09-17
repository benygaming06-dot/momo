using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;

namespace KurtDhylanMotoShopInventory.Views;

public static class Ui
{
    public static readonly Color Red = Color.FromArgb("#F20D18");
    public static readonly Color Dark = Color.FromArgb("#0B0B0D");
    public static readonly Color Card = Color.FromArgb("#17171A");
    public static readonly Color Card2 = Color.FromArgb("#202024");
    public static readonly Color Muted = Color.FromArgb("#A8A8B0");
    public static readonly Color Green = Color.FromArgb("#32D583");

    public static Label Title(string text, double size = 26) => new()
    {
        Text = text, FontSize = size, FontAttributes = FontAttributes.Bold,
        TextColor = Colors.White, Margin = new Thickness(0, 0, 0, 6)
    };

    public static Label MutedLabel(string text, double size = 13) => new()
    {
        Text = text, FontSize = size, TextColor = Muted
    };

    public static Border CardFrame(View content) => new()
    {
        Content = content, BackgroundColor = Card, Stroke = Color.FromArgb("#29292E"), StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = 18 }, Padding = 16, Margin = new Thickness(0, 5)
    };

    public static Button RedButton(string text) => new()
    {
        Text = text, BackgroundColor = Red, TextColor = Colors.White,
        CornerRadius = 14, FontAttributes = FontAttributes.Bold
    };

    public static Button DarkButton(string text) => new()
    {
        Text = text, BackgroundColor = Card2, TextColor = Colors.White,
        CornerRadius = 14, FontAttributes = FontAttributes.Bold
    };

    public static Entry Entry(string placeholder, Keyboard? keyboard = null)
    {
        var e = new Entry { Placeholder = placeholder, BackgroundColor = Card2, TextColor = Colors.White, PlaceholderColor = Color.FromArgb("#77777F"), HeightRequest = 52 };
        if (keyboard != null) e.Keyboard = keyboard;
        return e;
    }

    public static Border Pill(string text)
    {
        return new Border
        {
            BackgroundColor = Color.FromArgb("#2A1012"), Stroke = Red, StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(10, 5), Content = new Label { Text = text, TextColor = Red, FontSize = 12, FontAttributes = FontAttributes.Bold }
        };
    }
}
