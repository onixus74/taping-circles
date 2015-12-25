using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class GPGManager : MonoBehaviour
{

 
    public int incrementalCount = 5;

    //leaderboard strings
    private string leaderboard = "CgkIj4_O2YYFEAIQBg";
    //achievement strings
    private string achievement = "CgkIj4_O2YYFEAIQAw";
    private string incremental = "Your unique achievement id";
    
    public static GPGManager instance = null;

    // Use this for initialization

	

    public void Login()
    {
        Social.localUser.Authenticate((bool success) =>
          {
              if (success)
              {
                  Debug.Log("You've successfully logged in");
              }
              else
              {
                  Debug.Log("Login failed for some reason");
              }
          });
    }

    public void UnlockAchievement(string ach)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(ach, 200.0f, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("You've successfully logged in");
                }
                else
                {
                    Debug.Log("Login failed for some reason");
                }
            });
        }
    }

    public void UnlockIncrementalAchievement()
    {

        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).IncrementAchievement(incremental, 5, (bool success) =>
            {
            //The achievement unlocked successfully
        });
        }

    }
    
    
    public void ReportScoreToLeaderboard(int score){
      if (Social.localUser.authenticated)
            {
                Social.ReportScore(score, leaderboard, (bool success) =>
                {
                    
                });
            }
    }
    
    public void ShowLeaderboard(){
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else 
        {
            Login();
        }
    }
    
    public void ShowLeaderboard(string leaderboard){
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard);
        }
        else 
        {
            Login();
        }
    }
    
    public void ShowAchievements(){
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else 
        {
            Login();
        }
    }
    
    public void SignOut(){
      ((PlayGamesPlatform)Social.Active).SignOut();
    }
    
    void Awake()
    {

        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }
    void Start (){
        PlayGamesPlatform.Activate();
        // if (!Social.localUser.authenticated) 
        // {
        //     Login();       
        // }
        // Login(); 
    }
    
}
