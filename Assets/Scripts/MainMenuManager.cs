using UnityEngine;
using System.Collections;
using Grow.Highway;
using Grow.Insights;
using Grow.Sync;
using Grow.Gifting;
using Grow.Leaderboards;
using Soomla.Profile;


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
        StartCoroutine("loadMainScene");
        SoomlaProfile.Initialize();

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


