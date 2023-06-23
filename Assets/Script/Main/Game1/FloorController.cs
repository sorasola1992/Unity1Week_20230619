using System.Collections;
using System.Collections.Generic;
using Unity1Week_20230619.Main.Game0;
using Unity1Week_20230619.Main;
using Unity1Week_20230619.Main.Game1;
using UnityEngine;
using System.Diagnostics;
using Unity1Week_20230619;
using DG.Tweening;
using TMPro;

public class FloorController : MonoBehaviour
{
    [SerializeField] GameObject floorPrefab;
    Transform playerTransform;
    List<GameObject> cloneList = new List<GameObject>();

    readonly float OFFSET_POS_X = 7f;
    readonly float OFFSET_POS_Y = -5.2f;
    readonly int FLOORSIZE_X = 24;


    public void Init()
    {
        foreach (var obj in cloneList)
        {
            Destroy(obj);
        }
        cloneList.Clear();

        playerTransform = GameObject.Find("MiniGameObj_1/Player").transform;
    }


    public void Control()
    {
        var create_count = (playerTransform.position.x/ FLOORSIZE_X) - cloneList.Count;


        for (; create_count > 0; create_count--)
        {
            GameObject instance = Instantiate(floorPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            cloneList.Add(instance);

            instance.transform.position = new Vector3((cloneList.Count * FLOORSIZE_X) + OFFSET_POS_X, OFFSET_POS_Y, 0);
            instance.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"{cloneList.Count * 100}m";
        }

    }

    public float GetDistance()
    {
        float distance = playerTransform.position.x;
        distance = (distance - OFFSET_POS_X) / FLOORSIZE_X * 100;

        return distance;
    }

}
