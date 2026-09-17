using KurtDhylanMotoShopInventory.Services;

namespace KurtDhylanMotoShopInventory.Views;

public class MainPage : ContentPage
{
    readonly AppDataStore store = AppDataStore.Instance;
    readonly Label productCount = new();
    readonly Label serviceCount = new();
    readonly Label cartCount = new();

    public MainPage()
    {
        Title = "Kurt Dhylan Moto Shop";
        BackgroundColor = Ui.Dark;

        var logo = new Image { Source = "appicon.svg", HeightRequest = 110, WidthRequest = 110, HorizontalOptions = LayoutOptions.Center };
        var shop = Ui.Title("KURT DHYLAN", 30); shop.HorizontalOptions = LayoutOptions.Center;
        var sub = new Label { Text = "MOTO SHOP INVENTORY", TextColor = Ui.Red, FontSize = 14, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center };

        var dashboard = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(GridLength.Star), new(GridLength.Star) }, ColumnSpacing = 10 };
        dashboard.Add(MiniStat("PRODUCTS", productCount), 0); dashboard.Add(MiniStat("SERVICES", serviceCount), 1); dashboard.Add(MiniStat("CART", cartCount), 2);

        var products = Ui.RedButton("PRODUCTS\nSearch / Manage"); products.Clicked += async (_, _) => await Navigation.PushAsync(new ProductsPage());
        var services = Ui.DarkButton("SERVICES\nSearch / Manage"); services.Clicked += async (_, _) => await Navigation.PushAsync(new ServicesPage());
        var cart = Ui.DarkButton("🛒  ORDER / CHECKOUT"); cart.Clicked += async (_, _) => await Navigation.PushAsync(new CartPage());
        var addProduct = Ui.DarkButton("＋  ADD PRODUCT"); addProduct.Clicked += async (_, _) => await Navigation.PushAsync(new AddProductPage());
        var addService = Ui.DarkButton("＋  ADD SERVICE"); addService.Clicked += async (_, _) => await Navigation.PushAsync(new AddServicePage());

        var content = new VerticalStackLayout { Spacing = 12, Padding = new Thickness(18, 18, 18, 28) };
        content.Add(logo); content.Add(shop); content.Add(sub); content.Add(new BoxView { HeightRequest = 1, Color = Color.FromArgb("#2A2A2F"), Margin = new Thickness(0, 8) });
        content.Add(dashboard); content.Add(new Label { Text = "INVENTORY MENU", TextColor = Ui.Muted, FontSize = 12, FontAttributes = FontAttributes.Bold, Margin = new Thickness(4, 12, 0, 0) });
        content.Add(products); content.Add(services); content.Add(cart); content.Add(addProduct); content.Add(addService);
        content.Add(new Label { Text = "Offline-first • Inventory is saved on this device", TextColor = Color.FromArgb("#707078"), FontSize = 11, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 12, 0, 0) });
        Content = new ScrollView { Content = content };
    }

    View MiniStat(string title, Label value)
    {
        value.TextColor = Colors.White; value.FontSize = 22; value.FontAttributes = FontAttributes.Bold; value.HorizontalTextAlignment = TextAlignment.Center;
        var stack = new VerticalStackLayout { Spacing = 2, HorizontalOptions = LayoutOptions.Fill };
        stack.Add(value); stack.Add(new Label { Text = title, TextColor = Ui.Muted, FontSize = 9, HorizontalTextAlignment = TextAlignment.Center });
        return Ui.CardFrame(stack);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await store.LoadAsync();
        RefreshCounts();
        CartService.Instance.Changed -= OnCartChanged;
        CartService.Instance.Changed += OnCartChanged;
    }

    void OnCartChanged(object? sender, EventArgs e) => RefreshCounts();
    void RefreshCounts()
    {
        productCount.Text = store.Products.Count.ToString();
        serviceCount.Text = store.Services.Count.ToString();
        cartCount.Text = CartService.Instance.Items.Sum(x => x.Quantity).ToString();
    }
}
