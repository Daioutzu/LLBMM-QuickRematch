//#define DebugLog
using Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LLScreen;
using LLBML;
using LLBML.Players;

namespace QuickRematch
{
    class CharacterSelector : MonoBehaviour
    {

        public void Init(Character _character, CharacterVariant _variant)
        {
            playerIndex = P2P.localPeer?.playerNr ?? 0;
            player = Player.GetPlayer(playerIndex);
            character = _character;
            variant = _variant;
        }

        public static Character prevCharacter { get; private set; } = Character.NONE;
        public static CharacterVariant prevVariant { get; private set; }

        int playerIndex;
        Player player;
        bool characterSelected;
        bool variantSelected;
        Character character;
        CharacterVariant variant;

        void Update()
        {
            if (ScreenApi.CurrentScreens[0]?.screenType == ScreenType.PLAYERS && player.DidJoinedMatch)
            {
                if (player.selected == true)
                {
                    if (player.Character != prevCharacter)
                    {
                        prevCharacter = player.Character;
                    }
                    else if (player.variant != prevVariant)
                    {
                        prevVariant = player.CharacterVariant;
                    }
                }

                if (prevCharacter == Character.NONE) { return; }

                if (player.CharacterVariant == variant)
                {
                    variantSelected = true;
                }

                if (player.selected == true && variantSelected == false)
                {
                    List<CharacterVariant> list = EPCDKLCABNC.LJBIMAPKPME(character, (!player.IsLocalPeer) ? player.nr : -1);
                    if (variant != CharacterVariant.DEFAULT)
                    {
                        int varIndex = list.IndexOf(variant);
                        player.CharacterVariant = list[varIndex - 1];
                        CharacterButtonPress(playerIndex);
                    }
#if DebugLog
                    Debug.Log("[LLBMM] (debug) Variant Button Pushed");
#endif
                    variantSelected = true;
                }

                if (player.Character  == character)
                {
                    characterSelected = true;
                }

                if (player.selected == false && player.DidJoinedMatch == true && characterSelected == false)
                {
                    CharacterButtonPress(playerIndex);
#if DebugLog
                    Debug.Log("[LLBMM] (debug) Character Button Pushed");
#endif
                    characterSelected = true;
                }
            }
            else
            {
                variantSelected = false;
                characterSelected = false;
            }
        }

        void CharacterButtonPress(int playerIndex)
        {
            var screenPlayers = ScreenApi.CurrentScreens[0] as ScreenPlayers;

            for (int i = 0; i <= screenPlayers.pnCharacterButtons.childCount - 1; i++)
            {
                var button = screenPlayers.pnCharacterButtons.GetChild(i).gameObject.GetComponent<PlayersCharacterButton>();
                if (button.character == character)
                {
                    button.btCharacter.OnClick(playerIndex); break;
                }
            }
        }

    }
}
