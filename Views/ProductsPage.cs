using KurtDhylanMotoShopInventory.Models;
using KurtDhylanMotoShopInventory.Services;

namespace KurtDhylanMotoShopInventory.Views;

public class ProductsPage : ContentPage
{
    readonly Entry search = Ui.Entry("🔎  Search products...");
    readonly VerticalStackLayout list = new() { Spacing = 8 };
    readonly AppDataStore store = AppDataStore.Instance;

    public ProductsPage()
    {
        Title = "Products"; BackgroundColor = Ui.Dark;
        search.TextChanged += (_, _) => Render();
        var add = Ui.RedButton("＋ ADD PRODUCT"); add.Clicked += async (_, _) => { await Navigation.PushAsync(new AddProductPage()); };
        var top = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(95) }, ColumnSpacing = 8 };
        top.Add(search, 0); top.Add(add, 1);
        var content = new VerticalStackLayout { Spacing = 12, Padding = 18 };
        content.Add(Ui.Title("PRODUCT INVENTORY")); content.Add(top); content.Add(Ui.MutedLabel("Tap ADD TO CART to create an order.")); content.Add(list);
        Content = new ScrollView { Content = content };
    }

    protected override async void OnAppearing() { base.OnAppearing(); await store.LoadAsync(); Render(); }

    void Render()
    {
        list.Clear();
        var q = search.Text?.Trim() ?? "";
        var items = store.Products.Where(p => string.IsNullOrWhiteSpace(q) || $"{p.Brand} {p.Model} {p.Name}".Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        if (items.Count == 0) { list.Add(Ui.CardFrame(new VerticalStackLayout { Children = { Ui.Title("No products", 18), Ui.MutedLabel("Add your first product to the inventory.") } })); return; }
        foreach (var p in items) list.Add(ProductCard(p));
    }

    View ProductCard(Product p)
    {
        var title = Ui.Title(p.Name, 18);
        var brand = new Label { Text = $"{p.Brand}  •  {p.Model}", TextColor = Ui.Muted, FontSize = 12 };
        var price = new Label { Text = $"₱{p.Price:N2}", TextColor = Colors.White, FontSize = 19, FontAttributes = FontAttributes.Bold };
        var stock = Ui.Pill($"STOCK {p.Stocks}");
        var add = Ui.RedButton("ADD TO CART"); add.HeightRequest = 44; add.Clicked += async (_, _) =>
        {
            if (!CartService.Instance.AddProduct(p)) await DisplayAlert("Unavailable", p.Stocks <= 0 ? "This product is out of stock." : "Cart quantity cannot exceed current stock.", "OK");
            else await DisplayAlert("Added", $"{p.Name} added to order.", "OK");
        };
        var del = new Button { Text = "DELETE", BackgroundColor = Color.FromArgb("#2A1517"), TextColor = Ui.Red, HeightRequest = 44, CornerRadius = 14 };
        del.Clicked += async (_, _) => { if (await DisplayAlert("Delete product?", $"Remove {p.Name} from inventory?", "DELETE", "CANCEL")) { await store.RemoveProductAsync(p.Id); Render(); } };
        var buttons = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(90) }, ColumnSpacing = 8 };
        buttons.Add(add, 0); buttons.Add(del, 1);
        var info = new VerticalStackLayout { Spacing = 4 }; info.Add(title); info.Add(brand); info.Add(price); info.Add(stock); info.Add(buttons);
        return Ui.CardFrame(info);
    }
}
