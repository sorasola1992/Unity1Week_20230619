using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unity1Week_20230619.Setting
{
    public class SettingController : MonoBehaviour
    {
        /// <summary>メニュー</summary>
        [SerializeField] readonly List<Transform> menuObj = new List<Transform>();

        /// <summary>選択カーソル位置</summary>
        int selectpos;

        readonly List<float> volumeList = new List<float>();
        void Start()
        {
            // 音量調整要素取得
            var parent = GameObject.Find("SettingPanel/Menu").transform;
            for (int i = 0; i < parent.childCount; i++)
            {
                // インジケータ
                menuObj.Add(parent.GetChild(i).GetChild(0).GetChild(0));
            }
            // 戻る
            menuObj.Add(GameObject.Find("SettingPanel/Back").transform);


            volumeList.Add(SoundManager.Instance.BgmVolume);
            volumeList.Add(SoundManager.Instance.SeVolume);


            Init();
        }

        private void Update()
        {
            Control();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            // パラメータ初期化
            selectpos = 0;
            menuObj[selectpos].GetComponent<Slider>().Select();
        }

        /// <summary>
        /// 操作処理
        /// </summary>
        public void Control()
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // カーソル表示更新
                selectpos = (selectpos + 1) % menuObj.Count;

                // フォーカス更新
                if (selectpos != menuObj.Count - 1) menuObj[selectpos].GetComponent<Slider>().Select();
                else menuObj[selectpos].GetComponent<Button>().Select();


            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // カーソル表示更新
                selectpos = (selectpos + menuObj.Count - 1) % menuObj.Count;

                // フォーカス更新
                if (selectpos != menuObj.Count - 1) menuObj[selectpos].GetComponent<Slider>().Select();
                else menuObj[selectpos].GetComponent<Button>().Select();

            }

            // 決定
            if (Input.GetButtonDown("Submit"))
            {
                // 戻る
                if (selectpos == menuObj.Count - 1)
                {
                    SceneManager.LoadScene(Define.GAME_TITLE);
                }
            }

            // 戻る
            if (Input.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene(Define.GAME_TITLE);

            }
        }

    }
}