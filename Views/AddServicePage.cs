using KurtDhylanMotoShopInventory.Models;
using KurtDhylanMotoShopInventory.Services;

namespace KurtDhylanMotoShopInventory.Views;

public class AddServicePage : ContentPage
{
    readonly Entry labor = Ui.Entry("Labor / Service name");
    readonly Entry price = Ui.Entry("Price", Keyboard.Numeric);

    public AddServicePage()
    {
        Title = "Add Service"; BackgroundColor = Ui.Dark;
        var save = Ui.RedButton("SAVE SERVICE"); save.Clicked += Save;
        var cancel = Ui.DarkButton("CANCEL"); cancel.Clicked += async (_, _) => await Navigation.PopAsync();
        var form = new VerticalStackLayout { Spacing = 12, Padding = 18 };
        form.Add(Ui.Title("ADD SERVICE")); form.Add(Ui.MutedLabel("Add labor/service pricing for checkout."));
        form.Add(labor); form.Add(price); form.Add(save); form.Add(cancel);
        Content = new ScrollView { Content = form };
    }

    async void Save(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(labor.Text) || !decimal.TryParse(price.Text, out var p) || p < 0)
        { await DisplayAlert("Check details", "Labor name and valid price are required.", "OK"); return; }
        await AppDataStore.Instance.AddServiceAsync(new ServiceItem { LaborName = labor.Text.Trim(), Price = p });
        await DisplayAlert("Saved", "Service added.", "OK");
        await Navigation.PopAsync();
    }
}
