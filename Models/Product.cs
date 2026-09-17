namespace KurtDhylanMotoShopInventory.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Stocks { get; set; }
}
