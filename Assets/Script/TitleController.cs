using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity1Week_20230619
{
    public class TitleController : MonoBehaviour
    {
        /// <summary>メニュー</summary>
        [SerializeField] List<TextMeshProUGUI> menuTextMeshPro = new List<TextMeshProUGUI>();

        int selectpos;

        void Start()
        {
            // 音量調整要素取得
            var parent = GameObject.Find("TitlePanel/Menu").transform;
            for (int i = 0; i < parent.childCount; i++)
            {
                menuTextMeshPro.Add(parent.GetChild(i).GetComponent<TextMeshProUGUI>()) ;
            }

            Init();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Init()
        {
            // パラメータ初期化
            selectpos = 0;
            menuTextMeshPro[selectpos].color = Color.red;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // カーソル表示更新
                menuTextMeshPro[selectpos].color = Color.white;
                selectpos = (selectpos + 1) % menuTextMeshPro.Count;
                menuTextMeshPro[selectpos].color = Color.red;


            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // カーソル表示更新
                menuTextMeshPro[selectpos].color = Color.white;
                selectpos = (selectpos + menuTextMeshPro.Count - 1) % menuTextMeshPro.Count;
                menuTextMeshPro[selectpos].color = Color.red;
            }

            // 決定
            if (Input.GetButtonDown("Submit"))
            {
                switch (selectpos)
                {
                    case 0:
                        SceneManager.LoadScene(Define.GAME_MAIN);
                        break;
                    case 1:
                        SceneManager.LoadScene(Define.GAME_SETTING);
                        break;
                }
            }

        }


    }
}
