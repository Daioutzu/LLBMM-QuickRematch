using LLGUI;
using LLHandlers;
using LLScreen;
using Multiplayer;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace QuickRematch
{
    class QuickRematch : MonoBehaviour
    {

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

        private void AddModOptions(ModMenuIntegration MMI)
        {
            MMI.AddToWriteQueue("(slider)rematchTimer", "2|0|6");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (EPCDKLCABNC.KFFJOEAJLEH(Character.PONG, CharacterVariant.ALT4, -1))
                {
                    Debug.Log("[LLBMM] Variant ALT4 is unlocekd");
                }
            }

            if (UIScreen.currentScreens[0]?.screenType == ScreenType.PLAYERS)
            {
                if (UIScreen.currentScreens[0]?.GetComponent<CharacterSelector>() == null)
                {
                    var characterSelector = UIScreen.currentScreens[0].gameObject.AddComponent<CharacterSelector>();
                    characterSelector.Initialize(CharacterSelector.prevCharacter, CharacterSelector.prevVariant);
                    characterSelector.transform.SetParent(UIScreen.currentScreens[0].transform);
                    Debug.Log("[LLBMM] CharacterSelector Created");
                }
            } 
#if DEBUG
            PlayerScreenControls(); 
#endif
            CreateRematcher();
            //PostScreenRematcher();
        }

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

        void CreateRematcher()
        {
            if (!UIScreen.loadingScreenActive && UIScreen.currentScreens[0]?.screenType == ScreenType.GAME_RESULTS && UIScreen.currentScreens[1]?.screenType != ScreenType.UNLOCK)
            {
                if (UIScreen.currentScreens[0]?.GetComponent<PostScreenRematcher>() == null)
                {
                    if ((UIScreen.currentScreens[0] as PostScreen).showWinner == false)
                    {
                        return;
                    }
                    var postScreenRematcher = UIScreen.currentScreens[0].gameObject.AddComponent<PostScreenRematcher>();
                    postScreenRematcher.transform.SetParent(UIScreen.currentScreens[0].transform);
                    Debug.Log("[LLBMM] PostScreenRematcher Created");
                }
            }
        }

        void OnGUI()
        {
#if DEBUG
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
