using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyCall : MonoBehaviour
{
    [Header("비상방송")]
    [SerializeField] GameObject EmgcSound;  //8초
    [Header("경종")]
    [SerializeField] GameObject WakeSound;
    [Header("싸이렌")]
    [SerializeField] GameObject SirenSound;

    protected bool soundon;

    void Start()
    {
        soundon = false;
    }

    void Update()
    {
        if(!soundon)
        {
            Invoke("EmgcOn", 6f);
            soundon = true;
        }
            

        if(EmgcSound.GetComponent<AudioSource>().isPlaying)
        {
            if(WakeSound!=null)
                WakeSound.GetComponent<AudioSource>().mute = true;
            if(SirenSound!=null)
                SirenSound.GetComponent<AudioSource>().mute = true;
        }
    }

    protected void EmgcOn()
    {
        Debug.Log("비상방송on");
        EmgcSound.GetComponent<AudioSource>().Play();
        Invoke("EmgcOff", 16f); //비상방송 2사이클
    }

    protected void EmgcOff()
    {
        Debug.Log("비상방송off");
        EmgcSound.GetComponent<AudioSource>().Stop();
        if (WakeSound != null)
            WakeSound.GetComponent<AudioSource>().mute = false;
        if (SirenSound != null)
            SirenSound.GetComponent<AudioSource>().mute = false;

        soundon = false;
    }
}
