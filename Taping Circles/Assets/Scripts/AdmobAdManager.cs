using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class AdmobAdManager : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;

	public string Admob_BannerID;
	public string Admob_InterstitialID;

	public AdPosition Position;

	//public bool TestMode;
	public bool LoadBannerOnInit;
	private static AdmobAdManager admobinstance ;

	// description :  if you want to call ads on your gameover than you have to call this method on your game over event : AdmobAdManager.admobinstance.LoadInterstitialAds();
	void Awake()
	{
		if(!admobinstance)
		{
			admobinstance=this;
		}
		else
		{
			Destroy(this.gameObject) ;
		}

		DontDestroyOnLoad(this.gameObject) ;
	}
	void Start()
	{
		if(LoadBannerOnInit)
		{
			LoadBannerAds ();
		}
	}



//	void OnMouseDown()
//	{
//		///Debug.Log (GUIMenuButton.ComeFromGame);
//		if (gameObject.tag=="Full") 
//		{
//			LoadInterstitialAds();	
//		//	GUIMenuButton.ComeFromGame=false;
//		}
//		if(gameObject.tag=="Banner")
//		{
//			LoadBannerAds ();
//		}
//	}
	public void LoadBannerAds()
	{
		StopCoroutine ("show2");
		RequestBanner();
		StartCoroutine ("show2");
	}
	IEnumerator show2()
	{
		yield return new WaitForSeconds (1);
		bannerView.Show();
	}
	public void HideBannerAds()
	{
		bannerView.Hide();
	}
	public void DistroyBannerAds()
	{
		bannerView.Destroy();
	}

	public void LoadInterstitialAds()
	{
		//HideBannerAds ();
		//DistroyBannerAds ();

		StopCoroutine ("shows");
		RequestInterstitial();
		StartCoroutine ("shows");
	}
	IEnumerator shows()
	{
		yield return new WaitForSeconds (4);
		ShowInterstitialAds ();
	}
	public void ShowInterstitialAds()
	{
		ShowInterstitial();
	}
	public void DistroyInterstitialAds()
	{
		interstitial.Destroy();
	}


    private void RequestBanner()
    {
        #if UNITY_EDITOR
            string adUnitId = "unused";
        #elif UNITY_ANDROID
			string adUnitId = Admob_BannerID;
        #elif UNITY_IPHONE
			string adUnitId = Admob_BannerID;
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, Position);
        // Register for ad events.
        bannerView.AdLoaded += HandleAdLoaded;
        bannerView.AdFailedToLoad += HandleAdFailedToLoad;
        bannerView.AdOpened += HandleAdOpened;
        bannerView.AdClosing += HandleAdClosing;
        bannerView.AdClosed += HandleAdClosed;
        bannerView.AdLeftApplication += HandleAdLeftApplication;
        // Load a banner ad.
        bannerView.LoadAd(createAdRequest());
    }

    public void RequestInterstitial()
    {
        #if UNITY_EDITOR
            string adUnitId = "unused";
        #elif UNITY_ANDROID
			string adUnitId = Admob_InterstitialID;
        #elif UNITY_IPHONE
			string adUnitId = Admob_InterstitialID;
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create an interstitial.
        interstitial = new InterstitialAd(adUnitId);
        // Register for ad events.
        interstitial.AdLoaded += HandleInterstitialLoaded;
        interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.AdOpened += HandleInterstitialOpened;
        interstitial.AdClosing += HandleInterstitialClosing;
        interstitial.AdClosed += HandleInterstitialClosed;
        interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        interstitial.LoadAd(createAdRequest());
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                .AddKeyword("game")
                .SetGender(Gender.Male)
                .SetBirthday(new DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();

    }

    private void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            print("Interstitial is not ready yet.");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");

    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
		//DistroyInterstitialAds ();
		//LoadBannerAds ();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion
}
