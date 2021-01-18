using LLScreen;
using Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QuickRematch
{
    class CharacterSelector : MonoBehaviour
    {

        public void Initialize(Character _character, CharacterVariant _variant)
        {
            playerIndex = P2P.localPeer?.playerNr ?? 0;
            player = ALDOKEMAOMB.MNOLFHGAIMC(playerIndex);
            character = _character;
            variant = _variant;
        }

        void OnDestroy()
        {

        }

        public static Character prevCharacter { get; private set; } = Character.NONE;
        public static CharacterVariant prevVariant { get; private set; }

        int playerIndex;
        ALDOKEMAOMB player;
        bool characterSelected;
        bool variantSelected;
        public Character character { get; private set; }
        public CharacterVariant variant { get; private set; }

        void Update()
        {
            if (UIScreen.currentScreens[0]?.screenType == ScreenType.PLAYERS && player.PNHOIDECPJE)
            {
                if (player.CHNGAKOIJFE == true)
                {
                    if (player.LALEEFJMMLH != prevCharacter)
                    {
                        prevCharacter = player.LALEEFJMMLH;
                        Debug.Log($"[LLBMM] character set {prevCharacter}");
                    }
                    else if (player.AIINAIDBHJI != prevVariant)
                    {
                        prevVariant = player.AIINAIDBHJI;
                        Debug.Log($"[LLBMM] variant set {prevVariant}");
                    }
                }

                if (prevCharacter == Character.NONE) { return; }

                if (player.CHNGAKOIJFE == true && variantSelected == false)
                {
                    List<CharacterVariant> list = EPCDKLCABNC.LJBIMAPKPME(character, (!player.GAFCIHKIGNM) ? player.CJFLMDNNMIE : -1);
                    if (variant != CharacterVariant.DEFAULT)
                    {
                        int varIndex = list.IndexOf(variant);
                        player.AIINAIDBHJI = list[varIndex - 1];
                        CharacterButtonPress(playerIndex);
                    }
                    variantSelected = true;
                }

                if (player.PNHOIDECPJE == true && characterSelected == false)
                {
                    CharacterButtonPress(playerIndex);
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
            var screenPlayers = (UIScreen.currentScreens[0] as ScreenPlayers);

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
