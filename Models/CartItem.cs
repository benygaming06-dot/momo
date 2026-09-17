namespace KurtDhylanMotoShopInventory.Models;

public class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Type { get; set; } = "Product";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; } = 1;
    public Guid SourceId { get; set; }
    public decimal Amount => UnitPrice * Quantity;
}
