using System;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using LLScreen;
using LLBML;
using LLBML.States;
using LLBML.Players;

namespace QuickRematch
{

    public static class CharacterRepicker_Patch
    {

        [HarmonyPatch(typeof(ScreenPlayersStage), nameof(ScreenPlayersStage.OnOpen))]
        [HarmonyPostfix]
        public static void SaveCharacter()
        {
            QuickRematch.StoreCharacterSelected(Player.GetLocalPlayer());
        }
        /*
        [HarmonyPatch(typeof(ALDOKEMAOMB), nameof(ALDOKEMAOMB.EKNFACPOJCM))] // Player.JoinMatch
        [HarmonyPostfix]
        public static void JoinMatch_ReselectCharacter(ref ALDOKEMAOMB __instance)
        {
            Player player = __instance;
            if (player.nr == NetworkApi.LocalPlayerNumber)
            {
                QuickRematch.RestoreCharacter(player);
                if (GameStates.IsInOnlineLobby())
                {
                    QuickRematch.QuickReready();
                }
            }
        }
        */

        private static bool doRefresh = false;
        [HarmonyPatch(typeof(HDLIJDBFGKN), "GDHHGDONCKN")] // GameStatesLobbyOnline.ReceivedPlayerState
        [HarmonyPrefix]
        public static bool ReceivePlayerState_Prefix(JMLEHJLKPAC CNPOFPNNPAB, bool IDPADEAGKPJ)
        {
            bool inResults = IDPADEAGKPJ;
            if (inResults)
                return true;
            PlayerLobbyState pls = CNPOFPNNPAB;
            if (pls.playerNr == NetworkApi.LocalPlayerNumber)
            {
                doRefresh = 
                    pls.character != (Character)QuickRematch.storedCharacter.Value ||
                    pls.variant != (CharacterVariant)QuickRematch.storedVariant.Value ||
                    !pls.ready;
            }
            return true;
        }

        [HarmonyPatch(typeof(HDLIJDBFGKN), "GDHHGDONCKN")] // GameStatesLobbyOnline.ReceivedPlayerState
        [HarmonyPostfix]
        public static void ReceivePlayerState_ReselectCharacter(JMLEHJLKPAC CNPOFPNNPAB, bool IDPADEAGKPJ)
        {
            bool inResults = IDPADEAGKPJ;
            if (inResults)
                return;
            if (doRefresh)
            {
                QuickRematch.RestoreCharacter(Player.GetLocalPlayer());
                QuickRematch.QuickReready();
                doRefresh = false;
            }
        }

        /*
        private static bool previousLobbyState = false;
        private static int SetLobbyActiveCount = 0;
        [HarmonyPatch(typeof(HDLIJDBFGKN), "ELCMCCEHBOP")] // GameStatesLobbyOnline.SetLobbyActive
        [HarmonyPostfix]
        public static void ReceivePlayerState_SetLobbyActive(bool __0, bool __1)
        {
            bool lobbyActive = __0;
            bool forced = __1;
            SetLobbyActiveCount++;
            if (previousLobbyState != lobbyActive)
            {
                QuickRematch.Log.LogInfo("SetLobbyActive new state: " + lobbyActive + " | forced? " + forced +" | On update n" + SetLobbyActiveCount);
                previousLobbyState = lobbyActive;
                if (lobbyActive == true || (lobbyActive == false && forced == true))
                {
                    QuickRematch.RestoreCharacter(Player.GetLocalPlayer());
                    QuickRematch.QuickReready();
                }
            }
        }
        */
        /*
        private static AccessTools.FieldRef<HDLIJDBFGKN, bool> fr_autoready = AccessTools.FieldRefAccess<HDLIJDBFGKN, bool>("BOEPIJPONCK");
        [HarmonyPatch(typeof(ScreenPlayers), nameof(ScreenPlayers.ShowReadyButton))]
        [HarmonyPostfix]
        public static void QuickReready(ScreenPlayers __instance, int playerNr, bool visible)
        {
            if (visible)
            {
                if (!Player.GetPlayer(playerNr).ready)
                {
                    if (GameStates.IsInOnlineLobby() && fr_autoready.Invoke(GameStatesLobbyUtils.GetOnlineLobby()))
                    {
                        GameStatesLobbyUtils.MakeSureReadyIs(true, true);
                        GameStates.Send(Msg.SEL_READY, playerNr, playerNr);
                    }
                }
            }
        }*/

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
                    GameStates.Send(new Message(Msg.SEL_REMATCH, -1, 1, null, -1));
                    break;
                case NIPJFJKNGHO.PJGPNEKNIGJ: //ResultButtons.CONTINUE
                case NIPJFJKNGHO.DNDFPMJJMJC: //ResultButtons.CONTINUE_QUIT
                    GameStates.Send(Msg.START, -1, -1);
                    break;
                default:
                    GameStates.Send(new Message(Msg.SEL_REMATCH, -1, 0, null, -1));
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
