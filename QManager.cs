using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class QManager : MonoBehaviour
{
    [Header("물음표 색상")]
    [SerializeField] Renderer render;
    [Header("빨간메테리얼")]
    [SerializeField] Material redQM;
    [Header("회색메테리얼")]
    [SerializeField] Material grayQM;
    [Header("파랑메테리얼")]
    [SerializeField] Material blueQM;

    [Space(10)]
    [Header("회전 여부")]
    [SerializeField] bool turn;
    [Header("회전 속도")]
    [SerializeField] float rspeed;
    [Header("X값 회전")]
    [SerializeField] bool turnx;
    [Header("Y값 회전")]
    [SerializeField] bool turny;
    [Header("Z값 회전")]
    [SerializeField] bool turnz;

    [Space(10)]
    [Header("움직임 여부")]
    [SerializeField] bool shack;
    [Header("움직임 속도")]
    [SerializeField] float sspeed;
    [Header("위아래 최대/최소값")]
    [SerializeField] float movemax;
    [Header("X값 이동")]
    [SerializeField] bool shackx;
    [Header("Y값 이동")]
    [SerializeField] bool shacky;
    [Header("Z값 이동")]
    [SerializeField] bool shackz;

    [Space(10)]
    [Header("오디오")]
    [Tooltip("(확인)재생중인 오디오 소스")]
    [SerializeField] AudioSource myAudio;              //진행중인 오디오
    [Tooltip("문제 사운드")]
    [SerializeField] AudioClip[] Qsound;        //문제 나레이션
    [Tooltip("대답 사운드")]
    [SerializeField] AudioClip[] Asound;         //대답 나레이션
    [Tooltip("(확인)재생중인 문제 오디오 번호")]
    [SerializeField] int nnum;

    [Space(10)]
    [Header("오브젝트")]
    [Tooltip("문제 오브젝트")]
    [SerializeField] GameObject Qobject;                //문제 오브젝트
    [Tooltip("대답 오브젝트")]
    [SerializeField] GameObject[] Aobject;              //대답 오브젝트

    [Header("정답번호")]
    [SerializeField] int[] anum;                          //정답 번호(0부터~                 
    [SerializeField] int anumcount;                         //정답 갯수 카운트
    [SerializeField] int anumlast;                          //마지막 번호

    private Interactable interactable;  //상호작용
    private Vector3 firstposition;    //처음위치
    private Quaternion firstrotation;
    private bool flag;
    private bool ndone;
    private bool qdone;
    private bool alldone;
    private bool touch;
  

    void Start()
    {
        render = GetComponent<MeshRenderer>();
        interactable = GetComponent<Interactable>();    //상호작용 컴포넌트를 적용
        myAudio = gameObject.GetComponent<AudioSource>();  //오디오 소스 받아오기   

        AlReadyQM();    //기본 회색

        /*
        Qobject.SetActive(false);
        for(int i=0;i<Aobject.Length;i++)
        {
            Aobject[i].SetActive(false);
        }
        */

        //자식 오브젝트 사라지기
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        firstposition = transform.position;
        firstrotation = transform.rotation;

        nnum = 0;
        anumcount = anum.Length;
        anumlast = anum[0];
        touch = false;
        flag = false;
        qdone = false;
        alldone = false;
        ndone = false;
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabTypes = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);  //이 오브젝트를 잡고 있는지에 대한 bool값

        //물체를 잡고 있지않다면
        if (interactable.attachedToHand == null && grabTypes != GrabTypes.None)
        {
            touch = true;
        }
        else if (isGrabEnding)   //물체를 잡고 있다고 놓았을때
        {
            //Debug.Log("bye");
        }
    }


    void Update()
    {
        /*움직임*/
        Shake();    //진자운동
        Turn();     //회전

        if (touch)
        {
            Debug.Log(gameObject + ":grab");
            if (!flag && GameObject.Find("SchoolManager").GetComponent<SchoolManager>().MainAudioPlayDone())
            {
                Debug.Log(gameObject + " :Qstart");
                //물음표 비활성화
                render.enabled = false;
                turn = false;
                shack = false;

                //물음표 제자리로
                transform.position = firstposition;
                transform.rotation = firstrotation;

                //자식 오브젝트 나타나기
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(true);
                }

                for (int j = 0; j < Aobject.Length; j++)
                {
                    Aobject[j].GetComponent<BoxCollider>().enabled = false;
                }
                /*
                //문제와 정답 오브젝트 활성화
                Qobject.SetActive(true);
                for (int i = 0; i < Aobject.Length; i++)
                {
                    Aobject[i].SetActive(true);
                }
                */
                flag = true;
                touch = false;
            }
        }
        //문제 활성화
        if (flag)
        {
            if(!ndone)
            {
                if (myAudio.clip==null)
                {
                    myAudio.clip = Qsound[nnum];
                    myAudio.Play();
                    if(nnum < Qsound.Length)
                        nnum++;
                    Debug.Log("playfirstQaudion!: " + nnum);
                }
                else if(!myAudio.isPlaying&& nnum < Qsound.Length)
                {
                    Debug.Log("playQaudion!: " + nnum);
                    myAudio.clip = Qsound[nnum];
                    myAudio.Play();
                    nnum++;
                }

                if (!myAudio.isPlaying&&nnum == Qsound.Length)
                {
                    Debug.Log("doneQaudion!");
                    for (int i = 0; i < Aobject.Length; i++)
                    {
                       Aobject[i].GetComponent<BoxCollider>().enabled = true;
                    }
                    ndone = true;
                }
            }
            

            if(ndone)
            {
                if (!qdone)
                {
                    for (int i = 0; i < Aobject.Length; i++)
                    {
                        if (Aobject[i].GetComponent<TouchObject>().ActiveObject())
                        {
                            myAudio.clip = Asound[i];
                            myAudio.Play();
                            Aobject[i].GetComponent<TouchObject>().InActiveObject();

                            for(int j=0;j<anum.Length;j++)
                            {
                                if (i == anum[j])
                                {
                                    Debug.Log("check last num: " + anumlast);
                                    anumcount-=1;

                                    if(anumcount==0)
                                    {
                                        anumlast = j;
                                        Debug.Log("my last num: " + anumlast);
                                        qdone = true;
                                    } 
                                }
                            }
                            
                        }

                    }
                }
                else if (qdone)//정답을 맞추면!
                {
                    if (!myAudio.isPlaying && myAudio.clip == Asound[anum[anumlast]])
                    {
                        //자식 오브젝트 사라지기
                        for (int i = 0; i < gameObject.transform.childCount; i++)
                        {
                            gameObject.transform.GetChild(i).gameObject.SetActive(false);
                        }

                        /*
                        //퀴즈창 사라지기
                        Qobject.SetActive(false);


                        //퀴즈 답지들 사라지기
                        for (int i = 0; i < Aobject.Length; i++)
                        {
                            Aobject[i].SetActive(false);
                        }
                        */
                        //물음표 정상화
                        render.enabled = true;
                        shack = true;
                        turn = true;
                        DoneQM();               //파란색으로!
                    }
                }
            }
            
        }
    }

    private void Shake()
    {
        //진자운동
        if (shack)
        {
            if (shackx)
            {
                Vector3 v = firstposition;
                v.x += movemax * Mathf.Sin(Time.time * sspeed);
                transform.position = v;
            }
            else if (shacky)
            {
                Vector3 v = firstposition;
                v.y += movemax * Mathf.Sin(Time.time * sspeed);
                transform.position = v;
            }
            else if (shackz)
            {
                Vector3 v = firstposition;
                v.z += movemax * Mathf.Sin(Time.time * sspeed);
                transform.position = v;
            }
        }
    }

    private void Turn()
    {
        //회전
        if (turn)
        {
            if (turnx)
            {
                transform.Rotate(new Vector3(rspeed * Time.deltaTime, 0, 0));
            }
            else if (turny)
            {
                transform.Rotate(new Vector3(0, rspeed * Time.deltaTime, 0));
            }
            else if (turnz)
            {
                transform.Rotate(new Vector3(0, 0, rspeed * Time.deltaTime));
            }

        }
    }


    //다음 물음표
    public void NextQM()
    {
        render.material = redQM;    //빨간색으로 교체
    }

    //준비중 물음표
    public void AlReadyQM()
    {
        render.material = grayQM;
    }

    //끝난 물음표
    public void DoneQM()
    {
        render.material = blueQM;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        flag = false;
        alldone = true;
    }

    //물음표가 비활성화->활성화/활성화->비활성화
    public void ShowQM()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public bool DoneQ()
    {
        if (qdone&&alldone)
            return true;
        else
            return false;
    }

    public bool TouchQ()
    {
        if (flag)
            return true;
        else
            return false;
    }

    public void AutoTouch()
    {
        touch = true;
    }
}
