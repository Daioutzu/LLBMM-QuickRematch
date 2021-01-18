using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickRematch
{
#if DEBUG
    static class BlazeUnobfuscated
    {
        static public JOFJHDJHJGI CurrentGameState => DNPFJHMAIBP.HHMOGKIMBNM();
        static public bool isOnline => JOMBNFKIHIC.GDNFJCCCKDM;
        static public OnlineMode onlineMode => JOMBNFKIHIC.EAENFOJNNGP;

        public enum GameState
        {
            NONE,
            INTRO,
            MENU,
            QUIT,
            LOBBY_LOCAL,
            LOBBY_ONLINE,
            LOBBY_CHALLENGE,
            CHALLENGE_LADDER,
            CHALLENGE_LOST,
            STORY_GRID,
            STORY_COMIC,
            LOBBY_TRAINING,
            LOBBY_TUTORIAL,
            CREDITS,
            OPTIONS_GAME,
            OPTIONS_INPUT,
            OPTIONS_AUDIO,
            OPTIONS_VIDEO,
            GAME_INTRO,
            GAME,
            GAME_PAUSE,
            GAME_RESULT,
            UNLOCKS,
            LOBBY_STORY
        }

        public static GameState GetCurrentGameState()
        {
            int gameState = (int)CurrentGameState;
            if ((GameState)gameState != null)
            {
                return (GameState)gameState;
            }
            else
            {
                return GameState.NONE;
            }
        }

    } 
#endif

}
