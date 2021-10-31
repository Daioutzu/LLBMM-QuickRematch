using System;
using HarmonyLib;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using LLBML.Players;

namespace QuickRematch
{
    [BepInPlugin(PluginInfos.PLUGIN_ID, PluginInfos.PLUGIN_NAME, PluginInfos.PLUGIN_VERSION)]
    [BepInDependency(LLBML.PluginInfos.PLUGIN_ID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("no.mrgentle.plugins.llb.modmenu", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("LLBlaze.exe")]
    class QuickRematch : BaseUnityPlugin
    {
        public static ManualLogSource Log { get; private set; } = null;
        public static QuickRematch Instance { get; private set; } = null;
        public static ConfigEntry<bool> autoRematch;
        public static ConfigEntry<bool> autoSelectCharacter;
        public static ConfigEntry<bool> reselectRandom;

        public static Character characterSelected = Character.NONE;
        public static CharacterVariant variantSelected = CharacterVariant.DEFAULT;

        void Awake()
        {
            Instance = this;
            Log = this.Logger;
            AddModOptions(this.Config);

            var harmoInstance = new Harmony(PluginInfos.PLUGIN_ID);
            Logger.LogInfo("Patching QuickRematch");
            harmoInstance.PatchAll(typeof(QuickRematch_Patch));
            Logger.LogInfo("Patching CharacterRepicker");
            harmoInstance.PatchAll(typeof(CharacterRepicker_Patch));
        }

        void Start()
        {
            Logger.LogInfo($"{PluginInfos.PLUGIN_NAME} Started");

            LLBML.Utils.ModDependenciesUtils.RegisterToModMenu(this.Info);
        }

        void AddModOptions(ConfigFile config)
        {
            autoRematch = config.Bind<bool>("Toggles", "autoRematch", true);
            autoSelectCharacter = config.Bind<bool>("Toggles", "autoSelectCharacter", true);
            reselectRandom = config.Bind<bool>("Toggles", "reselectRandom", true);
        }


        public static void StoreCharacterSelected(Player player)
        {
            if (player.CharacterSelectedIsRandom && reselectRandom.Value)
            {
                characterSelected = Character.RANDOM;
                variantSelected = CharacterVariant.DEFAULT;
            }
            else
            {
                characterSelected = player.Character;
                variantSelected = player.CharacterVariant;
            }
        }
    }
}
