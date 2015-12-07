using UnityEngine;
using System.Collections;
using Grow.Highway;
using Grow.Insights;
using Grow.Sync;
using Grow.Gifting;
using Grow.Leaderboards;
using Soomla.Profile;
using Soomla;
using System.Collections.Generic;
using System;


namespace Soomla.Store.IAP
{
    public class MainMenuManager : MonoBehaviour
    {
        AsyncOperation async;

        void Start()
        {
            StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;

            StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;

           
            GrowHighway.Initialize();
            bool modelSync = true;
            bool stateSync = true;
            GrowSync.Initialize(modelSync, stateSync);
            GrowGifting.Initialize();
            SoomlaProfile.Initialize();
            GrowInsights.Initialize();
            SoomlaStore.Initialize(new IAPAssets());
            StartCoroutine("loadMainScene");

        }
        IEnumerator loadMainScene()
        {
            async = Application.LoadLevelAsync(1);
            async.allowSceneActivation = false;
            yield return async;
        }

        //[LINKED to Button]
        //Start Game when button Play CLicked
        public void loadGame()
        {
            async.allowSceneActivation = true;
        }
        public void loadGameNotAsync(){
            Application.LoadLevel(1);
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

        }
    }
}


