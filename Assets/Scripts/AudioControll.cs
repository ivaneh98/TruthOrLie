using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControll : MonoBehaviour
{
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!audio.isPlaying)
            Destroy(gameObject);
    }
    public void Off()
    {
        audio.mute=true;
    }
    public void On()
    {
        audio.mute = false;
    }
}
