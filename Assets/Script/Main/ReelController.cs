using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1Week_20230619.Main
{
    public enum ReelState
    {
        Play,
        Stay,

    }


    public class ReelController : MonoBehaviour
    {
        [SerializeField] GameObject[] imgobj; //絵柄のプレハブを格納
        [SerializeField] int[] currentID = new int[REELMAX]; //配列に全体の絵柄idを格納
        GameObject[] tmp_obj = new GameObject[REELMAX];
        Transform[] img_pos = new Transform[REELMAX];

        Transform pos;  //リールのTransform
        Vector3 initpos; //リールの初期位置

        public int speed; //リールの回転速度
        public ReelState state { private set; get; }


        const int REELMAX = 5;
        const float SLOTSIZE = 1.0f;


        // スロットの図柄停止時、コールバック
        public event Action<int> ChangImageID;
        int _imgID = Define.DEFAULT_GAMEID;
        public int imgID
        {
            get => _imgID;
            set
            {
                // 値が変更された時、イベント発火
                if (_imgID != value)
                {
                    //DebugLogger.Log($"{_imgID} →{value}");
                    _imgID = value;
                    ChangImageID?.Invoke(value);
                }
            }
        }


        void Start()
        {

            Init();

            imgobj = Resources.LoadAll<GameObject>("SlotPrefab");

            int[] shufflIDarray = imgobj.Select((_, index) => index).ToArray();
            Function.ShuffleArray(shufflIDarray);

            for (int i = 0; i < REELMAX; i++)
            {
                Vector3 pos = new Vector3(0.0f, (SLOTSIZE * i), 0.0f);
                int tmp = 0;

                if (i == REELMAX - 1)
                {
                    tmp = currentID[0];
                }
                else
                {
                    tmp = shufflIDarray[i];
                }

                currentID[i] = tmp;
                tmp_obj[i] = (GameObject)Instantiate(imgobj[tmp]); //プレハブからGameObjectを生成
                tmp_obj[i].transform.SetParent(transform, false);//リールのオブジェクトを親にする
                img_pos[i] = tmp_obj[i].GetComponent<Transform>();
                img_pos[i].localPosition = pos;
            }
        }
          

        void Update()
        {
            if (pos.localPosition.y < -(SLOTSIZE * REELMAX - 1))
            {
                pos.localPosition = initpos;
            }

            // 回転中
            if (state == ReelState.Play)
            {
                pos.localPosition = new Vector3(pos.localPosition.x, pos.localPosition.y - (speed * Time.deltaTime), pos.localPosition.z);
            }
            else
            {
                if (pos.localPosition.y % SLOTSIZE < -0.06f)
                {   //絵柄をマスで固定するために回転スピードを弱める
                    state = ReelState.Stay;
                    pos.localPosition = new Vector3(pos.localPosition.x, pos.localPosition.y - 0.03f, pos.localPosition.z);
                }
                else
                {   //固定完了
                    if (state == ReelState.Stay)
                    {
                        int under = -1 * (int)(pos.localPosition.y / SLOTSIZE);  //何マス回転（移動）したか
                        imgID = currentID[(under)];    //絵柄を特定
                    }
                }

            }
        }

        void Init()
        {
            pos = GetComponent<Transform>();
            initpos = pos.localPosition;
            ReelMove();
        }

        public void ReelStop()
        {
            state = ReelState.Stay;
        }

        public void ReelMove()
        {
            state = ReelState.Play;
            imgID = -1;

        }
    }
}
