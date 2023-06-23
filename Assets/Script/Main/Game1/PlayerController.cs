using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1Week_20230619.Main.Game1
{
    public enum Phase
    {
        Shuffle,
        Launch,
        Result,
    }

    public class PlayerController : MonoBehaviour
    {
        public PlayerParameter playerParameter { private set; get; }
        Rigidbody2D rb;
        SpriteRenderer playerSpriteRenderer;
        Sprite[] kanimg;

        [SerializeField] GameObject GaugeCanvas;
        [SerializeField] GameObject MissObj;
         List<Image> powergauge = new List<Image>();

        public Phase phase;
        readonly float OFFSET_POS_X = 7f;

        float burst = 100f;
        float currentRotation = 0f;
        float rotationSpeed = 2f;
        bool isLaunch = false;
        float penalty = 0;

        void Start()
        {
            kanimg = Resources.LoadAll<Sprite>("Game1");
        }

        public void Init()
        {
            powergauge.Clear();
            rb = GetComponent<Rigidbody2D>();
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
            playerParameter = transform.GetComponent<PlayerParameter>();
            playerParameter.Init();

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            phase = Phase.Shuffle;
            isLaunch = false;
            rb.simulated = false;
            GaugeCanvas.SetActive(true);
            MissObj.SetActive(false);

            foreach (Transform child in GaugeCanvas.transform)
            {
                powergauge.Add(child.GetComponent<Image>());
            }

            for (var i = 1; i < powergauge.Count; i++)
            {
                powergauge[i].fillAmount = 0;
            }
         
        }


        public void Control(float time)
        {

            if (time <= 0f && phase == Phase.Shuffle)
            {
                phase = Phase.Launch;
                transform.DOComplete();
                transform.position = new Vector3(OFFSET_POS_X, -4, 0);
                playerParameter.param.id = 4;
                playerSpriteRenderer.sprite = kanimg[playerParameter.param.id];
                GaugeCanvas.SetActive(false);
                MissObj.SetActive(false);

            }

            switch (phase)
            {
                case Phase.Shuffle: ShuffleControl(); break;
                case Phase.Launch: LaunchControl(); break;
            }
        }

        void ShuffleControl()
        {
            //if (Input.GetKey(KeyCode.DownArrow)) {
            //    playerParameter.param.power++;
            //}

            // 誤入力
            if (   (Input.GetKeyDown(KeyCode.DownArrow) && playerParameter.param.id == 4)
                || (Input.GetKeyDown(KeyCode.UpArrow)   && playerParameter.param.id == 6))
            {
                penalty = 100;
                MissObj.SetActive(true);
            }
            if (penalty > 0)
            {
                penalty--;
                if (penalty < 10) MissObj.SetActive(false);
                return;
            }

            // 振る
            if (Input.GetKeyDown(KeyCode.DownArrow) && playerParameter.param.id == 6)
            {
                transform.DOLocalMove(new Vector3(transform.position.x, -1f), 0.2f).SetEase(Ease.InOutElastic);
                playerParameter.param.id = 4;
                playerSpriteRenderer.sprite = kanimg[playerParameter.param.id];
                playerParameter.param.power++;

            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && playerParameter.param.id == 4)
            {
                transform.DOLocalMove(new Vector3(transform.position.x, 1f), 0.2f).SetEase(Ease.InOutElastic);
                playerParameter.param.id = 6;
                playerSpriteRenderer.sprite = kanimg[playerParameter.param.id];
                playerParameter.param.power++;

            }

            for (var i = 1; i < powergauge.Count; i++)
            {
                powergauge[i].fillAmount = Mathf.Clamp(playerParameter.param.power-((i-1)*100), 0, 100) * 0.01f;
            }

        }

        void LaunchControl()
        {

            // 発射
            if (Input.GetButtonDown("Submit"))
            {
                rb.AddForce(transform.up * burst * (int)(playerParameter.param.power * 0.1));
                isLaunch = true;
                rb.simulated = true;
            }

            if (!isLaunch)
            {
                currentRotation -= rotationSpeed;
                // 角度制限
                if (currentRotation >= 0f || currentRotation <= -180f)
                {
                    rotationSpeed = -(rotationSpeed);
                }
                transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);

            }

            if (rb.IsSleeping() || transform.position.y < -100)
            {
                phase = Phase.Result;
            }

        }
    }
}