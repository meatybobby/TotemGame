using UnityEngine;
using System.Collections;
using Soomla.Store;

public class MyStoreAssets : IStoreAssets {

    public int GetVersion()
    {
        return 0;
    }

    public VirtualCurrency[] GetCurrencies()
    {
        return new VirtualCurrency[] { COIN_CURRENCY };
    }

    public VirtualGood[] GetGoods()
    {
        return new VirtualGood[] { LASER_GUN_GOOD, SNIPER_SCOPE_GOOD, AUTOMATIC_CANON_GOOD };
    }

    public VirtualCurrencyPack[] GetCurrencyPacks()
    {
        return new VirtualCurrencyPack[] {  };
    }

    public VirtualCategory[] GetCategories()
    {
        return new VirtualCategory[] { };
    }

    public const string COIN_CURRENCY_ITEM_ID = "currency_coin";

    public const string LASER_GUN_ITEM_ID = "laser_gun";
    public const string SNIPER_SCOPE_ITEM_ID = "sniper_scope";
    public const string AUTOMATIC_CANON_ITEM_ID = "automatic_canon";

    public static VirtualCurrency COIN_CURRENCY = new VirtualCurrency("Coin", "", COIN_CURRENCY_ITEM_ID);

    public static VirtualGood LASER_GUN_GOOD = new SingleUseVG("Laser Gun", "", LASER_GUN_ITEM_ID, new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID, 100));
    public static VirtualGood SNIPER_SCOPE_GOOD = new SingleUseVG("Sniper Scope", "", SNIPER_SCOPE_ITEM_ID, new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID, 100));
    public static VirtualGood AUTOMATIC_CANON_GOOD = new SingleUseVG("Automatic Canon", "", AUTOMATIC_CANON_ITEM_ID, new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID, 100));

}
