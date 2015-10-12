using UnityEngine;
using System.Collections;
using System;
using Soomla.Profile;

public class SocialManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SoomlaProfile.Initialize();
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
	
	public void GoogleLogin(){
		SoomlaProfile.Login(
			Provider.GOOGLE                        // Social Provider
			//  new BadgeReward("loggedIn", "Logged In!") // Reward
		);
	}
}
