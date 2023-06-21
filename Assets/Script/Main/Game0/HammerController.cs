using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Unity1Week_20230619.Main.Game0
{
    public class HammerController : MonoBehaviour
    {
        float hitRotation = 145f;

        Vector3 initialPosition;
        Quaternion initialRotation;
        Dictionary<string, int> scoreDictionary = new Dictionary<string, int>();

        [SerializeField] GameObject[] Effect = new GameObject[2];

        bool attack = false; 
        public void Init()
        {
            // 初期位置と回転を保存する
            initialPosition = transform.position;
            initialRotation = transform.localRotation;

            scoreDictionary.Clear();
            attack = false;
        }

        void AddScore(string id, int score)
        {
            if (scoreDictionary.ContainsKey(id))
            {
                scoreDictionary[id] += score;
            }
            else
            {
                scoreDictionary.Add(id, score);
            }
        }

        public void Control()
        {

            if (Input.GetButton("Submit"))
            {
                if (attack) return;
                transform.position -= new Vector3(0.8f, 0f, 0f);
                transform.localRotation = Quaternion.Euler(0, 0, hitRotation);
                attack = true;
            }
            else
            {
                transform.position = initialPosition;
                transform.localRotation = initialRotation;
                attack = false;
            }

            if (attack) return;

            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position -= new Vector3(0f, 2f, 0f);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0f, 2f, 0f);

            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(3f, 0f, 0f);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position -= new Vector3(3f, 0f, 0f);
            }

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var param = other.GetComponent<MoguraParameter>().param;
            param.life--;

            if ((param.id != 0)&& (param.life == 0))
            {
                SoundManager.Instance.PlaySe(1);
                AddScore(param.id.ToString(), 1);


                GameObject instance = Instantiate(Effect[Random.Range(0, Effect.Length)], other.transform);
                instance.transform.DOJump(instance.transform.position + new Vector3(0.5f, 0.5f, 0f), 0.5f, 1, 0.3f);
                Destroy(instance, 0.5f);
                param.id++;
            }
            //DebugLogger.Log(other.name);
        }

        public Dictionary<string, int> GetScoreDictionary()
        {
            return scoreDictionary;
        }
    }
}
