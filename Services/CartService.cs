using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory.Services;

public sealed class CartService
{
    private static readonly Lazy<CartService> _instance = new(() => new CartService());
    public static CartService Instance => _instance.Value;
    public List<CartItem> Items { get; } = new();
    public decimal Total => Items.Sum(x => x.Amount);
    public event EventHandler? Changed;
    private CartService() { }

    public bool AddProduct(Product p)
    {
        var existing = Items.FirstOrDefault(x => x.SourceId == p.Id && x.Type == "Product");
        if (existing != null)
        {
            if (existing.Quantity >= p.Stocks) return false;
            existing.Quantity++;
        }
        else
        {
            if (p.Stocks <= 0) return false;
            Items.Add(new CartItem { SourceId = p.Id, Name = p.Name, Type = "Product", UnitPrice = p.Price });
        }
        Changed?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public void AddService(ServiceItem s)
    {
        Items.Add(new CartItem { SourceId = s.Id, Name = s.LaborName, Type = "Service", UnitPrice = s.Price });
        Changed?.Invoke(this, EventArgs.Empty);
    }

    public void Remove(CartItem item) { Items.Remove(item); Changed?.Invoke(this, EventArgs.Empty); }
    public void Clear() { Items.Clear(); Changed?.Invoke(this, EventArgs.Empty); }
}
