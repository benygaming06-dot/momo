using System.Globalization;
using KurtDhylanMotoShopInventory.Models;
using KurtDhylanMotoShopInventory.Services;

namespace KurtDhylanMotoShopInventory.Views;

public class CartPage : ContentPage
{
    readonly VerticalStackLayout itemsLayout = new() { Spacing = 8 };
    readonly Label totalLabel = new();
    readonly Label changeLabel = new();
    readonly Entry payment = Ui.Entry("Payment", Keyboard.Numeric);
    readonly Entry customer = Ui.Entry("Customer name (optional)");

    public CartPage()
    {
        Title = "Order / Payment"; BackgroundColor = Ui.Dark;
        payment.TextChanged += (_, _) => UpdateChange();
        var checkout = Ui.RedButton("CHECK OUT  →  RECEIPT"); checkout.Clicked += Checkout;
        var clear = Ui.DarkButton("CLEAR ORDER"); clear.Clicked += async (_, _) => { if (CartService.Instance.Items.Count > 0 && await DisplayAlert("Clear order?", "Remove all items from the current order?", "CLEAR", "CANCEL")) { CartService.Instance.Clear(); Render(); } };
        var content = new VerticalStackLayout { Spacing = 12, Padding = 18 };
        content.Add(Ui.Title("ORDER / PAYMENT")); content.Add(Ui.MutedLabel("Build the cart, enter payment, then checkout to generate a receipt."));
        content.Add(customer); content.Add(new Label { Text = "TOTAL ORDER", TextColor = Ui.Muted, FontSize = 12, FontAttributes = FontAttributes.Bold }); content.Add(itemsLayout);
        totalLabel.FontSize = 24; totalLabel.FontAttributes = FontAttributes.Bold; totalLabel.HorizontalTextAlignment = TextAlignment.End; content.Add(totalLabel);
        content.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#303036") });
        content.Add(new Label { Text = "PAYMENT", TextColor = Ui.Muted, FontSize = 12, FontAttributes = FontAttributes.Bold }); content.Add(payment);
        changeLabel.FontSize = 18; changeLabel.FontAttributes = FontAttributes.Bold; changeLabel.HorizontalTextAlignment = TextAlignment.End; content.Add(changeLabel);
        content.Add(checkout); content.Add(clear);
        Content = new ScrollView { Content = content };
    }

    protected override void OnAppearing() { base.OnAppearing(); Render(); }

    void Render()
    {
        itemsLayout.Clear();
        foreach (var item in CartService.Instance.Items.ToList()) itemsLayout.Add(ItemCard(item));
        if (!CartService.Instance.Items.Any()) itemsLayout.Add(Ui.CardFrame(new VerticalStackLayout { Children = { Ui.Title("Your order is empty", 18), Ui.MutedLabel("Add products or services from the inventory.") } }));
        totalLabel.Text = $"TOTAL AMOUNT  ₱{CartService.Instance.Total:N2}";
        UpdateChange();
    }

    View ItemCard(CartItem item)
    {
        var amount = new Label { Text = $"₱{item.Amount:N2}", FontSize = 17, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.End };
        var title = new Label { Text = item.Name, FontSize = 16, FontAttributes = FontAttributes.Bold };
        var sub = new Label { Text = $"{item.Type}  •  ₱{item.UnitPrice:N2} × {item.Quantity}", TextColor = Ui.Muted, FontSize = 12 };
        var minus = new Button { Text = "−", BackgroundColor = Ui.Card2, HeightRequest = 40, WidthRequest = 45, Padding = 0 };
        var plus = new Button { Text = "+", BackgroundColor = Ui.Card2, HeightRequest = 40, WidthRequest = 45, Padding = 0 };
        minus.Clicked += (_, _) => { if (item.Quantity > 1) item.Quantity--; else CartService.Instance.Remove(item); Render(); };
        plus.Clicked += async (_, _) =>
        {
            if (item.Type == "Product")
            {
                var p = AppDataStore.Instance.Products.FirstOrDefault(x => x.Id == item.SourceId);
                if (p == null || item.Quantity >= p.Stocks) { await DisplayAlert("Stock limit", "You cannot add more than the current stock.", "OK"); return; }
            }
            item.Quantity++; Render();
        };
        var qty = new HorizontalStackLayout { Spacing = 6, HorizontalOptions = LayoutOptions.End, Children = { minus, new Label { Text = item.Quantity.ToString(), VerticalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, WidthRequest = 28, HorizontalTextAlignment = TextAlignment.Center }, plus } };
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(125) }, RowSpacing = 5 };
        grid.Add(title, 0, 0); grid.Add(amount, 1, 0); grid.Add(sub, 0, 1); grid.Add(qty, 1, 1);
        return Ui.CardFrame(grid);
    }

    void UpdateChange()
    {
        if (!decimal.TryParse(payment.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var paid)) paid = 0;
        var diff = paid - CartService.Instance.Total;
        changeLabel.Text = diff >= 0 ? $"EXCHANGE / CHANGE  ₱{diff:N2}" : $"REMAINING  ₱{Math.Abs(diff):N2}";
        changeLabel.TextColor = diff >= 0 ? Ui.Green : Ui.Red;
    }

    async void Checkout(object? sender, EventArgs e)
    {
        var cart = CartService.Instance.Items.ToList();
        if (cart.Count == 0) { await DisplayAlert("Empty order", "Add at least one item before checkout.", "OK"); return; }
        if (!decimal.TryParse(payment.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var paid) || paid < CartService.Instance.Total)
        { await DisplayAlert("Payment", "Payment must be equal to or greater than the total amount.", "OK"); return; }
        foreach (var item in cart.Where(x => x.Type == "Product"))
        {
            var p = AppDataStore.Instance.Products.FirstOrDefault(x => x.Id == item.SourceId);
            if (p == null || p.Stocks < item.Quantity) { await DisplayAlert("Stock changed", $"Not enough stock for {item.Name}.", "OK"); return; }
        }
        foreach (var item in cart.Where(x => x.Type == "Product")) await AppDataStore.Instance.DecreaseStockAsync(item.SourceId, item.Quantity);
        var receipt = new Receipt { CustomerName = string.IsNullOrWhiteSpace(customer.Text) ? "Walk-in Customer" : customer.Text.Trim(), Date = DateTime.Now, Items = cart, Payment = paid };
        CartService.Instance.Clear();
        await Navigation.PushAsync(new ReceiptPage(receipt));
    }
}
