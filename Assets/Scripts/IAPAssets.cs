using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store.IAP                                                                                                                 //Allows for access to Soomla API
{
    public class IAPAssets : IStoreAssets
    {

        public int GetVersion()
        {                                                                                                               // Get Current Version
            return 0;
        }

        public VirtualCurrency[] GetCurrencies()
        {                                                                              // Get/Setup Virtual Currencies
            return new VirtualCurrency[] { COIN_CURRENCY };
        }

        public VirtualGood[] GetGoods()
        {                                                                                               // Add "TURN_GREEN" IAP to GetGoods
            return new VirtualGood[] { REMOVE_ADS,REVEAL_FROGS,RESET_LEVEL};
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {                                                               // Get/Setup Currency Packs
            return new VirtualCurrencyPack[] {ONE_THUNS_COIN_PACK,FIVE_THUNS_COIN_PACK,TEN_THUNS_COIN_PACK};
        }

        public VirtualCategory[] GetCategories()
        {                                                                              // Get/ Setup Categories (for In App Purchases)
            return new VirtualCategory[] { };
        }
        
        /** Static Final Members **/

	    public const string COIN_CURRENCY_ITEM_ID    = "coin_currency_id" ;

	    public const string ONE_THUNS_COIN_PACK_ID     = "coins_1000_prod_id";

	    public const string FIVE_THUNS_COIN_PACK_ID    = "coins_5000_prod_id";

	    public const string TEN_THUNS_COIN_PACK_ID     = "coins_10000_prod_id";

        public const string REMOVE_ADS_ITEM_ID         = "remove_ads";
        
        public const string REVEAL_FROGS_ITEM_ID         = "reveal_frogs";
        
        public const string RESET_LEVEL_ITEM_ID         = "reset_level";                            


        // Frog Smash IAP Goods
        
        public static VirtualGood REMOVE_ADS = new LifetimeVG(
                "remove_ads",                         // Name of IAP
                "This will remove the ads",     // Description of IAP
                "remove_ads_item_id",                 // Item ID (different from 'product id" used by itunes, this is used by soomla                                             3.
            new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID, 50000));
        
        public static VirtualGood REVEAL_FROGS = new SingleUseVG(
            "Reveal Frogs",         // Name
            "Reveal Frogs",         // Description
            "reveal_frogs",   // Item ID
            new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID,300));
            
        public static VirtualGood RESET_LEVEL = new SingleUseVG(
            "Reveal Frogs",         // Name
            "Reveal Frogs",         // Description
            "reset_level",    // Item ID
            new PurchaseWithVirtualItem(COIN_CURRENCY_ITEM_ID,1000));
        


        
        //Virtual Currency
        public static VirtualCurrency COIN_CURRENCY = new VirtualCurrency(
            "Coin",                               // Name
            "Coin currency",                      // Description
            COIN_CURRENCY_ITEM_ID                    // Item ID
        );  
        
        public static VirtualCurrencyPack ONE_THUNS_COIN_PACK = new VirtualCurrencyPack(
             "1000 Coins",                          // Name
             "1000 coin currency units",            // Description
             "coins_1000_prod",                       // Item ID
             1000,                                  // Number of currencies in the pack
             COIN_CURRENCY_ITEM_ID ,                    // ID of the currency associated with this pack
             new PurchaseWithMarket(                // Purchase type (with real money $)
                ONE_THUNS_COIN_PACK_ID,               // Product ID
                0.99                                // Price (in real money $)
                )
        );
        
        public static VirtualCurrencyPack FIVE_THUNS_COIN_PACK = new VirtualCurrencyPack(
             "5000 Coins",                          // Name
             "5000 coin currency units",            // Description
             "coins_5000_prod",                       // Item ID
             5000,                                  // Number of currencies in the pack
             COIN_CURRENCY_ITEM_ID ,                    // ID of the currency associated with this pack
             new PurchaseWithMarket(                // Purchase type (with real money $)
                FIVE_THUNS_COIN_PACK_ID,               // Product ID
                2.99                                // Price (in real money $)
                )
        );
        
        public static VirtualCurrencyPack TEN_THUNS_COIN_PACK = new VirtualCurrencyPack(
             "10000 Coins",                          // Name
             "10000 coin currency units",            // Description
             "coins_10000_prod",                     // Item ID
             10000,                                  // Number of currencies in the pack
             COIN_CURRENCY_ITEM_ID ,                     // ID of the currency associated with this pack
             new PurchaseWithMarket(                 // Purchase type (with real money $)
                TEN_THUNS_COIN_PACK_ID,               // Product ID
                4.99                                 // Price (in real money $)
                )
        );
   
    }
}
