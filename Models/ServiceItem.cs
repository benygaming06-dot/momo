namespace KurtDhylanMotoShopInventory.Models;

public class ServiceItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string LaborName { get; set; } = "";
    public decimal Price { get; set; }
}
