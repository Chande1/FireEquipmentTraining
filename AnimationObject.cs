using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class AnimationObject : MonoBehaviour
{
    [SerializeField] float soundtime;
    [SerializeField] Animator IngAnimator;
    [SerializeField] string AnimatorObject;
    [SerializeField] string AnimatorBool;
    //[SerializeField] GameObject Rhand;
    //[SerializeField] GameObject Lhand;
    private Interactable interactable;  //상호작용

    private bool flag;
    public bool movedone;

    void Start()
    {
        interactable = GetComponent<Interactable>();    //상호작용 컴포넌트를 적용
        //Rhand = GameObject.Find("mesh_hand_R");
        //Lhand = GameObject.Find("mesh_hand_L");
        flag = false;
        movedone = false;
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

                //Debug.Log("boo!");
                GameObject.Find("LeftHand").SetActive(false);    //왼손 감추기
                GameObject.Find("RightHand").SetActive(false);    //오른손 감추기
                IngAnimator = GameObject.Find(AnimatorObject).GetComponent<Animator>();
                IngAnimator.SetBool(AnimatorBool, true);           //애니메이터 파라미터 온->애니메이션 재생
                flag = true;
            }

        }
        else if (isGrabEnding)   //물체를 잡고 있다고 놓았을때
        {
            Debug.Log("bye");

        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (!flag)
        {
            //Debug.Log("boo!");
            GameObject.Find("LeftHand").SetActive(false);    //왼손 감추기
            GameObject.Find("RightHand").SetActive(false);    //오른손 감추기
            IngAnimator = GameObject.Find(AnimatorObject).GetComponent<Animator>();
            IngAnimator.SetBool(AnimatorBool, true);           //애니메이터 파라미터 온->애니메이션 재생
            flag = true ;
        }
    }
    */

    private void Update()
    {
        IngAnimator = GameObject.Find(AnimatorObject).GetComponent<Animator>();

        if (!AnimatorIsPlaying()&&flag&&!movedone)    //애니메이션이 끝날때
        {
            movedone = true;
            Invoke("ShowHands", 1f);
            Debug.Log(gameObject + ":anidone");
        }
    }

    public bool AnimatorIsPlaying()
    {
        return IngAnimator.GetCurrentAnimatorStateInfo(0).length > IngAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    protected void ShowHands()
    {
        GameObject.Find("SteamVRObjects").transform.Find("LeftHand").gameObject.SetActive(true);
        GameObject.Find("SteamVRObjects").transform.Find("RightHand").gameObject.SetActive(true);
    }

    private void SoundStart()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void ReAnimPlay()
    {
        flag = false;
        movedone = false;
    }
}
