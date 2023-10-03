using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public enum Mode
{
    Spring1,
    Spring2,
    Fire,
    Pump,
    Campus1,
    Campus2,
    Campus3,
    Campus4,
    Campus5,
    Campus6
}
public class SPSpaceManager : MonoBehaviour
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
    [SerializeField] GameObject greenlight;             //초록불
    [SerializeField] GameObject redlight;             //초록불

    [SerializeField] GameObject player;                 //플레이어

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


    protected GameObject Line1;
    protected GameObject Line2;
    protected GameObject Line3;
    protected GameObject Line4;
    protected GameObject Line5;
    protected GameObject Line6;
    protected GameObject Line7;
    protected GameObject Line8;
    protected GameObject Line9;

    protected GameObject NT1;
    protected GameObject NT2;
    protected GameObject NT3;
    protected GameObject NT4;
    protected GameObject NT5;
    protected GameObject NT6;
    protected GameObject NT7;
    protected GameObject NT8;
    protected GameObject NT9;
    protected GameObject NT10;

    protected GameObject Gage1;
    protected GameObject Gage2;
    protected GameObject Gage3;
    protected GameObject Gage4;
    protected GameObject Gage5;
    protected GameObject Gage6;

    protected GameObject MovePoint1;
    protected GameObject MovePoint2;
    protected GameObject MovePoint3;
    protected GameObject MovePoint4;
    protected GameObject MovePoint5;
    protected GameObject MovePoint6;
    protected GameObject MovePoint7;
    protected GameObject MovePoint8;
    protected GameObject MovePoint9;


    protected GameObject WaterDrop;


    protected bool flag;
    protected bool outlineflag;                     //외각선 깜박임용 플래그
    protected bool stay;                            //화면 전환 대기용
    protected bool stay2;
    protected bool stay3;
    protected bool fade;

    protected Vector3 barposition;

    void Awake()
    {
        //이동 포인트
        if (GameObject.Find("MovePoint1") != null)
        {
            MovePoint1 = GameObject.Find("MovePoint1");
            MovePoint1.SetActive(false);
        }
        if (GameObject.Find("MovePoint2") != null)
        {
            MovePoint2 = GameObject.Find("MovePoint2");
            MovePoint2.SetActive(false);
        }
        if (GameObject.Find("MovePoint3") != null)
        {
            MovePoint3 = GameObject.Find("MovePoint3");
            MovePoint3.SetActive(false);
        }
        if (GameObject.Find("MovePoint4") != null)
        {
            MovePoint4 = GameObject.Find("MovePoint4");
            MovePoint4.SetActive(false);
        }
        if (GameObject.Find("MovePoint5") != null)
        {
            MovePoint5 = GameObject.Find("MovePoint5");
            MovePoint5.SetActive(false);
        }
        if (GameObject.Find("MovePoint6") != null)
        {
            MovePoint6 = GameObject.Find("MovePoint6");
            MovePoint6.SetActive(false);
        }
        if (GameObject.Find("MovePoint7") != null)
        {
            MovePoint7 = GameObject.Find("MovePoint7");
            MovePoint7.SetActive(false);
        }
        if (GameObject.Find("MovePoint8") != null)
        {
            MovePoint8 = GameObject.Find("MovePoint8");
            MovePoint8.SetActive(false);
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
        //ingNum = 0;         //행동번호 0으로 초기화
        startpoint = GameObject.Find("StartPoint");
        player = GameObject.Find("Player");
        IngAudio = gameObject.GetComponent<AudioSource>();  //오디오 소스 받아오기
        flag = false;
        outlineflag = false;
        stay = false;
        stay2 = false;
        stay3 = false;
        fade = false;
        barposition = GameObject.Find("Point_off valve_2_bar").transform.position;
    }

    void SettingObject()
    {
        
        //이름표 등록
        if (GameObject.Find("NameTag") != null)
        {
            NT1 = GameObject.Find("NameTag");
            NT1.SetActive(false);
        }
        if (GameObject.Find("NameTag2") != null)
        {
            NT2 = GameObject.Find("NameTag2");
            NT2.SetActive(false);
        }
        if (GameObject.Find("NameTag3") != null)
        {
            NT3 = GameObject.Find("NameTag3");
            NT3.SetActive(false);
        }
        if (GameObject.Find("NameTag4") != null)
        {
            NT4 = GameObject.Find("NameTag4");
            NT4.SetActive(false);
        }
        if (GameObject.Find("NameTag5") != null)
        {
            NT5 = GameObject.Find("NameTag5");
            NT5.SetActive(false);
        }
        if (GameObject.Find("NameTag6") != null)
        {
            NT6 = GameObject.Find("NameTag6");
            NT6.SetActive(false);
        }
        if (GameObject.Find("NameTag7") != null)
        {
            NT7 = GameObject.Find("NameTag7");
            NT7.SetActive(false);
        }
        if (GameObject.Find("NameTag8") != null)
        {
            NT8 = GameObject.Find("NameTag8");
            NT8.SetActive(false);
        }
        if (GameObject.Find("NameTag9") != null)
        {
            NT9 = GameObject.Find("NameTag9");
            NT9.SetActive(false);
        }
        if (GameObject.Find("NameTag10") != null)
        {
            NT10 = GameObject.Find("NameTag10");
            NT10.SetActive(false);
        }


        //화살표 등록
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

        //게이지창 등록
        if (GameObject.Find("Gage1") != null)
        {
            Gage1 = GameObject.Find("Gage1");
            Gage1.SetActive(false);
        }
        if (GameObject.Find("Gage2") != null)
        {
            Gage2 = GameObject.Find("Gage2");
            Gage2.SetActive(false);
        }
        if (GameObject.Find("Gage3") != null)
        {
            Gage3 = GameObject.Find("Gage3");
            Gage3.SetActive(false);
        }
        if (GameObject.Find("Gage4") != null)
        {
            Gage4 = GameObject.Find("Gage4");
            Gage4.SetActive(false);
        }
        if (GameObject.Find("Gage5") != null)
        {
            Gage5 = GameObject.Find("Gage5");
            Gage5.SetActive(false);
        }
        if (GameObject.Find("Gage6") != null)
        {
            Gage6 = GameObject.Find("Gage6");
            Gage6.SetActive(false);
        }

        if (GameObject.Find("greenlight") != null)
        {
            greenlight = GameObject.Find("greenlight");
            greenlight.SetActive(false);
        }
        if (GameObject.Find("redlight") != null)
        {
            redlight = GameObject.Find("redlight");
            redlight.SetActive(false);
        }

        if (GameObject.Find("WaterDrop") != null)
        {
            WaterDrop = GameObject.Find("WaterDrop");
            WaterDrop.SetActive(false);
        }

        /*외곽선 전용*/
        if (GameObject.Find("Object001").GetComponent<BlinkObject>())
            GameObject.Find("Object001").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("outline_002").GetComponent<BlinkObject>())
            GameObject.Find("outline_002").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("lamp_green").GetComponent<BlinkObject>())
            GameObject.Find("lamp_green").GetComponent<BlinkObject>().enabled = false;
        if (GameObject.Find("lamp_red").GetComponent<BlinkObject>())
            GameObject.Find("lamp_red").GetComponent<BlinkObject>().enabled = false;

        //흡입밸브
        if (GameObject.Find("Object008").GetComponent<BlinkObject>())
            GameObject.Find("Object008").GetComponent<BlinkObject>().enabled = false;
        //스트레이너
        if (GameObject.Find("SPS_MO01_2_16").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_16").GetComponent<BlinkObject>().enabled = false;
        //화살표
        if (GameObject.Find("SPS_MO01_2_17").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_17").GetComponent<BlinkObject>().enabled = false;
        //플랙시블조인트
        if (GameObject.Find("SPS_MO01_2_18").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_18").GetComponent<BlinkObject>().enabled = false;
        //토출측 배관
        if (GameObject.Find("SPS_MO01_2_1").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_1").GetComponent<BlinkObject>().enabled = false;
        //주펌프 토출측 개폐밸브
        if (GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().enabled = false;
        //릴리프 밸브
        if (GameObject.Find("SPS_MO01_2_5").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_5").GetComponent<BlinkObject>().enabled = false;
        //배수 배관
        if (GameObject.Find("SPS_MO01_2_15").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_15").GetComponent<BlinkObject>().enabled = false;
        //성능 시험 배관
        if (GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().enabled = false;
        //유량계
        if (GameObject.Find("SPS_MO01_2_19").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_19").GetComponent<BlinkObject>().enabled = false;
        //압력기
        if (GameObject.Find("SPS_MO01_2_6").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_6").GetComponent<BlinkObject>().enabled = false;
        //성능 시험 배관 2차측
        if (GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>())
            GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>().enabled = false;

        /*나레이션중 행동 제한*/
        if (GameObject.Find("SDoorPoint") != null)
            GameObject.Find("SDoorPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("SwitchPoint") != null)
            GameObject.Find("SwitchPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("SwitchPoint2") != null)
            GameObject.Find("SwitchPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        /*나레이션중 행동 제한*/
        if (GameObject.Find("ARVPoint") != null)
            GameObject.Find("ARVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARVPoint2") != null)
            GameObject.Find("ARVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARVPoint3") != null)
            GameObject.Find("ARVPoint3").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARVPoint4") != null)
            GameObject.Find("ARVPoint4").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARVPoint5") != null)
            GameObject.Find("ARVPoint5").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARVPoint6") != null)
            GameObject.Find("ARVPoint6").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("ARVPoint7") != null)
            GameObject.Find("ARVPoint7").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("MVPoint") != null)
            GameObject.Find("MVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("MVPoint2") != null)
            GameObject.Find("MVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("MVPoint3") != null)
            GameObject.Find("MVPoint3").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("MVPoint4") != null)
            GameObject.Find("MVPoint4").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("MVPoint5") != null)
            GameObject.Find("MVPoint5").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("MVPoint6") != null)
            GameObject.Find("MVPoint6").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화

        if (GameObject.Find("PWPoint") != null)
            GameObject.Find("PWPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("PWPoint2") != null)
            GameObject.Find("PWPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("PWPoint3") != null)
            GameObject.Find("PWPoint3").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
        if (GameObject.Find("PWPoint4") != null)
            GameObject.Find("PWPoint4").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화 비활성화
    }


    void Update()
    {
        /*모드별로 재생*/
        switch (modeNum)
        {
            case Mode.Spring1:
                switch (ingNum)
                {
                    case 0:
                        if (!flag)
                        {
                            
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            //문 콜라이더 활성화
                            GameObject.Find("SDoorPoint").GetComponent<BoxCollider>().enabled = true;
                        }

                        //문이 열리면
                        if (GameObject.Find("SDoorPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            GameObject.Find("SDoorPoint").SetActive(false);
                            NextNum();
                        }

                        break;
                    case 1:     //동력제어반 앞

                        /*외각선 깜박이기*/
                        if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                            GameObject.Find("Object001").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;

                    case 2: //펌프 운전 선택 스위치
                        if (!IngAudio.isPlaying)
                        {
                            //스위치를 누를수 있음
                            GameObject.Find("SwitchPoint").GetComponent<BoxCollider>().enabled = true;
                            //스위치를 누르면
                            if (GameObject.Find("SwitchPoint").GetComponent<MoveAnimationObject>().movedone)
                            {
                                if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();   //모드 번호 기억하기
                            }
                        }
                        break;

                    case 3: //마지막으로~---------------------------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            GameObject.Find("lamp_green").GetComponent<BlinkObject>().enabled = false;
                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }
                        //오디오가 다 끝나면!
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 4: //문을 열고~
                        if (flag && !IngAudio.isPlaying)
                        {
                            //문 콜라이더 활성화
                            GameObject.Find("SDoorPoint").GetComponent<BoxCollider>().enabled = true;
                        }

                        //문이 열리면
                        if (GameObject.Find("SDoorPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            //레버 돌려놓기
                            GameObject.Find("PWController").GetComponent<Animator>().SetBool("PWC_change", true);
                            GameObject.Find("SDoorPoint").SetActive(false);
                            NextNum();
                        }
                        break;
                    case 5: //동력제어반 앞으로~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 6: //먼저 동력제어반~
                        if (GameObject.Find("outline_002").GetComponent<BlinkObject>())
                            GameObject.Find("outline_002").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("outline_002").GetComponent<BlinkObject>().EndBlink();
                            NextNum();
                        }
                        break;
                    case 7: //펌프 운전 선택 스위치가~
                        /*외각선 깜박이기*/
                        if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                            GameObject.Find("Object001").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("SwitchPoint2").GetComponent<BoxCollider>().enabled = true;
                            if (GameObject.Find("SwitchPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().EndBlink();
                                //초록불 점등
                                greenlight.SetActive(true);
                                //GameObject.Find("SwitchPoint2").SetActive(false);//소리가 씹혀서 생략

                                NextNum();
                            }
                        }

                        break;
                    case 8: //펌프정지표시등의~
                        /*외각선 깜박이기*/
                        GameObject.Find("lamp_green").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("lamp_green").GetComponent<BlinkObject>().EndBlink();
                            if (GameObject.Find("Highlighter"))
                                Destroy(GameObject.Find("Highlighter"));
                            NextNum();
                        }
                        break;
                    case 9: //펌프가 정지 된 것을~

                        if (!IngAudio.isPlaying)
                        {
                            SaveModeIngNum();//진행 번호 저장
                            Invoke("StayTime", 5);
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                        }

                        break;
                }

                break;


            case Mode.Spring2:
                switch (ingNum)
                {
                    case 0: //준비작동식 스프링클러~
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 1: //먼저 동력제어반~
                        //문을 열고
                        if (!IngAudio.isPlaying)
                        {
                            //문 콜라이더 활성화
                            GameObject.Find("SDoorPoint").GetComponent<BoxCollider>().enabled = true;
                        }

                        //문이 열리면
                        if (GameObject.Find("SDoorPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            GameObject.Find("SDoorPoint").SetActive(false);
                            NextNum();
                        }

                        break;
                    case 2:     //동력제어반 앞

                        /*외각선 깜박이기*/
                        if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                            GameObject.Find("Object001").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;

                    case 3: //펌프 운전 선택 스위치
                        /*외각선 깜박이기*/
                        if (flag)
                            if (!IngAudio.isPlaying)
                            {
                                //스위치를 누를수 있음
                                GameObject.Find("SwitchPoint").GetComponent<BoxCollider>().enabled = true;
                                //스위치를 누르면
                                if (GameObject.Find("SwitchPoint").GetComponent<MoveAnimationObject>().movedone)
                                {
                                    if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                                        GameObject.Find("Object001").GetComponent<BlinkObject>().EndBlink();
                                    SaveModeIngNum();   //모드 번호 기억하기
                                }
                            }

                        break;
                    case 4: //마지막으로~------------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성


                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("PWController").GetComponent<Animator>().SetBool("PWC_change", true);
                            NextNum();
                        }

                        break;
                    case 5: //문을 열고 안으로~
                        if (!IngAudio.isPlaying)
                        {
                            //문 콜라이더 활성화
                            GameObject.Find("SDoorPoint").GetComponent<BoxCollider>().enabled = true;
                        }

                        //문이 열리면
                        if (GameObject.Find("SDoorPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            GameObject.Find("SDoorPoint").SetActive(false);
                            NextNum();
                        }
                        break;
                    case 6: //동력제어반으로~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 7: //먼저~
                        /*외각선 깜박이기*/
                        //해당 오브젝트가 아직 분리되지 않았음
                        if (GameObject.Find("outline_002").GetComponent<BlinkObject>())
                            GameObject.Find("outline_002").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("outline_002").GetComponent<BlinkObject>().EndBlink();
                            NextNum();
                        }

                        break;
                    case 8: //펌프 운전 선택~
                        /*외각선 깜박이기*/
                        if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                            GameObject.Find("Object001").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("SwitchPoint2").GetComponent<BoxCollider>().enabled = true;
                            if (GameObject.Find("SwitchPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                if (GameObject.Find("Object001").GetComponent<BlinkObject>())
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().EndBlink();
                                //초록불 점등
                                greenlight.SetActive(true);
                                //GameObject.Find("greenlight").GetComponent<Image>().enabled = true;
                                //GameObject.Find("SwitchPoint2").SetActive(false); 소리 틀기용

                                NextNum();
                            }
                        }

                        break;


                    case 9:    //펌프정지 표시등의~
                        /*외각선 깜박이기*/
                        GameObject.Find("lamp_green").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("lamp_green").GetComponent<BlinkObject>().EndBlink();
                            if (GameObject.Find("Highlighter"))
                                Destroy(GameObject.Find("Highlighter"));
                            NextNum();
                        }
                        break;

                    case 10:    //펌프가 정지~
                        if (!IngAudio.isPlaying)
                        {
                            SaveModeIngNum();//진행 번호 저장
                            Invoke("StayTime", 5);
                            if(stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("Main");
                        }
                        break;
                }
                break;
            case Mode.Fire:

                break;
            case Mode.Pump:
                switch(ingNum)
                {
                    case 0: //23_01:소화펌프 흡입측~
                        //한번만 작동해야한다!
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;          //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;    //시작 방향
                            player.GetComponent<PlayerContr>().StopContr();                     //컨트롤러 비활성

                            GameObject.Find("MVPoint6").GetComponent<GageAnimationObject>().enabled = false;

                            //변수 초기화
                            fade = false;
                            stay = false;
                            stay2 = false;
                            flag = true;
                        }
                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 1: //23_2:펌프 흡입축 배관~
                        //소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;

                    case 2: //23_3:아래의 가이드를 따라~
                        if(!stay2)
                        {
                            //라인 활성화
                            Line1.SetActive(true);
                            //도착지점 활성화
                            MovePoint1.SetActive(true);

                            stay2 = true ;
                        }
                       
                        if (!IngAudio.isPlaying)
                        {
                            if(flag)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                flag = false;
                            }
                            
                            if(MovePoint1.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                Line1.SetActive(false);                         //라인 없애기
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint1.SetActive(false);                    //비활성화
                                
                                NextNum();
                            }
                        }

                        break;
                    case 3: //26_1:흡입밸브의 스크류가~-----------(B지점 도착)
                        if(!flag)
                        {
                            //외곽선 표시
                            GameObject.Find("Object008").GetComponent<BlinkObject>().enabled = true;

                            //이름표 표시
                            NT1.SetActive(true);

                            flag = true;
                        }
                       

                        if (!IngAudio.isPlaying)
                        {
                            //외곽선 삭제
                            GameObject.Find("Object008").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("Object008").GetComponent<BlinkObject>().ReBlink();

                            //이름표 비활성화
                            NT1.SetActive(false);

                            NextNum();
                        }
                        break;

                    case 4: //27_01:다음은 스트레이너 정상설치~
                        if(flag)
                        {
                            //외곽선 표시
                            GameObject.Find("SPS_MO01_2_16").GetComponent<BlinkObject>().enabled = true;

                            //이름표 표시
                            NT2.SetActive(true);

                            flag = false;
                        }
                       

                        if (!IngAudio.isPlaying)
                        {
                            //외곽선 삭제
                            GameObject.Find("SPS_MO01_2_16").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("SPS_MO01_2_16").GetComponent<BlinkObject>().ReBlink();

                            //이름표 비활성화
                            NT2.SetActive(false);

                            NextNum();
                        }
                        break;

                    case 5: //27_2:화살표의 방향표시가~
                        if(!flag)
                        {
                            //외곽선 표시
                            GameObject.Find("SPS_MO01_2_17").GetComponent<BlinkObject>().enabled = true;
                            flag = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            //외곽선 삭제
                            GameObject.Find("SPS_MO01_2_17").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("SPS_MO01_2_17").GetComponent<BlinkObject>().ReBlink();

                            NextNum();
                        }
                        break;
                    case 6: //28_1:플랙시블조인트가~
                        if(flag)
                        {
                            //외곽선 표시
                            GameObject.Find("SPS_MO01_2_18").GetComponent<BlinkObject>().enabled = true;

                            //이름표 표시
                            NT3.SetActive(true);
                            flag = false;
                        }
                       
                        if (!IngAudio.isPlaying)
                        {
                            //외곽선 삭제
                            GameObject.Find("SPS_MO01_2_18").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("SPS_MO01_2_18").GetComponent<BlinkObject>().ReBlink();

                            //이름표 비활성화
                            NT3.SetActive(false);

                            NextNum();
                        }
                        break;
                    case 7: //28_2:소화펌프 흡입축~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 8: //28_3:다음은 소화펌프~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 9: //28_4:아래의 가이드를 따라~
                        if(stay2)
                        {
                            Line2.SetActive(true);
                            //도착지점 활성화
                            MovePoint2.SetActive(true);

                            stay2 = false;
                        }

                        if (!IngAudio.isPlaying)
                        {
                            if(!flag)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                flag = true;
                            }
                            if (MovePoint2.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                Line2.SetActive(false);//라인 없애기
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint2.SetActive(false);//도착지점 비활성화
                                NextNum();
                            }
                        }
                        break;
                    case 10:    //31_01:펌프 토출측 배관과~
                        if(flag)
                        {
                            //외곽선 표시
                            GameObject.Find("SPS_MO01_2_1").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().enabled = true;

                            //이름표 표시
                            NT4.SetActive(true);
                            NT5.SetActive(true);

                            flag = false;
                        }
                        

                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 11:    //31_2:주펌프 토출측 개폐표시형~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 12:    //31_3:소화펌프 토출측 배관류~
                        if (!IngAudio.isPlaying)
                        {
                            //외곽선 삭제
                            GameObject.Find("SPS_MO01_2_1").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("SPS_MO01_2_1").GetComponent<BlinkObject>().ReBlink();
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().ReBlink();

                            //이름표 비활성화
                            NT4.SetActive(false);
                            NT5.SetActive(false);

                            NextNum();
                        }
                        break;
                    case 13:    //33_1:다음은 소화펌프 성능시험을~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 14:    //33_2:주펌프 토출측~
                        if(!flag)
                        {
                            Gage1.SetActive(true);                                                      //게이지
                            //밸브 활성화
                            GameObject.Find("MVPoint").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().enabled = true; //외곽선
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().notblink = true;//깜박x

                            flag = true;
                        }
                       
                        //애니메이션 끝!
                        if (GameObject.Find("MVPoint").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            if(!stay2)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().ReBlink();

                                //게이지 비활성화
                                Gage1.SetActive(false);
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                
                                stay = false;
                                stay2 = true;
                            }
                            
                            if(stay)
                            {
                                GameObject.Find("MVPoint").GetComponent<GageAnimationObject>().enabled = false;
                                GameObject.Find("MVPoint").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                NextNum();
                            }
                        }
                        break;
                    case 15:    //34_1:릴리프밸브가~
                        if(flag)
                        {
                            //외곽선 표시
                            GameObject.Find("SPS_MO01_2_5").GetComponent<BlinkObject>().enabled = true;

                            //이름표 표시
                            NT6.SetActive(true);

                            flag = false;
                        }
                       

                        if (!IngAudio.isPlaying)
                        {
                            if(stay2)
                            {
                                stay = false;
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                stay2 = false;
                            }
                            
                            if (stay)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_5").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_5").GetComponent<BlinkObject>().ReBlink();

                                //이름표 비활성화
                                NT6.SetActive(false);

                                NextNum();
                            }
                        }
                        break;
                    case 16:    //34_2:다음은 릴리프밸브가~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 17:    //34_3:배수배관 설치 여부를 확인하기 위해~

                        if(!stay2)
                        {
                            Line3.SetActive(true);     //라인 활성화
                            MovePoint3.SetActive(true);//도착지점 활성화
                            stay2 = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            if(!flag)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                flag = true;
                            }
                            if (MovePoint3.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                player.GetComponent<PlayerContr>().StopContr();         //컨트롤러 비활성
                                MovePoint3.SetActive(false);                            //도착지점 비활성화

                                NextNum();
                            }
                        }
                        break;
                    case 18:    //37_1:배수배관이 정상적으로~
                        if(flag)
                        {
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러  비활성
                            Line3.SetActive(false);//라인 없애기
                            GameObject.Find("SPS_MO01_2_15").GetComponent<BlinkObject>().enabled = true;//외곽선 표시
                            NT7.SetActive(true);//이름표 표시
                            flag = false;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            if (stay2)
                            {
                                stay = false;
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                stay2 = false;
                            }


                            if (stay)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_15").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_15").GetComponent<BlinkObject>().ReBlink();

                                //이름표 비활성화
                                NT7.SetActive(false);

                                NextNum();
                            }
                        }
                        
                        break;
                    case 19:    //37_2:다음은 성능시험배관을~
                        if(!flag)
                        {
                            Line4.SetActive(true);
                            MovePoint4.SetActive(true);
                            flag = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                            if (MovePoint4.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint4.SetActive(false);//도착지점 비활성화
                                
                                NextNum();
                            }
                        }
                        break;
                    case 20:    //39_1:성능시험 배관 1차 밸브~
                        if(!stay3)
                        {
                            Line4.SetActive(false);//라인 없애기
                            GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().enabled = true;//외곽선 표시
                            NT8.SetActive(true);//이름표 표시
                            stay3 = true;
                        }
                       
                        if (!IngAudio.isPlaying)
                        {
                            if (!stay2)
                            {
                                stay = false;
                                flag = true;
                                fade = false;

                                stay2 = true;
                            }

                            if (flag)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().ReBlink();

                                //이름표 비활성화
                                NT8.SetActive(false);

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
                    case 21:    //41_1:다음은 소화펌프 정격유랑 성능시험을~---------------------------fade후
                        //한번만 작동해야한다!
                        if (!flag && IngAudio.clip==s_narration[21])
                        {
                            SettingObject();

                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = startpoint.transform.eulerAngles;
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            //진행
                            GameObject.Find("MVPoint").GetComponent<GageAnimationObject>().ReGage();
                            GameObject.Find("Point_off valve_2_bar").transform.position = barposition;
                            //gage이미지
                            Gage1.transform.GetChild(1).GetComponent<Image>().fillAmount = 0;
                            //gage숫자
                            Gage1.transform.GetChild(6).GetComponent<Text>().text = "0";
                            GameObject.Find("MVPoint").GetComponent<GageAnimationObject>().enabled = true;
                            GameObject.Find("MVPoint6").GetComponent<GageAnimationObject>().enabled = false;
                            //변수 초기화
                            fade = false;
                            stay = false;
                            stay2 = false;
                            stay3 = false;

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }


                        if (flag&&!IngAudio.isPlaying)
                        {
                            NextNum();
                        }

                        break;
                    case 22:    //41_2:주펌프 토출측 개폐밸브를~
                        if(flag)
                        {
                            //밸브 활성화
                            GameObject.Find("MVPoint").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            Gage1.SetActive(true);                                                      //게이지
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().enabled = true; //외곽선
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().notblink = true;//깜박x
                            flag = false;
                        }
                      
                        //애니메이션 끝!
                        if (GameObject.Find("MVPoint").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            if(!stay2)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().ReBlink();

                                //게이지 비활성화
                                Gage1.SetActive(false);
                                
                                stay = false;
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }
                            
                            if (stay)
                            {
                                GameObject.Find("MVPoint").GetComponent<GageAnimationObject>().enabled = false;
                                GameObject.Find("MVPoint").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                NextNum();
                            }
                        }
                        break;
                    case 23:    //41_3:다음은 성능시험배관 1차측~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 24:    //41_4:아래 가이드를 따라~
                        if(!flag)
                        {
                            Line5.SetActive(true);
                            MovePoint4.GetComponent<TeleportPoint>().ReTel(false);
                            MovePoint4.SetActive(true);
                            flag = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                            if (MovePoint4.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line5.SetActive(false);
                                MovePoint4.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                NextNum();
                            }
                        }
                        break;
                    case 25:    //45_1:성능시험배관 1차측~
                        if(flag)
                        {
                            //밸브 활성화
                            GameObject.Find("MVPoint2").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            Gage2.SetActive(true);                                                      //게이지
                            GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().enabled = true; //외곽선
                            GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().notblink = true;//깜빡x

                            flag = false;
                        }
                        
                        //애니메이션 끝!
                        if (GameObject.Find("MVPoint2").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            if (stay2)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().ReBlink();
                                //게이지 비활성화
                                Gage2.SetActive(false);

                                stay = false;
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                stay2 = false;
                            }

                            if (stay)
                            {
                                GameObject.Find("MVPoint2").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                NextNum();
                            }
                        }
                        break;
                    case 26:    //45_2:개방이 완료되면~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 27:    //45_3:아래 가이드를 따라~
                        if(!flag)
                        {
                            Line6.SetActive(true);
                            MovePoint5.SetActive(true);
                            flag = true;
                        }
                        if (!IngAudio.isPlaying)
                        {
                            if(!stay2)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                stay2 = true;
                            }
                            
                            if (MovePoint5.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                Line6.SetActive(false);
                                MovePoint5.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                NextNum();
                            }
                        }
                        break;
                    case 28:    //47_1:제어반에서 주펌프 선택스위치를~
                        if(flag)
                        {
                            GameObject.Find("PWPoint").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            GameObject.Find("Object001").GetComponent<BlinkObject>().enabled = true; //외곽선

                            flag = false;
                        }

                        //애니메이션 끝!
                        if (GameObject.Find("PWPoint").GetComponent<AnimationObject>().movedone)
                        {
                            if (!GameObject.Find("PWPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                if(!stay3)
                                {
                                    //외곽선 삭제
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().EndBlink();
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().ReBlink();
                                    GameObject.Find("PWPoint").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                    GameObject.Find("PWController").GetComponent<Animator>().SetBool("PWC2_AtoH", false); //애니메이션 돌아가기

                                    //외곽선 활성
                                    greenlight.SetActive(true);
                                    GameObject.Find("lamp_red").GetComponent<BlinkObject>().enabled = true;
                                    GameObject.Find("PWPoint2").GetComponent<BoxCollider>().enabled = true;      //콜라이더

                                    stay3 = true;
                                }
                            }
                            else if(GameObject.Find("PWPoint2").GetComponent<AnimationObject>().movedone)
                            {
                                if (stay2)
                                {
                                    greenlight.SetActive(false);
                                    redlight.SetActive(true);

                                    //외곽선 삭제
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().EndBlink();
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().ReBlink();
                                    GameObject.Find("lamp_red").GetComponent<BlinkObject>().EndBlink();
                                    GameObject.Find("lamp_red").GetComponent<BlinkObject>().ReBlink();
                                    GameObject.Find("PWPoint2").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                    GameObject.Find("PWController").GetComponent<Animator>().SetBool("PWC2_on", false); //애니메이션 돌아가기

                                    stay = false;
                                    //5초 후 활동
                                    Invoke("StayTime", 5f);
                                    stay2 = false;
                                }

                                if (stay)
                                {
                                    NextNum();
                                }
                            }
                        }

                        break;
                    case 29:    //47_2:이제 유량계를~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 30:    //47_3:아래 가이드를 따라~
                        if (!flag)
                        {
                            Line7.SetActive(true);
                            MovePoint1.SetActive(true);
                            MovePoint1.GetComponent<TeleportPoint>().ReTel(false);
                            flag = true;
                        }
                        

                        if (!IngAudio.isPlaying)
                        {
                            if(!stay2)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                stay2 = true;
                            }
                            
                            if (MovePoint1.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line7.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint1.SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 31:    //49_1:유량계를 확인~
                        if(flag)
                        {
                            GameObject.Find("SPS_MO01_2_19").GetComponent<BlinkObject>().enabled = true; //외곽선
                            GameObject.Find("MainPump").GetComponent<Animator>().SetBool("MPW_up", true); //애니메이션 재생

                            NT9.SetActive(true);
                            flag = false;
                        }
                       

                        if (stay2)
                        {
                            stay = false;

                            //5초 후 활동
                            Invoke("StayTime", 5f);

                            stay2 = false;
                        }

                        if (stay)
                        {
                            NT9.SetActive(false);
                            GameObject.Find("MainPump").GetComponent<Animator>().SetBool("MPW_up", false); //애니메이션 돌아가기

                            //외곽선 삭제
                            GameObject.Find("SPS_MO01_2_19").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("SPS_MO01_2_19").GetComponent<BlinkObject>().ReBlink();

                            NextNum();
                        }
                        break;
                    case 32:    //49_2:유량계는 학교 시설마다~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 33:    //49_3:이제 압력게이지를~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 34:    //49_4:아래 가이드를 따라~
                        if(!flag)
                        {
                            Line2.SetActive(true);
                            MovePoint2.SetActive(true);
                            MovePoint2.GetComponent<TeleportPoint>().ReTel(false);
                            flag = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            if(!stay2)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                stay2 = true;
                            }
                           

                            if (MovePoint2.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line2.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint2.SetActive(false);
                               
                                NextNum();
                            }
                        }
                        break;
                    case 35:    //51_1:압력계의 압력게이지로~
                        if(flag)
                        {
                            GameObject.Find("SPS_MO01_2_6").GetComponent<BlinkObject>().enabled = true; //외곽선
                            GameObject.Find("MainPump").GetComponent<Animator>().SetBool("MPG_up", true); //애니메이션 재생
                            NT10.SetActive(true);
                            flag = false;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            if (stay2)
                            {
                                stay = false;

                                //5초 후 활동
                                Invoke("StayTime", 5f);

                                stay2 = false;
                            }

                            if (stay)
                            {
                                NT10.SetActive(false);
                                GameObject.Find("MainPump").GetComponent<Animator>().SetBool("MPG_up", false); //애니메이션 돌아가기

                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_6").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_6").GetComponent<BlinkObject>().ReBlink();

                                NextNum();
                            }
                        } 
                        break;
                    case 36:    //51_2:다음은 성능시험 배관~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 37:    //51_3:아래 가이드를 따라 성능~
                        if (!flag)
                        {
                            Line3.SetActive(true);
                            MovePoint6.SetActive(true);
                            flag = true;
                        }
                       
                        if (!IngAudio.isPlaying)
                        {
                            player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성

                            if (MovePoint6.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line3.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint6.SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 38:    //52_1:성능시험 배관 2차측~
                        if(flag)
                        {
                            //밸브 활성화
                            GameObject.Find("MVPoint3").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            Gage3.SetActive(true);                                                      //게이지
                            GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>().enabled = true; //외곽선
                            flag = false;
                        }
                       
                        //애니메이션 끝!
                        if (GameObject.Find("MVPoint3").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            //외곽선 삭제
                            GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>().ReBlink();

                            //게이지 비활성화
                            Gage3.SetActive(false);

                            if (!stay2)
                            {
                                stay = false;
                                stay2 = true;
                            }

                            //5초 후 활동
                            Invoke("StayTime", 5f);

                            if (stay)
                            {
                                GameObject.Find("MVPoint3").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                NextNum();
                            }
                        }
                        break;
                    case 39:    //54_1:개방을 완료하고~

                        if (!IngAudio.isPlaying)
                        {
                            WaterDrop.SetActive(true);

                            if (stay2)
                            {
                                stay = false;
                                flag = true;
                                fade = false;

                                stay2 = false;
                            }

                            if (flag)
                            {
                                Invoke("StayTime", 7f);
                                flag = false;
                            }
                            if (stay)
                            {
                                Invoke("FadeToBlack", 2f);
                                stay = false;
                            }
                            if (fade)
                            {
                                WaterDrop.SetActive(false);
                                Invoke("FadeFromBlack", 2f);
                                fade = false;
                            }
                        }
                            

                        break;
                    case 40:    //56_1:마지막으로~---------------------------------------2차 fade후
                        //한번만 작동해야한다!
                        if (!flag && IngAudio.clip == s_narration[40])
                        {
                            SettingObject();

                            MovePoint4.SetActive(true);
                            player.transform.position = MovePoint4.transform.position;  //시작 위치로 플레이어 이동
                            player.transform.eulerAngles = MovePoint4.transform.eulerAngles;
                            MovePoint4.SetActive(false);
                            
                            player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성

                            GameObject.Find("MVPoint").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("MVPoint6").GetComponent<GageAnimationObject>().enabled = false;

                            //변수 초기화
                            fade = false;
                            stay = false;
                            stay2 = false;
                            stay3 = false;

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }


                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 41:    //56_2:성능시험관 1차측~
                        //밸브 활성화
                        if(flag)
                        {
                            GameObject.Find("MVPoint4").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            Gage4.SetActive(true);                                                      //게이지
                            GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().enabled = true; //외곽선
                            flag = false;
                        }
                       
                        //애니메이션 끝!
                        if (GameObject.Find("MVPoint4").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            if (!stay2)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_9").GetComponent<BlinkObject>().ReBlink();

                                //게이지 비활성화
                                Gage4.SetActive(false);

                                stay = false;
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }
                            if (stay)
                            {
                                GameObject.Find("MVPoint4").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                NextNum();
                            }
                        }
                            break;
                    case 42:    //56-3:이제 가이드를 따라~
                        if(!flag)
                        {
                            Line8.SetActive(true);
                            MovePoint6.SetActive(true);
                            MovePoint6.GetComponent<TeleportPoint>().ReTel(false);
                            flag = true;
                        }
                        
                        if (!IngAudio.isPlaying)
                        {
                            if(stay2)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                stay2 = false;
                            }
                            if (MovePoint6.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line8.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint6.SetActive(false);
                                NextNum();
                            }
                        }
                        break;
                    case 43:    //58-1:성능시험 배관 2차측~
                        if(flag)
                        {
                            //밸브 활성화
                            GameObject.Find("MVPoint5").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            Gage5.SetActive(true);                                                      //게이지
                            GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>().enabled = true; //외곽선
                            flag = false;
                        }
                        //애니메이션 끝!
                        if (GameObject.Find("MVPoint5").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            if (!stay2)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_10").GetComponent<BlinkObject>().ReBlink();

                                //게이지 비활성화
                                Gage5.SetActive(false);

                                stay = false;
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                GameObject.Find("MVPoint5").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                NextNum();
                            }
                        }
                        break;

                    case 44:    //58-2:이제 가이드를 따라~
                        if(!flag)
                        {
                            Line2.SetActive(true);
                            MovePoint2.SetActive(true);
                            MovePoint2.GetComponent<TeleportPoint>().ReTel(false);
                            flag = true;
                        }
                        if (!IngAudio.isPlaying)
                        {
                            if(stay2)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                stay2 = false;
                            }
                            
                            if (MovePoint2.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line2.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint2.SetActive(false);
                                NextNum();
                            }
                        }
                        break;

                    case 45:    //60-1:주펌프 토출측~
                        if(flag)
                        {
                            //밸브 활성화
                            GameObject.Find("MVPoint6").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            GameObject.Find("MVPoint6").GetComponent<GageAnimationObject>().enabled = true;
                            Gage6.SetActive(true);                                                      //게이지
                            GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().enabled = true; //외곽선
                            flag = false;
                        }
                        
                        //애니메이션 끝!
                        if (GameObject.Find("MVPoint6").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            if (!stay2)
                            {
                                //외곽선 삭제
                                GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("SPS_MO01_2_2").GetComponent<BlinkObject>().ReBlink();

                                //게이지 비활성화
                                Gage6.SetActive(false);
                                stay = false;
                                //5초 후 활동
                                Invoke("StayTime", 5f);
                                stay2 = true;
                            }

                            if (stay)
                            {
                                GameObject.Find("MVPoint6").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                NextNum();
                            }
                        }
                        break;
                    case 46:    //60-2:개방이 완료되었으면~
                        if(!flag)
                        {
                            Line9.SetActive(true);

                            //도착지점 활성화
                            MovePoint5.SetActive(true);
                            MovePoint5.GetComponent<TeleportPoint>().ReTel(false);
                            flag = true;
                        }
                        if (!IngAudio.isPlaying)
                        {
                            if(stay2)
                            {
                                player.GetComponent<PlayerContr>().PlayContr(); //컨트롤러 활성
                                stay2 = false;
                            }
                            if (MovePoint5.GetComponent<TeleportPoint>().MoveDone())   //다음 지점에 도착 완료
                            {
                                //도착지점 비활성화
                                Line9.SetActive(false);
                                player.GetComponent<PlayerContr>().StopContr(); //컨트롤러 비활성
                                MovePoint5.SetActive(false);
                                NextNum();
                            }
                        }
                        
                        break;
                    case 47:    //62_1:동력제어반으로~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                            break;
                    case 48:    //62-2:제어반에서~
                        if(flag)
                        {
                            GameObject.Find("PWPoint3").GetComponent<BoxCollider>().enabled = true;      //콜라이더
                            GameObject.Find("lamp_green").GetComponent<BlinkObject>().enabled = true; //외곽선
                            flag = false;
                        }
                        //애니메이션 끝!
                        if (GameObject.Find("PWPoint3").GetComponent<AnimationObject>().movedone)
                        {
                            if(!stay3)
                            {
                                GameObject.Find("PWPoint3").GetComponent<BoxCollider>().enabled = false;

                                //외곽선 삭제
                                GameObject.Find("lamp_green").GetComponent<BlinkObject>().EndBlink();
                                GameObject.Find("lamp_green").GetComponent<BlinkObject>().ReBlink();
                                GameObject.Find("PWPoint4").GetComponent<BoxCollider>().enabled = true;      //콜라이더

                                //불빛
                                greenlight.SetActive(true);
                                redlight.SetActive(false);

                                //외곽선 활성
                                GameObject.Find("Object001").GetComponent<BlinkObject>().enabled = true;

                                GameObject.Find("PWController").GetComponent<Animator>().SetBool("PWC2_off", false); //애니메이션 돌아가기

                                stay3 = true;
                            }
                           
                            if (GameObject.Find("PWPoint4").GetComponent<AnimationObject>().movedone)
                            {
                                if (!stay2)
                                {
                                    GameObject.Find("PWPoint4").GetComponent<BoxCollider>().enabled = false;      //콜라이더
                                    GameObject.Find("PWController").GetComponent<Animator>().SetBool("PWC2_AtoH", false); //애니메이션 돌아가기

                                    //외곽선 삭제
                                    GameObject.Find("Object001").GetComponent<BlinkObject>().EndBlink();
                                    GameObject.Find("lamp_red").GetComponent<BlinkObject>().EndBlink();

                                    stay = false;
                                    //5초 후 활동
                                    Invoke("StayTime", 10f);
                                    stay2 = true;
                                }

                                if (stay)
                                {
                                    SaveModeIngNum();//진행 번호 저장
                                    Valve.VR.SteamVR_LoadLevel.Begin("Main");
                                }
                            }
                        }
                        break;



                }
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
            GameObject.Find("Campus").SetActive(false);

        }
        else if (mode.Equals("Spring2"))
        {
            modeNum = Mode.Spring2;
            s_narration = spring2audio;
            ModeObject = GameObject.Find("Spring2");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Campus").SetActive(false);
       
        }
        else if (mode.Equals("Fire"))
        {
            modeNum = Mode.Fire;
            s_narration = fireaudio;
            ModeObject = GameObject.Find("Fire");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Campus").SetActive(false);
           
        }
        else if (mode.Equals("Pump"))
        {
            modeNum = Mode.Pump;
            s_narration = pumpaudio;
            ModeObject = GameObject.Find("Pump");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus").SetActive(false);
            
        }
        else if (mode.Equals("Campus1"))
        {
            modeNum = Mode.Campus1;
            s_narration = campusaudio1;
            ModeObject = GameObject.Find("Campus");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("Fire").SetActive(false);
            GameObject.Find("Campus").SetActive(false);
        }
        

        Debug.Log("Here my mode: " + PlayerPrefs.GetString("mode") + "(" + mode + ")");
    }

    public void ChangeMode(string _mode, int _num)
    {
        mode = _mode;
        ingNum = _num;
    }


    //행동번호 다음번호로
    public void NextNum()
    {
        ingNum++;
    }

    //행동번호 원하는 번호로
    public void ChangeNum(int _num)
    {
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

