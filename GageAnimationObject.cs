using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;

public class GageAnimationObject : MonoBehaviour
{
    [SerializeField] Animator IngAnimator;
    [SerializeField] string AnimatorObject;
    [SerializeField] string AnimatorBool;

    [SerializeField] Image Gage;            //게이지 이미지
    [SerializeField] Text GageNum;         //게이지 텍스트
    [SerializeField] float GageAmount;        //게이지양

    [SerializeField] GameObject valvebar;   //움직이게 할 벨브바
    [SerializeField] float moveheight; //움직일 높이
    [SerializeField] bool barup;            //벨브바 상태

    [SerializeField] bool usenextani;
    [SerializeField] string NextanimBool;

    [SerializeField] bool Move;             //이동여부
    [SerializeField] bool point;
    [SerializeField] bool scene;
    [SerializeField] GameObject nextPosition; //다음 위치
    [SerializeField] string nextScene;      //다음 씬
    [SerializeField] float inoutTime = 1f;   //페이드 인/아웃 시간
    [SerializeField] float fadeTime = 0.5f; //페이드 인/아웃 대기 시간
    [SerializeField] float stopTime = 3f; //페이드 되기 전 시간

    private Interactable interactable;  //상호작용
    private bool fade;
    private bool flag;
    private bool aniflag;

    public bool movedone;

    void Start()
    {
        interactable = GetComponent<Interactable>();    //상호작용 컴포넌트를 적용
        GageAmount = 0;
        fade = false;
        flag = false;
        aniflag = false;
        movedone = false;
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabTypes = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);  //이 오브젝트를 잡고 있는지에 대한 bool값

        //물체를 잡고 있지않다면
        if (interactable.attachedToHand == null && grabTypes != GrabTypes.None)
        {
            Debug.Log(gameObject+":grab");
            if (!flag)
            {
                Debug.Log(gameObject + ":hide hand");
                GameObject.Find("LeftHand").SetActive(false);    //왼손 감추기
                GameObject.Find("RightHand").SetActive(false);    //오른손 감추기
                IngAnimator = GameObject.Find(AnimatorObject).GetComponent<Animator>();

                if(usenextani)
                {
                    if(!aniflag)
                    {
                        IngAnimator.SetBool(AnimatorBool, true);           //애니메이터 파라미터 온->애니메이션 재생
                        aniflag = true;

                    }
                    else
                    {
                        IngAnimator.SetBool(NextanimBool, true);           //다음 애니메이션
                    }
                }
                else
                    IngAnimator.SetBool(AnimatorBool, true);           //애니메이터 파라미터 온->애니메이션 재생

                flag = true;
            }

        }
        else if (isGrabEnding)   //물체를 잡고 있다고 놓았을때
        {
            Debug.Log("bye");

        }
    }

    private void Update()
    {
        IngAnimator = GameObject.Find(AnimatorObject).GetComponent<Animator>();

        if (AnimatorIsPlaying())  //애니메이션이 진행중일때
        {
            
            if (!IngAnimator.GetCurrentAnimatorStateInfo(0).IsName("PValveIdle")&&
               !IngAnimator.GetCurrentAnimatorStateInfo(0).IsName("PValveIdle2")&&
               !IngAnimator.GetCurrentAnimatorStateInfo(0).IsName("MPumpIdle")&&
               !IngAnimator.GetCurrentAnimatorStateInfo(0).IsName("CValveClose"))    //idle상태가 아니고
            {
                if (IngAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2&&  //20프로까지는 재생하지 않고
                    IngAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8)  //전체의 80프로 까지만 재생
                {
                    if (valvebar != null)
                    {
                        if (barup)
                        {
                            valvebar.transform.Translate(new Vector3(0, moveheight, 0));
                        }
                        else
                        {
                            valvebar.transform.Translate(new Vector3(0, moveheight*-1, 0));
                        }
                    }
                }
            }

        }

        if (AnimatorIsPlaying() && flag)    //애니메이션이 끝날때
        {

            FillGage(20);                           //한번에 20씩 차는 게이지
            //Invoke("ShowHands", 2f);
            
            if (GageAmount>=100)    //게이지가 다 차면
            {
                if (Move)   //움직임을 체크했을 경우
                {
                    Invoke("StopTime", stopTime);
                    Invoke("ShowHands", 2f);
                    flag = false;
                }
                else
                {
                    IngAnimator.SetBool(AnimatorBool, false);           //애니메이터 되돌리기
                    Debug.Log(gameObject + ":anidone");
                    Invoke("ShowHands", 2f);
                    movedone = true;
                    flag = false;
                }
            }
            else
            {
                if(usenextani)  //다음 애니메이션을 사용합니다
                {
                    Debug.Log(gameObject + ":next ani/animationbool :"+NextanimBool);
                    IngAnimator.SetBool(NextanimBool, false);           //다음 애니메이션
                    Invoke("ShowHands", 2f);
                    flag = false;
                }
                else
                {
                    Debug.Log(gameObject + ":i'm back");
                    IngAnimator.SetBool(AnimatorBool, false);           //애니메이터 되돌리기
                    Invoke("ShowHands", 2f);
                    flag = false;
                }
            }
        }
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
    }

    public void FillGage(float _gage)
    {
        if(GageAmount<100)
        {
            GageAmount += _gage;
            Gage.fillAmount = GageAmount / 100;
            int gageamount = (int)GageAmount;
            GageNum.text = gageamount.ToString()+"%";
        }
    }

    public bool AnimatorIsPlaying()
    {
        return IngAnimator.GetCurrentAnimatorStateInfo(0).length > IngAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    protected void ShowHands()
    {
        Debug.Log(gameObject + " :show hands!");
        GameObject.Find("SteamVRObjects").transform.Find("LeftHand").gameObject.SetActive(true);
        GameObject.Find("SteamVRObjects").transform.Find("RightHand").gameObject.SetActive(true);
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

    public void ChangMoveInfo()
    {
        if(point)
        {
            Debug.Log("ChangeMoveInfo:point->scene");
            scene = true;
            point = false;
        }
        else if(scene)
        {
            Debug.Log("ChangeMoveInfo:scene->point");
            point = true;
            scene = false;
        }
    }

    public bool GetGageCountDone()
    {
        if (GageAmount >= 100)
        {
            return true;
        }
        else
            return false;
    }

    public void ReGage()
    {
        GageAmount = 0;
        fade = false;
        flag = false;
        aniflag = false;
        movedone = false;
    }

    public bool Idle()
    {
        if (IngAnimator.GetCurrentAnimatorStateInfo(0).IsName("PValveIdle") ||
               IngAnimator.GetCurrentAnimatorStateInfo(0).IsName("PValveIdle2") ||
               IngAnimator.GetCurrentAnimatorStateInfo(0).IsName("MPumpIdle"))    //idle상태가 아니고
        {
            return true;
        }
        else
            return false;
    }
}


