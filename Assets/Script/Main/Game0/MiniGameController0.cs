using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Unity1Week_20230619.Main.Game0
{
    public class MiniGameController0 : MiniGameControllerBase, IController, IInitializa
    {

        [SerializeField] HammerController hammerController;
        [SerializeField] MoguraController moguraController;
        [SerializeField] GameObject ResultPanel;
        [SerializeField] List<TextMeshProUGUI> ResultText = new List<TextMeshProUGUI>();

        public void Init()
        {
            ResultText.Clear();

            base.Init(60);
            Announce.text = "モグラ叩き \nに、チャレンジ！";

            hammerController = miniGameObject.transform.Find("Hammer").GetComponent<HammerController>();
            moguraController = miniGameObject.transform.Find("MoguraObj").GetComponent<MoguraController>();
            hammerController.Init();
            moguraController.Init();

            ResultPanel.gameObject.SetActive(false);

            foreach (UnityEngine.Transform child in ResultPanel.transform.Find("Result"))
            {
                ResultText.Add(child.GetComponent<TextMeshProUGUI>());
            }
        }

        public void Control()
        {
            base.TimeUpdate();


            if (gameState != GameState.Play) return;

            // Test用
            //if (Input.GetButtonDown("Cancel"))
            //{
            //    gameState = GameState.End;
            //}

            hammerController.Control();
            moguraController.Control();

            if (timerController.ElapsedTime <= 0f)
            {
                gameState = GameState.End;
            }

            if (gameState == GameState.End && drawResultCoroutine == null)
            {
                ResultPanel.gameObject.SetActive(true);
                drawResultCoroutine = StartCoroutine(DrawResult(CalcScore(), 0));
            }
        }


        protected override IEnumerator DrawResult(float score, int gameID)
        {
            timerController.StopTimer();
            Announce.text = "今回のスコア：" + score;
            Data.instance.rankingDate.Add(new RankingData(score, gameID));

            yield return new WaitForSeconds(5.0f);

            // スロットへ
            miniGameObject.SetActive(false);
            gameController.SetStartSlotAndGameID(false);

            drawResultCoroutine = null;

        }

        int CalcScore()
        {
            int score = 0;
            foreach (var tex in ResultText)
            {
                tex.text = "×0";
            }

            foreach (var pair in hammerController.GetScoreDictionary())
            {
                string key = pair.Key;
                int value = pair.Value;

                // 画像ID
                switch (key)
                {
                    case "1":
                        score += value * 10;
                        ResultText[0].text = $"×{value}";
                        break;
                    case "3":
                        score += value * 20;
                        ResultText[1].text = $"×{value}";
                        break;
                    case "5":
                        score += value * 30;
                        ResultText[2].text = $"×{value}";
                        break;
                    case "7":
                        score += value * -50;
                        ResultText[3].text = $"×{value}";
                        break;
                    case "9":
                        score += value * 700;
                        ResultText[4].text = $"×{value}";
                        break;
                    case "11":
                        score += value * 1000;
                        ResultText[5].text = $"×{value}";
                        break;
                    default:
                        DebugLogger.Log(key);
                        break;
                }
            }
            return score;
        }
    }
}
