using KurtDhylanMotoShopInventory.Models;
using KurtDhylanMotoShopInventory.Services;

namespace KurtDhylanMotoShopInventory.Views;

public class ServicesPage : ContentPage
{
    readonly Entry search = Ui.Entry("🔎  Search services...");
    readonly VerticalStackLayout list = new() { Spacing = 8 };
    readonly AppDataStore store = AppDataStore.Instance;

    public ServicesPage()
    {
        Title = "Services"; BackgroundColor = Ui.Dark;
        search.TextChanged += (_, _) => Render();
        var add = Ui.RedButton("＋ ADD SERVICE"); add.Clicked += async (_, _) => await Navigation.PushAsync(new AddServicePage());
        var top = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(105) }, ColumnSpacing = 8 };
        top.Add(search, 0); top.Add(add, 1);
        var content = new VerticalStackLayout { Spacing = 12, Padding = 18 };
        content.Add(Ui.Title("SERVICES / LABOR")); content.Add(top); content.Add(list);
        Content = new ScrollView { Content = content };
    }

    protected override async void OnAppearing() { base.OnAppearing(); await store.LoadAsync(); Render(); }

    void Render()
    {
        list.Clear();
        var q = search.Text?.Trim() ?? "";
        var items = store.Services.Where(s => string.IsNullOrWhiteSpace(q) || s.LaborName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        if (items.Count == 0) { list.Add(Ui.CardFrame(new VerticalStackLayout { Children = { Ui.Title("No services", 18), Ui.MutedLabel("Add labor/service pricing to show it here.") } })); return; }
        foreach (var s in items) list.Add(ServiceCard(s));
    }

    View ServiceCard(ServiceItem s)
    {
        var add = Ui.RedButton("ADD TO ORDER"); add.HeightRequest = 44; add.Clicked += async (_, _) => { CartService.Instance.AddService(s); await DisplayAlert("Added", $"{s.LaborName} added to order.", "OK"); };
        var del = new Button { Text = "DELETE", BackgroundColor = Color.FromArgb("#2A1517"), TextColor = Ui.Red, HeightRequest = 44, CornerRadius = 14 };
        del.Clicked += async (_, _) => { if (await DisplayAlert("Delete service?", $"Remove {s.LaborName}?", "DELETE", "CANCEL")) { await store.RemoveServiceAsync(s.Id); Render(); } };
        var buttons = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(90) }, ColumnSpacing = 8 }; buttons.Add(add, 0); buttons.Add(del, 1);
        var stack = new VerticalStackLayout { Spacing = 6, Children = { Ui.Title(s.LaborName, 18), new Label { Text = "LABOR / SERVICE", TextColor = Ui.Muted, FontSize = 11 }, new Label { Text = $"₱{s.Price:N2}", FontSize = 20, FontAttributes = FontAttributes.Bold }, buttons } };
        return Ui.CardFrame(stack);
    }
}
