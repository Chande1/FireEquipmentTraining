using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
public class AOfficeManager : MonoBehaviour
{
    [Header("메모 전용")]
    [SerializeField]
    [Multiline(5)] string memo;

    [Header("개발자 모드")]
    [SerializeField] bool W;            //
    [SerializeField] string Wmode;      //개발자 모드
    [SerializeField] int Wnum;
    [Header("현재 모드")]
    [SerializeField] string mode;       //현재 모드
    [SerializeField] Mode modeNum;                       //모드 번호
    [Header("현재 행동 번호")]
    [SerializeField] int ingNum;                        //현재 행동 번호
    [SerializeField] GameObject ModeObject;             //모드 객체
    [SerializeField] GameObject startpoint;             //시작지점
    [SerializeField] GameObject player;                 //플레이어

    [Header("외부 오디오")]
    [SerializeField] GameObject soundbox1;  //싸이렌
    [SerializeField] GameObject soundbox2;  //경종
    [SerializeField] GameObject soundbox3;  //비상방송

    [Header("나레이션")]
    [Tooltip("(확인)재생중인 오디오 소스")]
    [SerializeField] AudioSource IngAudio;              //진행중인 오디오
    [SerializeField] bool ndone;                      //나레이션 완료 체크
    [SerializeField] AudioClip[] s_narration;         //재생중인 오디오 배열
    [Space(10f)]
    [SerializeField] AudioClip[] spring1audio;        //습식 스프링쿨러 오디오
    [Space(10f)]
    [SerializeField] AudioClip[] spring2audio;        //프리앤셕 스프링쿨러 오디오
    [Space(10f)]
    [SerializeField] AudioClip[] fireaudio;           //자동화재탐지설비 오디오
    [Space(10f)]
    [SerializeField] AudioClip[] pumpaudio;           //소화펌프실 오디오
    [Space(10f)]
    [SerializeField] AudioClip[] campusaudio;        //교내 소방 설비 오디오

    protected GameObject StepLight1;
    protected GameObject StepLight2;
    protected GameObject StepLight3;
    protected GameObject StepLight4;
    protected GameObject StepLight5;
    protected GameObject SwichLight;
    protected GameObject UCLight;
    protected GameObject NormalLight;


    protected bool flag;                            //기본 깃발
    protected bool outlineflag;                     //외각선 깜박임용 플래그
    protected bool stay;                            //잠시후 전용 변수
    protected bool stay2;                           //오직 stay관리를 위한 변수
    protected bool fade;                            //페이드 인아웃
    protected bool sflag;                           //경종용

    void Start()
    {
        if (LoadModeIngNum() == 0)
        {
            ingNum = 0;
            Debug.Log("1");
        }
        else if (LoadModeIngNum() > 0)
        {
            ingNum = LoadModeIngNum()+1;  //지난 진행 번호 불러오기
            Debug.Log("2");
        }
        else if (LoadModeIngNum() >= s_narration.Length)
        {
            ingNum = 0;         //행동번호 0으로 초기화
            Debug.Log("3");
        }
        else
        {
            ingNum = 0;
            Debug.Log("4");
        }

        //만약 개발자 모드가 켜져있다면
        if (W)
        {
            ChangeMode(Wmode, Wnum);
        }

        SettingMode();      //모드 확인
        SettingObject();    //오브젝트 세팅
        ndone = false;
        startpoint = GameObject.Find("StartPoint");
        player = GameObject.Find("Player");
        IngAudio = gameObject.GetComponent<AudioSource>();  //오디오 소스 받아오기
        flag = false;
        outlineflag = false;
        stay = false;
        stay2 = false;
        fade = false;
        sflag = false;
    }


    void Update()
    {
        player = GameObject.Find("Player");

        /*모드별로 재생*/
        switch (modeNum)
        {
            case Mode.Spring1:
                switch (ingNum)
                {
                    case 0: //45-1:수신반에 전달된 신호를~
                        
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            //사이렌 경종 울리기
                            soundbox1.SetActive(true);
                            soundbox2.SetActive(true);
                            soundbox1.GetComponent<AudioSource>().Play();
                            soundbox2.GetComponent<AudioSource>().Play();
                            
                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }
                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 1: //45-2:당직실 문을 열고~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = true;  //박스콜라이더 활성화
                                Debug.Log("<color=cyan>I'm here!</color>");
                                flag = false;
                            }
                            if (GameObject.Find("AODPoint").GetComponent<MoveAnimationObject>().movedone)
                            {
                                GameObject.Find("AODPoint").SetActive(false);
                                NextNum();
                            }
                        }

                        break;
                    case 2: //32-1:수신반은 관계자에게~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 3: //32-2:수신반에 있는 화재표시등 점등~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 4: //32-3:습식 스프링쿨러 설비가~
                        if (!IngAudio.isPlaying)
                        {
                            if(!flag)
                            {
                                stay = false;
                                SaveModeIngNum();//진행 번호 저장
                                Invoke("StayTime", 5);
                                flag = true;
                            }
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("RRoom");
                        }

                        break;
                    case 5:     //45-1:이제 수신반을 복구하기 위해~--------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동

                            //사이렌 경종 울리기
                            soundbox1.SetActive(true);
                            soundbox2.SetActive(true);
                            soundbox1.GetComponent<AudioSource>().Play();
                            soundbox2.GetComponent<AudioSource>().Play();

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }
                        
                        //그냥 소리만 재생
                        if (flag&&!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 6: //45-2:문을 열고~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = true;  //박스콜라이더 활성화
                                flag = false;
                            }

                            if (GameObject.Find("AODPoint").GetComponent<MoveAnimationObject>().movedone)
                            {
                                GameObject.Find("AODPoint").SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 7: //47-1:수신반의 복구를~
                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 8: //47-2:수신반에 있는 복구 버튼을 누릅니다
                        if (!IngAudio.isPlaying)
                        {
                            if (!flag)
                            {
                                //복구 버튼 외곽선(b)
                                GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().enabled = true;
                                //복구 버튼 활성화
                                if (GameObject.Find("RCVPoint"))
                                    GameObject.Find("RCVPoint").GetComponent<BoxCollider>().enabled = true;

                                flag = true;
                            }
                            
                            //버튼 애니끝!
                            if (GameObject.Find("RCVPoint").GetComponent<AnimationObject>().movedone)
                            {
                                //빨간불 꺼짐
                                GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("ReceiverLight").GetComponent<RedLight>().enabled = false;
                                GameObject.Find("redlight1").SetActive(false);
                                GameObject.Find("redlight2").SetActive(false);
                                //소리 꺼짐
                                GameObject.Find("SoundBox1").SetActive(false);
                                GameObject.Find("SoundBox2").SetActive(false);
                                //버튼 비활성화
                                GameObject.Find("RCVPoint").SetActive(false);
                                
                                NextNum();
                            }
                        }
                        break;
                    case 9:    //47-3:울리고 있는 주 경종 및~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                stay = false;
                                SaveModeIngNum();//진행 번호 저장
                                //5초 후!
                                Invoke("StayTime", 5);
                                flag = false;
                            }
                            
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("SPSpace");
                        }
                        break;
                }
                break;
            case Mode.Spring2:
                switch (ingNum)
                {
                    case 0: //67-1:이제 수신반에~
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동

                            //사이렌 경종 울리기
                            soundbox1.SetActive(true);
                            soundbox2.SetActive(true);
                            soundbox1.GetComponent<AudioSource>().Play();
                            soundbox2.GetComponent<AudioSource>().Play();

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }
                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 1: //67-2:문을 열고 들어가~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                //문 활성화
                                GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = true;
                                flag = false;
                            }
                            if (GameObject.Find("AODPoint").GetComponent<MoveAnimationObject>().movedone)
                            {
                                GameObject.Find("AODPoint").SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 2: //32-2:수신반은 관계자에게~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 3: //32-3:수신반에 있는~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 4: //69-3:준비작동식 설비가~
                        if (!IngAudio.isPlaying)
                        {
                            if(!flag)
                            {
                                stay = false;
                                SaveModeIngNum();//진행 번호 저장
                                Invoke("StayTime", 5);
                                flag = true;
                            } 
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("RRoom");
                        }

                        break;
                    case 5: //79-1:이제 수신반을 복구하기 위해~------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동

                            //사이렌 경종 울리기
                            soundbox1.SetActive(true);
                            soundbox2.SetActive(true);
                            soundbox1.GetComponent<AudioSource>().Play();
                            soundbox2.GetComponent<AudioSource>().Play();

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 6: //79-2:문을 열고~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                //문 활성화
                                GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = true;
                                flag = false;
                            }
                            if (GameObject.Find("AODPoint").GetComponent<MoveAnimationObject>().movedone)
                            {
                                GameObject.Find("AODPoint").SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 7: //81-1:수신반의 복구를~
                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 8: //881-2:수신반에 있는 복구 버튼을 누릅니다
                        

                        if (!IngAudio.isPlaying)
                        {
                            if(!flag)
                            {
                                //복구 외곽선
                                GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().enabled = true;
                                //복구 버튼 활성화
                                if (GameObject.Find("RCVPoint"))
                                    GameObject.Find("RCVPoint").GetComponent<BoxCollider>().enabled = true;
                                flag = true;
                            }
                           
                            if (GameObject.Find("RCVPoint").GetComponent<AnimationObject>().movedone)
                            {
                                //빨간불 꺼짐
                                GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("ReceiverLight").GetComponent<RedLight>().enabled = false;
                                GameObject.Find("redlight1").SetActive(false);
                                GameObject.Find("redlight2").SetActive(false);
                                //소리 끄기
                                GameObject.Find("SoundBox1").SetActive(false);
                                GameObject.Find("SoundBox2").SetActive(false);
                                GameObject.Find("RCVPoint").SetActive(false);

                                NextNum();
                            }
                        }
                        break;
                    case 9:    //81-3:울리고 있는 부저 사이렌 소리를~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                stay = false;
                                SaveModeIngNum();//진행 번호 저장
                                Invoke("StayTime", 5);
                                flag = false;
                            }
                           
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("RRoom");
                        }
                        break;
                }
                break;
            case Mode.Fire:
                switch(ingNum)
                {
                    case 0: //11_1:평상시 비화재보~
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동

                            //싸이렌
                            soundbox1.SetActive(true);
                            soundbox1.GetComponent<AudioSource>().Play();
                            //경종
                            soundbox2.SetActive(true);
                            soundbox2.GetComponent<AudioSource>().Play();
                            //비상방송
                            soundbox3.SetActive(true);
                            //soundbox3.GetComponent<AudioSource>().Play();

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                            stay = false;
                            stay2 = false;
                        }
                        
                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 1: //11_2:실제 화재는~
                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 2://11_3:문을 열고~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                //문 활성화
                                GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = true;
                                flag = false;
                            }
                            if (GameObject.Find("AODPoint").GetComponent<MoveAnimationObject>().movedone)
                            {
                                GameObject.Find("AODPoint").SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 3://12_1:수신반의~
                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 4://12_2:교실 내 비화재 상황에서~
                        if (!IngAudio.isPlaying)
                        {
                            if(!flag)
                            {
                                stay = false;
                                SaveModeIngNum();//진행 번호 저장
                                //5초후!
                                Invoke("StayTime", 5);
                                flag = true;
                            }
                            
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("4FSchool");
                        }
                        break;
                    case 5: //15_1:수신반 확인을 위해~-------------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동

                            //싸이렌
                            soundbox1.SetActive(true);
                            soundbox1.GetComponent<AudioSource>().Play();
                            //경종
                            soundbox2.SetActive(true);
                            soundbox2.GetComponent<AudioSource>().Play();
                            //비상방송
                            soundbox3.SetActive(true);
                            //soundbox3.GetComponent<AudioSource>().Play();

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                            stay = false;
                            stay2 = false;
                        }
                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 6: //15_2:문을 열고~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                //문 활성화
                                GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = true;  //박스콜라이더 활성화
                                flag = false;
                            }
                            if (GameObject.Find("AODPoint").GetComponent<MoveAnimationObject>().movedone)
                            {
                                GameObject.Find("AODPoint").SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 7: //16_1:주경종,지구경종~
                        if(!flag)
                        {
                            //버튼 외곽선 표시
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled = true;
                            //버튼 깜박이지 않음
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().notblink = true;

                            //버튼 콜라이더 활성화
                            GameObject.Find("MCPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("SCPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("EMPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("SRPoint").GetComponent<BoxCollider>().enabled = true;

                            flag = true;
                        }
                        //주경종
                        if(GameObject.Find("MCPoint").GetComponent<AnimationObject>().movedone)
                        {
                            SwichLight.SetActive(true);

                            if (!sflag && soundbox2.GetComponent<AudioSource>().volume < 0.2)
                            {
                                soundbox2.SetActive(false);
                            }
                            else if (sflag)
                            {
                                if (soundbox2.GetComponent<AudioSource>().volume > 0.2)
                                {
                                    soundbox2.GetComponent<AudioSource>().volume -= 0.15f;
                                }
                                sflag = false;
                            }
                            
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_MC_on", false); //애니메이션 돌아가기
                            GameObject.Find("MCPoint").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().EndBlink();
                        }
                        //지구경종
                        if (GameObject.Find("SCPoint").GetComponent<AnimationObject>().movedone)
                        {
                            SwichLight.SetActive(true);

                            if (!sflag&& soundbox2.GetComponent<AudioSource>().volume <0.2)
                            {
                                soundbox2.SetActive(false);
                            }
                            else if (sflag)
                            {
                                if (soundbox2.GetComponent<AudioSource>().volume > 0.2)
                                {
                                    soundbox2.GetComponent<AudioSource>().volume -= 0.15f;
                                    
                                }
                                sflag = false;
                            }
                                
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_SC_on", false); //애니메이션 돌아가기
                            GameObject.Find("SCPoint").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().EndBlink();
                        }
                        //비상음성
                        if (GameObject.Find("EMPoint").GetComponent<AnimationObject>().movedone)
                        {
                            SwichLight.SetActive(true);

                            if (soundbox2.GetComponent<AudioSource>().volume > 0)
                                sflag = true;

                            soundbox3.SetActive(false);
                            if(soundbox1.activeSelf)
                                soundbox1.GetComponent<AudioSource>().mute = false;
                            if (soundbox2.activeSelf)
                                soundbox2.GetComponent<AudioSource>().mute = false;

                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_E_on", false); //애니메이션 돌아가기
                            GameObject.Find("EMPoint").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().EndBlink();
                        }
                        //사이렌
                        if (GameObject.Find("SRPoint").GetComponent<AnimationObject>().movedone)
                        {
                            SwichLight.SetActive(true);

                            if (soundbox2.GetComponent<AudioSource>().volume > 0)
                                sflag = true;

                            soundbox1.SetActive(false);
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_S_on", false); //애니메이션 돌아가기
                            GameObject.Find("SRPoint").GetComponent<BoxCollider>().enabled = false;
                            if(GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().EndBlink();
                        }

                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying&&flag)
                        {
                            //모든 버튼을 눌러야 넘어가기
                            if (GameObject.Find("MCPoint").GetComponent<AnimationObject>().movedone&&
                                GameObject.Find("SCPoint").GetComponent<AnimationObject>().movedone&&
                                GameObject.Find("EMPoint").GetComponent<AnimationObject>().movedone&&
                                GameObject.Find("SRPoint").GetComponent<AnimationObject>().movedone)
                            {
                                if(!stay2)
                                {
                                    stay = false;
                                    //3초대기
                                    Invoke("StayTime", 5f);
                                    stay2 = true;
                                }
                                if(stay)
                                    NextNum();
                            }    
                        }

                        break;
                    case 8: //16_2:감지기가~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 9: //16-3:발신기의~
                        if(flag)
                        {
                            GameObject.Find("redlight3").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("redlight3").GetComponent<BlinkObject>().notblink = true;

                            flag = false;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("redlight3").GetComponent<BlinkObject>().EndBlink();
                            NextNum();
                        }
                        break;
                    case 10: //17_1:감지기를 교체하거나~
                        if(!flag)
                        {
                            //복구
                            GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("RSTPoint").GetComponent<BoxCollider>().enabled = true;
                            flag = true;
                        }
                        if (GameObject.Find("RSTPoint").GetComponent<AnimationObject>().movedone)
                        {
                            if(stay2)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_check", false); //애니메이션 돌아가기
                                GameObject.Find("RSTPoint").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().EndBlink();

                                //빨간불 소등
                                if (GameObject.Find("redlight1") != null)
                                    GameObject.Find("redlight1").SetActive(false);
                                if (GameObject.Find("redlight2") != null)
                                    GameObject.Find("redlight2").SetActive(false);

                                stay = false;
                                //3초 대기
                                Invoke("StayTime", 3f);
                                stay2 = false;
                            }

                            if(stay)
                            {
                                //버튼 외곽선 초기화
                                GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().ReBlink();
                                GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().ReBlink();
                                GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().ReBlink();
                                GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().ReBlink();
                                NextNum();
                            }
                        }
                        break;
                    case 11:    //18_1:음향장치의 복구~
                        if(flag)
                        {
                            //버튼 외곽선 표시
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled = true;

                            //버튼 깜박이지 않음
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().notblink = true;

                            //버튼 콜라이더 활성화
                            GameObject.Find("MCPoint2").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("SCPoint2").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("EMPoint2").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("SRPoint2").GetComponent<BoxCollider>().enabled = true;

                            flag = false;
                        }
                        //주경종
                        if (GameObject.Find("MCPoint2").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_MC_off", false); //애니메이션 돌아가기
                            GameObject.Find("MCPoint2").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().EndBlink();
                        }
                        //지구경종
                        if (GameObject.Find("SCPoint2").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_SC_off", false); //애니메이션 돌아가기
                            GameObject.Find("SCPoint2").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().EndBlink();
                        }
                        //비상음성
                        if (GameObject.Find("EMPoint2").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_E_off", false); //애니메이션 돌아가기
                            GameObject.Find("EMPoint2").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().EndBlink();
                        }
                        //사이렌
                        if (GameObject.Find("SRPoint2").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_S_off", false); //애니메이션 돌아가기
                            GameObject.Find("SRPoint2").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().EndBlink();
                        }

                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            //모든 버튼을 눌러야 넘어가기
                            if (GameObject.Find("MCPoint2").GetComponent<AnimationObject>().movedone &&
                                GameObject.Find("SCPoint2").GetComponent<AnimationObject>().movedone &&
                                GameObject.Find("EMPoint2").GetComponent<AnimationObject>().movedone &&
                                GameObject.Find("SRPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                if(!stay2)
                                {
                                    SwichLight.SetActive(false);
                                    stay = false;
                                    //3초대기
                                    Invoke("StayTime", 3f);
                                    stay2 = true;
                                }
                                if (stay)
                                    NextNum();
                            }
                        }
                        break;
                    case 12:    //19_1:마지막으로 스위치주의등의~
                        if(!flag)
                        {
                            GameObject.Find("redlight4").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("redlight4").GetComponent<BlinkObject>().notblink = true;
                            flag = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            stay = false;
                            fade = false;
                            flag = true;
                            NextNum();
                        }
                        break;
                    case 13:    //:19_2:만약 점등되었다면~
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                GameObject.Find("redlight4").GetComponent<BlinkObject>().EndBlink();
                                Invoke("StayTime", 5);
                                flag = false;
                            }
                            if (stay)
                            {
                                Invoke("FadeToBlack", 2f);
                                stay = false;
                            }
                            if (fade)
                            {
                                Invoke("FadeFromBlack", 2f);
                                fade = false;
                            }
                        }
                        break;
                    case 14:    //21_1:다음은 화재를~-----------------------------------------------(fade후
                        if(!flag &&IngAudio.clip==s_narration[14])
                        {
                            player.transform.position = GameObject.Find("MovePoint").transform.position;
                            player.transform.eulerAngles = GameObject.Find("MovePoint").transform.eulerAngles;
                            
                            /*상태 변화*/
                            if (GameObject.Find("redlight1") != null)
                                GameObject.Find("redlight1").SetActive(false);
                            if (GameObject.Find("redlight2") != null)
                                GameObject.Find("redlight2").SetActive(false);
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_ARS_on", true); //애니메이션 돌아가기
                            
                            stay = false;
                            stay2 = false;
                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag&&!IngAudio.isPlaying)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_ARS_on", false); //애니메이션 돌아가기
                            //애니메이션 완료 초기화
                            if (GameObject.Find("MCPoint") != null)
                                GameObject.Find("MCPoint").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("SCPoint") != null)
                                GameObject.Find("SCPoint").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("EMPoint") != null)
                                GameObject.Find("EMPoint").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("SRPoint") != null)
                                GameObject.Find("SRPoint").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("MCPoint2") != null)
                                GameObject.Find("MCPoint2").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("SCPoint2") != null)
                                GameObject.Find("SCPoint2").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("EMPoint2") != null)
                                GameObject.Find("EMPoint2").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("SRPoint2") != null)
                                GameObject.Find("SRPoint2").GetComponent<AnimationObject>().ReAnimPlay();
                            
                            //외곽선 초기화
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().ReBlink();

                            if (!stay2)
                            {
                                stay = false;
                                //3초 후에
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                                NextNum();
                        }

                        
                        break;
                    case 15:    //22_1:먼저 연동정지를 위해~
                        if(flag)
                        {
                            //버튼 외곽선 표시
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().enabled = true;


                            //버튼 깜박이지 않음
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().notblink = true;

                            //버튼 콜라이더 활성화
                            GameObject.Find("MCPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("SCPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("EMPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("SRPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("SPPoint").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("FSPoint").GetComponent<BoxCollider>().enabled = true;

                            flag = false;
                        }
                        

                        //주경종
                        if (GameObject.Find("MCPoint").GetComponent<AnimationObject>().movedone)
                        {
                            
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_MC_on", false); //애니메이션 돌아가기
                            GameObject.Find("MCPoint").GetComponent<BoxCollider>().enabled = false;

                            SwichLight.SetActive(true);
                            if (GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().EndBlink();
                        }
                        //지구경종
                        if (GameObject.Find("SCPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_SC_on", false); //애니메이션 돌아가기
                            GameObject.Find("SCPoint").GetComponent<BoxCollider>().enabled = false;

                            SwichLight.SetActive(true);
                            if (GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().EndBlink();
                        }
                        //비상음성
                        if (GameObject.Find("EMPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_E_on", false); //애니메이션 돌아가기
                            GameObject.Find("EMPoint").GetComponent<BoxCollider>().enabled = false;

                            SwichLight.SetActive(true);
                            if (GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().EndBlink();
                        }
                        //사이렌
                        if (GameObject.Find("SRPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_S_on", false); //애니메이션 돌아가기
                            GameObject.Find("SRPoint").GetComponent<BoxCollider>().enabled = false;

                            SwichLight.SetActive(true);
                            if (GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().EndBlink();
                        }
                        //스프링쿨러
                        if (GameObject.Find("SPPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_SP_on", false); //애니메이션 돌아가기
                            GameObject.Find("SPPoint").GetComponent<BoxCollider>().enabled = false;

                            SwichLight.SetActive(true);
                            if (GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().EndBlink();
                        }
                        //방화셔터
                        if (GameObject.Find("FSPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_FS_on", false); //애니메이션 돌아가기
                            GameObject.Find("FSPoint").GetComponent<BoxCollider>().enabled = false;

                            SwichLight.SetActive(true);
                            if (GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().enabled)
                            {
                                GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().EndBlink();
                            }
                                
                        }

                        //모든 버튼을 눌러야 넘어가기
                        if (GameObject.Find("MCPoint").GetComponent<AnimationObject>().movedone &&
                            GameObject.Find("SCPoint").GetComponent<AnimationObject>().movedone &&
                            GameObject.Find("EMPoint").GetComponent<AnimationObject>().movedone &&
                            GameObject.Find("SRPoint").GetComponent<AnimationObject>().movedone &&
                            GameObject.Find("SPPoint").GetComponent<AnimationObject>().movedone &&
                            GameObject.Find("FSPoint").GetComponent<AnimationObject>().movedone)
                        {
                            //그냥 소리만 재생
                            if (!IngAudio.isPlaying)
                            {
                                if(stay2)
                                {
                                    stay = false;
                                    //3초대기
                                    Invoke("StayTime", 3f);
                                    stay2 = false;
                                }
                                
                                if (stay)
                                    NextNum();
                            }
                        }
                        break;
                    case 16:    //23_1:축적/비축적 선택스위치를~
                        if(!flag)
                        {
                            //버튼 외곽선 표시
                            GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("ACMPoint").GetComponent<BoxCollider>().enabled = true;
                            flag = true;
                        }

                        if (GameObject.Find("ACMPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_ACM_on", false); //애니메이션 돌아가기
                            GameObject.Find("ACMPoint").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().enabled)
                            {
                                GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().EndBlink();
                            }

                            if(!stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }
                            
                            if (stay)
                                NextNum();
                        }
                        break;
                    case 17:    //24_1:다음은 동작시험스위치~
                        if(flag)
                        {
                            //동작
                            GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("PPoint").GetComponent<BoxCollider>().enabled = true;

                            //자동복구
                            GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("ARPoint").GetComponent<BoxCollider>().enabled = true;

                            flag = false;
                        }

                        //동작
                        if (GameObject.Find("PPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_P_on", false); //애니메이션 돌아가기
                            GameObject.Find("PPoint").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().enabled)
                            {
                                GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().EndBlink();
                            }
                        }
                        //자동복구
                        if (GameObject.Find("ARPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_AR_on", false); //애니메이션 돌아가기
                            GameObject.Find("ARPoint").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().enabled)
                            {
                                GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().EndBlink();
                            }
                        }
                        //둘 다 눌렀으면!
                        if(GameObject.Find("PPoint").GetComponent<AnimationObject>().movedone&&
                            GameObject.Find("ARPoint").GetComponent<AnimationObject>().movedone)
                        {
                            if(stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = false;
                            }
                            
                            if (stay)
                                NextNum();
                        }
                        
                        break;
                    case 18:    //25_1:회로선택 스위치를 돌려서~
                        if(!flag)
                        {
                            GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().notblink = true;

                            //버튼 외곽선 초기화
                            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().ReBlink();

                            flag = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 19:    //25_2:1단계
                        if (!IngAudio.isPlaying)
                            GameObject.Find("1Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("1Point").GetComponent<AnimationObject>().movedone)
                        {
                            
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_1_on", false); //애니메이션 돌아가기
                            StepLight1.SetActive(true);
                            GameObject.Find("1Point").GetComponent<BoxCollider>().enabled = false;

                            if (!stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 20:    //25_3:2단계
                        if (!IngAudio.isPlaying)
                            GameObject.Find("2Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("2Point").GetComponent<AnimationObject>().movedone)
                        {
                            
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_2_on", false); //애니메이션 돌아가기
                            StepLight2.SetActive(true);
                            GameObject.Find("2Point").GetComponent<BoxCollider>().enabled = false;

                            if (stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = false;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 21:    //25_4:3단계
                        if (!IngAudio.isPlaying)
                            GameObject.Find("3Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("3Point").GetComponent<AnimationObject>().movedone)
                        {
                            
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_3_on", false); //애니메이션 돌아가기
                            StepLight3.SetActive(true);
                            GameObject.Find("3Point").GetComponent<BoxCollider>().enabled = false;
                            
                            if (!stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 =true;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 22:    //25_5:4단계
                        if (!IngAudio.isPlaying)
                            GameObject.Find("4Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("4Point").GetComponent<AnimationObject>().movedone)
                        {
                           
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_4_on", false); //애니메이션 돌아가기
                            StepLight4.SetActive(true);
                            GameObject.Find("4Point").GetComponent<BoxCollider>().enabled = false;

                            if (stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = false;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 23:    //25_6:5단계
                        if(!IngAudio.isPlaying)
                             GameObject.Find("5Point").GetComponent<BoxCollider>().enabled = true;
                        
                        if (GameObject.Find("5Point").GetComponent<AnimationObject>().movedone)
                        {
                            
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_5_on", false); //애니메이션 돌아가기
                            StepLight5.SetActive(true);
                            GameObject.Find("5Point").GetComponent<BoxCollider>().enabled = false;

                            if (!stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 24:    //26_1:확인이 완료되었으면 회로선택~

                        //회로선택스위치를 초기로 돌리면 모든 버튼 활성되서 누르기
                        GameObject.Find("RSPoint").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("RSPoint").GetComponent<AnimationObject>().movedone)
                        {
                            if(flag)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_RS_on", false); //애니메이션 돌아가기
                                GameObject.Find("RSPoint").GetComponent<BoxCollider>().enabled = false;
                                StepLight5.SetActive(false);
                                StepLight4.SetActive(false);
                                StepLight3.SetActive(false);
                                StepLight2.SetActive(false);
                                StepLight1.SetActive(false);

                                if (GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().EndBlink();

                                //버튼 외곽선 표시
                                GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled = true;
                                GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled = true;
                                GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled = true;
                                GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled = true;
                                GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().enabled = true;
                                GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().enabled = true;
                                GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().enabled = true;
                                GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().enabled = true;


                                //버튼 깜박이지 않음
                                GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().notblink = true;
                                GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().notblink = true;
                                GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().notblink = true;
                                GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().notblink = true;
                                GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().notblink = true;
                                GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().notblink = true;
                                GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().notblink = true;
                                GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().notblink = true;


                                //버튼 콜라이더 활성화
                                GameObject.Find("MCPoint2").GetComponent<BoxCollider>().enabled = true;
                                GameObject.Find("SCPoint2").GetComponent<BoxCollider>().enabled = true;
                                GameObject.Find("EMPoint2").GetComponent<BoxCollider>().enabled = true;
                                GameObject.Find("SRPoint2").GetComponent<BoxCollider>().enabled = true;
                                GameObject.Find("SPPoint2").GetComponent<BoxCollider>().enabled = true;
                                GameObject.Find("FSPoint2").GetComponent<BoxCollider>().enabled = true;
                                GameObject.Find("PPoint2").GetComponent<BoxCollider>().enabled = true;
                                GameObject.Find("ARPoint2").GetComponent<BoxCollider>().enabled = true;

                                flag = false;
                            }
                            //주경종
                            if (GameObject.Find("MCPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_MC_off", false); //애니메이션 돌아가기
                                GameObject.Find("MCPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().EndBlink();
                            }
                            //지구경종
                            if (GameObject.Find("SCPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_SC_off", false); //애니메이션 돌아가기
                                GameObject.Find("SCPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().EndBlink();
                            }
                            //비상음성
                            if (GameObject.Find("EMPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_E_off", false); //애니메이션 돌아가기
                                GameObject.Find("EMPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().EndBlink();
                            }
                            //사이렌
                            if (GameObject.Find("SRPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_S_off", false); //애니메이션 돌아가기
                                GameObject.Find("SRPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().EndBlink();
                            }
                            //스프링쿨러
                            if (GameObject.Find("SPPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_SP_off", false); //애니메이션 돌아가기
                                GameObject.Find("SPPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().EndBlink();
                            }
                            //방화셔터
                            if (GameObject.Find("FSPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_FS_off", false); //애니메이션 돌아가기
                                GameObject.Find("FSPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().enabled)
                                {
                                    GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().EndBlink();
                                }

                            }
                            //동작
                            if (GameObject.Find("PPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_P_off", false); //애니메이션 돌아가기
                                GameObject.Find("PPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().enabled)
                                {
                                    GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().EndBlink();
                                }
                            }
                            //자동복구
                            if (GameObject.Find("ARPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_AR_off", false); //애니메이션 돌아가기
                                GameObject.Find("ARPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().enabled)
                                {
                                    GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().EndBlink();
                                }
                            }
                            

                            //그냥 소리만 재생
                            if (!IngAudio.isPlaying)
                            {
                                //모든 버튼을 눌러야 넘어가기
                                if (GameObject.Find("MCPoint2").GetComponent<AnimationObject>().movedone &&
                                    GameObject.Find("SCPoint2").GetComponent<AnimationObject>().movedone &&
                                    GameObject.Find("EMPoint2").GetComponent<AnimationObject>().movedone &&
                                    GameObject.Find("SRPoint2").GetComponent<AnimationObject>().movedone &&
                                    GameObject.Find("SPPoint2").GetComponent<AnimationObject>().movedone &&
                                    GameObject.Find("FSPoint2").GetComponent<AnimationObject>().movedone&&
                                    GameObject.Find("PPoint2").GetComponent<AnimationObject>().movedone&&
                                    GameObject.Find("ARPoint2").GetComponent<AnimationObject>().movedone
                                    )
                                {
                                    SwichLight.SetActive(false);
                                    GameObject.Find("RSPoint").GetComponent<BoxCollider>().enabled = false;
                                    fade = false;
                                    stay = false;
                                    flag = false;
                                    NextNum();
                                }
                            }
                        }
                        break;
                    case 25:    //26_2:마지막으로 비축적전환버튼을~
                        if (!IngAudio.isPlaying&&stay2)
                        {
                            GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().notblink = true;
                            GameObject.Find("ACMPoint2").GetComponent<BoxCollider>().enabled = true;

                            //축적
                            if (GameObject.Find("ACMPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_ACM_off", false); //애니메이션 돌아가기
                                GameObject.Find("ACMPoint2").GetComponent<BoxCollider>().enabled = false;
                                if (GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().enabled)
                                {
                                    GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().EndBlink();
                                }
                                fade = false;
                                stay = false;
                                flag = true;
                                stay2 = false;
                            }
                        }
                        if (flag)
                        {
                            GameObject.Find("redlight4").GetComponent<BlinkObject>().EndBlink();
                            Invoke("StayTime", 5);
                            flag = false;
                        }
                        if (stay)
                        {
                            Invoke("FadeToBlack", 2f);
                            stay = false;
                        }
                        if (fade)
                        {
                            Invoke("FadeFromBlack", 2f);
                            fade = false;
                        }
                        break;
                    case 26:    //28_1:다음은 수신기에서~----------------------------------------------------------------fade후
                        if (!flag&&IngAudio.clip==s_narration[26])
                        {
                            player.transform.position = GameObject.Find("MovePoint").transform.position;
                            player.transform.eulerAngles = GameObject.Find("MovePoint").transform.eulerAngles;
                            
                            /*상태 변화*/
                            if (GameObject.Find("redlight1") != null)
                                GameObject.Find("redlight1").SetActive(false);
                            if (GameObject.Find("redlight2") != null)
                                GameObject.Find("redlight2").SetActive(false);

                            StepLight1.SetActive(false);
                            StepLight2.SetActive(false);
                            StepLight3.SetActive(false);
                            StepLight4.SetActive(false);
                            StepLight5.SetActive(false);

                            fade = false;
                            stay2 = false;
                            stay = false;
                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag&& !IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 27:    //28_2:도통시험버튼을 누르세요
                        GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().notblink = true;
                        GameObject.Find("EVPoint").GetComponent<BoxCollider>().enabled = true;

                        //축적
                        if (GameObject.Find("EVPoint").GetComponent<AnimationObject>().movedone)
                        {
                            UCLight.SetActive(true);
                            SwichLight.SetActive(true);
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_EV_on", false); //애니메이션 돌아가기
                            GameObject.Find("EVPoint").GetComponent<BoxCollider>().enabled = false;
                            if (GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().enabled)
                            {
                                GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().EndBlink();
                            }

                            //외곽선 표시 초기화
                            GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().ReBlink();
                            NextNum();
                        }

                        break;
                   case 28:    //29_1:회로선택스위치를~
                        GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().notblink = true;

                        if (!IngAudio.isPlaying)
                        {
                            //애니메이션 완료 초기화
                            if (GameObject.Find("1Point") != null)
                                GameObject.Find("1Point").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("2Point") != null)
                                GameObject.Find("2Point").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("3Point") != null)
                                GameObject.Find("3Point").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("4Point") != null)
                                GameObject.Find("4Point").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("5Point") != null)
                                GameObject.Find("5Point").GetComponent<AnimationObject>().ReAnimPlay();
                            if (GameObject.Find("RSPoint") != null)
                                GameObject.Find("RSPoint").GetComponent<AnimationObject>().ReAnimPlay();
                            NextNum();
                        }
                        break;
                    case 29:    //29-2:1단계로~
                        if (!IngAudio.isPlaying)
                            GameObject.Find("1Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("1Point").GetComponent<AnimationObject>().movedone)
                        {
                            UCLight.SetActive(false);
                            NormalLight.SetActive(true);
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_1_on", false); //애니메이션 돌아가기
                            GameObject.Find("1Point").GetComponent<BoxCollider>().enabled = false;

                            if (!stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 30:    //29-3:2단계로~
                        if (!IngAudio.isPlaying)
                            GameObject.Find("2Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("2Point").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_2_on", false); //애니메이션 돌아가기
                            GameObject.Find("2Point").GetComponent<BoxCollider>().enabled = false;

                            if (stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = false;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 31:    //29-4:3단계로~
                        if (!IngAudio.isPlaying)
                            GameObject.Find("3Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("3Point").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_3_on", false); //애니메이션 돌아가기
                            GameObject.Find("3Point").GetComponent<BoxCollider>().enabled = false;

                            if (!stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 32:    //29-5:4단계로~
                        if (!IngAudio.isPlaying)
                            GameObject.Find("4Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("4Point").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_4_on", false); //애니메이션 돌아가기
                            GameObject.Find("4Point").GetComponent<BoxCollider>().enabled = false;

                            if (stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = false;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 33:    //29-6:5단계로~
                        if (!IngAudio.isPlaying)
                            GameObject.Find("5Point").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("5Point").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_5_on", false); //애니메이션 돌아가기
                            GameObject.Find("5Point").GetComponent<BoxCollider>().enabled = false;

                            

                            if (!stay2)
                            {
                                stay = false;
                                //3초대기
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                                NextNum();
                        }
                        break;
                    case 34:    //29_7:수신기에 전압계가~

                        if(!IngAudio.isPlaying)
                        {
                            NormalLight.SetActive(true);
                            UCLight.SetActive(false);
                            StepLight1.SetActive(false);
                            StepLight2.SetActive(false);
                            StepLight3.SetActive(false);
                            StepLight4.SetActive(false);
                            StepLight5.SetActive(false);
                            SwichLight.SetActive(true);
                            NextNum();
                        }

                        break;
                    case 35:    //30_1:회로스위치를 초기위치로~
                        GameObject.Find("RSPoint").GetComponent<BoxCollider>().enabled = true;
                        if (GameObject.Find("RSPoint").GetComponent<AnimationObject>().movedone)
                        {
                           
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_RS_on", false); //애니메이션 돌아가기
                            GameObject.Find("RSPoint").GetComponent<BoxCollider>().enabled = false;
                            NormalLight.SetActive(false);
                            UCLight.SetActive(true);
                            if(GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().enabled)
                                GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().EndBlink();
                            
                            //외곽선 초기화
                            GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().ReBlink();
                            NextNum();
                        }
                        break;
                    case 36:    //30_2:마지막으로~
                        GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().notblink = true;

                        GameObject.Find("EVPoint2").GetComponent<BoxCollider>().enabled = true;
                        if (GameObject.Find("EVPoint2").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("Receiver").GetComponent<Animator>().SetBool("RCV_EV_off", false); //애니메이션 돌아가기
                            GameObject.Find("EVPoint2").GetComponent<BoxCollider>().enabled = false;
                            UCLight.SetActive(false);
                            SwichLight.SetActive(false);

                            if (!IngAudio.isPlaying)
                            {
                                if(stay2)
                                {
                                    stay = false;
                                    SaveModeIngNum();//진행 번호 저장
                                    Invoke("StayTime", 5);
                                    stay2 = false;
                                }
                               
                                if (stay)
                                    Valve.VR.SteamVR_LoadLevel.Begin("4FSchool");
                            }
                        }
                        break;
                }
                break;
            case Mode.Pump:

                break;
            case Mode.Campus1:

                break;


        }

        if (IngAudio.clip != null && IngAudio.clip != s_narration[ingNum])              //다음 대사로 넘어간 경우 다음대사로 빠르게 교체
        {
            IngAudio.Stop();
            ndone = false;
        }
        if (!ndone && !IngAudio.isPlaying)                                            //오디오가 멈춰있을때
        {
            IngAudio.clip = s_narration[ingNum];                            //해당 번호 나레이션 재생
            IngAudio.Play();
            ndone = true;                                                   //나레이션 작동중
        }
    }

    void SaveModeIngNum()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, ingNum);
        PlayerPrefs.Save();
        Debug.Log(SceneManager.GetActiveScene().name + " : " + ingNum);
    }

    int LoadModeIngNum()
    {
        Debug.Log(SceneManager.GetActiveScene().name + " : " + ingNum);
        return PlayerPrefs.GetInt(SceneManager.GetActiveScene().name, ingNum);
    }

    void SettingMode()
    {
        //모드
        if (PlayerPrefs.GetString("mode") != null)     //현재 플레이어 정보에 모드 데이터가 있다면
        {
            mode = PlayerPrefs.GetString("mode");   //데이터를 받아오기
        }

        if (W)
            mode = Wmode;

        if (mode.Equals("Spring1"))
        {
            modeNum = Mode.Spring1;
            s_narration = spring1audio;
            ModeObject = GameObject.Find("Spring1");
            GameObject.Find("Spring2").gameObject.SetActive(false);
            GameObject.Find("Fire").gameObject.SetActive(false);
            GameObject.Find("Pump").gameObject.SetActive(false);
            GameObject.Find("Campus1").gameObject.SetActive(false);
        }
        else if (mode.Equals("Spring2"))
        {
            modeNum = Mode.Spring2;
            s_narration = spring2audio;
            ModeObject = GameObject.Find("Spring2");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
        }
        else if (mode.Equals("Fire"))
        {
            modeNum = Mode.Fire;
            s_narration = fireaudio;
            ModeObject = GameObject.Find("Fire");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
        }
        else if (mode.Equals("Pump"))
        {
            modeNum = Mode.Pump;
            s_narration = pumpaudio;
            ModeObject = GameObject.Find("Pump");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
        }
        else if (mode.Equals("Campus1"))
        {
            modeNum = Mode.Campus1;
            s_narration = campusaudio;
            ModeObject = GameObject.Find("Campus");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
        }

        Debug.Log("Here my mode: " + PlayerPrefs.GetString("mode") + "(" + mode + ")");
    }

    void SettingObject()
    {
        //싸이렌
        if (GameObject.Find("SoundBox1") != null)
        {
            soundbox1 = GameObject.Find("SoundBox1");
            soundbox1.SetActive(false);
        }

        //경종
        if (GameObject.Find("SoundBox2") != null)
        {
            soundbox2 = GameObject.Find("SoundBox2");
            soundbox2.SetActive(false);
        }
        //비상방송
        if (GameObject.Find("SoundBox3") != null)
        {
            soundbox3 = GameObject.Find("SoundBox3");
            soundbox3.SetActive(false);
        }

        /*나레이션중 행동 제한*/
        if (GameObject.Find("AODPoint") != null)
            GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("MCPoint") != null)
            GameObject.Find("MCPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("SCPoint") != null)
            GameObject.Find("SCPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("EMPoint") != null)
            GameObject.Find("EMPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("SRPoint") != null)
            GameObject.Find("SRPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("RSTPoint") != null)
            GameObject.Find("RSTPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화


        if (GameObject.Find("MCPoint2") != null)
            GameObject.Find("MCPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("SCPoint2") != null)
            GameObject.Find("SCPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("EMPoint2") != null)
            GameObject.Find("EMPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("SRPoint2") != null)
            GameObject.Find("SRPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("SPPoint") != null)
            GameObject.Find("SPPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("FSPoint") != null)
            GameObject.Find("FSPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("ACMPoint") != null)
            GameObject.Find("ACMPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("PPoint") != null)
            GameObject.Find("PPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARPoint") != null)
            GameObject.Find("ARPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("1Point") != null)
            GameObject.Find("1Point").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("2Point") != null)
            GameObject.Find("2Point").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("3Point") != null)
            GameObject.Find("3Point").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("4Point") != null)
            GameObject.Find("4Point").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("5Point") != null)
            GameObject.Find("5Point").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("RSPoint") != null)
            GameObject.Find("RSPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("SPPoint2") != null)
            GameObject.Find("SPPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("FSPoint2") != null)
            GameObject.Find("FSPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ACMPoint2") != null)
            GameObject.Find("ACMPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("PPoint2") != null)
            GameObject.Find("PPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARPoint2") != null)
            GameObject.Find("ARPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("EVPoint") != null)
            GameObject.Find("EVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("EVPoint2") != null)
            GameObject.Find("EVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        /*외곽선*/

        //복구
        if (GameObject.Find("AO_MO01_B05") != null)
            GameObject.Find("AO_MO01_B05").GetComponent<BlinkObject>().enabled = false;
        //주경종
        if (GameObject.Find("AO_MO01_B06") != null)
            GameObject.Find("AO_MO01_B06").GetComponent<BlinkObject>().enabled = false;
        //지구
        if (GameObject.Find("AO_MO01_B07") != null)
            GameObject.Find("AO_MO01_B07").GetComponent<BlinkObject>().enabled = false;
        //비상방송
        if (GameObject.Find("AO_MO01_B08") != null)
            GameObject.Find("AO_MO01_B08").GetComponent<BlinkObject>().enabled = false;
        //사이렌
        if (GameObject.Find("AO_MO01_B09") != null)
            GameObject.Find("AO_MO01_B09").GetComponent<BlinkObject>().enabled = false;
        //발신기 표시등
        if (GameObject.Find("redlight3") != null)
            GameObject.Find("redlight3").GetComponent<BlinkObject>().enabled = false;
        //스위치 주의등
        if (GameObject.Find("redlight4") != null)
            GameObject.Find("redlight4").GetComponent<BlinkObject>().enabled = false;
        //스프링쿨러
        if (GameObject.Find("AO_MO01_B10") != null)
            GameObject.Find("AO_MO01_B10").GetComponent<BlinkObject>().enabled = false;
        //방화셔터
        if (GameObject.Find("AO_MO01_B012") != null)
            GameObject.Find("AO_MO01_B012").GetComponent<BlinkObject>().enabled = false;
        //축적스위치
        if (GameObject.Find("AO_MO_Switch02") != null)
            GameObject.Find("AO_MO_Switch02").GetComponent<BlinkObject>().enabled = false;
        //동작
        if (GameObject.Find("AO_MO01_B03") != null)
            GameObject.Find("AO_MO01_B03").GetComponent<BlinkObject>().enabled = false;
        //자동복구
        if (GameObject.Find("AO_MO01_B04") != null)
            GameObject.Find("AO_MO01_B04").GetComponent<BlinkObject>().enabled = false;
        //다이얼
        if (GameObject.Find("AO_MO_Dial01") != null)
            GameObject.Find("AO_MO_Dial01").GetComponent<BlinkObject>().enabled = false;
        //도통
        if (GameObject.Find("AO_MO01_B02") != null)
            GameObject.Find("AO_MO01_B02").GetComponent<BlinkObject>().enabled = false;

        /*지구표시*/
        if (GameObject.Find("1steplight") != null)
        {
            StepLight1 = GameObject.Find("1steplight");
            StepLight1.SetActive(false);
        }
        if (GameObject.Find("2steplight") != null)
        {
            StepLight2 = GameObject.Find("2steplight");
            StepLight2.SetActive(false);
        }
        if (GameObject.Find("3steplight") != null)
        {
            StepLight3 = GameObject.Find("3steplight");
            StepLight3.SetActive(false);
        }
        if (GameObject.Find("4steplight") != null)
        {
            StepLight4 = GameObject.Find("4steplight");
            StepLight4.SetActive(false);
        }
        if (GameObject.Find("5steplight") != null)
        {
            StepLight5 = GameObject.Find("5steplight");
            StepLight5.SetActive(false);
        }
        if (GameObject.Find("swichlight") != null)
        {
            SwichLight = GameObject.Find("swichlight");
            SwichLight.SetActive(false);
        }
        if (GameObject.Find("uclight") != null)
        {
            UCLight = GameObject.Find("uclight");
            UCLight.SetActive(false);
        }
        if (GameObject.Find("normallight") != null)
        {
            NormalLight = GameObject.Find("normallight");
            NormalLight.SetActive(false);
        }
    }

    //행동번호 다음번호로
    public void NextNum()
    {
        ingNum++;
    }
    //원하는 모드
    public void ChangeMode(string _mode, int _num)
    {
        mode = _mode;
        ingNum = _num;
    }

    protected void OutLineTurn()
    {
        if (outlineflag)
            outlineflag = false;
        else
            outlineflag = true;
        flag = true;
    }

    protected void StayTime()
    {
        Debug.Log("Stay");
        stay = true;
    }

    //페이드용
    private void FadeToBlack() //원래화면->검은 화면
    {
        SteamVR_Fade.Start(Color.clear, 0f);
        SteamVR_Fade.Start(Color.black, 1f);    //인아웃 시간 설정

        GameObject.Find("SteamVRObjects").transform.Find("LeftHand").gameObject.SetActive(true);
        GameObject.Find("SteamVRObjects").transform.Find("RightHand").gameObject.SetActive(true);

        Invoke("CountTime", 0.5f);  //인아웃 대기 시간
        Debug.Log("fadeout");

    }
    private void FadeFromBlack()   //검은화면->원래화면
    {
        SteamVR_Fade.Start(Color.black, 0f);
        SteamVR_Fade.Start(Color.clear, 1f);    //인아웃 시간 설정

        Debug.Log("fadein");

        stay = false;
        NextNum();
    }

    private void CountTime()
    {
        fade = true;
    }
}

