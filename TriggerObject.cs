using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class TriggerObject : MonoBehaviour
{
    
    private GameObject thisobject;  //현재 오브젝트
    private GameObject otherobject; //해당 오브젝트
    [SerializeField] bool point;
    [SerializeField] bool scene;
    [SerializeField] GameObject otherPoint;
    [SerializeField] GameObject nextPosition; //다음 위치
    [SerializeField] string nextScene;      //다음 씬
    [SerializeField] float fadeTime = 1f;   //페이드 인/아웃 시간
    [SerializeField] float stopTime = 0.5f; //페이드 인/아웃 대기 시간
    private bool fade;
    public bool movedone;

    void Start()
    {
        thisobject = GetComponent<GameObject>();
        otherobject = otherPoint.GetComponent<GameObject>().gameObject;
        fade = false;
        movedone = false;
    }

    private void Update()
    {
        if (fade)
        {
            Player player = Player.instance;//creates a reference to your player
            player.transform.position = nextPosition.transform.position; //move to start position
            player.transform.eulerAngles = nextPosition.transform.eulerAngles;
            Invoke("FadeFromBlack", fadeTime);
            movedone = true;
            fade = false;
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.CompareTag("endpoint")&&
        if (other.gameObject == otherPoint && !fade&&!movedone)    //엔터된 콜라이더가 선택한 오브젝트 일때
        {
            if(point)
            {
                Debug.Log(gameObject+":objectmove");
                FadeToBlack();
            } 
            else if(scene)
            {
                Debug.Log(gameObject+":scenemove:"+nextScene);
                Valve.VR.SteamVR_LoadLevel.Begin(nextScene);
            }
                
            
            //Valve.VR.SteamVR_LoadLevel.Begin("SPSpace_01");  //해당 씬으로 이동하면서 페이드 아웃
        }
    }

    private void FadeToBlack() //원래화면->검은 화면
    {
        SteamVR_Fade.Start(Color.clear, 0f);
        SteamVR_Fade.Start(Color.black, fadeTime);
        Invoke("CountTime", stopTime);
        Debug.Log("fadeout");
    }
    private void FadeFromBlack()   //검은화면->원래화면
    {
        SteamVR_Fade.Start(Color.black, 0f);
        SteamVR_Fade.Start(Color.clear, fadeTime);
        Debug.Log("fadein");
    }

    private void CountTime()
    {
        fade = true;
    }
}
