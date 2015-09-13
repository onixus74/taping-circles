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
            return new VirtualCurrency[] { };
        }

        public VirtualGood[] GetGoods()
        {                                                                                               // Add "TURN_GREEN" IAP to GetGoods
            return new VirtualGood[] { REMOVE_ADS };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {                                                               // Get/Setup Currency Packs
            return new VirtualCurrencyPack[] { };
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
        public static VirtualGood REMOVE_ADS = new LifetimeVG(
                "remove_ads",                         // Name of IAP
                "This will remove the ads",     // Description of IAP
                "remove_ads_item_id",                 // Item ID (different from 'product id" used by itunes, this is used by soomla                                             3.
            new PurchaseWithMarket(REMOVE_ADS_ITEM_ID, 0.99)
        );
    }
}
