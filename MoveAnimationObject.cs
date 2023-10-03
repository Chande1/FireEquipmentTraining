using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class MoveAnimationObject : MonoBehaviour
{
    [SerializeField] float soundtime;
    [SerializeField] bool point;
    [SerializeField] bool scene;
    [SerializeField] GameObject nextPosition; //다음 위치
    [SerializeField] string nextScene;      //다음 씬
    [SerializeField] float inoutTime = 1f;   //페이드 인/아웃 시간
    [SerializeField] float fadeTime = 0.5f; //페이드 인/아웃 대기 시간
    [SerializeField] float stopTime = 3f; //페이드 되기 전 시간
    [SerializeField] Animator IngAnimator;
    [SerializeField] string AnimatorObject;
    [SerializeField] string AnimatorBool;
    private Interactable interactable;  //상호작용
    private bool fade;
    private bool flag;
    public bool movedone;
    public bool tempdone;

    void Start()
    {
        interactable = GetComponent<Interactable>();    //상호작용 컴포넌트를 적용
        fade = false;
        flag = false;
        movedone = false;
        tempdone = false;
    }

    private void Update()
    {
        IngAnimator = GameObject.Find(AnimatorObject).GetComponent<Animator>();

        if (fade)
        {
            //Player player = Player.instance;//creates a reference to your player//steamVr Player 프리팹 사용가능
            GameObject player = GameObject.Find("Player");
            player.transform.position = nextPosition.transform.position; //move to start position
            player.transform.eulerAngles = nextPosition.transform.eulerAngles;
            Invoke("FadeFromBlack", inoutTime);
            flag = true;
            fade = false;
        }
        if (!AnimatorIsPlaying() && flag && !movedone)    //애니메이션이 끝날때
        {
            tempdone = true;
            Debug.Log(gameObject + ":tempdone");
        }
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabTypes = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);  //이 오브젝트를 잡고 있는지에 대한 bool값

        //물체를 잡고 있지않다면
        if (interactable.attachedToHand == null && grabTypes != GrabTypes.None)
        {
            Debug.Log("grab");
            if (!flag)
            {
                //만약 효과음이 있다면 원하는 시간 후 재생
                if (gameObject.GetComponent<AudioSource>())
                    Invoke("SoundStart", soundtime);
                GameObject.Find("LeftHand").SetActive(false);    //왼손 감추기
                GameObject.Find("RightHand").SetActive(false);    //오른손 감추기
                IngAnimator = GameObject.Find(AnimatorObject).GetComponent<Animator>();
                IngAnimator.SetBool(AnimatorBool, true);           //애니메이터 파라미터 온->애니메이션 재생
                Invoke("StopTime", stopTime);
                flag = true;
            }

        }
        else if (isGrabEnding)   //물체를 잡고 있다고 놓았을때
        {
            Debug.Log("bye");

        }
    }

    public bool AnimatorIsPlaying()
    {
        return IngAnimator.GetCurrentAnimatorStateInfo(0).length > IngAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private void FadeToBlack() //원래화면->검은 화면
    {
        SteamVR_Fade.Start(Color.clear, 0f);
        SteamVR_Fade.Start(Color.black, inoutTime);

        GameObject.Find("SteamVRObjects").transform.Find("LeftHand").gameObject.SetActive(true);
        GameObject.Find("SteamVRObjects").transform.Find("RightHand").gameObject.SetActive(true);

        Invoke("CountTime", fadeTime);
        Debug.Log("fadeout");
        
    }
    private void FadeFromBlack()   //검은화면->원래화면
    {
        SteamVR_Fade.Start(Color.black, 0f);
        SteamVR_Fade.Start(Color.clear, inoutTime);
        movedone = true;
        Debug.Log("fadein");
    }

    private void CountTime()
    {
        fade = true;
    }

    private void StopTime()
    {

        Debug.Log("stoptime");
        if (point)
        {
            Debug.Log(gameObject + ":objectmove");
            FadeToBlack();
            
        }
        else if (scene)
        {
            Debug.Log(gameObject + ":scenemove:" + nextScene);
            movedone = true;
            Valve.VR.SteamVR_LoadLevel.Begin(nextScene);
        }
    }

    private void SoundStart()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
