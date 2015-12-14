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

            StartCoroutine("loadMainScene");

        }
        IEnumerator loadMainScene()
        {
            async = Application.LoadLevelAsync(2);
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
            Application.LoadLevel(2);
        }

    }
}


