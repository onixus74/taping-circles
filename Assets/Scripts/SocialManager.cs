using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Soomla;
using Soomla.Profile;
using Soomla.Store;
using Soomla.Levelup;
using Grow.Highway;
using Grow.Sync;
using Grow.Leaderboards;

public class SocialManager : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
			 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void FacebookLogin(){
		SoomlaProfile.Login(
			Provider.FACEBOOK                       // Social Provider
			//  new BadgeReward("loggedIn", "Logged In!") // Reward
		);
	}
	
	public void TwitterLogin(){
		SoomlaProfile.Login(
			Provider.TWITTER                        // Social Provider
			//  new BadgeReward("loggedIn", "Logged In!") // Reward
		);
	}
	public void ShareStatusFacebook(){
		if (SoomlaProfile.IsLoggedIn(Provider.FACEBOOK)) {
			SoomlaProfile.UpdateStatusWithConfirmation(
				Provider.FACEBOOK,                      // Provider
				"Check out this great challenging memory game",   // Message to post as status
				"",                                     // Payload
				null  ,                              // Reward
				"Do you want to share this fuckin awsom status?"     // Message to show in the confirmation dialog
			);
		}
		else
		{
			FacebookLogin();
		}
	}
	public void ShareStatusTwitter(){
			if (SoomlaProfile.IsLoggedIn(Provider.TWITTER)) {
			SoomlaProfile.UpdateStatusWithConfirmation(
				Provider.TWITTER,                      // Provider
				"Check out this great challenging memory game",   // Message to post as status
				"",                                     // Payload
				null   ,                                // Reward
				"Do you want to share this fuckin awsom status?"     // Message to show in the confirmation dialog
			);
			}
			else
			{
				TwitterLogin();
			}

	}
	public void MultiShare(){
		SoomlaProfile.MultiShare(
    		"I'm happy. I can be shared everywhere.",
    		""
		);
	}
	
	public void RateMyApp(){
		SoomlaProfile.OpenAppRatingPage();
	}
	
	public void InviteFacebook(){
		SoomlaProfile.Invite(
    Provider.FACEBOOK,                          // Provider
    "Let's use SOOMLA together!",               // Invitation message
    "SOOMLA Invitation",                        // Dialog title
    "",                                         // Payload
    null                                        // Reward
);
	}

}
