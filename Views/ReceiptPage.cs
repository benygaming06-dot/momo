using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory.Views;

public class ReceiptPage : ContentPage
{
    readonly Receipt receipt;
    public ReceiptPage(Receipt receipt)
    {
        this.receipt = receipt;
        Title = "Receipt"; BackgroundColor = Ui.Dark;
        var stack = new VerticalStackLayout { Spacing = 8, Padding = 18 };
        var head = new VerticalStackLayout { Spacing = 2, Children = { Ui.Title("RECEIPT", 30), new Label { Text = "KURT DHYLAN MOTO SHOP", TextColor = Ui.Red, FontSize = 16, FontAttributes = FontAttributes.Bold }, Ui.MutedLabel("Ride • Build • Trust") } };
        stack.Add(head); stack.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#303036"), Margin = new Thickness(0, 8) });
        stack.Add(new Label { Text = $"CUSTOMER: {receipt.CustomerName}", FontAttributes = FontAttributes.Bold });
        stack.Add(new Label { Text = $"DATE: {receipt.Date:MMM dd, yyyy • hh:mm tt}", TextColor = Ui.Muted, FontSize = 12 });
        stack.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#303036"), Margin = new Thickness(0, 8) });
        var table = new VerticalStackLayout { Spacing = 0 };
        table.Add(HeaderRow());
        int n = 1;
        foreach (var item in receipt.Items) table.Add(Row(n++, item));
        stack.Add(Ui.CardFrame(table));
        stack.Add(new Label { Text = $"TOTAL  ₱{receipt.Total:N2}", FontSize = 24, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.End });
        stack.Add(new Label { Text = $"PAYMENT  ₱{receipt.Payment:N2}", TextColor = Ui.Muted, HorizontalTextAlignment = TextAlignment.End });
        stack.Add(new Label { Text = $"CHANGE  ₱{receipt.Change:N2}", TextColor = Ui.Green, FontSize = 18, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.End });
        var done = Ui.RedButton("DONE"); done.Clicked += async (_, _) => await Navigation.PopToRootAsync();
        var share = Ui.DarkButton("SHARE RECEIPT"); share.Clicked += ShareReceipt;
        stack.Add(done); stack.Add(share);
        Content = new ScrollView { Content = stack };
    }

    View HeaderRow()
    {
        var g = new Grid { Padding = new Thickness(4, 7), ColumnDefinitions = new ColumnDefinitionCollection { new(30), new(GridLength.Star), new(80), new(80) } };
        g.Add(new Label { Text = "NO", TextColor = Ui.Muted, FontSize = 10 }, 0);
        g.Add(new Label { Text = "ITEM", TextColor = Ui.Muted, FontSize = 10 }, 1);
        g.Add(new Label { Text = "PRICE", TextColor = Ui.Muted, FontSize = 10 }, 2);
        g.Add(new Label { Text = "AMOUNT", TextColor = Ui.Muted, FontSize = 10, HorizontalTextAlignment = TextAlignment.End }, 3);
        return g;
    }

    View Row(int n, CartItem item)
    {
        var g = new Grid { Padding = new Thickness(4, 10), ColumnDefinitions = new ColumnDefinitionCollection { new(30), new(GridLength.Star), new(80), new(80) } };
        g.Add(new Label { Text = n.ToString(), FontSize = 12 }, 0); g.Add(new Label { Text = $"{item.Name}\n{item.Quantity} × ₱{item.UnitPrice:N2}", FontSize = 12 }, 1); g.Add(new Label { Text = $"₱{item.UnitPrice:N2}", FontSize = 12 }, 2); g.Add(new Label { Text = $"₱{item.Amount:N2}", FontSize = 12, HorizontalTextAlignment = TextAlignment.End }, 3); return g;
    }

    async void ShareReceipt(object? sender, EventArgs e)
    {
        var text = $"KURT DHYLAN MOTO SHOP\nCustomer: {receipt.CustomerName}\nDate: {receipt.Date:g}\n\n" + string.Join("\n", receipt.Items.Select((x, i) => $"{i + 1}. {x.Name} x{x.Quantity} = ₱{x.Amount:N2}")) + $"\n\nTOTAL: ₱{receipt.Total:N2}\nPAYMENT: ₱{receipt.Payment:N2}\nCHANGE: ₱{receipt.Change:N2}";
        await Share.Default.RequestAsync(new ShareTextRequest { Text = text, Title = "Kurt Dhylan Receipt" });
    }
}
