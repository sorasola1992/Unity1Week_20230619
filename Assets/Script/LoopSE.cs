using UnityEngine;

//ChatGPT
public class LoopSE : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip Clip;
    public float loopStartTime;

    void Start()
    {
        Source = this.GetComponent<AudioSource>();
        Source.clip = Clip;
    }

    void Update()
    {
        if (Source.time >= Source.clip.length) 
        {
		Source.time =loopStartTime;
		}
    }
}
