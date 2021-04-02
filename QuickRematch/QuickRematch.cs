//#define ShowScreenNames
//#define PlayerStage
using LLGUI;
using LLHandlers;
using LLScreen;
using Multiplayer;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace QuickRematch
{
    class QuickRematch : MonoBehaviour
    {

        private const string modVersion = "1.0.1";
        private const string repositoryOwner = "Daioutzu";
        private const string repositoryName = "LLBMM-QuickRematch";

        public static QuickRematch Instance { get; private set; }
        public static ModMenuIntegration MMI { get; private set; }

        public static void Initialize()
        {
            GameObject gameObject = new GameObject("QuickRematch"); //The game object is what we use to interact with our mod
            Instance = gameObject.AddComponent<QuickRematch>();
            DontDestroyOnLoad(gameObject); // Makes sure our game object isn't destroyed
            Debug.Log("[LLBMM] QuickRematch: Intitialized");
        }

        void Start()
        {
            if (MMI == null) { MMI = gameObject.AddComponent<ModMenuIntegration>(); Debug.Log($"[LLBMM] {Instance.name}: Added GameObject \"ModMenuIntegration\""); }
            MMI.OnInitConfig += AddModOptions;
            Debug.Log($"[LLBMM] {Instance.name} Started");
        }

        static void AddModOptions(ModMenuIntegration MMI)
        {
            MMI.AddToWriteQueue("(bool)autoCharacterSelector", "true");
            MMI.AddToWriteQueue("(bool)autoRematcher", "true");
            MMI.AddToWriteQueue("(slider)rematchTimer", "2|0|6");
        }

        void Update()
        {


#if DEBUG
            PlayerScreenControls(); 
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                BDCINPKBMBL.skinsAvaliableforUnlocking[(int)Character.KID] ^= 1 << (int)CharacterVariant.STATIC_ALT;
                BDCINPKBMBL.skinsAvaliableforUnlocking[(int)Character.ROBOT] ^= 1 << (int)CharacterVariant.STATIC_ALT;
            }
#endif


            CreateCharacterSelector();
            CreateRematcher();
            //PostScreenRematcher();
        }
#if PlayerStage
        void LateUpdate()
        {
            SetFocuToStage();
        }
#endif
        bool autoSelectCharacter = true;
        bool autoRematcherToggle = true;
#if PlayerStage
        static private void Place(LLButton button, Vector2 pos, Vector3 scale)
        {
            RectTransform component = button.GetComponent<RectTransform>();
            component.anchoredPosition = pos;
            component.localScale = scale;
        }

        LLButton CreateStageButton(ScreenPlayersStage playerStage, Stage stage, Vector2 pos)
        {
            Sprite sprite;
            if (stage == (Stage)(-1) || stage > Stage.FACTORY_2D)
            {
                sprite = JPLELOFJOOH.BNFIDCAPPDK("_spritePreviewRandom");
            }
            else if (stage == (Stage)(-2))
            {
                sprite = Resources.Load<Sprite>("Textures/_spritePreviewRandom_2d");
            }
            else
            {
                sprite = JPLELOFJOOH.BNFIDCAPPDK("_spritePreview" + stage);
            }

            LLButton llbutton = LLButton.CreateImageButton(playerStage.transform, sprite, new Color(0.6f, 0.6f, 0.6f, 1), Color.white);
            Place(llbutton, pos, new Vector3(0.4f, 0.4f));
            llbutton.onClick = delegate (int playerIndex)
            {
                SelectStage(playerIndex, (int)stage);
            };
            llbutton.onHoverOut = delegate (int playerIndex)
            {
                AudioHandler.PlaySfx(Sfx.MENU_SCROLL);
            };
            return llbutton;
        }

        static List<LLButton> stageButtons;
        int cursorIndex = 0;

        void SetFocuToStage()
        {
            if (UIScreen.currentScreens[1]?.screenType == ScreenType.PLAYERS_STAGE)
            {
                if (Controller.all.GetButtonDown(InputAction.LEFT))
                {
                    cursorIndex--;
                    if (cursorIndex < 0)
                    {
                        cursorIndex = stageButtons.Count - 1;
                    }
                    UIInput.mainCursor.SetFocus(stageButtons[cursorIndex]);
                }
                else if (Controller.all.GetButtonDown(InputAction.RIGHT))
                {
                    cursorIndex++;
                    if (cursorIndex >= stageButtons.Count)
                    {
                        cursorIndex = 0;
                    }
                    UIInput.mainCursor.SetFocus(stageButtons[cursorIndex]);
                }
                else if (Controller.all.GetButtonDown(InputAction.DOWN))
                {
                    if (cursorIndex < 6)
                    {
                        cursorIndex += 6;
                    }
                    else
                    {
                        cursorIndex -= 6;
                    }

                    if (cursorIndex >= stageButtons.Count)
                    {
                        cursorIndex = stageButtons.Count - 1;
                    }
                    UIInput.mainCursor.SetFocus(stageButtons[cursorIndex]);
                }
                if (Controller.all.GetButtonDown(InputAction.UP))
                {
                    if (cursorIndex > 6)
                    {
                        cursorIndex -= 6;
                    }
                    else
                    {
                        cursorIndex += 6;
                    }

                    if (cursorIndex < 0 || cursorIndex >= stageButtons.Count)
                    {
                        cursorIndex = 0;
                    }
                    UIInput.mainCursor.SetFocus(stageButtons[cursorIndex]);
                }
            }
        }
#endif
        void PrintComponetns(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            string txt = "";
            foreach (var comp in gameObject.GetComponents(typeof(Component)))
            {
                txt += $"{comp}\n";
            }
            Debug.Log(txt);
        }

        void CreateCharacterSelector()
        {
            if (MMI != null)
            {
                autoSelectCharacter = MMI.GetTrueFalse(MMI.configBools["(bool)autoCharacterSelector"]); ;
            }
#if PlayerStage
            if (UIScreen.currentScreens[1]?.screenType == ScreenType.PLAYERS_STAGE && Input.GetKeyDown(KeyCode.LeftShift))
            {

                var playerStage = UIScreen.currentScreens[1] as ScreenPlayersStage;
                int buttonCount = playerStage.stageButtonsContainer.childCount;
                Debug.Log($"[LLBMM] (debug) number of buttons: {buttonCount}");
                LLButton sndLastStage = playerStage.stageButtonsContainer.GetChild(buttonCount - 2).GetComponent<LLButton>();
                int lastButtonIndex = 0;
                Stage lastStage = Stage.ROOM21;

                int startPosX = -520;
                int startPosY = 150;
                int num = 210;
                stageButtons = new List<LLButton>();
                stageButtons.Add(CreateStageButton(playerStage, (Stage)(27), new Vector2(startPosX, startPosY)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.OUTSKIRTS, new Vector2(startPosX + num, startPosY)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.SEWERS, new Vector2(startPosX + num * 2, startPosY)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.JUNKTOWN, new Vector2(startPosX + num * 3, startPosY)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.CONSTRUCTION, new Vector2(startPosX + num * 4, startPosY)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.FACTORY, new Vector2(startPosX + num * 5, startPosY)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.SUBWAY, new Vector2(startPosX, startPosY - 110)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.STADIUM, new Vector2(startPosX + num, startPosY - 110)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.STREETS, new Vector2(startPosX + num * 2, startPosY - 110)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.POOL, new Vector2(startPosX + num * 3, startPosY - 110)));
                stageButtons.Add(CreateStageButton(playerStage, Stage.ROOM21, new Vector2(startPosX + num * 4, startPosY - 110)));

                playerStage.stageButtonsContainer.gameObject.SetActive(false);
                playerStage.btLeft.SetActive(playerStage.btLeft.visible = false);
                playerStage.btRight.SetActive(playerStage.btRight.visible = false);
                UIInput.mainCursor.SetFocus(stageButtons[cursorIndex]);
                PrintComponetns(playerStage.transform.GetChild(0).gameObject);
                Color color;
                ColorUtility.TryParseHtmlString("#242424", out color);
                playerStage.transform.GetChild(0).gameObject.GetComponent<Image>().color = color;
                playerStage.transform.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 153);
                playerStage.transform.GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 95);
                //playerStage.transform.GetChild(4).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 400);
                playerStage.lbTitle.gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 310);


                if (int.TryParse(sndLastStage.name.Trim("Button_".ToCharArray()), out lastButtonIndex))
                {
                    if (lastStage == (Stage)(-1))
                    {
                        sndLastStage.SetSprite(JPLELOFJOOH.BNFIDCAPPDK("_spritePreviewRandom"));
                    }
                    else if (lastStage == (Stage)(-2))
                    {
                        sndLastStage.SetSprite(Resources.Load<Sprite>("Textures/_spritePreviewRandom_2d"));
                    }
                    else
                    {
                        sndLastStage.SetSprite(JPLELOFJOOH.BNFIDCAPPDK("_spritePreview" + lastStage));
                    }
                    sndLastStage.onClick = delegate (int playerIndex)
                    {
                        SelectStage(playerIndex, (int)lastStage);
                    };
                }

                for (int i = 0; i < buttonCount; i++)
                {
                    LLButton currentButton = playerStage.stageButtonsContainer.GetChild(i).GetComponent<LLButton>();
                    Debug.Log($"[LLBMM] (debug) {currentButton.name}");
                    if (currentButton.name == $"Button_{(int)lastStage}")
                    {
                        Stage stage = (Stage)(lastButtonIndex);
                        if (stage == (Stage)(-1))
                        {
                            currentButton.SetSprite(JPLELOFJOOH.BNFIDCAPPDK("_spritePreviewRandom"));
                        }
                        else if (stage == (Stage)(-2))
                        {
                            currentButton.SetSprite(Resources.Load<Sprite>("Textures/_spritePreviewRandom_2d"));
                        }
                        else
                        {
                            currentButton.SetSprite(JPLELOFJOOH.BNFIDCAPPDK("_spritePreview" + stage));
                        }

                        currentButton.onClick = delegate (int playerIndex)
                        {
                            SelectStage(playerIndex, (int)stage);
                        };
                        continue;
                    }
                }

            }
#endif
            if (autoSelectCharacter && UIScreen.currentScreens[0]?.screenType == ScreenType.PLAYERS && (P2P.localPeer?.playerNr > -1 || JOMBNFKIHIC.GDNFJCCCKDM == false))
            {
                if (UIScreen.currentScreens[0]?.GetComponent<CharacterSelector>() == null)
                {
                    var characterSelector = UIScreen.currentScreens[0].gameObject.AddComponent<CharacterSelector>();
                    characterSelector.Init(CharacterSelector.prevCharacter, CharacterSelector.prevVariant);
                    characterSelector.transform.SetParent(UIScreen.currentScreens[0].transform);
#if DebugLog
                    Debug.Log("[LLBMM] CharacterSelector Created");
#endif
                }
            }
        }

#if PlayerStage
        static private void SelectStage(int playerNr, int stage_selection)
        {
            AudioHandler.PlaySfx(Sfx.LOBBY_START_GAME);
            DNPFJHMAIBP.GKBNNFEAJGO(Msg.SEL_STAGE, playerNr, stage_selection);
        }
#endif

#if DEBUG
        static void PlayerScreenControls()
        {

            if (UIScreen.currentScreens[0]?.screenType == ScreenType.PLAYERS)
            {
                ScreenPlayers screenPlayers = UIScreen.currentScreens[0] as ScreenPlayers;
                if (P2P.isHost == true || P2P.localPeer == null)
                {
                    if (Controller.all.GetButtonDown(InputAction.EXPRESS_UP))
                    {
                        if (UIScreen.currentScreens[1]?.screenType == ScreenType.PLAYERS_SETTINGS)
                        {
                            (UIScreen.currentScreens[1] as ScreenPlayersSettings).btBack.onClick(0);
                        }
                        else
                        {
                            screenPlayers.btOptions.onClick(0);
                        }
                    }
                }

                if (Controller.all.GetButtonDown(InputAction.EXPRESS_DOWN))
                {
                    screenPlayers.btGameMode.onClick(0);
                }

                //ALDOKEMAOMB.ICOCPAFKCCE((ALDOKEMAOMB player) =>
                //{
                //    //isLocal && playerIndex == 0
                //    if (Controller.all.GetButtonDown(InputAction.EXPRESS_DOWN))
                //    {
                //        //DNPFJHMAIBP.GKBNNFEAJGO(Msg.SEL_MODE, 0, -1);
                //        P2P.SendAll(new Message(Msg.PEER_CHANGE_SETTING, 0, (int)GameMode.COMPETITIVE, null, -1));
                //    }
                //});
            }

        } 
#endif

#if DEBUG
        static void PostRematchPressed(int playerIndex)
        {
            Debug.Log("[LLBMM] You pressed the Rematch Button");
        } 
#endif

        bool pendingUnlock;

        void CreateRematcher()
        {
            if (MMI != null)
            {
                autoRematcherToggle = MMI.GetTrueFalse(MMI.configBools["(bool)autoRematcher"]);
            }

            if (autoRematcherToggle && !UIScreen.loadingScreenActive && UIScreen.currentScreens[0]?.screenType == ScreenType.GAME_RESULTS)
            {
                if (UIScreen.currentScreens[0]?.GetComponent<PostScreenRematcher>() == null && pendingUnlock == false)
                {
                    var postScreen = UIScreen.currentScreens[0] as PostScreen;
                    pendingUnlock = PostScreenRematcher.IsPendingUnlock();
                    if (postScreen.showWinner == true && pendingUnlock == false)
                    {
                        var postScreenRematcher = UIScreen.currentScreens[0].gameObject.AddComponent<PostScreenRematcher>();
                        postScreenRematcher.transform.SetParent(UIScreen.currentScreens[0].transform);
#if DebugLog
                    Debug.Log("[LLBMM] PostScreenRematcher Created");
#endif
                    }
                }
            }
            else if (pendingUnlock)
            {
                pendingUnlock = false;
            }
        }

        void OnGUI()
        {
#if ShowScreenNames
            //UIScreen.currentScreens
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < UIScreen.currentScreens.Length; i++)
            {
                if (UIScreen.currentScreens[i] == null)
                {
                    continue;
                }
                sb.AppendLine(UIScreen.currentScreens[i].screenType.ToString());
            }
            GUIStyle label = new GUIStyle(GUI.skin.box)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
            };
            GUI.Label(new Rect(0, 120, 390, 60), sb.ToString(), label);
#endif
        }
    }
}
