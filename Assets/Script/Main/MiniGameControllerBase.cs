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
        [SerializeField] protected GameObject miniGameObject;
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

        protected void Init(float time = 5f)
        {
            gameState = GameState.Init;
            timerController.ResetTimer(time);
            miniGameObject.SetActive(true);

            gameController  = GetComponent<GameController>();
            Time            = miniGameObject.transform.Find("Canvas_1/Time").GetComponent<TextMeshProUGUI>();
            Announce        = miniGameObject.transform.Find("Canvas_1/Announce").GetComponent<TextMeshProUGUI>();
            ReadyStatus     = miniGameObject.transform.Find("Canvas_1/ReadyStatus").GetComponent<TextMeshProUGUI>();

            Time.text = "残り時間：" + timerController.GetTime();
            StartCoroutine(timerController.Countdown());
        }

        // 時間処理
        protected void TimeUpdate(bool draw = true)
        {
            if (!draw) Time.text = "";
            else       Time.text = "残り時間：" + timerController.GetTime();
            
            if (timerController.ElapsedTime == timerController.startTime) ReadyStatus.text = timerController.AnnounceText;
            timerController.TimeUpdate();

            // 経過時間が進んだ時ゲーム開始
            if (timerController.ElapsedTime != timerController.startTime) gameState = GameState.Play;
        }

        // 結果表示
        protected virtual IEnumerator DrawResult(float score, int gameID)
        {
            timerController.StopTimer();
            Announce.text = "終了\n今回のスコア：" + score;
            Data.instance.rankingDate.Add(new RankingData(score, gameID));

            yield return new WaitForSeconds(3.0f);

            // スロットへ
            miniGameObject.SetActive(false);
            gameController.SetStartSlotAndGameID(false);

            drawResultCoroutine = null;

        }

    }
}
