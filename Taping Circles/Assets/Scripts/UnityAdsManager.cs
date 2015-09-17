using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using Soomla;
using Soomla.Store;

public class UnityAdsManager : MonoBehaviour {
//Just apply this script to an UI Button

    public string COIN_ID = "coin_ID";
    Button btn;
    // Use this for initialization
    void Start () {
        btn = GetComponent<Button>();
        Advertisement.Initialize ("75747");
        Advertisement.allowPrecache = true;
    }

    void LateUpdate()
    {
        if (Advertisement.isReady() && Advertisement.isSupported==true)
        { btn.interactable = true;} else { btn.interactable = false;}
    }

    //UI Button action
    public void ShowAds()
    {
        ShowOptions options = new ShowOptions();
        options.pause=true;
        options.resultCallback=HandleShowResult;
        Advertisement.Show(null,options);
    }
    void HandleShowResult(ShowResult result)
    {
        //Give 200 Coins if the user watched the Ads Completely
        if(result==ShowResult.Finished)
        {
            StoreInventory.GiveItem(COIN_ID, 200);
        }
    }

}



