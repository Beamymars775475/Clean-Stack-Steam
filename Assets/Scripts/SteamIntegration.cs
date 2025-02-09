using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SteamIntegration : MonoBehaviour
{

    public void IsThisAchievementUnlocked(string id) 
    { 
        var ach = new Steamworks.Data.Achievement("id"); 

        Debug.Log($"Achievement {id} status: " + ach.State); 
    } 

    public void UnlockAchievement(string id) 
    { 
        var ach = new Steamworks.Data.Achievement(id); 
        ach.Trigger(); 
        
        Debug.Log($"Achievement {id} unlocked"); 
    } 

    public void ClearAchievementStatus(string id)
    { 
        var ach = new Steamworks.Data.Achievement(id); 

        ach.Clear(); 
        Debug.Log($"Achievement (id) cleared"); 
    }


    void Awake()
    {
        try
        {
            Steamworks.SteamClient.Init(3276530);
            Debug.Log(Steamworks.SteamClient.Name);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        UnlockAchievement("ACH_TEST");
    }

    // Update is called once per frame
    void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        Debug.Log("Shutdown Steamworks !");
        Steamworks.SteamClient.Shutdown();
    }

}
