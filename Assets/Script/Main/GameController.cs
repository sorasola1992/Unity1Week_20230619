using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity1Week_20230619.Main
{
    public class GameController : MonoBehaviour
    {
        // ゲーム決定時初期化
        int _gameID = Define.DEFAULT_GAMEID;
        public int gameID
        {
            get => _gameID;
            set
            {
                _gameID = value;
                if (Define.DEFAULT_GAMEID != value)
                {
                    ChangeGameInit();
                }
            }
        }

        // 操作リスト
        delegate void IFunction();
        List<IFunction> ControlList = new List<IFunction>();
        List<IFunction> InitList    = new List<IFunction>();

        //
        bool isMiniGame = false;

        void Start()
        {            
            // 操作処理
            ControlList.Add(GetComponent<Game0.MiniGameController0>().Control);
            ControlList.Add(GetComponent<Game1.MiniGameController1>().Control);
            ControlList.Add(GetComponent<Game2.MiniGameController2>().Control);
            ControlList.Add(GetComponent<Game3.MiniGameController3>().Control);

            // 初期化処理
            InitList.Add(GetComponent<Game0.MiniGameController0>().Init);
            InitList.Add(GetComponent<Game1.MiniGameController1>().Init);
            InitList.Add(GetComponent<Game2.MiniGameController2>().Init);
            InitList.Add(GetComponent<Game3.MiniGameController3>().Init);
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().name != Define.GAME_MAIN) return;
            if (!isMiniGame) GetComponent<SlotController>().Control();
            else             ControlList[(int)gameID].Invoke();
        }

        void ChangeGameInit()
        {
            InitList[(int)gameID].Invoke();
        }

        /// <summary>
        /// bool型乱数の取得
        /// </summary>
        /// <param name="isMiniGame">ミニゲーム中か</param>
        /// <param name="id">ミニゲーム番号スロット時-1</param>
        public void SetStartSlotAndGameID(bool isMiniGame, int gameID = Define.DEFAULT_GAMEID)
        {
            this.isMiniGame = isMiniGame;
            this.gameID     = gameID;
            //DebugLogger.Log($"isMiniGame:{isMiniGame} gameID:{gameID}");

        }
    }
}
