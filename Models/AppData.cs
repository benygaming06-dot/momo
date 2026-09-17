namespace KurtDhylanMotoShopInventory.Models;

public class AppData
{
    public List<Product> Products { get; set; } = new();
    public List<ServiceItem> Services { get; set; } = new();
}
