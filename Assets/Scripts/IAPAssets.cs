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
            return new VirtualGood[] { REMOVE_ADS };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {                                                               // Get/Setup Currency Packs
            return new VirtualCurrencyPack[] {ONE_THUNS_COIN_PACK ,FIVE_THUNS_COIN_PACK ,TEN_THUNS_COIN_PACK};
        }

        public VirtualCategory[] GetCategories()
        {                                                                              // Get/ Setup Categories (for In App Purchases)
            return new VirtualCategory[] { };
        }

        //****************************BOILERPLATE ABOVE(modify as you see fit/ if nessisary)***********************
        public const string TURN_GREEN_PRODUCT_ID = "com.alexanderyounggames.soomlaiaptutorial.turngreen";                              //create a string to store the "turn green" in app purchase
        public const string REMOVE_ADS_ITEM_ID = "remove_ads";                              //create a string to store the "turn green" in app purchase


        /** Lifetime Virtual Goods (aka - lasts forever **/

        // Create the 'TURN_GREEN' LifetimeVG In-App Purchase
        public static VirtualGood TURN_GREEN = new LifetimeVG(
            "turn_green",                         // Name of IAP
            "This will turn the cube green.",     // Description of IAP
            "turn_green_item_id",                 // Item ID (different from 'product id" used by itunes, this is used by soomla)

        // 1. assign the purchase type of the IAP (purchaseWithMarket == item cost real money),
        // 2. assign the IAP as a market item (using its ID)
        // 3. set the item to be a non-consumable purchase type

        //                  1.                                      2.                                              3.
        new PurchaseWithMarket(TURN_GREEN_PRODUCT_ID, 0.99)
    );


        // Frog Smash IAP Goods
        
        public static VirtualGood REMOVE_ADS = new LifetimeVG(
                "remove_ads",                         // Name of IAP
                "This will remove the ads",     // Description of IAP
                "remove_ads_item_id",                 // Item ID (different from 'product id" used by itunes, this is used by soomla                                             3.
            new PurchaseWithMarket(REMOVE_ADS_ITEM_ID, 0.99)
        );
        

        
        //Virtual Currency
        public static VirtualCurrency COIN_CURRENCY = new VirtualCurrency(
            "Coin",                               // Name
            "Coin currency",                      // Description
            "coin_currency_id"                    // Item ID
        );  
        
        public static VirtualCurrencyPack ONE_THUNS_COIN_PACK = new VirtualCurrencyPack(
             "1000 Coins",                          // Name
             "1000 coin currency units",            // Description
             "coin_currency_id",                       // Item ID
             1000,                                  // Number of currencies in the pack
             "coin_currency_id",                    // ID of the currency associated with this pack
             new PurchaseWithMarket(                // Purchase type (with real money $)
                "coins_1000_prod_id",               // Product ID
                0.99                                // Price (in real money $)
                )
        );
        
        public static VirtualCurrencyPack FIVE_THUNS_COIN_PACK = new VirtualCurrencyPack(
             "5000 Coins",                          // Name
             "5000 coin currency units",            // Description
             "coin_currency_id",                       // Item ID
             5000,                                  // Number of currencies in the pack
             "coin_currency_id",                    // ID of the currency associated with this pack
             new PurchaseWithMarket(                // Purchase type (with real money $)
                "coins_5000_prod_id",               // Product ID
                2.99                                // Price (in real money $)
                )
        );
        
        public static VirtualCurrencyPack TEN_THUNS_COIN_PACK = new VirtualCurrencyPack(
             "10000 Coins",                          // Name
             "10000 coin currency units",            // Description
             "coin_currency_id",                     // Item ID
             10000,                                  // Number of currencies in the pack
             "coin_currency_id",                     // ID of the currency associated with this pack
             new PurchaseWithMarket(                 // Purchase type (with real money $)
                "coins_10000_prod_id",               // Product ID
                4.99                                 // Price (in real money $)
                )
        );
   
    }
}
