using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class EndPoint : MonoBehaviour
{
    private GameObject thisobject;  //현재 오브젝트
    private GameObject otherobject; //해당 오브젝트
    public GameObject endpoint; //도착점
    private float _fadeDuration = 3f;   //페이드 인/아웃 시간
    private bool fade;

    void Start()
    {
        thisobject = GetComponent<GameObject>();
        otherobject = endpoint.GetComponent<GameObject>().gameObject;
        fade = false;
    }

    private void Update()
    {
        if(fade)
        {
            Invoke("FadeFromWhite", _fadeDuration);
            fade = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.CompareTag("endpoint")&&
        if (other.gameObject==endpoint&&!fade)    //엔터된 콜라이더의 태그가 endpoint일때
        {
            FadeToWhite();
            
            //Valve.VR.SteamVR_LoadLevel.Begin("SPSpace_01");  //해당 씬으로 이동하면서 페이드 아웃
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == endpoint && !fade)    //엔터된 콜라이더의 태그가 endpoint일때
        {
            fade = false;
        }
    }

    private void FadeToWhite()
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.white, _fadeDuration);
        fade = true;
        Debug.Log("fadeout");
    }
    private void FadeFromWhite()
    {
        //set start color
        SteamVR_Fade.Start(Color.white, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, _fadeDuration);
        Debug.Log("fadein");
    }
}
