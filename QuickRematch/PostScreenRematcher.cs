using LLGUI;
using LLHandlers;
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
    class PostScreenRematcher : MonoBehaviour
    {

        PostScreen postScreen;

        public PostScreenRematcher()
        {
            if (UIScreen.currentScreens[0]?.screenType == ScreenType.GAME_RESULTS)
            {
                postScreen = UIScreen.currentScreens[0] as PostScreen;
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

        void Update()
        {
            Rematcher();
        }

         void Awake()
        {
            if (QuickRematch.MMI != null)
            {
                waitTime = (float)QuickRematch.MMI.GetSliderValue("(slider)rematchTimer");
            }
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
                    AudioHandler.StopSfxLooping();
                    Debug.Log("[LLBMM] Quick Rematch");
                }
            }
            //DNPFJHMAIBP.GKBNNFEAJGO(new Message(Msg.SEL_REMATCH, -1, 1, null, -1));
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
