using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class testGPG : MonoBehaviour
{

 
    public int incrementalCount = 5;

    //leaderboard strings
    private string leaderboard = "CgkIj4_O2YYFEAIQBg";
    //achievement strings
    private string achievement = "CgkIj4_O2YYFEAIQAw";
    private string incremental = "Your unique achievement id";

    // Use this for initialization
    void Start()
    {
        PlayGamesPlatform.Activate();
    }

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

    public void UnlockAchievement()
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievement, 200.0f, (bool success) =>
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
    
    
    public void ReportScoreToLeaderboard(){
      if (Social.localUser.authenticated)
            {
                Social.ReportScore(50, leaderboard, (bool success) =>
                {
                    if (success)
                    {
                        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard);
                    }
                    else
                    {
                //Debug.Log("Login failed for some reason");
            }
                });
            }
    }
    
    public void ShowLeaderboard(){
      Social.ShowLeaderboardUI();
    }
    
    public void ShowLeaderboard(string leaderboard){
      ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard);
    }
    
    public void ShowAchievements(){
      Social.ShowAchievementsUI();
    }
    
    public void SignOut(){
      ((PlayGamesPlatform)Social.Active).SignOut();
    }
    
}
