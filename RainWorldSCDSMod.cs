// Old code saved for posterity.
// New version is RainWorldSCDSNew.cs, by forthbridge.

/*

using BepInEx;
using UnityEngine;

// mod-specific using
using Menu;
using MoreSlugcats;

// Prevent certain MethodAccessException errors, etc. from happening (keep just in case)
using System.Security.Permissions;
#pragma warning disable CS0618 // Do not remove the following line.
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RainWorldSCDSMod
{
    [BepInPlugin("kadw.rainworldscds", "Rain World SCDS", "1.0")]
    public class AutoDatingSimCode : BaseUnityPlugin
    {
        private static SlugcatStats.Name selectedSlug = null;

        public void OnEnable()
        {
            On.Menu.MainMenu.Update += MainMenu_Update;
            On.MoreSlugcats.DatingSim.Update += DatingSim_Update;

            Debug.Log("Rain World SCDS running");
        }

        // Based on MainMenu.eeCheck() --- the method that activates the secret campaign.
        private static bool switched = false; // prevent requesting more than once if key is held for more than 1 frame (not necessary but feels neater?)
        private static void MainMenu_Update(On.Menu.MainMenu.orig_Update orig, MainMenu self)
        {
            orig(self);

            if ((Input.GetKey(KeyCode.D) || RWInput.CheckThrowButton(0, self.manager.rainWorld)) && !switched)
            {
                Debug.Log("Rain World SCDS: Switching to Slugcat Dating Sim.");

                // Switch to Sofanthiel is required for ProcessManager.Update to slow down/pitch down the music
                // Save selectedSlug first so we can return to it after returning to main menu
                selectedSlug = self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat;
                self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel;

                self.manager.RequestMainProcessSwitch(MoreSlugcatsEnums.ProcessID.DatingSim);
                switched = true;
            }
        }

        // Add the ability to quit out of the dating sim back to the main menu, to easily restart.
        private static bool lastPauseButton = false;
        private static void DatingSim_Update(On.MoreSlugcats.DatingSim.orig_Update orig, DatingSim self)
        {
            orig(self);

            bool flag = RWInput.CheckPauseButton(0, self.manager.rainWorld);
            if (flag && !lastPauseButton)
            {
                if (self.manager.musicPlayer != null)
                {
                    self.manager.musicPlayer.song.StopAndDestroy();
                    self.manager.musicPlayer.song = null;
                }

                // Switch back to ordinary slugcat select menu, and return music to normal.
                if (selectedSlug != null)
                {
                    self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = selectedSlug;
                }
                else
                {
                    self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = SlugcatStats.Name.White;
                }

                self.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.MainMenu);
                self.PlaySound(SoundID.MENU_Switch_Page_Out);
                switched = false;
                Debug.Log("Rain World SCDS: Switching to Main Menu.");
            }
            lastPauseButton = flag;
        }
    }
}
*/
