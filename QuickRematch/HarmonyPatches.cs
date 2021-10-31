using System;
using System.Collections;
using HarmonyLib;
using LLScreen;
using LLBML;
using LLBML.Players;

namespace QuickRematch
{

    public static class CharacterRepicker_Patch
    {

        [HarmonyPatch(typeof(ScreenPlayersStage), nameof(ScreenPlayersStage.OnOpen))]
        [HarmonyPostfix]
        public static void SaveCharacter()
        {
            QuickRematch.StoreCharacterSelected(Player.GetPlayer(NetworkApi.LocalPlayerNumber));
            QuickRematch.Log.LogInfo("Saving char: " + QuickRematch.characterSelected +" | " + QuickRematch.variantSelected);
        }

        [HarmonyPatch(typeof(ALDOKEMAOMB), nameof(ALDOKEMAOMB.EKNFACPOJCM))] // Player.JoinMatch
        [HarmonyPostfix]
        public static void Reselect_Character(ALDOKEMAOMB __instance)
        {
            if (!QuickRematch.autoSelectCharacter.Value || QuickRematch.characterSelected == Character.NONE)
                return;

            Player player = __instance;
            if (player.nr == NetworkApi.LocalPlayerNumber)
            {
                QuickRematch.Log.LogInfo("Restoring char to " + QuickRematch.characterSelected + " | " + QuickRematch.variantSelected);
                StateApi.SendMessage(Msg.SEL_CHAR, player.nr, (int)QuickRematch.characterSelected);
                if (QuickRematch.characterSelected != Character.RANDOM && QuickRematch.variantSelected != CharacterVariant.DEFAULT)
                {
                    Player.GetPlayer(player.nr).CharacterVariant = QuickRematch.variantSelected - 1;
                    StateApi.SendMessage(Msg.SEL_SKIN, player.nr, player.nr);
                    //StateApi.SendMessage(Msg.SEL_READY, player.nr, player.nr);
                }
            }
        }
    }
    public static class QuickRematch_Patch
    {
        [HarmonyPatch(typeof(PostScreen), nameof(PostScreen.ShowButtonsNow))]
        [HarmonyPostfix]
        public static void RematchAfterGains(PostScreen __instance)
        {
            if (!QuickRematch.autoRematch.Value)
                return;

            QuickRematch.Log.LogInfo("Rematching");

            switch (__instance.resultButtons)
            {
                case NIPJFJKNGHO.DLPDHJFPKMJ: //ResultButtons.REMATCH_QUIT
                    StateApi.SendMessage(new Message(Msg.SEL_REMATCH, -1, 1, null, -1));
                    break;
                case NIPJFJKNGHO.PJGPNEKNIGJ: //ResultButtons.CONTINUE
                case NIPJFJKNGHO.DNDFPMJJMJC: //ResultButtons.CONTINUE_QUIT
                    StateApi.SendMessage(Msg.START, -1, -1);
                    break;
                default:
                    DNPFJHMAIBP.GKBNNFEAJGO(new Message(Msg.SEL_REMATCH, -1, 0, null, -1));
                    break;
            }
            QuickRematch.Log.LogInfo("Rematched");
        }

        [HarmonyPatch(typeof(PostScreen), nameof(PostScreen.CFillXpBar))]
        [HarmonyPostfix]
        public static IEnumerator DisableXpGains(IEnumerator incoming)
        {
            QuickRematch.Log.LogInfo("StoppedXpGains_PassPost");
            yield break;
        }

        [HarmonyPatch(typeof(CPNJEILDILH), nameof(CPNJEILDILH.PEORFKFKGGGG))]
        [HarmonyPostfix]
        public static IEnumerator DisableCurrencyGains(IEnumerator incoming)
        {
            QuickRematch.Log.LogInfo("StoppedCurrencyGains_PassPost");
            yield break;
        }

        /*
        [HarmonyPatch(typeof(CPNJEILDILH), nameof(CPNJEILDILH.CNNHCLFLALF))]
        [HarmonyPrefix]
        public static void SkipWinOn(ref bool __1)
        {
            QuickRematch.Log.LogInfo("Turned on SkipWin");
            __1 = true;
        }*/

        /*
        public static IEnumerator Break()
        {
            yield break;
        }

        [HarmonyPatch(typeof(PostScreen), nameof(PostScreen.CFillXpBar))]
        [HarmonyPrefix]
        public static void DisableXpGains_Pre(ref IEnumerator __result)
        {
            QuickRematch.Log.LogInfo("StoppedXpGains_pre");
            __result = Break();
        }
        [HarmonyPatch(typeof(CPNJEILDILH), nameof(CPNJEILDILH.PEORFKFKGGGG))]
        [HarmonyPrefix]
        public static void DisableCurrencyGains_Pre(ref IEnumerator __result)
        {
            QuickRematch.Log.LogInfo("StoppedCurrencyGains_pre");
            __result = Break();
        }*/
    }
}
