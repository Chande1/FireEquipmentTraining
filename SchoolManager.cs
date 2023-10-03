using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SchoolManager : MonoBehaviour
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
    [SerializeField] GameObject soundbox1;
    [SerializeField] GameObject soundbox2;

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
    [Header("일반교실")]
    [SerializeField] AudioClip[] campusaudio1;        //교내 소방 설비 오디오
    [Header("과학실")]
    [SerializeField] AudioClip[] campusaudio2;        //교내 소방 설비 오디오
    [Header("유치원")]
    [SerializeField] AudioClip[] campusaudio3;        //교내 소방 설비 오디오
    [Header("급식실")]
    [SerializeField] AudioClip[] campusaudio4;        //교내 소방 설비 오디오
    [Header("복도")]
    [SerializeField] AudioClip[] campusaudio5;        //교내 소방 설비 오디오
    [Header("계단")]
    [SerializeField] AudioClip[] campusaudio6;        //교내 소방 설비 오디오



    protected bool flag;
    protected bool outlineflag;                     //외각선 깜박임용 플래그
    protected bool stay;
    protected bool stay2;
    protected bool fade;

    protected GameObject RedLight;
    protected GameObject RedLight2;

    protected GameObject Arrow1;
    protected GameObject Arrow2;
    protected GameObject Arrow3;
    protected GameObject Arrow4;
    protected GameObject Arrow5;
    protected GameObject Arrow6;
    protected GameObject Arrow7;
    protected GameObject Arrow8;
    protected GameObject Arrow9;

    protected GameObject NT1;
    protected GameObject NT2;
    protected GameObject NT3;
    protected GameObject NT4;
    protected GameObject NT5;

    protected GameObject Dirty;
    protected GameObject Dirty2;

    protected GameObject Feg1;

    protected GameObject QM1E;
    protected GameObject QM7E;
    protected GameObject QM11E;
    protected GameObject QM13E;
    protected GameObject QM14E;

    protected GameObject Line1;
    protected GameObject Line2;
    protected GameObject Line3;
    protected GameObject Line4;
    protected GameObject Line5;
    protected GameObject Line6;
    protected GameObject Line7;
    protected GameObject Line8;
    protected GameObject Line9;

    protected GameObject TPoint1;
    protected GameObject TPoint2;
    protected GameObject TPoint3;
    protected GameObject TPoint4;
    protected GameObject TPoint5;
    protected GameObject TPoint6;
    protected GameObject TPoint7;
    protected GameObject TPoint8;
    protected GameObject TPoint9;

    protected GameObject TA1;
    protected GameObject TA2;
    protected GameObject TA3;
    protected GameObject TA4;
    protected GameObject TA5;
    protected GameObject TA6;
    protected GameObject TA7;

    void Awake()
    {
        //이동 포인트
        if (GameObject.Find("TPoint1") != null)
        {
            TPoint1 = GameObject.Find("TPoint1");
            TPoint1.SetActive(false);
        }
        if (GameObject.Find("TPoint2") != null)
        {
            TPoint2 = GameObject.Find("TPoint2");
            TPoint2.SetActive(false);
        }
        if (GameObject.Find("TPoint3") != null)
        {
            TPoint3 = GameObject.Find("TPoint3");
            TPoint3.SetActive(false);
        }
        if (GameObject.Find("TPoint4") != null)
        {
            TPoint4 = GameObject.Find("TPoint4");
            TPoint4.SetActive(false);
        }
        if (GameObject.Find("TPoint5") != null)
        {
            TPoint5 = GameObject.Find("TPoint5");
            TPoint5.SetActive(false);
        }
        if (GameObject.Find("TPoint6") != null)
        {
            TPoint6 = GameObject.Find("TPoint6");
            TPoint6.SetActive(false);
        }
        if (GameObject.Find("TPoint7") != null)
        {
            TPoint7 = GameObject.Find("TPoint7");
            TPoint7.SetActive(false);
        }
        if (GameObject.Find("TPoint8") != null)
        {
            TPoint8 = GameObject.Find("TPoint8");
            TPoint8.SetActive(false);
        }
    }

    void Start()
    {
        if (LoadModeIngNum() == 0)
        {
            ingNum = 0;
            Debug.Log("1");
        }
        else if (LoadModeIngNum() > 0)
        {
            ingNum = LoadModeIngNum() + 1;  //지난 진행 번호 불러오기
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
        SettingObject();
        ndone = false;

        startpoint = GameObject.Find("StartPoint");
        player = GameObject.Find("Player");
        IngAudio = gameObject.GetComponent<AudioSource>();  //오디오 소스 받아오기   

        soundbox1 = GameObject.Find("SoundBox1");
        soundbox2 = GameObject.Find("SoundBox2");

        flag = false;
        outlineflag = false;
        stay = false;
        stay2 = false;
            
        fade = false;
    }

    void SettingObject()
    {
        /*외부소리 작동*/
        //사이렌 경종 울리기
        soundbox1 = GameObject.Find("SoundBox1");
        soundbox2 = GameObject.Find("SoundBox2");
        ;
        /*나레이션중 행동 제한*/
        if (GameObject.Find("AODoorPoint") != null)
            GameObject.Find("AODoorPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("Card1") != null)
            GameObject.Find("Card1").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("Card2") != null)
            GameObject.Find("Card2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("Card3") != null)
            GameObject.Find("Card3").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("4F_CellingDirt") != null)
        {
            Dirty = GameObject.Find("4F_CellingDirt");
            Dirty.SetActive(false);
        }
        if(GameObject.Find("4FProp_01_CeilingProp04_Blink_2")!=null)
        {
            Dirty2 = GameObject.Find("4FProp_01_CeilingProp04_Blink_2");
            Dirty2.SetActive(false);
        }

        //텔레포트 공간 등록
        if (GameObject.Find("TeleportionArea") != null)
        {
            TA1 = GameObject.Find("TeleportionArea");
            TA1.SetActive(false);
        }
        if (GameObject.Find("TeleportionArea2") != null)
        {
            TA2 = GameObject.Find("TeleportionArea2");
            TA2.SetActive(false);
        }
        if (GameObject.Find("TeleportionArea3") != null)
        {
            TA3 = GameObject.Find("TeleportionArea3");
            TA3.SetActive(false);
        }
        if (GameObject.Find("TeleportionArea4") != null)
        {
            TA4 = GameObject.Find("TeleportionArea4");
            TA4.SetActive(false);
        }
        if (GameObject.Find("TeleportionArea5") != null)
        {
            TA5 = GameObject.Find("TeleportionArea5");
            TA5.SetActive(false);
        }
        if (GameObject.Find("TeleportionArea6") != null)
        {
            TA6 = GameObject.Find("TeleportionArea6");
            TA6.SetActive(false);
        }
        if (GameObject.Find("TeleportionArea7") != null)
        {
            TA7 = GameObject.Find("TeleportionArea7");
            TA7.SetActive(false);
        }

        


        //화살표 등록
        if (GameObject.Find("ArrowMark") != null)
        {
            Arrow1 = GameObject.Find("ArrowMark");
            Arrow1.SetActive(false);
        }
        if (GameObject.Find("ArrowMark2") != null)
        {
            Arrow2 = GameObject.Find("ArrowMark2");
            Arrow2.SetActive(false);
        }
        if (GameObject.Find("ArrowMark3") != null)
        {
            Arrow3 = GameObject.Find("ArrowMark3");
            Arrow3.SetActive(false);
        }
        if (GameObject.Find("ArrowMark4") != null)
        {
            Arrow4 = GameObject.Find("ArrowMark4");
            Arrow4.SetActive(false);
        }
        if (GameObject.Find("ArrowMark5") != null)
        {
            Arrow5 = GameObject.Find("ArrowMark5");
            Arrow5.SetActive(false);
        }
        if (GameObject.Find("ArrowMark6") != null)
        {
            Arrow6 = GameObject.Find("ArrowMark6");
            Arrow6.SetActive(false);
        }
        if (GameObject.Find("ArrowMark7") != null)
        {
            Arrow7 = GameObject.Find("ArrowMark7");
            Arrow7.SetActive(false);
        }
        if (GameObject.Find("ArrowMark8") != null)
        {
            Arrow8 = GameObject.Find("ArrowMark8");
            Arrow8.SetActive(false);
        }
        if (GameObject.Find("ArrowMark9") != null)
        {
            Arrow9 = GameObject.Find("ArrowMark9");
            Arrow9.SetActive(false);
        }


        //이름표 등록
        if (GameObject.Find("NameTag1") != null)
        {
            NT1 = GameObject.Find("NameTag1");
            NT1.SetActive(false);
        }
        if (GameObject.Find("NameTag2") != null)
        {
            NT2 = GameObject.Find("NameTag2");
            NT2.SetActive(false);
        }


        //문제 관련 오브젝트
        if (GameObject.Find("QM1E") != null)
        {
            QM1E = GameObject.Find("QM1E");
            QM1E.SetActive(false);
        }
        if (GameObject.Find("QM7E") != null)
        {
            QM7E = GameObject.Find("QM7E");
            QM7E.SetActive(false);
        }
        if (GameObject.Find("QM11E") != null)
        {
            QM11E = GameObject.Find("QM11E");
            QM11E.SetActive(false);
        }
        if (GameObject.Find("QM13E") != null)
        {
            QM13E = GameObject.Find("QM13E");
            QM13E.SetActive(false);
        }
        if (GameObject.Find("QM14E") != null)
        {
            QM14E = GameObject.Find("QM14E");
            QM14E.SetActive(false);
        }

        //길방향 등록
        if (GameObject.Find("Line1") != null)
        {
            Line1 = GameObject.Find("Line1");
            Line1.SetActive(false);
        }
        if (GameObject.Find("Line2") != null)
        {
            Line2 = GameObject.Find("Line2");
            Line2.SetActive(false);
        }
        if (GameObject.Find("Line3") != null)
        {
            Line3 = GameObject.Find("Line3");
            Line3.SetActive(false);
        }
        if (GameObject.Find("Line4") != null)
        {
            Line4 = GameObject.Find("Line4");
            Line4.SetActive(false);
        }
        if (GameObject.Find("Line5") != null)
        {
            Line5 = GameObject.Find("Line5");
            Line5.SetActive(false);
        }
        if (GameObject.Find("Line6") != null)
        {
            Line6 = GameObject.Find("Line6");
            Line6.SetActive(false);
        }
        if (GameObject.Find("Line7") != null)
        {
            Line7 = GameObject.Find("Line7");
            Line7.SetActive(false);
        }
        if (GameObject.Find("Line8") != null)
        {
            Line8 = GameObject.Find("Line8");
            Line8.SetActive(false);
        }
        if (GameObject.Find("Line9") != null)
        {
            Line9 = GameObject.Find("Line9");
            Line9.SetActive(false);
        }

        //사용할 빨간불이 있는지 검사
        if (GameObject.Find("RedLight") != null)
        {
            RedLight = GameObject.Find("RedLight");
            RedLight.SetActive(false);
        }
        if (GameObject.Find("redlight") != null)
        {
            RedLight2 = GameObject.Find("redlight");
            RedLight2.SetActive(false);
        }

        /*외곽선*/
        //교실-연기 감지기
        if (GameObject.Find("4FProp_01_CeilingProp04_Blink") != null)
            GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().enabled = false;
        
        //교실-차동식 열감지기
        if (GameObject.Find("4FProp_01_CeilingProp02") != null)
            GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().enabled = false;
        //교실-냉난방기
        if (GameObject.Find("AO_AT_Airconditioner") != null)
            GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().enabled = false;
        //교실-스프링쿨러
        if (GameObject.Find("4FProp_01_CeilingProp03") != null)
            GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().enabled = false;
        //교실-소화기
        if (GameObject.Find("AO_AT_Fextinguisher") != null)
            GameObject.Find("AO_AT_Fextinguisher").GetComponent<BlinkObject>().enabled = false;
        //교실-비상방송용스피커
        if (GameObject.Find("4FProp_01_CeilingProp01") != null)
            GameObject.Find("4FProp_01_CeilingProp01").GetComponent<BlinkObject>().enabled = false;
        //교실-비상방송용스피커2
        if (GameObject.Find("4FProp_01_CeilingProp01_2") != null)
            GameObject.Find("4FProp_01_CeilingProp01_2").GetComponent<BlinkObject>().enabled = false;
        //과학실-간이소화기
        if (GameObject.Find("4FProp_02_FEProp06") != null)
            GameObject.Find("4FProp_02_FEProp06").GetComponent<BlinkObject>().enabled = false;
        //과학실-투척용소화기
        if (GameObject.Find("4FProp_02_FEProp05") != null)
            GameObject.Find("4FProp_02_FEProp05").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("4FProp_02_FEProp05_7") != null)
            GameObject.Find("4FProp_02_FEProp05_7").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("4FProp_02_FEProp05_8") != null)
            GameObject.Find("4FProp_02_FEProp05_8").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("4FProp_02_FEProp05_9") != null)
            GameObject.Find("4FProp_02_FEProp05_9").GetComponent<BlinkObject>().enabled = false;

        //유치원-투척용소화기
        if (GameObject.Find("4FProp_02_FEProp05_3") != null)
            GameObject.Find("4FProp_02_FEProp05_3").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("4FProp_02_FEProp05_4") != null)
            GameObject.Find("4FProp_02_FEProp05_4").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("4FProp_02_FEProp05_5") != null)
            GameObject.Find("4FProp_02_FEProp05_5").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("4FProp_02_FEProp05_6") != null)
            GameObject.Find("4FProp_02_FEProp05_6").GetComponent<BlinkObject>().enabled = false;
        //급식실-자동확산소화기
        if (GameObject.Find("4FProp_02_FEProp08") != null)
            GameObject.Find("4FProp_02_FEProp08").GetComponent<BlinkObject>().enabled = false;
        //급식실-열감지기
        if (GameObject.Find("4FProp_01_CeilingProp02_2") != null)
            GameObject.Find("4FProp_01_CeilingProp02_2").GetComponent<BlinkObject>().enabled = false;
        //급식실-K급 소화기
        if (GameObject.Find("4FProp_02_FEProp03_2") != null)
            GameObject.Find("4FProp_02_FEProp03_2").GetComponent<BlinkObject>().enabled = false;
        //복도-완강기
        if (GameObject.Find("4FProp_02_WGG01") != null)
            GameObject.Find("4FProp_02_WGG01").GetComponent<BlinkObject>().enabled = false;
        //복도-연기감지기
        if (GameObject.Find("4FProp_01_CeilingProp04_2") != null)
            GameObject.Find("4FProp_01_CeilingProp04_2").GetComponent<BlinkObject>().enabled = false;
        //복도-소화전
        if (GameObject.Find("OM_Fireplug") != null)
            GameObject.Find("OM_Fireplug").GetComponent<BlinkObject>().enabled = false;
        //계단-계단통로유도등
        if (GameObject.Find("AO_AT_Exitlight_02") != null)
            GameObject.Find("AO_AT_Exitlight_02").GetComponent<BlinkObject>().enabled = false;
        

        /*진행오브젝트제한*/
        //교실-오염물질
        if (GameObject.Find("4F_CellingDirt") != null)
        {
            Dirty = GameObject.Find("4F_CellingDirt");
            Dirty.SetActive(false);
        }
        //교실-소화기
        if (GameObject.Find("AO_AT_Fextinguisher") != null)
        {
            Feg1 = GameObject.Find("AO_AT_Fextinguisher");
            Feg1.SetActive(false);
        }
        
    }

    void Update()
    {
        player = GameObject.Find("Player");

        /*모드별로 재생*/
        switch (modeNum)
        {
            case Mode.Spring1:
                break;
            case Mode.Spring2:
                break;
            case Mode.Fire:
                switch(ingNum)
                {
                    case 0: //13-1:오작동 감지기가 있는 2층~

                        if (!flag&&IngAudio.clip!=null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            /*외부소리 작동*/
                            //사이렌 경종 울리기
                            soundbox1.GetComponent<AudioSource>().Play();
                            soundbox2.GetComponent<AudioSource>().Play();

                            //작동 진행
                            if (GameObject.Find("CardCanvas") != null)
                            {
                                GameObject.Find("CardCanvas").SetActive(false);
                            }
                            flag = true;
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;

                    case 1: //13-2:문을 열고 교실~
                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("AODPoint").GetComponent<BoxCollider>().enabled = true;  //박스콜라이더 활성화
                        }
                        //문 여는 애니메이션 끝!
                        if (GameObject.Find("AODPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            RedLight.SetActive(true);
                            GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AODPoint").SetActive(false);
                            NextNum();
                        }
                        break;
                    case 2: //14-1-1:감지기의 표시등이~
                        //감지기 방향으로 화살표 제시
                        Arrow1.SetActive(true);
                        
                        if(!IngAudio.isPlaying)
                        {
                            if(!stay2)
                            {
                                stay = false;
                                //3초 후!
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                NextNum();
                            }
                        }
                            
                        break;
                    case 3: //14-1-2:화재가 발생했는지~
                        Arrow1.SetActive(false);

                        if (!IngAudio.isPlaying)
                        {
                            if(stay2)
                            {
                                stay = false;
                                //5초 후!
                                Invoke("StayTime", 5f);
                                stay2 = false;
                            }

                            if (stay)
                            {
                                stay = false;
                                NextNum();
                            }
                        }
                        break;
                    case 4: //14-2:실제 화재가~
                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().EndBlink();
                            NextNum();
                        }
                        break;
                    case 5: //감지기의 오작동~
                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 6: //다시 당직실로 이동해서~
                        if (!IngAudio.isPlaying)
                        {
                            if(!stay2)
                            {
                                stay = false;
                                SaveModeIngNum();//진행 번호 저장
                                Invoke("StayTime", 5);
                                stay2 = true;
                            }
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("AOffice");
                        }

                        break;
                    case 7: //32_1:비화재보일 경우~------------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            //시작 위치
                            if (GameObject.Find("MovePoint2") != null)
                            {
                                player.transform.position = GameObject.Find("MovePoint2").transform.position;  //시작 위치로 플레이어 이동
                                player.transform.eulerAngles = GameObject.Find("MovePoint2").transform.eulerAngles;
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                            }

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            flag = false;
                            NextNum();
                        }
                        break;
                    case 8: //33_1:첫번째~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 9: //33_2:또는 정온식~
                        if (!IngAudio.isPlaying)
                        {
                            flag = true;
                            stay = false;
                            fade = false;
                            NextNum();
                            
                        }
                        break;
                    case 10:    //33_3:물음표를 터치해서~
                        if (!IngAudio.isPlaying)
                        {
                            if (GameObject.Find("Card1") != null)
                            {
                                //컬러 변경
                                GameObject.Find("Card1").GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                                //콜라이더 활성화
                                GameObject.Find("Card1").GetComponent<BoxCollider>().enabled = true;
                            }

                            if (GameObject.Find("Card1").GetComponent<ClickCard>().changedone)
                            {
                                if (GameObject.Find("Card2") != null)
                                {
                                    //컬러 변경
                                    GameObject.Find("Card2").GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                                    //콜라이더 활성화
                                    GameObject.Find("Card2").GetComponent<BoxCollider>().enabled = true;
                                }

                                if (GameObject.Find("Card2").GetComponent<ClickCard>().changedone)
                                {
                                    if (GameObject.Find("Card3") != null)
                                    {
                                        //컬러 변경
                                        GameObject.Find("Card3").GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                                        //콜라이더 활성화
                                        GameObject.Find("Card3").GetComponent<BoxCollider>().enabled = true;
                                    }

                                    if (GameObject.Find("Card3").GetComponent<ClickCard>().changedone)
                                    {
                                        NextNum();
                                    }
                                }
                            }
                        }
                        break;
                    case 11:    //32_1:확인이 완료~
                        if (flag)
                        {
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
                    case 12:    //35_1:두번째~----------------------------fade후
                        if(!flag&& IngAudio.clip == s_narration[12])
                        {
                            //플레이어 위치
                            player.transform.position = GameObject.Find("MovePoint3").transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = GameObject.Find("MovePoint3").transform.eulerAngles;

                            //외곽선
                            GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().enabled = true;

                            //화살표
                            Arrow2.SetActive(true);

                            //카드
                            if (GameObject.Find("CardCanvas") != null)
                            {
                                GameObject.Find("CardCanvas").SetActive(false);
                            }

                            //이름표
                            NT1.SetActive(true);
                            NT2.SetActive(true);

                            flag = true;
                        }

                        if (flag&&!IngAudio.isPlaying)
                        {
                            GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().ReBlink();
                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                            NextNum();
                        }
                        break;
                    case 13:    //35_2:감지기가 냉난방기에서~
                        if (!IngAudio.isPlaying)
                        {
                            if(!stay2)
                            {
                                //외곽선 삭제
                                if (GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().EndBlink();
                                if (GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().EndBlink();

                                Arrow2.SetActive(false);


                                flag = true;
                                stay = false;
                                fade = false;
                                stay2 = true;
                            }
                            
                            if (flag)
                            {
                                Invoke("StayTime", 5f);
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
                    case 14:    //36_1:세번째~
                        if(!flag&&IngAudio.clip==s_narration[14])
                        {
                            Debug.Log("<color=cyan>I'm here!</color>");
                            //오염물질
                            Dirty.SetActive(true);

                            //네임택 제거
                            if (NT1 != null)
                                NT1.SetActive(false);
                            if (NT2 != null)
                                NT2.SetActive(false);

                            //외곽선
                            GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().enabled = true;

                            flag = true;
                        }
                        if (flag&&!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                            break;
                    case 15:    //36_2:임시조치로는~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 16:    //36_03:근본조치는~
                        if(stay2)
                        {
                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                        }
                        if (!IngAudio.isPlaying)
                        {
                            if (flag)
                            {
                                Invoke("StayTime", 3f);
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
                    case 17:    //37_1:네번재~
                        if(!stay2)
                        {
                            Dirty.SetActive(false);
                            Dirty2.SetActive(true);
                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = true;
                        }

                        if (!IngAudio.isPlaying)
                        {
                            if (flag)
                            {
                                Invoke("StayTime", 3f);
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
                    case 18:    //38_1:마지막 다섯번째~
                        if(!flag&& IngAudio.clip == s_narration[18])
                        {
                            //플레이어 위치
                            player.transform.position = GameObject.Find("MovePoint4").transform.position; 
                            player.transform.eulerAngles = GameObject.Find("MovePoint4").transform.eulerAngles;

                            GameObject.Find("OM_Fireplug").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("FireDoor").GetComponent<Animator>().SetBool("FD_on", true);
                            flag = true;
                        }
                        
                        if (flag&&!IngAudio.isPlaying)
                        {
                            if(stay2)
                            {
                                if (GameObject.Find("OM_Fireplug").GetComponent<BlinkObject>().enabled)
                                    GameObject.Find("OM_Fireplug").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();//진행 번호 저장
                                stay = false;
                                Invoke("StayTime", 5);
                                stay2 = false;
                            }

                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                        }

                        break;
                }
                
                break;
            case Mode.Pump:

                break;
            case Mode.Campus1:
                switch(ingNum)
                {
                    case 0: //36_1:교실로 이동~

                        if (!flag && IngAudio.clip != null)
                        {
                            TA1.SetActive(true);

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("QM1").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                            NextNum();
                        }
                        break;

                    case 1: //36-2:물음표를 터치하여~
                        if(!IngAudio.isPlaying)
                        {
                            //퀴즈가 완전히 끝나면 다음으로!
                            if(GameObject.Find("QM1").GetComponent<QManager>().DoneQ())
                            {
                                flag = false;
                                NextNum();
                            }
                        }

                        break;
                    case 2: //39-1:(퀴즈 맞춘후) 분말 소화기를 터치하여~
                        QM1E.SetActive(true);
                        GameObject.Find("QM1").GetComponent<MeshRenderer>().enabled = false;

                        if (!IngAudio.isPlaying)
                        {
                            if(QM1E.GetComponent<TouchObject>().ActiveObject())
                            {
                                flag = true;
                                QM1E.GetComponent<TouchObject>().InActiveObject();
                                QM1E.GetComponent<BoxCollider>().enabled = false;
                            }

                            //페이드
                            if (flag)
                            {
                                Invoke("StayTime", 2f);
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
                    case 3: //39-2:분말소화기는~
                        //소화기 자리에 소화기 위치
                        QM1E.SetActive(false);
                        Feg1.SetActive(true);
                        Arrow3.SetActive(true);
                        GameObject.Find("AO_AT_Fextinguisher").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            if(!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");
                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }

                            if(stay)
                            {
                                GameObject.Find("QM1").GetComponent<MeshRenderer>().enabled = true;
                                Arrow3.SetActive(false);
                                GameObject.Find("QM2").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                                Line1.SetActive(true);
                                TPoint5.SetActive(true);
                                GameObject.Find("AO_AT_Fextinguisher").GetComponent<BlinkObject>().EndBlink();
                                player.GetComponent<PlayerContr>().PlayContr(); //플레이어 컨트롤러 해제
                                NextNum();
                            }
                        }

                        break;
                    case 4: //39-3:아래 물음표를 따라 다음~

                        //퀴즈를 클릭한 경우(
                        if(GameObject.Find("QM2").GetComponent<QManager>().TouchQ())
                        {
                            /*외곽선*/
                            //차동식 열감지기
                                GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().enabled = true;
                            //냉난방기
                                GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().enabled = true;
                            //스프링쿨러
                                GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().enabled = true;


                            Line1.SetActive(false);
                            TPoint5.SetActive(false);
                            player.GetComponent<PlayerContr>().StopContr();
                        }
                        //퀴즈가 끝난 경우
                        if (GameObject.Find("QM2").GetComponent<QManager>().DoneQ())
                        {
                            if (stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                /*외곽선*/
                                //차동식 열감지기
                                GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().EndBlink();
                                //냉난방기
                                GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().EndBlink();
                                //스프링쿨러
                                GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().EndBlink();

                                //5초 대기
                                stay = false;
                                flag = true;
                                stay2 = false;
                            }

                            //페이드
                            if (flag)
                            {
                                Invoke("StayTime", 2f);
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
                                player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                                fade = false;
                            }
                        }

                        break;
                    case 5: //(퀴즈후)42-1:스프링쿨러는 4층 인상인~
                        //스프링쿨러 바라보는 각도
                        Arrow4.SetActive(true);
                        player.transform.LookAt(new Vector3(GameObject.Find("4FProp_01_CeilingProp03").transform.position.x,
                            player.transform.position.y,GameObject.Find("4FProp_01_CeilingProp03").transform.position.z));

                        GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().enabled = true;

                        if(!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }  
                            
                            if(stay)
                            {
                                //스프링쿨러
                                GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("QM3").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                                Line2.SetActive(true);
                                TPoint6.SetActive(true);
                                player.GetComponent<PlayerContr>().PlayContr(); //플레이어 컨트롤러 해제
                                NextNum();
                            }
                        }

                        break;
                    case 6: //42-2:아래 가이드 화살표를~
                        Arrow4.SetActive(false);
                        //퀴즈를 클릭한 경우(
                        if (GameObject.Find("QM3").GetComponent<QManager>().TouchQ())
                        {
                            
                            /*외곽선*/
                            //차동식 열감지기
                            GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().enabled = true;
                            //연기 감지기
                            GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().enabled = true;
                            //스프링쿨러
                            GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().enabled = true;

                            Line2.SetActive(false);
                            TPoint6.SetActive(false);
                            player.GetComponent<PlayerContr>().StopContr();
                        }
                        //퀴즈가 끝난 경우
                        if (GameObject.Find("QM3").GetComponent<QManager>().DoneQ())
                        {
                            if (stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                /*외곽선*/
                                //차동식 열감지기
                                GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().EndBlink();
                                //연기 감지기
                                GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().EndBlink();
                                //스프링쿨러
                                GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().EndBlink();

                                //5초 대기
                                stay = false;
                                flag = true;
                                stay2 = false;
                            }

                            //페이드
                            if (flag)
                            {
                                Invoke("StayTime", 2f);
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
                                player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                                fade = false;
                            }
                        }
                        break;
                    case 7: //(퀴즈후)45-1:차동식 포스트형 열감지기는~
                            //열감지기 바라보는 각도
                        player.transform.LookAt(new Vector3(GameObject.Find("4FProp_01_CeilingProp02").transform.position.x,
                            player.transform.position.y, GameObject.Find("4FProp_01_CeilingProp02").transform.position.z));
                        
                        GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().enabled = true;

                        Arrow5.SetActive(true);

                        if (!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                //열감지기
                                GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("QM4").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                                Line3.SetActive(true);
                                TPoint7.SetActive(true);
                                player.GetComponent<PlayerContr>().PlayContr(); //플레이어 컨트롤러 해제
                                NextNum();
                            }
                        }

                        break;
                    case 8: //45-2:아래 가이드 화살표를 따라~
                        Arrow5.SetActive(false);
                        //퀴즈를 클릭한 경우(
                        if (GameObject.Find("QM4").GetComponent<QManager>().TouchQ())
                        {
                            /*외곽선*/
                            //비상방송용스피커
                            GameObject.Find("4FProp_01_CeilingProp01").GetComponent<BlinkObject>().enabled = true;
                            //연기 감지기
                            GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().enabled = true;
                            //스프링쿨러
                            GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().enabled = true;

                            Line3.SetActive(false);
                            TPoint7.SetActive(false);
                            player.GetComponent<PlayerContr>().StopContr();
                        }
                        //퀴즈가 끝난 경우
                        if (GameObject.Find("QM4").GetComponent<QManager>().DoneQ())
                        {
                            if (stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                /*외곽선*/
                                //비상방송용스피커
                                GameObject.Find("4FProp_01_CeilingProp01").GetComponent<BlinkObject>().EndBlink();
                                //연기 감지기
                                GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().EndBlink();
                                //스프링쿨러
                                GameObject.Find("4FProp_01_CeilingProp03").GetComponent<BlinkObject>().EndBlink();

                                //5초 대기
                                stay = false;
                                flag = true;
                                stay2 = false;
                            }

                            //페이드
                            if (flag)
                            {
                                Invoke("StayTime", 2f);
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
                                player.transform.position = GameObject.Find("ViewPoint").transform.position;
                                fade = false;
                            }
                        }
                        break;
                    case 9: //48-1:연기감지기는 교실 당~
                        Arrow6.SetActive(true);

                        player.transform.LookAt(new Vector3(GameObject.Find("4FProp_01_CeilingProp04_Blink").transform.position.x,
                            player.transform.position.y, GameObject.Find("4FProp_01_CeilingProp04_Blink").transform.position.z));

                        GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 3f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                //연기감지기
                                GameObject.Find("4FProp_01_CeilingProp04_Blink").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("QM5").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                                Line4.SetActive(true);
                                TPoint8.SetActive(true);
                                player.GetComponent<PlayerContr>().PlayContr(); //플레이어 컨트롤러 해제
                                GameObject.Find("4FProp_01_CeilingProp01_2").GetComponent<BlinkObject>().ReBlink();
                                GameObject.Find("4FProp_01_CeilingProp01").GetComponent<BlinkObject>().ReBlink();
                                GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().ReBlink();
                                GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().ReBlink();
                                NextNum();
                            }
                        }
                        break;
                    case 10:    //48-2:아래 가이드 화살표를 따라~
                        Arrow6.SetActive(false);
                        //퀴즈를 클릭한 경우(
                        if (GameObject.Find("QM5").GetComponent<QManager>().TouchQ())
                        {
                            /*외곽선*/
                            //비상방송용스피커
                            GameObject.Find("4FProp_01_CeilingProp01_2").GetComponent<BlinkObject>().enabled = true;
                            //비상방송용스피커
                            GameObject.Find("4FProp_01_CeilingProp01").GetComponent<BlinkObject>().enabled = true;
                            //열감지기
                            GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().enabled = true;
                            //냉방기
                            GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().enabled = true;

                            Line4.SetActive(false);
                            TPoint8.SetActive(false);
                            player.GetComponent<PlayerContr>().StopContr();
                        }
                        //퀴즈가 끝난 경우
                        if (GameObject.Find("QM5").GetComponent<QManager>().DoneQ())
                        {
                            if (stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                /*외곽선*/
                                //비상방송용스피커
                                GameObject.Find("4FProp_01_CeilingProp01_2").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("4FProp_01_CeilingProp01_2").GetComponent<BlinkObject>().ReBlink();
                                //비상방송용스피커
                                GameObject.Find("4FProp_01_CeilingProp01").GetComponent<BlinkObject>().EndBlink();
                                //열 감지기
                                GameObject.Find("4FProp_01_CeilingProp02").GetComponent<BlinkObject>().EndBlink();
                                //냉방기
                                GameObject.Find("AO_AT_Airconditioner").GetComponent<BlinkObject>().EndBlink();

                                //5초 대기
                                stay = false;
                                flag = true;
                                stay2 = false;
                            }

                            //페이드
                            if (flag)
                            {
                                Invoke("StayTime", 2f);
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
                    case 11:    //51-1:비상방송용스피커는 교실~
                        player.transform.LookAt(new Vector3(GameObject.Find("4FProp_01_CeilingProp01_2").transform.position.x,
                            player.transform.position.y, GameObject.Find("4FProp_01_CeilingProp01_2").transform.position.z));

                        GameObject.Find("4FProp_01_CeilingProp01_2").GetComponent<BlinkObject>().enabled = true;

                        Arrow9.SetActive(true);
                        if (!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 7f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                Arrow9.SetActive(false);
                                //스피커
                                GameObject.Find("4FProp_01_CeilingProp01_2").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();//진행 번호 저장
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                            }
                        }
                        break;
                }
                break;
            case Mode.Campus2:
                switch (ingNum)
                {
                    case 0: //53_1:과학실로 이동~

                        if (!flag && IngAudio.clip != null)
                        {
                            TA2.SetActive(true);

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("QM6").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                            //퀴즈를 클릭한 경우
                            if (GameObject.Find("QM6").GetComponent<QManager>().TouchQ())
                            {
                                GameObject.Find("QM6").GetComponent<BoxCollider>().enabled = false;
                                /*외곽선*/
                                // 과학실 - 간이소화기
                                if (GameObject.Find("4FProp_02_FEProp06") != null)
                                    GameObject.Find("4FProp_02_FEProp06").GetComponent<BlinkObject>().enabled = true;
                                //과학실-투척용소화기
                                if (GameObject.Find("4FProp_02_FEProp05") != null)
                                    GameObject.Find("4FProp_02_FEProp05").GetComponent<BlinkObject>().enabled = true;
                                if (GameObject.Find("4FProp_02_FEProp05_7") != null)
                                    GameObject.Find("4FProp_02_FEProp05_7").GetComponent<BlinkObject>().enabled = true;
                                if (GameObject.Find("4FProp_02_FEProp05_8") != null)
                                    GameObject.Find("4FProp_02_FEProp05_8").GetComponent<BlinkObject>().enabled = true;
                                if (GameObject.Find("4FProp_02_FEProp05_9") != null)
                                    GameObject.Find("4FProp_02_FEProp05_9").GetComponent<BlinkObject>().enabled = true;

                                player.GetComponent<PlayerContr>().PlayContr();
                                NextNum();
                            }
                        }
                        break;

                    case 1: //(퀴즈후)대사없음
                        
                        //퀴즈가 끝난 경우
                        if (GameObject.Find("QM6").GetComponent<QManager>().DoneQ())
                        {
                            if (!stay2)
                            {
                                player.GetComponent<PlayerContr>().StopContr();
                                Debug.Log("mainsound: " + ingNum + " is done");

                                /*외곽선*/
                                
                                //과학실-투척용소화기
                                if (GameObject.Find("4FProp_02_FEProp05") != null)
                                    GameObject.Find("4FProp_02_FEProp05").GetComponent<BlinkObject>().EndBlink();
                                if (GameObject.Find("4FProp_02_FEProp05_7") != null)
                                    GameObject.Find("4FProp_02_FEProp05_7").GetComponent<BlinkObject>().EndBlink();
                                if (GameObject.Find("4FProp_02_FEProp05_8") != null)
                                    GameObject.Find("4FProp_02_FEProp05_8").GetComponent<BlinkObject>().EndBlink();
                                if (GameObject.Find("4FProp_02_FEProp05_9") != null)
                                    GameObject.Find("4FProp_02_FEProp05_9").GetComponent<BlinkObject>().EndBlink();
                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }
                            if (stay)
                            {
                                // 과학실 - 간이소화기
                                if (GameObject.Find("4FProp_02_FEProp06") != null)
                                    GameObject.Find("4FProp_02_FEProp06").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();//진행 번호 저장
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                            }
                        }
                        
                        break;
                }   
                break;
            case Mode.Campus3:
                switch (ingNum)
                {
                    case 0: //59_1:유치원으로 이동~

                        if (!flag && IngAudio.clip != null)
                        {
                            TA3.SetActive(true);

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("QM7").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                            NextNum();
                        }
                        break;

                    case 1: //59_2:물음표를 터치하여~
                        if (!IngAudio.isPlaying)
                        {
                            //퀴즈가 완전히 끝나면 다음으로!
                            if (GameObject.Find("QM7").GetComponent<QManager>().DoneQ())
                            {
                                flag = false;
                                TPoint1.SetActive(true);
                                NextNum();
                            }
                        }
                        break;
                    case 2: //(퀴즈후)61-4:아래 가이드 화살표를 따라~
                        Line5.SetActive(true);

                        if(!IngAudio.isPlaying)
                        {
                            player.GetComponent<PlayerContr>().PlayContr();
                            
                            if (TPoint1.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line5.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                TPoint1.SetActive(false);
                                NextNum();
                            }
                        }

                        
                        break;
                    case 3: //62-1:투척용소화기는 높이가~
                        //투척용 소화기 외곽선
                        GameObject.Find("4FProp_02_FEProp05_3").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("4FProp_02_FEProp05_4").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("4FProp_02_FEProp05_5").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("4FProp_02_FEProp05_6").GetComponent<BlinkObject>().enabled = true;

                        if(!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                player.GetComponent<PlayerContr>().StopContr();
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }
                            if (stay)
                            {
                                /*외곽선*/
                                //유치원-투척용소화기
                                GameObject.Find("4FProp_02_FEProp05_3").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("4FProp_02_FEProp05_4").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("4FProp_02_FEProp05_5").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("4FProp_02_FEProp05_6").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();//진행 번호 저장
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                            }
                        }
                        break;
                }
                break;
            case Mode.Campus4:
                switch(ingNum)
                {
                    case 0: //65-1:급식실로 이동~

                        if (!flag && IngAudio.clip != null)
                        {
                            TA4.SetActive(true);

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("QM8").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                            NextNum();
                        }
                        break;

                    case 1: //65-2:물음표를 터치하여~
                        if (!IngAudio.isPlaying)
                        {
                            //퀴즈를 누르면
                            if(GameObject.Find("QM8").GetComponent<QManager>().TouchQ())
                            {
                                //급식실-자동확산소화기
                                GameObject.Find("4FProp_02_FEProp08").GetComponent<BlinkObject>().enabled = true;
                                //급식실-열감지기
                                GameObject.Find("4FProp_01_CeilingProp02_2").GetComponent<BlinkObject>().enabled = true;
                            }
                            //퀴즈가 완전히 끝나면 다음으로!
                            if (GameObject.Find("QM8").GetComponent<QManager>().DoneQ())
                            {
                                if (!stay2)
                                {
                                    Debug.Log("mainsound: " + ingNum + " is done");

                                    /*외곽선*/
                                    //급식실-자동확산소화기
                                    GameObject.Find("4FProp_02_FEProp08").GetComponent<BlinkObject>().EndBlink();
                                    //급식실-열감지기
                                    GameObject.Find("4FProp_01_CeilingProp02_2").GetComponent<BlinkObject>().EndBlink();

                                    //5초 대기
                                    stay = false;
                                    flag = true;
                                    stay2 = true;
                                }

                                //페이드
                                if (flag)
                                {
                                    Invoke("StayTime", 2f);
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
                                    GameObject.Find("4FProp_02_FEProp08").GetComponent<BlinkObject>().enabled = true;
                                    player.transform.LookAt(new Vector3(GameObject.Find("4FProp_02_FEProp08").transform.position.x,
                                        player.transform.position.y, GameObject.Find("4FProp_02_FEProp08").transform.position.z));
                                    Arrow7.SetActive(true);
                                    fade = false;
                                }
                            }
                        }
                        break;
                    case 2: //68-1:자동확산소화기는 급식실의~
                        
                        if (!IngAudio.isPlaying&&IngAudio.clip==s_narration[2])
                        {
                            if (stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = false;
                            }

                            if (stay)
                            {
                                //자동화산소화기
                                GameObject.Find("4FProp_02_FEProp08").GetComponent<BlinkObject>().EndBlink();
                                Arrow7.SetActive(false);
                                GameObject.Find("QM8").SetActive(false);
                                Invoke("CountTime", 5f);
                                stay = false;
                            }

                            if (fade)
                            {
                                //자동으로 터치작동
                                GameObject.Find("QM9").GetComponent<QManager>().AutoTouch();
                                player.GetComponent<PlayerContr>().PlayContr();
                                //k급 소화기 외곽선
                                GameObject.Find("4FProp_02_FEProp03_2").GetComponent<BlinkObject>().enabled = true;
                                fade = false;
                            }


                            if (GameObject.Find("QM9").GetComponent<QManager>().DoneQ())
                            {
                                player.GetComponent<PlayerContr>().StopContr();
                                
                                NextNum();
                            }
                        }

                        break;
                    case 3: //71-2:K급 소화기는~
                        if (!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                //k급 소화기 외곽선
                                GameObject.Find("4FProp_02_FEProp03_2").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();//진행 번호 저장
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                                stay = false;
                            }

                        }
                        break;
                }
                
                break;
            case Mode.Campus5:
                switch (ingNum)
                {
                    case 0: //74-1:복도로 이동~

                        if (!flag && IngAudio.clip != null)
                        {
                            TA5.SetActive(true);

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("QM10").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                            NextNum();
                        }
                        break;

                    case 1: //74-2:물음표를~
                        if (!IngAudio.isPlaying)
                        {
                            //퀴즈가 완전히 끝나면 다음으로!
                            if (GameObject.Find("QM10").GetComponent<QManager>().DoneQ())
                            {
                                if (!stay2)
                                {
                                    Debug.Log("mainsound: " + ingNum + " is done");

                                    //5초 대기
                                    stay = false;
                                    flag = true;
                                    stay2 = true;
                                }

                                //페이드
                                if (flag)
                                {
                                    Invoke("StayTime", 2f);
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
                                    player.transform.LookAt(new Vector3(GameObject.Find("4FProp_02_WGG01").transform.position.x,
                                        player.transform.position.y, GameObject.Find("4FProp_02_WGG01").transform.position.z));
                                    /*외곽선*/
                                    //완강기
                                    GameObject.Find("4FProp_02_WGG01").GetComponent<BlinkObject>().enabled = true;

                                    fade = false;
                                }
                            }
                        }
                        break;
                    case 2: //76-1:완강기는 3층에서~
                        if (!IngAudio.isPlaying)
                        {

                            if (stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = false;
                            }

                            if(stay)
                            {
                                /*외곽선*/
                                //완강기
                                GameObject.Find("4FProp_02_WGG01").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("QM11").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                                Line6.SetActive(true);
                                TPoint2.SetActive(true);
                                NextNum();
                            }
                        }
                        break;
                    case 3: //76-2:아래 가이드 화살표를~
                        if (!IngAudio.isPlaying)
                        {
                            //퀴즈 누르기 전!
                            if (!GameObject.Find("QM11").GetComponent<QManager>().TouchQ())
                            {
                                player.GetComponent<PlayerContr>().PlayContr();
                            }
                            else //누르고 나서!
                            {
                                Line6.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr();
                                TPoint2.SetActive(false);
                            }
                            //퀴즈 끝나면
                            if (GameObject.Find("QM11").GetComponent<QManager>().DoneQ())
                            {
                                QM11E.SetActive(true);
                                stay = false;
                                fade = false;
                                NextNum();
                            }
                        }
                        break;
                    case 4: //78-1:연기감지기를 터치하여~
                        if (!IngAudio.isPlaying)
                        {
                            if (QM11E.GetComponent<TouchObject>().ActiveObject())
                            {
                                flag = true;
                                QM11E.GetComponent<TouchObject>().InActiveObject();
                                QM11E.GetComponent<BoxCollider>().enabled = false;
                            }

                            //페이드
                            if (flag)
                            {
                                Invoke("StayTime", 2f);
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
                                QM11E.SetActive(false);
                                GameObject.Find("4FProp_01_CeilingProp04_2").GetComponent<BlinkObject>().enabled = true;
                                fade = false;
                            }
                        }
                        break;
                    case 5: //78-2:연기감지기를 복도에~
                        Arrow8.SetActive(true);
                        if (!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");
                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                GameObject.Find("QM12").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                                GameObject.Find("4FProp_01_CeilingProp04_2").GetComponent<BlinkObject>().EndBlink();
                                player.GetComponent<PlayerContr>().PlayContr(); //플레이어 컨트롤러 해제
                                Line7.SetActive(true);
                                TPoint3.SetActive(true);
                                Arrow8.SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 6: //78-3:아래 가이드를 따라~
                        if (!IngAudio.isPlaying)
                        {
                            //퀴즈 누르기 전!
                            if (!GameObject.Find("QM12").GetComponent<QManager>().TouchQ())
                            {
                                player.GetComponent<PlayerContr>().PlayContr();
                            }
                            else //누르고 나서!
                            {
                                Line7.SetActive(false);
                                TPoint3.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr();
                            }
                            //퀴즈 끝나면
                            if (GameObject.Find("QM12").GetComponent<QManager>().DoneQ())
                            {
                                if (stay2)
                                {
                                    Debug.Log("mainsound: " + ingNum + " is done");

                                    //5초 대기
                                    stay = false;
                                    flag = true;
                                    stay2 = false;
                                }

                                //페이드
                                if (flag)
                                {
                                    Invoke("StayTime", 2f);
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
                                    player.transform.LookAt(new Vector3(GameObject.Find("OM_Fireplug").transform.position.x,
                                        player.transform.position.y, GameObject.Find("OM_Fireplug").transform.position.z));
                                    /*외곽선*/
                                    //소화전
                                    GameObject.Find("OM_Fireplug").GetComponent<BlinkObject>().enabled = true;
                                    player.GetComponent<PlayerContr>().StopContr();
                                    fade = false;
                                }
                            }
                        }
                        break;
                    case 7: //80-1:옥내소화전은~
                        if(!IngAudio.isPlaying&&IngAudio.clip==s_narration[7])
                        {
                            if (!stay2)
                            {
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                GameObject.Find("OM_Fireplug").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();//진행 번호 저장
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                                stay = false;
                            }
                        }
                        break;
                }
                break;
            case Mode.Campus6:
                switch (ingNum)
                {
                    case 0: //83-1:계단으로 이동~
                        if (!flag && IngAudio.clip != null)
                        {
                            TA6.SetActive(true);

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;
                            stay = false;
                            fade = false;
                            stay2 = false;
                        }

                        //그냥 소리만 재생
                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("QM13").GetComponent<QManager>().NextQM();   //빨간 물음표로 바꾸기
                            NextNum();
                        }
                        break;

                    case 1: //83-2:물음표를 터치하여~
                        if (!IngAudio.isPlaying)
                        {
                            //퀴즈 누르기 전!
                            if (!GameObject.Find("QM13").GetComponent<QManager>().TouchQ())
                            {
                            }
                            else //누르고 나서!
                            {
                                player.GetComponent<PlayerContr>().StopContr();
                            }
                            //퀴즈 끝나면
                            if (GameObject.Find("QM13").GetComponent<QManager>().DoneQ())
                            {
                                QM13E.SetActive(true);
                                NextNum();
                            }
                        }
                        break;
                    case 2: //85-1:계단통로유도등을 터치해서~
                        if (!IngAudio.isPlaying)
                        {
                            if (QM13E.GetComponent<TouchObject>().ActiveObject())
                            {
                                flag = true;
                                QM13E.GetComponent<TouchObject>().InActiveObject();
                                QM13E.GetComponent<BoxCollider>().enabled = false;
                                QM13E.SetActive(false);
                                player.GetComponent<PlayerContr>().PlayContr();
                                TPoint4.SetActive(true);

                                NextNum();
                            }
                        }
                        break;
                    case 3: //85-4:아래 가이드 화살표를 따라~
                        Line8.SetActive(true);

                        if (!IngAudio.isPlaying)
                        {
                            if (TPoint4.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line8.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                TPoint4.SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 4: //86-1:계단통로유도등은 바닥에서~
                        //유도등
                        GameObject.Find("AO_AT_Exitlight_02").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                player.GetComponent<PlayerContr>().StopContr();
                                Debug.Log("mainsound: " + ingNum + " is done");

                                //5초 대기
                                stay = false;
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }
                            if (stay)
                            {
                                /*외곽선*/
                                //유도등
                                GameObject.Find("AO_AT_Exitlight_02").GetComponent<BlinkObject>().EndBlink();

                                SaveModeIngNum();//진행 번호 저장
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                            }
                        }
                        break;
                }
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
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
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
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
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
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
        }
        else if (mode.Equals("Pump"))
        {
            modeNum = Mode.Pump;
            s_narration = pumpaudio;
            ModeObject = GameObject.Find("Pump");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("fire").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
        }
        else if (mode.Equals("Campus1"))
        {
            modeNum = Mode.Campus1;
            s_narration = campusaudio1;
            ModeObject = GameObject.Find("Campus1");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
        }
        else if (mode.Equals("Campus2"))
        {
            modeNum = Mode.Campus2;
            s_narration = campusaudio2;
            ModeObject = GameObject.Find("Campus2");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
        }
        else if (mode.Equals("Campus3"))
        {
            modeNum = Mode.Campus3;
            s_narration = campusaudio3;
            ModeObject = GameObject.Find("Campus3");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
        }
        else if (mode.Equals("Campus4"))
        {
            modeNum = Mode.Campus4;
            s_narration = campusaudio4;
            ModeObject = GameObject.Find("Campus4");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
        }
        else if (mode.Equals("Campus5"))
        {
            modeNum = Mode.Campus5;
            s_narration = campusaudio5;
            ModeObject = GameObject.Find("Campus5");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
            GameObject.Find("Campus6").SetActive(false);
        }
        else if (mode.Equals("Campus6"))
        {
            modeNum = Mode.Campus6;
            s_narration = campusaudio6;
            ModeObject = GameObject.Find("Campus6");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus2").SetActive(false);
            GameObject.Find("Campus3").SetActive(false);
            GameObject.Find("Campus4").SetActive(false);
            GameObject.Find("Campus5").SetActive(false);
            GameObject.Find("Campus1").SetActive(false);
        }

        Debug.Log("Here my mode: " + PlayerPrefs.GetString("mode") + "(" + mode + ")");
    }

    //행동번호 다음번호로
    public void NextNum()
    {
        ingNum++;
    }

    public void ChangeNum(int _num)
    {
        ingNum = _num;
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
        Debug.Log("stay");
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

    //메인 오디오체크용
    public bool MainAudioPlayDone()
    {
        if (!IngAudio.isPlaying)
        {
            return true;
        }
        else if (IngAudio.isPlaying || IngAudio.clip != null)
        {
            return false;
        }
        else
            return false; ;
    }
}

