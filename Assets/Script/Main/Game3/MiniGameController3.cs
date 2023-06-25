using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Unity1Week_20230619.Main.Game3
{
    public class MiniGameController3 : MiniGameControllerBase, IController, IInitializa
    {

        [SerializeField] PlayerController playerController;
        [SerializeField] GameObject ResultCanvas;
        List<TextMeshProUGUI> ScoreText = new List<TextMeshProUGUI>();


        public void Init()
        {
            ResultCanvas.SetActive(false);

            ScoreText.Clear();

            base.Init(30);
            Announce.text = "下味調理\nに、チャレンジ！";

            playerController = miniGameObject.transform.Find("Player").GetComponent<PlayerController>();

            playerController.Init();

            foreach (Transform obj in ResultCanvas.transform.Find("ResultPanel/Result"))
            {
                ScoreText.Add(obj.GetComponent<TextMeshProUGUI>());
            }

        }

        public void Control()
        {
            base.TimeUpdate();

            if (gameState != GameState.Play) return;

            if (timerController.ElapsedTime <= 0f)
            {
                gameState = GameState.End;
                ResultUpdete();
            }
            else
            {
                playerController.Controller();
            }

            if (gameState == GameState.End && drawResultCoroutine == null)
            {
                drawResultCoroutine = StartCoroutine(DrawResult(playerController.GetScore(), 3));
            }
        }

        void ResultUpdete()
        {
            ScoreText[0].text = $"成功数：{playerController.success}回";
            ScoreText[1].text = $"失敗数：{playerController.miss}回";
            ScoreText[2].text = $"今回のスコア：{playerController.GetScore()}(成功数-失敗数)×{100 * playerController.point_offset}";
        }

        protected override IEnumerator DrawResult(float score, int gameID)
        {
            timerController.StopTimer();
            Data.instance.rankingDate.Add(new RankingData(score, gameID));
            ResultCanvas.SetActive(true);

            yield return new WaitForSeconds(5.0f);

            // スロットへ
            miniGameObject.SetActive(false);
            gameController.SetStartSlotAndGameID(false);

            drawResultCoroutine = null;

        }
    }
}

