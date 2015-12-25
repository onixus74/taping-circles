using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;
using Grow.Highway;
using Grow.Insights;
using ChartboostSDK;
using System.Collections.Generic;

namespace Soomla.Store.IAP
{
public class CharboostAdsManager : MonoBehaviour {
    private bool hasInterstitial = false;
	private bool hasMoreApps = false;
	private bool hasRewardedVideo = false;
	private bool hasInPlay = false;
	private int frameCount = 0;
    public bool autocache = true;
	public bool showInterstitial = true;
	public bool showMoreApps = true;
	public bool showRewardedVideo = true;
    private List<string> delegateHistory;
    
    void Start(){
        //  Chartboost.setAutoCacheAds(autocache);
        Chartboost.cacheRewardedVideo(CBLocation.Default);
        Chartboost.cacheInterstitial(CBLocation.Default);
    }
    
    void Update() {

    }
    
    public void ShowInterstitial(){
            
            Chartboost.showInterstitial(CBLocation.Default);
    }
    
    public void ShowRewardedVideo(){
        
        Chartboost.showRewardedVideo(CBLocation.Default);
    }
    

    public void ShowMoreApps() {
        Chartboost.showMoreApps(CBLocation.Default);
    }


}
}



