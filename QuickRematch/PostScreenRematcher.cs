
using System;
using UnityEngine;
using LLGUI;
using LLHandlers;
using LLScreen;
using Multiplayer;
using LLBML;

namespace QuickRematch
{
    class PostScreenRematcher : MonoBehaviour
    {

        static PostScreen postScreen;

        public PostScreenRematcher()
        {
            if (ScreenApi.CurrentScreens[0]?.screenType == ScreenType.GAME_RESULTS)
            {
                postScreen = ScreenApi.CurrentScreens[0] as PostScreen;
            }
            else
            {
                postScreen = null;
            }
        }

        void OnDestroy()
        {
            screenTimer = null;
            activeButton = null;
            AudioHandler.StopSfxLooping();
        }

        float waitTime = 2f;
        Timer screenTimer;
        LLButton activeButton;
        ResultButtons resultButton = ResultButtons.NONE;

        enum ResultButtons
        {
            NONE,
            REMATCH,
            CONTINUE,
        }

        void LateUpdate()
        {
            Rematcher();
        }

        void Awake()
        {
            waitTime = (float)QuickRematch.Instance.rematchTimer.Value;
            //rematchButton.SetActive(false);
            screenTimer = new Timer(waitTime);
            activeButton = GetButton(postScreen);
            activeButton.onClick += ActiveButtonPressed;
        }

        void ActiveButtonPressed(int playerIndex)
        {
            screenTimer.timeCanceled = true;
        }

        void Rematcher()
        {
            if (postScreen.showWinner == true)
            {
                if (screenTimer.timeCanceled == false)
                {
                    activeButton.SetTextCode($"RESULT_BT_{resultButton}");
                    if (GetMenuButton() || Cursor.visible == true || postScreen.resultButtons == NIPJFJKNGHO.EOCBBKOIFNO)
                    {
                        screenTimer.timeCanceled = true;
                        activeButton.onClick -= ActiveButtonPressed;
                        //rematchButton.SetActive(true);
                    }
                    else
                    {
                        activeButton.SetText($"{activeButton.GetText()}: {Mathf.CeilToInt(screenTimer.waitTime)}");
                    }
                }

                if (screenTimer?.keepWaiting == false && screenTimer.timeCanceled == false)
                {
                    screenTimer.timeCanceled = true;
                    int playerIndex = P2P.localPeer?.playerNr ?? 0;
                    activeButton.OnClick(playerIndex);
                    Debug.Log("[LLBMM] Quick Rematch");
                }
            }
            //DNPFJHMAIBP.GKBNNFEAJGO(new Message(Msg.SEL_REMATCH, -1, 1, null, -1));
        }

        public static bool IsPendingUnlock()
        {
            PostScreen updatePostScreen;
            if (ScreenApi.CurrentScreens[0]?.screenType == ScreenType.GAME_RESULTS)
            {
                updatePostScreen = ScreenApi.CurrentScreens[0] as PostScreen;
            }
            else
            {
                Debug.Log($"[LLBMM] PostScreen Not Found");
                return false;
            }

            if (!OEAINNHEMKA.aborted)
            {
                Character unlockStaticSkin = CheckStaticSkinUnlocked();
                if (unlockStaticSkin != Character.NONE && BlazeUnobfuscated.IsAvaliableForUnlocking(unlockStaticSkin, CharacterVariant.STATIC_ALT) == false)
                {
                    return true;
                }

                int level = EPCDKLCABNC.NKKKADCCLNL();
                int matchesPlayed = BDCINPKBMBL.matchesPlayed;

                if ((level >= 70 || matchesPlayed >= 500) && BlazeUnobfuscated.IsUnlocked(Stage.STREETS_2D) == false)
                {
                    return true;
                }
                if ((level >= 90 || matchesPlayed >= 670) && BlazeUnobfuscated.IsUnlocked(Stage.FACTORY_2D) == false)
                {
                    return true;
                }
                if ((level >= 110 || matchesPlayed >= 840) && BlazeUnobfuscated.IsUnlocked(Stage.SEWERS_2D) == false)
                {
                    return true;
                }
                if ((level >= 130 || matchesPlayed >= 1010) && BlazeUnobfuscated.IsUnlocked(Stage.ROOM21_2D) == false)
                {
                    return true;
                }
                if ((level >= 150 || matchesPlayed >= 1180) && BlazeUnobfuscated.IsUnlocked(Stage.SUBWAY_2D) == false)
                {
                    return true;
                }
                if ((level >= 170 || matchesPlayed >= 1350) && BlazeUnobfuscated.IsUnlocked(Stage.POOL_2D) == false)
                {
                    return true;
                }
                if ((level >= 190 || matchesPlayed >= 1520) && BlazeUnobfuscated.IsUnlocked(Stage.OUTSKIRTS_2D) == false)
                {
                    return true;
                }
            }

            var postScreenunlocks = FindObjectOfType<CPNJEILDILH>();
            if (postScreenunlocks != null)
            {
                //Unlock Zoot Suit
                if (postScreenunlocks.currencyGain <= 0 && postScreenunlocks.currencyPrev >= 9999 && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BOOM, CharacterVariant.MODEL_ALT2) == false)
                {
                    return true;
                }
            }

            if ((CGLLJHHAJAK.GIGAKBJGFDI.DDDJFFIDAFH(Achievement.COMPLETE_STORY) || BDCINPKBMBL.storyCompleted || (OEAINNHEMKA.controlledPlayerWon && JOMBNFKIHIC.PHGDGBCBJJA && AMCFBNJCBGA.GBJFMMHDKAN.eventCode == EventCode.BOSS)) && (CGLLJHHAJAK.GIGAKBJGFDI.DDDJFFIDAFH(Achievement.SPEED_1M) || BDCINPKBMBL.reached1MSpeed) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BOSS, CharacterVariant.MODEL_ALT2) == false)
            {
                return true;
            }

            if (OEAINNHEMKA.controlledPlayerWon == true)
            {
                if (JOMBNFKIHIC.PHGDGBCBJJA)
                {
                    if (AMCFBNJCBGA.GBJFMMHDKAN.eventCode == EventCode.BOSS && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BOSS, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    if (GameStatesStoryGrid.AreAllEventsFullyCompleted() && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.KID, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    Character rewardCharacter = AMCFBNJCBGA.GBJFMMHDKAN.rewardCharacter;
                    if (rewardCharacter != Character.NONE && updatePostScreen.winner.CHCOLMFCMNL <= 0 && BlazeUnobfuscated.IsAvaliableForUnlocking(rewardCharacter, AMCFBNJCBGA.GBJFMMHDKAN.rewardCharacterVariant) == false)
                    {
                        return true;
                    }
                }
                else if (JOMBNFKIHIC.ALEJJFPNIDP && IJMBLEOKIPB.challengeMatches[IJMBLEOKIPB.challengeNr].IFKPFHOFGGD == PBNCNBFDBBN.FBKNKOPIONI)
                {
                    if (updatePostScreen.winnerCharacter == Character.CROC && EPCDKLCABNC.HGHKKPCAKOM(Character.CROC) && (CGLLJHHAJAK.GIGAKBJGFDI.DDDJFFIDAFH(Achievement.COMPLETE_NO_LOSE) || BDCINPKBMBL.arcadeCompletedNoLose) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.CROC, CharacterVariant.MODEL_ALT2) == false)
                    {
                        return true;
                    }
                    if (updatePostScreen.winnerCharacter == Character.ROBOT && EPCDKLCABNC.HGHKKPCAKOM(Character.ROBOT) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.ROBOT, CharacterVariant.MODEL_ALT2) == false)
                    {
                        return true;
                    }
                    if (updatePostScreen.winnerCharacter == Character.GRAF && EPCDKLCABNC.HGHKKPCAKOM(Character.GRAF) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.GRAF, CharacterVariant.MODEL_ALT2) == false)
                    {
                        return true;
                    }
                    if (updatePostScreen.winnerCharacter == Character.COP && EPCDKLCABNC.HGHKKPCAKOM(Character.COP) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.COP, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    if (updatePostScreen.winnerCharacter == Character.ELECTRO && EPCDKLCABNC.HGHKKPCAKOM(Character.ELECTRO) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.ELECTRO, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    if (updatePostScreen.winnerCharacter == Character.CANDY && EPCDKLCABNC.HGHKKPCAKOM(Character.CANDY) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.CANDY, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    if (updatePostScreen.winnerCharacter == Character.SKATE && EPCDKLCABNC.HGHKKPCAKOM(Character.SKATE) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.SKATE, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    if (updatePostScreen.winnerCharacter == Character.BAG && EPCDKLCABNC.HGHKKPCAKOM(Character.BAG) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BAG, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    if (EPCDKLCABNC.JIAKEINJLMN() && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.KID, CharacterVariant.MODEL_ALT2) == false)
                    {
                        return true;
                    }
                }
                else if (!JOMBNFKIHIC.ALEJJFPNIDP && (JOMBNFKIHIC.GIGAKBJGFDI?.PNJOKAICMNN == GameMode.FREE_FOR_ALL || JOMBNFKIHIC.GIGAKBJGFDI?.PNJOKAICMNN == GameMode._1v1 || JOMBNFKIHIC.GIGAKBJGFDI?.PNJOKAICMNN == GameMode.COMPETITIVE) && JOMBNFKIHIC.GIGAKBJGFDI?.BLADHBMDPPK == false)
                {
                    int num2 = World.GetTimeLeftSecs() + 1;
                    if (updatePostScreen.winnerCharacter == Character.GRAF && (num2 % 60 == 11 || num2 % 60 == 33) && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.GRAF, CharacterVariant.MODEL_ALT) == false)
                    {
                        return true;
                    }
                    else if (updatePostScreen.winnerCharacter == Character.BAG && updatePostScreen.winner.AIINAIDBHJI == CharacterVariant.MODEL_ALT && BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BAG, CharacterVariant.MODEL_ALT2) == false)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static Character CheckStaticSkinUnlocked()
        {
            int num = EPCDKLCABNC.NKKKADCCLNL();
            int matchesPlayed = BDCINPKBMBL.matchesPlayed;
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BAG, CharacterVariant.STATIC_ALT) && (num >= 65 || matchesPlayed >= 460))
            {
                return Character.BAG;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.GRAF, CharacterVariant.STATIC_ALT) && (num >= 60 || matchesPlayed >= 420))
            {
                return Character.GRAF;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BOSS, CharacterVariant.STATIC_ALT) && (num >= 55 || matchesPlayed >= 380))
            {
                return Character.BOSS;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.ELECTRO, CharacterVariant.STATIC_ALT) && (num >= 50 || matchesPlayed >= 340))
            {
                return Character.ELECTRO;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.COP, CharacterVariant.STATIC_ALT) && (num >= 45 || matchesPlayed >= 300))
            {
                return Character.COP;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.SKATE, CharacterVariant.STATIC_ALT) && (num >= 40 || matchesPlayed >= 260))
            {
                return Character.SKATE;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.PONG, CharacterVariant.STATIC_ALT) && (num >= 35 || matchesPlayed >= 220))
            {
                return Character.PONG;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.CANDY, CharacterVariant.STATIC_ALT) && (num >= 30 || matchesPlayed >= 190))
            {
                return Character.CANDY;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.CROC, CharacterVariant.STATIC_ALT) && (num >= 25 || matchesPlayed >= 160))
            {
                return Character.CROC;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.BOOM, CharacterVariant.STATIC_ALT) && (num >= 20 || matchesPlayed >= 140))
            {
                return Character.BOOM;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.ROBOT, CharacterVariant.STATIC_ALT) && (num >= 15 || matchesPlayed >= 120))
            {
                return Character.ROBOT;
            }
            if (!BlazeUnobfuscated.IsAvaliableForUnlocking(Character.KID, CharacterVariant.STATIC_ALT) && (num >= 10 || matchesPlayed >= 100))
            {
                return Character.KID;
            }
            return Character.NONE;
        }

        LLButton GetButton(PostScreen postScreen)
        {
            switch (postScreen.resultButtons)
            {
                case NIPJFJKNGHO.DLPDHJFPKMJ: //ResultButtons.REMATCH_QUIT
                    resultButton = ResultButtons.REMATCH;
                    return postScreen.btRematch;
                case NIPJFJKNGHO.PJGPNEKNIGJ: //ResultButtons.CONTINUE
                case NIPJFJKNGHO.DNDFPMJJMJC: //ResultButtons.CONTINUE_QUIT
                    resultButton = ResultButtons.CONTINUE;
                    return postScreen.btContinue;
                default:
                    resultButton = ResultButtons.NONE;
                    return postScreen.btQuit;
            }
        }

        bool GetMenuButton()
        {
            return Controller.all.GetButton(InputAction.MENU_LEFT) | Controller.all.GetButton(InputAction.MENU_RIGHT)
                | Controller.all.GetButton(InputAction.MENU_DOWN) | Controller.all.GetButton(InputAction.MENU_UP) | Controller.all.GetButton(InputAction.BACK);
        }
    }
}
