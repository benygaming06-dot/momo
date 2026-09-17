using System.Text.Json;
using KurtDhylanMotoShopInventory.Models;

namespace KurtDhylanMotoShopInventory.Services;

public sealed class AppDataStore
{
    private static readonly Lazy<AppDataStore> _instance = new(() => new AppDataStore());
    public static AppDataStore Instance => _instance.Value;

    private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "inventory.json");
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private AppData _data = new();

    public IReadOnlyList<Product> Products => _data.Products;
    public IReadOnlyList<ServiceItem> Services => _data.Services;

    private AppDataStore() { }

    public async Task LoadAsync()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            _data = JsonSerializer.Deserialize<AppData>(json, _options) ?? new AppData();
        }
        catch
        {
            _data = new AppData();
        }
    }

    public async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_data, _options);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task AddProductAsync(Product product)
    {
        _data.Products.Add(product);
        await SaveAsync();
    }

    public async Task AddServiceAsync(ServiceItem service)
    {
        _data.Services.Add(service);
        await SaveAsync();
    }

    public async Task<bool> RemoveProductAsync(Guid id)
    {
        var item = _data.Products.FirstOrDefault(x => x.Id == id);
        if (item == null) return false;
        _data.Products.Remove(item);
        await SaveAsync();
        return true;
    }

    public async Task<bool> RemoveServiceAsync(Guid id)
    {
        var item = _data.Services.FirstOrDefault(x => x.Id == id);
        if (item == null) return false;
        _data.Services.Remove(item);
        await SaveAsync();
        return true;
    }

    public async Task<bool> DecreaseStockAsync(Guid productId, int quantity)
    {
        var item = _data.Products.FirstOrDefault(x => x.Id == productId);
        if (item == null || item.Stocks < quantity) return false;
        item.Stocks -= quantity;
        await SaveAsync();
        return true;
    }
}
