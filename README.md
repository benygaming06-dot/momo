# KURT DHYLAN MOTO SHOP INVENTORY

Offline-first .NET MAUI Android inventory/order app for a motorcycle shop.

## Included

- Red/black racing-style UI based on the supplied Kurt Dhylan Moto Shop icon/design.
- Add Products: Brand, Model, Name, Price, Stocks.
- Add Services: Labor/Service Name, Price.
- Search Products.
- Search Services.
- Add products/services to an order.
- Cart quantity controls.
- Stock validation and automatic stock deduction at checkout.
- Payment and change/exchange calculation.
- Customer name and automatic date/time on receipt.
- Receipt screen with share button.
- Local JSON persistence in the app's private data directory, so normal app updates do not erase inventory data.
- GitHub Actions workflow that builds an Android APK.

## GitHub Actions fix

The workflow intentionally publishes with `-f net10.0-android` and does **not** use `linux-x64`. The earlier `NETSDK1047` error happened because an Android project was being restored/published with a Linux desktop runtime identifier (`net10.0-android/linux-x64`).

## Build locally

Install .NET 10 SDK and the MAUI Android workload, then:

```bash
dotnet workload install maui-android
dotnet restore
 dotnet build -f net10.0-android
```

## Build APK on GitHub

1. Push this project to a GitHub repository.
2. Open **Actions**.
3. Run **Build Kurt Dhylan Moto Shop Inventory APK** using **Run workflow**, or push to `main`.
4. Open the completed workflow run.
5. Download the **KurtDhylanMotoShopInventory-APK** artifact.

## Data behavior

Inventory is stored locally as `inventory.json` inside the app's private application-data directory. Updating/reinstalling through a normal Android update (same application ID) keeps the existing app data. Uninstalling the app or clearing app storage can remove it, so a future version should add Export/Backup if the inventory becomes important.
