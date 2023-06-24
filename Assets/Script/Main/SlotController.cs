using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using unityroom.Api;

namespace Unity1Week_20230619.Main
{

    public class SlotController : MonoBehaviour, IController, IInitializa
    {
        public GameObject[] reels;
        List<Transform> maskTransform = new List<Transform>();
        List<ReelController> rc = new List<ReelController>();

        [SerializeField] GameObject ResultPanel, ReelPanel, ReelResult;
        [SerializeField] GameObject ResultTextPrefab;
        [SerializeField] GameObject arrow;
        [SerializeField] TextMeshProUGUI ScoreText;
        List<TextMeshProUGUI> ResultText = new List<TextMeshProUGUI>();
        GameController gameController;

        bool miniGameAllEnd = false;
        bool isSlotStopPush = false; // TODO 2重押し制御
        StandaloneInputModule standaloneInputModule;
        Coroutine drawResultCoroutine = null;

        void Start()
        {
            standaloneInputModule = GameObject.Find("EventSystem").GetComponent<StandaloneInputModule>();
            gameController = GetComponent<GameController>();
            var parent = ResultPanel.transform.Find("Result");
            for (var cnt = 0; cnt < reels.Length + 1; cnt++)
            {
                var Prefab = Instantiate(ResultTextPrefab, Vector3.zero, Quaternion.identity, parent);
                ResultText.Add(Prefab.GetComponent<TextMeshProUGUI>());
            }

            for (int i = 0; i < reels.Length; i++)
            {
                maskTransform.Add(reels[i].transform.root.transform); 
                rc.Add(reels[i].GetComponent<ReelController>());

                // コールバック設定
                rc[i].ChangImageID += ChangImageID;

            }
            Init();

        }

        public void Control()
        {
            ScoreUpdate();
            ArrowUpdate();

            if (drawResultCoroutine != null) return;
            //// TODO test用
            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    standaloneInputModule.enabled = true;
            //    if (!Function.ContainsScene(Define.GAME_RANKING)) naichilab.RankingLoader.Instance.SendScoreAndShowRanking(3, "01230");

            //}

            // 全ゲーム終了時、終了後のリトライ時に一度
            if (miniGameAllEnd != MiniGameAllEnd())
            {
                miniGameAllEnd = !miniGameAllEnd;

                var rankingData = Data.instance.rankingDate;
                string gameID = string.Join("", rankingData.Select(data => data.GameID));
                float totalScore = rankingData.Sum(data => data.Score);

                for (var i = 0; i < rankingData.Count; i++)
                {
                    ResultText[i].text = $"ゲームID:{rankingData[i].GameID}\tスコア：{rankingData[i].Score}";
                }

                // 1列揃ってる時最後非表示
                bool isAllEqual = rankingData.Select(data => data.GameID).Distinct().Count() == 1;
                if (isAllEqual)
                {
                    ResultText[reels.Length - 1].text = $"ボーナス！　{totalScore}×1.5！";
                    totalScore *= 1.5f;
                }

                ResultText[rankingData.Count].text = "合計：" + totalScore;

                ReelPanel.SetActive(!ReelPanel.activeSelf);
                ResultPanel.SetActive(!ResultPanel.activeSelf);

                // ゲーム終了時
                if (miniGameAllEnd)
                {
                    naichilab.RankingLoader.Instance.SendScoreAndShowRanking(totalScore, gameID);
                    UnityroomApiClient.Instance.SendScore(1, totalScore, ScoreboardWriteMode.HighScoreDesc);
                }
            }

            // TODO ランキング表示中、クリック有効
            if (Function.ContainsScene(Define.GAME_RANKING))
            {
                standaloneInputModule.enabled = true;
                return;
            }
            else
            {
                standaloneInputModule.enabled = false;
            }

            // ゲーム終了時
            if (miniGameAllEnd)
            {
                if (Input.GetButtonDown("Submit"))
                {
                    Init();
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    SceneManager.LoadScene(Define.GAME_TITLE);
                }
                return;
            }

            // ゲーム中、 2重押し制御
            if (Input.GetButtonDown("Submit") && !isSlotStopPush)
            {
                Stop();
            }

            if (Input.GetButtonDown("Cancel"))
            {
                Init();
            }

        }


        public void Init()
        {
            for (int i = 0; i < reels.Length; i++)
            {
                rc[i].ReelMove();
            }
            gameController.SetStartSlotAndGameID(false);
            Data.instance.rankingDate.Clear();
            isSlotStopPush = false;

        }

        public void Stop()
        {
            for (int i = 0; i < reels.Length; i++)
            {
                if (rc[i].state == ReelState.Play)
                {
                    rc[i].ReelStop();
                    break;
                }
            }
            isSlotStopPush = true;

        }

        /// <summary>
        ///ミニゲームがすべて終了したか
        /// </summary>
        /// <returns></returns>
        public bool MiniGameAllEnd()
        {
            for (int i = 0; i < reels.Length; i++)
            {
                if (rc[i].imgID == Define.DEFAULT_GAMEID)
                {
                    return false;
                }
            }

            return true;

        }

        // TODO エフェクト
        IEnumerator DrawResult(int id)
        {
            ReelResult.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            gameController.SetStartSlotAndGameID(false);
            Data.instance.rankingDate.Add(new RankingData(0, id));
            ReelResult.SetActive(false);
            drawResultCoroutine = null;
        }


        void ChangImageID(int id)
        {
            // 止めたリールが全て同じか否か
            if  (!CheckReelLine(id)) gameController.SetStartSlotAndGameID(true, id);
            else                     drawResultCoroutine = StartCoroutine(DrawResult(id));

            isSlotStopPush = false;

        }

        /// <summary>
        /// 全て同じゲームIDかどうか
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>全て同じ:true</returns>
        bool CheckReelLine(int id)
        {
            // 最後のリールか
            if (reels.Length-1 == Data.instance.rankingDate.Count)
            {
                foreach (var ranking in Data.instance.rankingDate)
                {
                    if (ranking.GameID != id)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }


        void ScoreUpdate()
        {
            var rankingData = Data.instance.rankingDate;
            ScoreText.text = $"現在のスコア：{rankingData.Sum(data => data.Score)}";
        }

        void ArrowUpdate()
        {
            var index = Mathf.Clamp(Data.instance.rankingDate.Count, 0, maskTransform.Count-1);
            arrow.transform.position = new Vector3(maskTransform[index].position.x, arrow.transform.position.y, 0f);
        }
    }
}
