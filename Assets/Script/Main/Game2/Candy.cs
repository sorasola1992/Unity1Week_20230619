
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Candy : MonoBehaviour, IFallObject
{
    // キャンディの種類
    public enum CandyType
    {
        SMALL,
        MEDIUM,
        LARGE,
        TYPE_NUM,
    }

    

    // キャンディの情報
    [System.Serializable]
    public struct CandyInfo
    {
        public Sprite graphic; // テクスチャ
        public CandyType type; // 種類
        public int score;      // 得点
    }



    [SerializeField] private List<CandyInfo> candiesList;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody2D;
    private long score;
    private CandyType candyType;
    private float speed;



    void IFallObject.Init()
    {
        CandyInfo candyInfo = candiesList[(int)candyType];
        candyType = (CandyType)(Random.Range((int)CandyType.SMALL, (int)CandyType.LARGE));
        spriteRenderer.sprite = candyInfo.graphic;
        score = candyInfo.score;
        speed = Random.Range(2.0f, 5.0f);
        rigidbody2D.velocity = new Vector2(0.0f, speed);

        
    }

    private void Awake()
    {
        TryGetComponent(out spriteRenderer);
        TryGetComponent(out rigidbody2D);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IFallObject.Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public long GetScore()
    {
        return score;
    }
}
