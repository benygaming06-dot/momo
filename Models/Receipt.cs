namespace KurtDhylanMotoShopInventory.Models;

public class Receipt
{
    public string CustomerName { get; set; } = "Walk-in Customer";
    public DateTime Date { get; set; } = DateTime.Now;
    public List<CartItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(x => x.Amount);
    public decimal Payment { get; set; }
    public decimal Change => Math.Max(0, Payment - Total);
}
