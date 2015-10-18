using UnityEngine;
using System.Collections;
using Grow.Highway;
using Grow.Insights;
using Grow.Sync;
using Grow.Gifting;
using Grow.Leaderboards;
using Soomla.Profile;
using Soomla;
namespace Soomla.Store.IAP
{
public class MainMenuManager : MonoBehaviour
{
    AsyncOperation async;

    void Start()
    {
        GrowHighway.Initialize();
        GrowInsights.Initialize();
        bool modelSync = true;
        bool stateSync = true;
        GrowSync.Initialize(modelSync, stateSync);
        GrowGifting.Initialize();
        SoomlaProfile.Initialize();
        SoomlaStore.Initialize(new IAPAssets());
        StartCoroutine("loadMainScene");

    }
    IEnumerator loadMainScene()
    {
        async = Application.LoadLevelAsync(1);
        async.allowSceneActivation = false;
        yield return async;
    }
    public void loadGame()
    {
        async.allowSceneActivation = true;
    }
}
}


