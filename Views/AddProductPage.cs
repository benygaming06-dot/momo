using KurtDhylanMotoShopInventory.Models;
using KurtDhylanMotoShopInventory.Services;

namespace KurtDhylanMotoShopInventory.Views;

public class AddProductPage : ContentPage
{
    readonly Entry brand = Ui.Entry("Brand (e.g. Honda)");
    readonly Entry model = Ui.Entry("Model (e.g. Click V3)");
    readonly Entry name = Ui.Entry("Product name");
    readonly Entry price = Ui.Entry("Price", Keyboard.Numeric);
    readonly Entry stocks = Ui.Entry("Stocks", Keyboard.Numeric);

    public AddProductPage()
    {
        Title = "Add Product"; BackgroundColor = Ui.Dark;
        var save = Ui.RedButton("SAVE PRODUCT"); save.Clicked += Save;
        var back = Ui.DarkButton("CANCEL"); back.Clicked += async (_, _) => await Navigation.PopAsync();
        var form = new VerticalStackLayout { Spacing = 12, Padding = 18 };
        form.Add(Ui.Title("ADD PRODUCT")); form.Add(Ui.MutedLabel("Enter the product details. It will be saved locally."));
        form.Add(brand); form.Add(model); form.Add(name); form.Add(price); form.Add(stocks); form.Add(save); form.Add(back);
        Content = new ScrollView { Content = form };
    }

    async void Save(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(name.Text) || !decimal.TryParse(price.Text, out var p) || p < 0 || !int.TryParse(stocks.Text, out var s) || s < 0)
        { await DisplayAlert("Check details", "Product name, valid price and valid stocks are required.", "OK"); return; }
        await AppDataStore.Instance.AddProductAsync(new Product { Brand = brand.Text?.Trim() ?? "", Model = model.Text?.Trim() ?? "", Name = name.Text.Trim(), Price = p, Stocks = s });
        await DisplayAlert("Saved", "Product added to inventory.", "OK");
        await Navigation.PopAsync();
    }
}
