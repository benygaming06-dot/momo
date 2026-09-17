namespace KurtDhylanMotoShopInventory;

public partial class App : Application
{
    public App() => InitializeComponent();

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new Views.MainPage())
        {
            BarBackgroundColor = Color.FromArgb("#0B0B0D"),
            BarTextColor = Colors.White
        });
    }
}
