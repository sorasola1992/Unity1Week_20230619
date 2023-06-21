using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Unity1Week_20230619.Main.Game0
{
    public class MoguraController : MonoBehaviour
    {

        [SerializeField] List<MoguraParameter> moguraParameter = new List<MoguraParameter>();
        [SerializeField] List<SpriteRenderer> moguraSpriteRenderer = new List<SpriteRenderer>();
        Sprite[] moguraimg;
        int[] spawnid = { 0, 1, 3, 5, 7, 9, 11 };
        int[] probabilities = { 10, 40, 20, 15, 10, 4, 1 };

        void Start()
        {
            moguraimg = Resources.LoadAll<Sprite>("Game0");

            // メニュー要素全取得
            for (int i = 0; i < transform.childCount; i++)
            {
                moguraParameter.Add(transform.GetChild(i).GetComponent<MoguraParameter>());
                moguraSpriteRenderer.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
            }
        }

        public void Init()
        {
            for (var i = 0; i < moguraParameter.Count; i++)
            {
                moguraParameter[i].Init();
                moguraSpriteRenderer[i].sprite = moguraimg[0];
            }
        }

        public void Control()
        {
            for (var i = 0; i < moguraParameter.Count; i++)
            {
                if (moguraParameter[i].param.count <= 0) {
                    moguraParameter[i].param.count = 300;

                    // 出現中の時、一度引っ込む
                    if (moguraParameter[i].param.id != 0) moguraParameter[i].param.id = 0;
                    else
                    {
                        moguraParameter[i].param.id = GetRandomSpawnId();
                        moguraParameter[i].param.life = 1;

                        // 滞在時間
                        switch (moguraParameter[i].param.id)
                        {
                            case  0: break;
                            case  1: moguraParameter[i].param.count = 500; break;
                            case  3: moguraParameter[i].param.count = 500; break;
                            case  5: moguraParameter[i].param.count = 500; break;
                            case  7: moguraParameter[i].param.count = 700; break;
                            case  9: moguraParameter[i].param.count =  50; break;
                            case 11: moguraParameter[i].param.count =  30; break;
                            default: DebugLogger.Log(moguraParameter[i].param.id); break;
                        }
                    }
                }
                else
                {
                    moguraParameter[i].param.count--;
                }
            }

            for (var i = 0; i < moguraSpriteRenderer.Count; i++)
            {
                moguraSpriteRenderer[i].sprite = moguraimg[moguraParameter[i].param.id];

            }
        }


        int GetRandomSpawnId()
        {
            // 確率の総和を計算
            int totalProbability = 0;
            for (int i = 0; i < probabilities.Length; i++)
            {
                totalProbability += probabilities[i];
            }

            // ランダムな数値を生成
            int randomValue = Random.Range(0, totalProbability);

            // 数値に基づいてspawnidを選択
            int cumulativeProbability = 0;
            for (int i = 0; i < spawnid.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue < cumulativeProbability)
                {
                    return spawnid[i];
                }
            }

            return spawnid[spawnid.Length - 1];
        }

    }
}