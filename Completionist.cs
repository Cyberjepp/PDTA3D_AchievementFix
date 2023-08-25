using System.Collections;
using System.Threading;
using MelonLoader;
using Steamworks;
using UnityEngine;

namespace PDTA3D_AchievementFix
{
    public class Completionist : MelonMod
    {
        private const SteamGame.Achievement CompletionistAchievement = SteamGame.Achievement.ACH_UNINSTALL_ME;
        private const float CheckDelayInSeconds = 5f;

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            // Only check if game is ready.
            if (Game.instance == null) return;
            
            LoggerInstance.Msg("Requesting current stats from Steam...");
            SteamUserStats.RequestCurrentStats();
            
            // Delay check so Steam has time to respond.
            Game.instance.StartCoroutine(DelayCheck());
        }

        private IEnumerator DelayCheck()
        {
            yield return new WaitForSeconds(CheckDelayInSeconds);
            CheckRequirements();
        }

        private void CheckRequirements()
        {
            LoggerInstance.Msg("Checking requirements...");
            
            SteamGame component = Game.instance.GetComponent<SteamGame>();
            for (var i = 0; i < component.Achievements.Length; i++)
            {
                var currentAchievement = (SteamGame.Achievement) i;
                var isUnlocked = component.Achievements[i];
                
                // We don't need the Completionist achievement to be completed.
                if (currentAchievement == CompletionistAchievement)
                {
                    if (isUnlocked)
                    {
                        LoggerInstance.Msg("You are already a Completionist - congratulations!");
                        return;
                    }
                    
                    continue;
                }

                if (!isUnlocked)
                {
                    LoggerInstance.Msg("There is more work to be done!");
                    return;
                }
            }
            
            Game.instance.UnlockAchievement(CompletionistAchievement);
            LoggerInstance.Msg("You are now a Completionist - congratulations!");
        } 
    }
}