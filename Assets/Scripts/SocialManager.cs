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

	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void FacebookLogin(){
		SoomlaProfile.Login(
			Provider.FACEBOOK                       // Social Provider
			//  new BadgeReward("loggedIn", "Logged In!") // Reward
		);
	}
	
	public void GoogleLogin(){
		SoomlaProfile.Login(
			Provider.GOOGLE                        // Social Provider
			//  new BadgeReward("loggedIn", "Logged In!") // Reward
		);
	}
	public void Showleaderboard(){
		


	}
	void Start () {
			 
	}
}
