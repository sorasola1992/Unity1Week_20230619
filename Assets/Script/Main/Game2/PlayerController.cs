using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Unity1Week_20230619.Main.Game2
{
    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update

        private bool isAlive = true;
        private long score = 0;

        bool attack = false;
        public void Init()
        {

        }

        void AddScore(ref long score)
        {
            this.score += score;
        }

        public void Control()
        {



 
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(3f, 0f, 0f);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-3f, 0f, 0f);
            }

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Arrow"))
            {
                isAlive = false;
            }

            if (other.CompareTag("Candy"))
            {
                long score = other.GetComponent<Candy>().GetScore();
                AddScore(ref score);
            }
        }
    }
}
