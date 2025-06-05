using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Steamworks;

public class SteamAchievement : MonoBehaviour
{
    public SaveJSonData saveJSonData;
    // Update is called once per frame
    void Update()
    {
        if (!SteamManager.Initialized) { return; }
        Shelter_Achievement();
       
    }

    void Shelter_Achievement()
    {
        SteamUserStats.SetAchievement("ABS_WAKE_UP");

        //int playerLevel = saveJSonData.playerStateData.Sum();
        int playerLevel = saveJSonData.playerStateData[0];
        if (playerLevel < 20 && playerLevel >= 10 ) SteamUserStats.SetAchievement("ABS_LEVELUP_10");
        else if(playerLevel < 30 && playerLevel >= 20) SteamUserStats.SetAchievement("ABS_LEVELUP_20");
        else if (playerLevel < 40 && playerLevel >= 30) SteamUserStats.SetAchievement("ABS_LEVELUP_30");
        else if (playerLevel < 50 && playerLevel >= 40) SteamUserStats.SetAchievement("ABS_LEVELUP_40");
        else if (playerLevel < 60 && playerLevel >= 50) SteamUserStats.SetAchievement("ABS_LEVELUP_50");
        else if (playerLevel < 70 && playerLevel >= 60) SteamUserStats.SetAchievement("ABS_LEVELUP_60");
        else if (playerLevel < 80 && playerLevel >= 70) SteamUserStats.SetAchievement("ABS_LEVELUP_70");
        else if (playerLevel < 90 && playerLevel >= 80) SteamUserStats.SetAchievement("ABS_LEVELUP_80");

        SteamUserStats.StoreStats();
    }
}
