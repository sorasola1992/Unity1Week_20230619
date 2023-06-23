using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity1Week_20230619.Main.Game1
{
    public class CameraController : MonoBehaviour
    {

        Transform playerTransform;

        public void Init()
        {
            playerTransform = GameObject.Find("MiniGameObj_1/Player").transform;
            transform.position = new Vector3(7, 0, -10);
        }

        public void Control()
        {
            MoveCamera();
        }

        void MoveCamera()
        {
            // 中央以降追従
            if (playerTransform.position.x >= transform.position.x)
            {
                transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
            }
        }
    }

}
