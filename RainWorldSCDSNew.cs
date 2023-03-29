using BepInEx;
using BepInEx.Logging;
using System.Security.Permissions;
using System.Security;
using UnityEngine;
using MoreSlugcats;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[module: UnverifiableCode]
#pragma warning restore CS0618 // Type or member is obsolete


namespace SCDSforthfix
{
    [BepInPlugin(MOD_ID, MOD_NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static new ManualLogSource Logger { get; private set; } = null!;

        public const string VERSION = "1.1";
        public const string MOD_NAME = "Rain World SCDS";
        public const string MOD_ID = "kadw.rainworldscds"; // keep ID the same, forthbridge did V1.1 though


        public void OnEnable()
        {
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;

            On.Menu.MainMenu.Update += MainMenu_Update;
            On.MoreSlugcats.DatingSim.Update += DatingSim_Update;
        }



        private bool isInit = false;

        private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);

            if (isInit) return;
            isInit = true;

            MachineConnector.SetRegisteredOI(MOD_ID, Options.instance);
        }



        private SlugcatStats.Name? selectedSlug;

        private void MainMenu_Update(On.Menu.MainMenu.orig_Update orig, Menu.MainMenu self)
        {
            orig(self);

            bool enterSimInput = Input.GetKey(Options.keybindEnterSim.Value);

            if (!enterSimInput) return;

            selectedSlug = self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat;
            self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel;

            self.manager.RequestMainProcessSwitch(MoreSlugcatsEnums.ProcessID.DatingSim);
            
            isExitingSim = false;
        }



        private bool isExitingSim = false;
        private bool wasResetSimInput = false;

        private void DatingSim_Update(On.MoreSlugcats.DatingSim.orig_Update orig, DatingSim self)
        {
            orig(self);

            if (isExitingSim) return;

            bool exitSimInput = RWInput.CheckPauseButton(0, self.manager.rainWorld) || Input.GetKey(Options.keybindExitSim.Value);

            if (exitSimInput && !isExitingSim)
            {
                if (self.manager.musicPlayer != null)
                {
                    self.manager.musicPlayer.song.StopAndDestroy();
                    self.manager.musicPlayer.song = null;
                }

                if (selectedSlug != null)
                    self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = selectedSlug;

                else
                    self.manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = SlugcatStats.Name.White;

                self.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.MainMenu);
                self.PlaySound(SoundID.MENU_Switch_Page_Out);
                
                isExitingSim = true;
                return;
            }



            bool resetSimInput = Input.GetKey(Options.keybindResetSim.Value);

            if (!resetSimInput)
                wasResetSimInput = false;

            if (resetSimInput && !wasResetSimInput)
            {
                self.InitNextFile("start.txt");
                wasResetSimInput = true;
            }

        }
    }
}
