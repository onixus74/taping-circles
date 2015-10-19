//ExampleWindowScript.cs
//Alexander Young
//February 5, 2015
//Description - Creates the functionality to allow for in-app purchasing, specifically with reguards the GUI and using purchases to make changes to the game

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Soomla;

namespace Soomla.Store.IAP
{
    public class IAPManager : MonoBehaviour
    {
        private Dictionary<string, bool> itemsAffordability;
        void Start()
        {
            StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
            StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
            StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;

        }

        public void onUnexpectedStoreError(int errorCode)
        {
            SoomlaUtils.LogError("ExampleEventHandler", "error with code: " + errorCode);
        }

        public void onSoomlaStoreInitialized()
        {

            // some usage examples for add/remove currency
            // some examples
            if (StoreInfo.Currencies.Count > 0)
            {
                try
                {
                    //How to give currency
                    //StoreInventory.GiveItem(StoreInfo.Currencies[0].ItemId,4000);
                    SoomlaUtils.LogDebug("SOOMLA ExampleEventHandler", "Currency balance:" + StoreInventory.GetItemBalance(StoreInfo.Currencies[0].ItemId));
                    
         
               
                }
                catch (VirtualItemNotFoundException ex)
                {
                    SoomlaUtils.LogError("SOOMLA ExampleWindow", ex.Message);
                }
            }
            setupItemsAffordability();
        }

        public void setupItemsAffordability()
        {
            itemsAffordability = new Dictionary<string, bool>();

            foreach (VirtualGood vg in StoreInfo.Goods)
            {
                itemsAffordability.Add(vg.ID, StoreInventory.CanAfford(vg.ID));
            }
        }

        public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded)
        {
            if (itemsAffordability != null)
            {
                List<string> keys = new List<string>(itemsAffordability.Keys);
                foreach (string key in keys)
                    itemsAffordability[key] = StoreInventory.CanAfford(key);
            }
        }

        public void BuyVirtualGood(VirtualGood virtualGood)
        {
            Debug.Log("SOOMLA/UNITY wants to buy: " + virtualGood.Name);
            try
            {
                StoreInventory.BuyItem(virtualGood.ItemId);
            }
            catch (Exception e)
            {
                Debug.LogError("SOOMLA/UNITY " + e.Message);
            }
        }

        public void BuyCurrencyPack(VirtualCurrencyPack virtualCurrency)
        {
            Debug.Log("SOOMLA/UNITY wants to buy: " + virtualCurrency.Name);
            try
            {
                StoreInventory.BuyItem(virtualCurrency.ItemId);
            }
            catch (Exception e)
            {
                Debug.LogError("SOOMLA/UNITY " + e.Message);
            }
        }
        
        
        //Custom Buy Methods
        public void BuyNoAdItem(string itemId ="remove_ads_item_id"){
            Debug.Log("SOOMLA/UNITY wants to buy: Remove Ads ");
            StoreInventory.BuyItem(itemId);    
        }
        
        public void BuyItemId(string itemId){
            Debug.Log("SOOMLA/UNITY wants to buy: "+itemId);
            StoreInventory.BuyItem(itemId);    
        }
        
        public void BuyOneThousId(string itemId="coins_1000_prod"){
            Debug.Log("SOOMLA/UNITY wants to buy: "+itemId);
            StoreInventory.BuyItem(itemId);    
        }
        
        public void BuyFiveThousId(string itemId="coins_5000_prod"){
            Debug.Log("SOOMLA/UNITY wants to buy: "+itemId);
            StoreInventory.BuyItem(itemId);    
        }
        
        public void BuyTenThousId(string itemId="coins_10000_prod"){
            Debug.Log("SOOMLA/UNITY wants to buy: "+itemId);
            StoreInventory.BuyItem(itemId);    
        }
        
        public void BuyRevealFrogs(string itemId="reveal_frogs"){
            Debug.Log("SOOMLA/UNITY wants to buy: "+itemId);
            StoreInventory.BuyItem(itemId);    
        }
        
        public void BuyResetLevel(string itemId="reset_level"){
            Debug.Log("SOOMLA/UNITY wants to buy: "+itemId);
            StoreInventory.BuyItem(itemId);    
        }

    }


}

//  //Allows for access to Soomla API                                                                              
//  public class ExampleWindow : MonoBehaviour
//  {
//      public Transform cube;                                                                                                                                                  // Stores the scene cube as a variable
//                                                                                                                                                                              //secTime/floatTime are used to check for IAP changes every 2 seconds (ideally, you should only check for updates when you absoultely need to. This just shows a way check for the IAP changes)
//      public float secTime = 2.0f;
//      public float totTime = 0.0f;
//      public bool greenCubeIAPOwned = false;

//      //Load the Scene with the cube/ setup the soomla intergration
//      void Start()
//      {
//          Application.LoadLevel("test");                                                                                                                         //Load actual scene
//          DontDestroyOnLoad(transform.gameObject);                                                                                                        //Allows this gameObject to remain during level loads, solving restart crashes
//                                                                                                                                                          //Intialize the store
//      }

//      //this is likely unnecessary, but may be required depending on how you plan on doing IAPS
//      public void onSoomlaStoreIntitialized()
//      {
//      }

//      //ASSIGN CUBE TO BE COLORED
//      void OnLevelWasLoaded(int level)
//      {                                                                                                                                                                                               //Assigns the cube if the level is loaded to correct level{                                                                                    
//          if (level == 1)
//          {                                                                                                                                                       //the second level in the build list (0 == first level, 1 == second level)
//              cube = GameObject.Find("testCube").transform;                                                                                  //Assign cube by finding it the the hierarchy in the scene ( via its name)
//          }
//      }

//      //UPDATE CUBE COLOR
//      //Assign cube color based on it (using playerprefs) (see CheckIAP_PurchaseStatus() function below to understand)
//      void Update()
//      {
//          if (Time.timeSinceLevelLoad > totTime)
//          {
//              CheckIAP_PurchaseStatus();                                                                                                                             //Check status of in app purchase (true/false if player has purchased it)
//              totTime = Time.timeSinceLevelLoad + secTime;
//          }
//          if (cube != null)
//          {
//              if (!greenCubeIAPOwned)
//              {
//                  cube.transform.renderer.material.color = Color.red;                                                                     // if player has purchased item, turn the cube green
//              }

//              if (greenCubeIAPOwned)
//              {
//                  cube.transform.renderer.material.color = Color.green;                                                           // if player has not purchased item (or hasnt restored previous purchases) turn the cube red
//              }
//          }
//      }

//      //CHECK IAP STATUS (0 = not owned, 1 = owned for GetItemBalance())
//      //Check the Status of the In App Purchase (true/false if player has bought it)
//      void CheckIAP_PurchaseStatus()
//      {
//          Debug.Log(StoreInventory.GetItemBalance("turn_green_item_id"));                                                        // Print the current status of the IAP
//          if (StoreInventory.GetItemBalance("turn_green_item_id") >= 1)
//          {
//              greenCubeIAPOwned = true;               // check if the non-consumable in app purchase has been bought or not
//          }
//      }

//      //GUI ELEMENTS
//      void OnGUI()
//      {
//          //Button To PURCHASE ITEM
//          if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.4f, 150, 150), "Make green?"))
//          {
//              try
//              {
//                  Debug.Log("attempt to purchase");

//                  StoreInventory.BuyItem("turn_green_item_id");                                                                          // if the purchases can be completed sucessfully
//              }
//              catch (Exception e)
//              {                                                                                                                                                                               // if the purchase cannot be completed trigger an error message connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
//                  Debug.Log("SOOMLA/UNITY" + e.Message);
//              }
//          }
//          //Button to RESTORE PURCHASES
//          if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.8f, 150, 150), "Restore\nPurchases"))
//          {
//              try
//              {
//                  SoomlaStore.RestoreTransactions();                                                                                                      // restore purchases if possible
//              }
//              catch (Exception e)
//              {
//                  Debug.Log("SOOMLA/UNITY" + e.Message);                                                                                         // if restoring purchases fails (connectivity issue, IAP doesnt exist on ItunesConnect, etc...)
//              }
//          }

//          //Button to RESTART LEVEL (ensure it doesnt crash)
//          if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height * 0.8f, 150, 150), "Restart"))
//          {
//              Application.LoadLevel(Application.loadedLevel);
//          }
//      }
//  }
