using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Unity1Week_20230619.Main.Game1
{
    public class MiniGameController1 : MiniGameControllerBase, IController, IInitializa
    {
        [SerializeField] PlayerController playerController;
        [SerializeField] CameraController cameraController;
        [SerializeField] FloorController floorController;
        [SerializeField] GameObject ResultPanel;
        [SerializeField] List<TextMeshProUGUI> ResultText = new List<TextMeshProUGUI>();
        [SerializeField] TextMeshProUGUI Announce1;

        public void Init()
        {
            ResultText.Clear();

            base.Init(60);
            Announce.text = "缶飛ばし \nに、チャレンジ！";
            ResultPanel.SetActive(false);

            playerController = miniGameObject.transform.Find("Player").GetComponent<PlayerController>();
            cameraController = miniGameObject.transform.Find("Camera").GetComponent<CameraController>();
            floorController = miniGameObject.transform.Find("Floor").GetComponent<FloorController>();
            Announce1 = miniGameObject.transform.Find("Canvas_1/Announce_1").GetComponent<TextMeshProUGUI>();

            playerController.Init();
            cameraController.Init();
            floorController.Init();

            foreach (Transform child in ResultPanel.transform.Find("Result"))
            {
                ResultText.Add(child.GetComponent<TextMeshProUGUI>());
            }

            Announce1.text = "";
        }

        public void Control()
        {
            base.TimeUpdate();

            if (gameState != GameState.Play) return;

            playerController.Control(timerController.ElapsedTime);
            cameraController.Control();
            floorController.Control();
            UpdateAnnounce();

            if (playerController.phase == Phase.Result && drawResultCoroutine == null)
            {
                gameState = GameState.End;
                ResultPanel.SetActive(true);
                drawResultCoroutine = StartCoroutine(DrawResult(CalcScore(), 1));
            }

        }

        protected override IEnumerator DrawResult(float score, int gameID)
        {
            timerController.StopTimer();
            Data.instance.rankingDate.Add(new RankingData(score, gameID));

            yield return new WaitForSeconds(5.0f);

            // スロットへ
            miniGameObject.SetActive(false);
            gameController.SetStartSlotAndGameID(false);

            drawResultCoroutine = null;

        }

        int CalcScore()
        {
            float distance = floorController.GetDistance();
            ResultText[0].text = $"{(Math.Floor(distance * 100) / 100).ToString()}M";
            ResultText[1].text = ((int)distance).ToString();
            return (int)distance;
        }

        void UpdateAnnounce()
        {
            switch (playerController.phase)
            {
                case Phase.Shuffle: Announce1.text = ShuffleAnnounceText(); break;
                case Phase.Launch:  Announce1.text = "決定：発射"; break;
                case Phase.Result:  Announce1.text = ""; break;
            }
        }

        string ShuffleAnnounceText()
        {
            string text = "";
            var param = playerController.playerParameter.param;
            text = param.id == 6 ? "下を押してください" : "上を押してください";
            return text;
        }
    }
}
