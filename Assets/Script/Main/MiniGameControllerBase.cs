using System.Collections;
using TMPro;
using UnityEngine;

namespace Unity1Week_20230619.Main
{
    public enum GameState
    {
        Init,
        Play,
        End,
    }

    public class MiniGameControllerBase : MonoBehaviour
    {
        [SerializeField] GameObject gameObject;
        protected TextMeshProUGUI Time, Announce, ReadyStatus;
        protected TimerController timerController = new TimerController();
        protected Coroutine drawResultCoroutine;
        protected GameController gameController;


        GameState _gameState = GameState.Init;
        protected GameState gameState
        {
            get => _gameState;
            set
            {
                if (_gameState != value)
                {
                    //DebugLogger.Log($"{value} →{_gameState}");
                    if (_gameState == GameState.Init)
                    {
                        Announce.text = "";
                    }
                    _gameState = value;

                }
            }
        }

        protected void Init()
        {
            gameState = GameState.Init;
            timerController.ResetTimer();
            gameObject.SetActive(true);

            gameController  = GetComponent<GameController>();
            Time            = gameObject.transform.Find("Canvas_1/Time").GetComponent<TextMeshProUGUI>();
            Announce        = gameObject.transform.Find("Canvas_1/Announce").GetComponent<TextMeshProUGUI>();
            ReadyStatus     = gameObject.transform.Find("Canvas_1/ReadyStatus").GetComponent<TextMeshProUGUI>();

            Time.text = "残り時間：" + timerController.GetTime();
            StartCoroutine(timerController.Countdown());
        }

        protected void Control()
        {
            //
            TimeUpdate();

        }

        // 時間処理
        void TimeUpdate()
        {
            if (timerController.ElapsedTime == timerController.startTime) ReadyStatus.text = timerController.AnnounceText;
            else                                                          Time.text = "残り時間：" + timerController.GetTime();
            timerController.TimeUpdate();

            // 経過時間が進んだ時ゲーム開始
            if (timerController.ElapsedTime != timerController.startTime) gameState = GameState.Play;
        }

        // 結果表示
        protected IEnumerator DrawResult(float score, int gameID)
        {
            timerController.StopTimer();
            Announce.text = "終了\n今回のスコア：" + score;
            Data.instance.rankingDate.Add(new RankingData(score, gameID));

            yield return new WaitForSeconds(3.0f);

            // スロットへ
            gameObject.SetActive(false);
            gameController.SetStartSlotAndGameID(false);

            drawResultCoroutine = null;

        }

    }
}
