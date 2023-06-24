using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.ComponentModel;

namespace Unity1Week_20230619.Main
{
    public enum ReelState
    {
        Play,
        Stay,
        End,
    }


    public class ReelController : MonoBehaviour
    {
        [SerializeField] GameObject[] imgobj; //絵柄のプレハブを格納
        [SerializeField] int[] currentID = new int[REELMAX]; //配列に全体の絵柄idを格納
        [SerializeField] GameObject FadeCanvasObj;

        Sprite[] slot_sprite = new Sprite[REELMAX];
        GameObject[] tmp_obj = new GameObject[REELMAX];
        Transform[] img_pos = new Transform[REELMAX];

        Transform pos;  //リールのTransform
        Vector3 initpos; //リールの初期位置

        public float speed; //リールの回転速度
        public ReelState state { private set; get; }


        const int REELMAX = 5;
        const float SLOTSIZE = 1.0f;
        const float DEFAULTSPEED = 0.5f;


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
            FadeCanvasObj = GameObject.Find("Canvas_2");
            imgobj = Resources.LoadAll<GameObject>("SlotPrefab");

            for (var i = 0; i < imgobj.Length; i++)
            {
                slot_sprite[i] = imgobj[i].GetComponent<SpriteRenderer>().sprite;
            }

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

            FadeCanvasObj.GetComponent<CanvasGroup>().alpha=0;
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
                        StartCoroutine(CurrentIDUpDate(under));
                        state = ReelState.End;
                    }
                }
            }
        }

        IEnumerator CurrentIDUpDate(int under)
        {
            FadeCanvasObj.GetComponent<CanvasGroup>().alpha = 1;
            FadeCanvasObj.GetComponentInChildren<Image>().sprite = slot_sprite[currentID[(under)]];
            FadeCanvasObj.transform.GetChild(0).DOScale(new Vector3(5f, 5f, 1f), 1.8f).SetEase(Ease.InSine); 
            yield return new WaitForSeconds(2);
            FadeCanvasObj.GetComponent<CanvasGroup>().alpha = 0;
            FadeCanvasObj.transform.GetChild(0).DOComplete();
            FadeCanvasObj.transform.GetChild(0).localScale= new Vector3(0.01f, 0.01f, 1f);
            imgID = currentID[(under)];
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
            speed = DEFAULTSPEED;
            state = ReelState.Play;
            imgID = -1;
        }
    }
}
