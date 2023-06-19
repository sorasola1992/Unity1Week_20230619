using TMPro;
using UnityEngine;

namespace Unity1Week_20230619.Main.Game0
{

    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f; // 移動速度

        Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            // 入力の取得
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            //float cancel = Input.GetAxis("Cancel");
            //float submit = Input.GetAxis("Submit");

            // 移動ベクトルの計算
            Vector2 movement = new Vector2(moveHorizontal, moveVertical) * (speed * Time.deltaTime);

            // Rigidbody2Dの移動
            rb.MovePosition(rb.position + movement);
        }
    }
}