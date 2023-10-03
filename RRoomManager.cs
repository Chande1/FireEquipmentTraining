using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
public class RRoomManager : MonoBehaviour
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


    protected GameObject VVBInfo;
    protected GameObject VVBInfo2;
    protected GameObject AVInfo;
    protected GameObject PVInfo;

    protected bool flag;
    protected bool outlineflag;                     //외각선 깜박임용 플래그
    protected bool stay;
    protected bool dirflag;
    protected bool stay2;

    protected GameObject Gage1;
    protected GameObject Gage2;
    protected GameObject Gage3;
    protected GameObject Gage4;
    protected GameObject Gage5;
    protected GameObject Gage6;

    protected GameObject WaterDrop2;
    protected GameObject WarningSound;

    void Start()
    {
        //ingNum = 0;

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
        ndone = false;

        startpoint = GameObject.Find("StartPoint");
        player = GameObject.Find("Player");
        IngAudio = gameObject.GetComponent<AudioSource>();  //오디오 소스 받아오기
        flag = false;
        outlineflag = false;
        stay = false;
        stay2 = false;
        dirflag = false;
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
                    case 0: //시험 벨브함을 이용한~
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동


                            /*나레이션중 행동 제한*/
                            if (GameObject.Find("RDoorPoint") != null)
                                GameObject.Find("RDoorPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VBoxPoint") != null)
                                GameObject.Find("VBoxPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VPoint") != null)
                                GameObject.Find("VPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VBoxPoint2") != null)
                                GameObject.Find("VBoxPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VPoint2") != null)
                                GameObject.Find("VPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            if (GameObject.Find("VVBInfo") != null)
                            {
                                VVBInfo = GameObject.Find("VVBInfo");
                                VVBInfo.SetActive(false);
                            }
                            if (GameObject.Find("VVBInfo2") != null)
                            {
                                VVBInfo2 = GameObject.Find("VVBInfo2");
                                VVBInfo2.SetActive(false);
                            }
                            if (GameObject.Find("AVInfo") != null)
                            {
                                AVInfo = GameObject.Find("AVInfo");
                                AVInfo.SetActive(false);
                            }
                            if (GameObject.Find("PVInfo") != null)
                            {
                                PVInfo = GameObject.Find("PVInfo");
                                PVInfo.SetActive(false);
                            }
                            if (GameObject.Find("AVDPoint") != null)
                                GameObject.Find("AVDPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            /*외곽선 전용*/
                            /*외곽선 전용*/
                            if (GameObject.Find("RRoom_MO02_Valve_02") != null)
                                GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_ring") != null)
                                GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_Cover") != null)
                                GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_1") != null)
                                GameObject.Find("off valve_1").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_2") != null)
                                GameObject.Find("off valve_2").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("drain valve") != null)
                                GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("Object031") != null)
                                GameObject.Find("Object031").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("MO_valve") != null)
                                GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RRM4_Button001") != null)
                                GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("setting valve") != null)
                                GameObject.Find("setting valve").GetComponent<BlinkObject>().enabled = false;

                            /*게이지 전용*/
                            if (GameObject.Find("RockGage1") != null)
                                Gage1 = GameObject.Find("RockGage1");
                            if (GameObject.Find("RockGage2") != null)
                                Gage2 = GameObject.Find("RockGage2");
                            if (GameObject.Find("RockGage3") != null)
                                Gage3 = GameObject.Find("RockGage3");
                            if (GameObject.Find("RockGage4") != null)
                                Gage4 = GameObject.Find("RockGage4");
                            if (GameObject.Find("RockGage5") != null)
                                Gage5 = GameObject.Find("RockGage5");
                            if (GameObject.Find("RockGage6") != null)
                                Gage6 = GameObject.Find("RockGage6");

                            if (GameObject.Find("GageCanvas"))
                            {
                                Gage1.SetActive(false);
                                Gage2.SetActive(false);
                                Gage3.SetActive(false);
                                Gage4.SetActive(false);
                                Gage5.SetActive(false);
                                Gage6.SetActive(false);
                            }

                            //물 파티클
                            if (GameObject.Find("WaterDrop2") != null)
                            {
                                WaterDrop2 = GameObject.Find("WaterDrop2");
                                WaterDrop2.SetActive(false);
                            }

                            //알람벨
                            if(GameObject.Find("WarningSound")!=null)
                            {
                                WarningSound = GameObject.Find("WarningSound");
                                WarningSound.SetActive(false);
                            }

                            flag = true;    //플레이어 포지션을 고정하지 않도록
                            stay2 = false;
                        }

                        //오디오가 안끝났다면
                        if (flag && !IngAudio.isPlaying)
                        {
                            //문 콜라이더 활성화
                            GameObject.Find("RDoorPoint").GetComponent<BoxCollider>().enabled = true;
                        }
                        //문을 열면
                        if (GameObject.Find("RDoorPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            GameObject.Find("RDoorPoint").SetActive(false);
                            NextNum();
                        }
                        break;
                    case 1:     //시험밸브함으로 이동~
                                //그냥 소리만 재생

                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 2: //시험벨브의 개방은~
                        //그냥 소리만 재생
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 3: //시험벨브함을 열어서~
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("VBoxPoint").GetComponent<BoxCollider>().enabled = true;
                        }
                        if (GameObject.Find("VBoxPoint").GetComponent<AnimationObject>().movedone)
                        {
                            //GameObject.Find("VBoxPoint").SetActive(false); 소리용 막기
                            NextNum();
                        }
                        break;
                    case 4: //시험 밸브함을 열면~
                            //밸브함 정보(압력계,시험밸브) 표기

                        //반복 막기용
                        if (flag)
                        {
                            VVBInfo.SetActive(true);
                            flag = false;
                        }

                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 5: //시험밸브함 안에~
                        /*외각선 깜박이기*/
                        GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                            flag = true;
                        }
                        break;

                    case 6: //시험 밸브를 작동~
                        //반복 막기용
                        if (flag)
                        {
                            VVBInfo.SetActive(false);
                            VVBInfo2.SetActive(true);   //개방,폐쇄 표기
                            flag = false;
                        }

                        //오디오가 끝나면
                        if (!IngAudio.isPlaying)
                        {
                            //밸브 활성화
                            GameObject.Find("VPoint").GetComponent<BoxCollider>().enabled = true;
                        }
                        if (GameObject.Find("VPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            //밸브 외곽선 비활성화
                            GameObject.Find("WaterDrop").GetComponent<AudioSource>().Stop();    //물소리 멈추기
                            GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("VPoint").SetActive(false);
                            NextNum();
                        }
                        break;
                    case 7: //알람밸브실은~
                        if (!IngAudio.isPlaying)
                        {
                            AVInfo.SetActive(true);
                            NextNum();
                        }
                        break;
                    case 8: //이제~
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("AVDPoint").GetComponent<BoxCollider>().enabled = true;
                        }

                        if (GameObject.Find("AVDPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("AVDPoint").SetActive(false);
                            flag = false;
                            dirflag = false;
                            NextNum();
                        }
                        break;
                    case 9: //알람밸브실의 내부~
                        if (!IngAudio.isPlaying)
                        {
                            if (!dirflag)    //천천히 시선 돌리기
                            {
                                Vector3 dir = GameObject.Find("MovePoint3").GetComponent<Transform>().transform.position -
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position;

                                GameObject.Find("Player").GetComponent<Transform>().transform.rotation =
                                    Quaternion.LookRotation(Vector3.RotateTowards(
                                        GameObject.Find("Player").GetComponent<Transform>().transform.forward, dir, 0.01f, 0f));

                                if (Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint3").GetComponent<Transform>().transform.rotation) <= 8)
                                {
                                    dirflag = true;
                                }
                                Debug.Log("dir:" + dir + "/prot:" + GameObject.Find("Player").GetComponent<Transform>().transform.rotation +
                                    "/angle:" + Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint3").GetComponent<Transform>().transform.rotation));
                            }
                            else
                            {
                                if (!flag)
                                {
                                    //시선 고정
                                    GameObject.Find("Player").GetComponent<Transform>().transform.LookAt(
                                        new Vector3(GameObject.Find("MovePoint3").gameObject.transform.position.x,
                                        GameObject.Find("Player").GetComponent<Transform>().transform.position.y,
                                        GameObject.Find("MovePoint3").gameObject.transform.position.z));

                                    //이동
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position =
                                   Vector3.MoveTowards(GameObject.Find("Player").GetComponent<Transform>().transform.position,
                                   GameObject.Find("MovePoint3").GetComponent<Transform>().transform.position, 0.005f);

                                }

                                //도착
                                if (GameObject.Find("Player").GetComponent<Transform>().transform.position ==
                                     GameObject.Find("MovePoint3").GetComponent<Transform>().transform.position)
                                {
                                    flag = true;
                                }

                                if (flag)
                                {
                                    NextNum();
                                }
                            }
                        }
                        break;
                    case 10:    //시험밸브가 개방되면~
                       
                        //외곽선
                        GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().enabled = true;
                        //물차오르는 애니메이션
                        GameObject.Find("AValve").GetComponent<Animator>().SetBool("AV_open", true);
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().EndBlink();
                            flag = true;
                            NextNum();
                        }
                        break;
                    case 11:
                        /*외각선 깜박이기*/
                        AVInfo.SetActive(false);
                        GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            //밸브 외곽선 비활성화
                            GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().EndBlink();
                            //flag = false;
                            NextNum();
                        }
                        break;

                    case 12:    //싸이렌/지구경종 소리
                        if (flag)
                        {
                            WarningSound.SetActive(true);
                            SaveModeIngNum();//진행 번호 저장
                            Invoke("StayTime", 5);
                            if(stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("AOffice");    //사무실로 이동
                        }
                        break;
                    case 13:    //작동 점검 완료~ -----------------------------------------------------------------------------------
                        //flag는 start()에서 다시 false로 초기화됨
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = GameObject.Find("StartPoint").transform.position;  //시작 위치로 플레이어 이동

                            /*나레이션중 행동 제한*/
                            if (GameObject.Find("RDoorPoint") != null)
                                GameObject.Find("RDoorPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VBoxPoint") != null)
                                GameObject.Find("VBoxPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VPoint") != null)
                                GameObject.Find("VPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VBoxPoint2") != null)
                                GameObject.Find("VBoxPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("VPoint2") != null)
                                GameObject.Find("VPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            if (GameObject.Find("VVBInfo") != null)
                            {
                                VVBInfo = GameObject.Find("VVBInfo");
                                VVBInfo.SetActive(false);
                            }
                            if (GameObject.Find("VVBInfo2") != null)
                            {
                                VVBInfo2 = GameObject.Find("VVBInfo2");
                                VVBInfo2.SetActive(false);
                            }
                            if (GameObject.Find("AVInfo") != null)
                            {
                                AVInfo = GameObject.Find("AVInfo");
                                AVInfo.SetActive(false);
                            }
                            if (GameObject.Find("PVInfo") != null)
                            {
                                PVInfo = GameObject.Find("PVInfo");
                                PVInfo.SetActive(false);
                            }
                            if (GameObject.Find("AVDPoint") != null)
                                GameObject.Find("AVDPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVTPoint") != null)
                                GameObject.Find("PVTPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            /*외곽선 전용*/
                            /*외곽선 전용*/
                            if (GameObject.Find("RRoom_MO02_Valve_02") != null)
                                GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_ring") != null)
                                GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_Cover") != null)
                                GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_1") != null)
                                GameObject.Find("off valve_1").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_2") != null)
                                GameObject.Find("off valve_2").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("drain valve") != null)
                                GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("Object031") != null)
                                GameObject.Find("Object031").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("MO_valve") != null)
                                GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RRM4_Button001") != null)
                                GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("setting valve") != null)
                                GameObject.Find("setting valve").GetComponent<BlinkObject>().enabled = false;

                            if (GameObject.Find("RockGage1") != null)
                                Gage1 = GameObject.Find("RockGage1");
                            if (GameObject.Find("RockGage2") != null)
                                Gage2 = GameObject.Find("RockGage2");
                            if (GameObject.Find("RockGage3") != null)
                                Gage3 = GameObject.Find("RockGage3");
                            if (GameObject.Find("RockGage4") != null)
                                Gage4 = GameObject.Find("RockGage4");
                            if (GameObject.Find("RockGage5") != null)
                                Gage5 = GameObject.Find("RockGage5");
                            if (GameObject.Find("RockGage6") != null)
                                Gage6 = GameObject.Find("RockGage6");

                            if (GameObject.Find("GageCanvas"))
                            {
                                Gage1.SetActive(false);
                                Gage2.SetActive(false);
                                Gage3.SetActive(false);
                                Gage4.SetActive(false);
                                Gage5.SetActive(false);
                                Gage6.SetActive(false);
                            }

                            //물 파티클
                            if (GameObject.Find("WaterDrop2") != null)
                            {
                                WaterDrop2 = GameObject.Find("WaterDrop2");
                                WaterDrop2.SetActive(false);
                            }
                            //알람벨
                            if (GameObject.Find("WarningSound") != null)
                            {
                                WarningSound = GameObject.Find("WarningSound");
                                WarningSound.SetActive(false);
                            }

                            //시험 밸브함 상태 변환
                            if (GameObject.Find("VVBox"))
                            {
                                GameObject.Find("VVBox").GetComponent<Animator>().SetBool("VVB_changeIdle", true);
                            }
                            //알람밸브 상태 변환
                            if (GameObject.Find("AValve"))
                            {
                                GameObject.Find("AValve").GetComponent<Animator>().SetBool("AV_changeIdle", true);
                            }

                            flag = true;
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 14:    //이제 시험 밸브함~
                        if (!IngAudio.isPlaying)
                        {
                            //문 콜라이더 활성화
                            GameObject.Find("RDoorPoint").GetComponent<BoxCollider>().enabled = true;
                        }
                        //문을 열면
                        if (GameObject.Find("RDoorPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            GameObject.Find("RDoorPoint").SetActive(false);
                            NextNum();
                        }
                        break;
                    case 15:    //시험 밸브함으로~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 16:    //시험밸브함을~
                        GameObject.Find("VBoxPoint2").GetComponent<BoxCollider>().enabled = true;
                        if (GameObject.Find("VBoxPoint2").GetComponent<AnimationObject>().movedone)
                        {
                            //GameObject.Find("VBoxPoint2").SetActive(false); 소리용 막기
                            NextNum();
                        }
                        break;
                    case 17:    //작동점검을 위해~
                        /*외각선 깜박이기*/
                        GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                            VVBInfo2.SetActive(true);   //개방,폐쇄 표기
                            flag = false;
                        }
                        break;

                    case 18:    //시험밸브함 안에~
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("VPoint2").GetComponent<BoxCollider>().enabled = true;
                        }
                        if (GameObject.Find("VPoint2").GetComponent<MoveAnimationObject>().movedone)
                        {
                            GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("VPoint2").SetActive(false);
                            stay = false;
                            NextNum();
                        }
                        break;
                    case 19:    //시험밸브의 복구를 위해~
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("AVDPoint").GetComponent<BoxCollider>().enabled = true;
                        }

                        if (GameObject.Find("AVDPoint").GetComponent<AnimationObject>().movedone)
                        {
                            Invoke("StayTime", 3f);

                            if(stay)
                            {
                                if (!dirflag)    //천천히 시선 돌리기
                                {
                                    Vector3 dir = GameObject.Find("MovePoint3").GetComponent<Transform>().transform.position -
                                        GameObject.Find("Player").GetComponent<Transform>().transform.position;

                                    GameObject.Find("Player").GetComponent<Transform>().transform.rotation =
                                        Quaternion.LookRotation(Vector3.RotateTowards(
                                            GameObject.Find("Player").GetComponent<Transform>().transform.forward, dir, 0.01f, 0f));

                                    if (Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                        GameObject.Find("MovePoint3").GetComponent<Transform>().transform.rotation) <= 8)
                                    {
                                        dirflag = true;
                                    }
                                    Debug.Log("dir:" + dir + "/prot:" + GameObject.Find("Player").GetComponent<Transform>().transform.rotation +
                                        "/angle:" + Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                        GameObject.Find("MovePoint3").GetComponent<Transform>().transform.rotation));
                                }
                                else
                                {
                                    if (!flag)
                                    {
                                        //시선 고정
                                        GameObject.Find("Player").GetComponent<Transform>().transform.LookAt(
                                            new Vector3(GameObject.Find("MovePoint3").gameObject.transform.position.x,
                                            GameObject.Find("Player").GetComponent<Transform>().transform.position.y,
                                            GameObject.Find("MovePoint3").gameObject.transform.position.z));

                                        //이동
                                        GameObject.Find("Player").GetComponent<Transform>().transform.position =
                                       Vector3.MoveTowards(GameObject.Find("Player").GetComponent<Transform>().transform.position,
                                       GameObject.Find("MovePoint3").GetComponent<Transform>().transform.position, 0.005f);

                                    }

                                    //도착
                                    if (GameObject.Find("Player").GetComponent<Transform>().transform.position ==
                                         GameObject.Find("MovePoint3").GetComponent<Transform>().transform.position)
                                    {
                                        flag = true;
                                    }

                                    if (flag)
                                    {
                                        GameObject.Find("AVDPoint").SetActive(false);
                                        NextNum();
                                    }

                                }
                            }
                           
                        }
                        break;
                    case 20:    //시험밸브를 폐쇄하면~
                                //해당 애니메이션이 구현
                        GameObject.Find("AValve").GetComponent<Animator>().SetBool("AV_close", true);
                        /*외각선 깜박이기*/
                        GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().EndBlink(); ;
                            NextNum();
                            stay = false;
                            flag = false;
                        }
                        break;
                    case 21:    //압력스위치인 리타~
                        //해당 부품이 구현되어 있지 않음으로 나레이션만 일단 재생
                        /*외각선 깜박이기*/
                        GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            //밸브 외곽선 비활성화
                            GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().EndBlink();
                            SaveModeIngNum();//진행 번호 저장
                            Invoke("StayTime", 5);
                            if (stay)
                                Valve.VR.SteamVR_LoadLevel.Begin("AOffice");    //사무실로 이동
                        }
                        break;
                }

                break;

            case Mode.Spring2:
                switch (ingNum)
                {
                    case 0: //준비작동식 밸브실은~
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동


                            /*나레이션중 행동 제한*/
                            if (GameObject.Find("PPDPoint") != null)
                                GameObject.Find("PPDPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVTPoint") != null)
                                GameObject.Find("PVTPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("WVPoint") != null)
                                GameObject.Find("WVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPoint") != null)
                                GameObject.Find("SPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPVPoint") != null)
                                GameObject.Find("SPVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVBPoint") != null)
                                GameObject.Find("PVBPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("WVPoint2") != null)
                                GameObject.Find("WVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPVPoint2") != null)
                                GameObject.Find("SPVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("CVPoint") != null)
                                GameObject.Find("CVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("CVPoint2") != null)
                                GameObject.Find("CVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVBPoint2") != null)
                                GameObject.Find("PVBPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVTPoint2") != null)
                                GameObject.Find("PVTPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            /*외곽선 전용*/
                            if (GameObject.Find("RRoom_MO02_Valve_02") != null)
                                GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_ring") != null)
                                GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_Cover") != null)
                                GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_1") != null)
                                GameObject.Find("off valve_1").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_2") != null)
                                GameObject.Find("off valve_2").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("drain valve") != null)
                                GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("Object031") != null)
                                GameObject.Find("Object031").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("MO_valve") != null)
                                GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RRM4_Button001") != null)
                                GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("setting valve") != null)
                                GameObject.Find("setting valve").GetComponent<BlinkObject>().enabled = false;

                            if (GameObject.Find("AVInfo") != null)
                            {
                                AVInfo = GameObject.Find("AVInfo");
                                AVInfo.SetActive(false);
                            }
                            if (GameObject.Find("PVInfo") != null)
                            {
                                PVInfo = GameObject.Find("PVInfo");
                                PVInfo.SetActive(false);
                            }

                            if (GameObject.Find("RockGage1") != null)
                                Gage1 = GameObject.Find("RockGage1");
                            if (GameObject.Find("RockGage2") != null)
                                Gage2 = GameObject.Find("RockGage2");
                            if (GameObject.Find("RockGage3") != null)
                                Gage3 = GameObject.Find("RockGage3");
                            if (GameObject.Find("RockGage4") != null)
                                Gage4 = GameObject.Find("RockGage4");
                            if (GameObject.Find("RockGage5") != null)
                                Gage5 = GameObject.Find("RockGage5");
                            if (GameObject.Find("RockGage6") != null)
                                Gage6 = GameObject.Find("RockGage6");

                            Gage1.SetActive(false);
                            Gage2.SetActive(false);
                            Gage3.SetActive(false);
                            Gage4.SetActive(false);
                            Gage5.SetActive(false);
                            Gage6.SetActive(false);

                            //물 파티클
                            if (GameObject.Find("WaterDrop2") != null)
                            {
                                WaterDrop2 = GameObject.Find("WaterDrop2");
                                WaterDrop2.SetActive(false);
                            }

                            //애니메이션 동시 실행 방지용
                            GameObject.Find("PVBPoint").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVTPoint").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVBPoint2").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVTPoint2").GetComponent<GageAnimationObject>().enabled = false;

                            stay2 = false;
                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            flag = false;
                            dirflag = false;
                            NextNum();
                        }

                        break;
                    case 1:     //준비작동식밸브실 문을~
                        GameObject.Find("PPDPoint").GetComponent<BoxCollider>().enabled = true;
                        
                        if (GameObject.Find("PPDPoint").GetComponent<AnimationObject>().movedone)
                        {
                            PVInfo.SetActive(true);
                            GameObject.Find("PPDPoint").GetComponent<BoxCollider>().enabled = false;
                            NextNum();
                        }
                        break;
                    case 2: //준비작동실 내부입니다
                        if (!IngAudio.isPlaying)
                        {

                            //밸브 게이지창 활성화
                            Gage1.SetActive(true);
                            if (!dirflag)    //천천히 시선 돌리기
                            {
                                Vector3 dir = GameObject.Find("MovePoint").GetComponent<Transform>().transform.position -
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position;

                                GameObject.Find("Player").GetComponent<Transform>().transform.rotation =
                                    Quaternion.LookRotation(Vector3.RotateTowards(
                                        GameObject.Find("Player").GetComponent<Transform>().transform.forward, dir, 0.01f, 0f));

                                if (Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint").GetComponent<Transform>().transform.rotation) <= 8)
                                {
                                    dirflag = true;
                                }
                                Debug.Log("dir:" + dir + "/prot:" + GameObject.Find("Player").GetComponent<Transform>().transform.rotation +
                                    "/angle:" + Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint").GetComponent<Transform>().transform.rotation));
                            }
                            else
                            {
                                if (!flag)
                                {
                                    //시선 고정
                                    GameObject.Find("Player").GetComponent<Transform>().transform.LookAt(
                                        new Vector3(GameObject.Find("MovePoint").gameObject.transform.position.x,
                                        GameObject.Find("Player").GetComponent<Transform>().transform.position.y,
                                        GameObject.Find("MovePoint").gameObject.transform.position.z));

                                    //이동
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position =
                                   Vector3.MoveTowards(GameObject.Find("Player").GetComponent<Transform>().transform.position,
                                   GameObject.Find("MovePoint").GetComponent<Transform>().transform.position, 0.005f);

                                }

                                //도착
                                if (GameObject.Find("Player").GetComponent<Transform>().transform.position ==
                                     GameObject.Find("MovePoint").GetComponent<Transform>().transform.position)
                                {
                                    flag = true;
                                }

                                if (flag)
                                {

                                    NextNum();
                                }
                            }
                        }
                        break;
                    case 3: //작동점검 전~
                        
                        /*외각선 깜박이기*/
                        GameObject.Find("off valve_2").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("off valve_2").GetComponent<BlinkObject>().notblink = true;

                        GameObject.Find("PVTPoint").GetComponent<BoxCollider>().enabled = true;
                        GameObject.Find("PVTPoint").GetComponent<GageAnimationObject>().enabled = true;
                        if (GameObject.Find("PVTPoint").GetComponent<GageAnimationObject>().movedone)
                        {
                            if(!stay2)
                            {
                                Gage1.SetActive(false);
                                stay = false;
                                Invoke("StayTime", 2f);
                                stay2 = true;
                            }

                            if(stay)
                            {
                                GameObject.Find("PValve").GetComponent<Animator>().SetBool("PVT_close", false); //애니메이션 돌아가기
                                GameObject.Find("PVTPoint").SetActive(false);
                                GameObject.Find("off valve_2").GetComponent<BlinkObject>().EndBlink();
                                NextNum();
                                stay = false;
                            } 
                        }
                        break;
                        /*
                    case 4: //다음은 스프링쿨러펌프~(본문삭제)
                        //*외각선 깜박이기
                        GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("drain valve").GetComponent<BlinkObject>().notblink = true;

                        GameObject.Find("WVPoint").GetComponent<BoxCollider>().enabled = true;
                        Gage2.SetActive(true);

                        if (GameObject.Find("WVPoint").GetComponent<GageAnimationObject>().movedone)
                        {
                            Gage2.SetActive(false);
                            GameObject.Find("PValve").GetComponent<Animator>().SetBool("WV_open", false); //애니메이션 돌아가기
                            GameObject.Find("WVPoint").SetActive(false);
                            GameObject.Find("drain valve").GetComponent<BlinkObject>().EndBlink();

                            NextNum();
                        }

                        break;
                        */
                    case 4: //준비작동식밸브 점검을~
                        if (!IngAudio.isPlaying)
                        {
                            PVInfo.SetActive(false);
                            NextNum();
                        }
                        break;
                    case 5: //슈퍼비조리~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 6: //이제 슈퍼비조리~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 7: //슈퍼비조리판넬 수동기동~(버튼 누르고 이동)
                        //외곽선 
                        GameObject.Find("Object031").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("SPoint").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("SPoint").GetComponent<MoveAnimationObject>().tempdone)
                        {
                            if(flag)
                            {
                                GameObject.Find("YLight").SetActive(false);
                                GameObject.Find("SPanel").GetComponent<Animator>().SetBool("SP_check", false);//자동으로 애니메이션 실행
                                GameObject.Find("Object031").GetComponent<BlinkObject>().EndBlink();
                                flag = false;
                            }
                           
                        }
                        if(GameObject.Find("SPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            NextNum();
                        }
                        break;
                    case 8: //슈퍼비조리판넬을 작동~(이동 후)
                        //외각선
                        GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("MO_valve").GetComponent<BlinkObject>().notblink = true;
                        GameObject.Find("PValve").GetComponent<Animator>().SetBool("SPV_aopen", true);//자동으로 애니메이션 실행
                        if (!IngAudio.isPlaying)
                        {
                            GameObject.Find("MO_valve").GetComponent<BlinkObject>().notblink = false;
                            GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = false;
                            //GameObject.Find("MO_valve").GetComponent<BlinkObject>().EndBlink();
                            flag = true;
                            NextNum();
                        }
                        break;
                    case 9:    //또는 슈퍼비조리판넬을~
                        //한번만 작동하도록
                        if (flag)
                        {
                            GameObject.Find("PValve").GetComponent<Animator>().SetBool("SPV_aopen", false);//자동으로 애니메이션 실행
                            flag = false;
                        }

                        //외각선 2개
                        GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().enabled = true;

                        if (!IngAudio.isPlaying)
                        {
                            if (GameObject.Find("SPVPoint"))
                            {
                                GameObject.Find("SPVPoint").GetComponent<BoxCollider>().enabled = true;
                                if (GameObject.Find("SPVPoint").GetComponent<MoveAnimationObject>().movedone)
                                {
                                    GameObject.Find("MO_valve").GetComponent<BlinkObject>().EndBlink();
                                    GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().EndBlink();
                                    SaveModeIngNum();   //모드 번호 기억하기
                                    GameObject.Find("SPVPoint").SetActive(false);
                                }
                            }
                        }

                        break;
                    case 10:    //작동점검 완료~-------------------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동


                            /*나레이션중 행동 제한*/
                                if (GameObject.Find("PPDPoint") != null)
                                GameObject.Find("PPDPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVTPoint") != null)
                                GameObject.Find("PVTPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("WVPoint") != null)
                                GameObject.Find("WVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPoint") != null)
                                GameObject.Find("SPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPVPoint") != null)
                                GameObject.Find("SPVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVBPoint") != null)
                                GameObject.Find("PVBPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("WVPoint2") != null)
                                GameObject.Find("WVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPVPoint2") != null)
                                GameObject.Find("SPVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("CVPoint") != null)
                                GameObject.Find("CVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("CVPoint2") != null)
                                GameObject.Find("CVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVBPoint2") != null)
                                GameObject.Find("PVBPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVTPoint2") != null)
                                GameObject.Find("PVTPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            /*외곽선 전용*/
                            if (GameObject.Find("RRoom_MO02_Valve_02") != null)
                                GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("Outline_receive") != null)
                                GameObject.Find("Outline_receive").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_Cover") != null)
                                GameObject.Find("RR_MO01_Cover").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_1") != null)
                                GameObject.Find("off valve_1").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_2") != null)
                                GameObject.Find("off valve_2").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("drain valve") != null)
                                GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("Object031") != null)
                                GameObject.Find("Object031").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("MO_valve") != null)
                                GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RRM4_Button001") != null)
                                GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("setting valve") != null)
                                GameObject.Find("setting valve").GetComponent<BlinkObject>().enabled = false;

                            if (GameObject.Find("AVInfo") != null)
                            {
                                AVInfo = GameObject.Find("AVInfo");
                                AVInfo.SetActive(false);
                            }
                            if (GameObject.Find("PVInfo") != null)
                            {
                                PVInfo = GameObject.Find("PVInfo");
                                PVInfo.SetActive(false);
                            }

                            if (GameObject.Find("RockGage1") != null)
                                Gage1 = GameObject.Find("RockGage1");
                            if (GameObject.Find("RockGage2") != null)
                                Gage2 = GameObject.Find("RockGage2");
                            if (GameObject.Find("RockGage3") != null)
                                Gage3 = GameObject.Find("RockGage3");
                            if (GameObject.Find("RockGage4") != null)
                                Gage4 = GameObject.Find("RockGage4");
                            if (GameObject.Find("RockGage5") != null)
                                Gage5 = GameObject.Find("RockGage5");
                            if (GameObject.Find("RockGage6") != null)
                                Gage6 = GameObject.Find("RockGage6");

                            Gage1.SetActive(false);
                            Gage2.SetActive(false);
                            Gage3.SetActive(false);
                            Gage4.SetActive(false);
                            Gage5.SetActive(false);
                            Gage6.SetActive(false);

                            //물 파티클
                            if (GameObject.Find("WaterDrop2") != null)
                            {
                                WaterDrop2 = GameObject.Find("WaterDrop2");
                                WaterDrop2.SetActive(false);
                            }

                            //벨브 애니메이션 꼬임 방지용
                            GameObject.Find("Point_off valve_2_bar").GetComponent<Transform>().Translate(0,-0.12f, 0);
                            //애니메이션 동시 실행 방지용
                            GameObject.Find("PVBPoint").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVTPoint").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVBPoint2").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVTPoint2").GetComponent<GageAnimationObject>().enabled = false;

                            //애니메이션 순서 방해 방지용
                            //GameObject.Find("PreparedValve").GetComponent<Animator>().SetBool("SecondValveOpen", true);

                            stay2 = false;
                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            flag = false;
                            dirflag = false;
                            NextNum();
                        }

                        break;
                    case 11:    //이제 준비작동식~
                        GameObject.Find("PPDPoint").GetComponent<BoxCollider>().enabled = true;
                        if (GameObject.Find("PPDPoint").GetComponent<AnimationObject>().movedone)
                        {
                            GameObject.Find("PValve").GetComponent<Animator>().SetBool("PV_change", true);  //밸브 돌려놓은 상태 아이들
                            NextNum();
                         }
                        break;
                    case 12:    //준비작동식밸브의~내부입니다~
                        if (!IngAudio.isPlaying)
                        {
                            
                            if (!dirflag)    //천천히 시선 돌리기
                            {
                                Vector3 dir = GameObject.Find("MovePoint").GetComponent<Transform>().transform.position -
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position;

                                GameObject.Find("Player").GetComponent<Transform>().transform.rotation =
                                    Quaternion.LookRotation(Vector3.RotateTowards(
                                        GameObject.Find("Player").GetComponent<Transform>().transform.forward, dir, 0.01f, 0f));

                                if (Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint").GetComponent<Transform>().transform.rotation) <= 8)
                                {
                                    dirflag = true;
                                }
                                Debug.Log("dir:" + dir + "/prot:" + GameObject.Find("Player").GetComponent<Transform>().transform.rotation +
                                    "/angle:" + Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint").GetComponent<Transform>().transform.rotation));
                            }
                            else
                            {
                                if (!flag)
                                {
                                    //시선 고정
                                    GameObject.Find("Player").GetComponent<Transform>().transform.LookAt(
                                        new Vector3(GameObject.Find("MovePoint").gameObject.transform.position.x,
                                        GameObject.Find("Player").GetComponent<Transform>().transform.position.y,
                                        GameObject.Find("MovePoint").gameObject.transform.position.z));

                                    //이동
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position =
                                   Vector3.MoveTowards(GameObject.Find("Player").GetComponent<Transform>().transform.position,
                                   GameObject.Find("MovePoint").GetComponent<Transform>().transform.position, 0.005f);

                                }

                                //도착
                                if (GameObject.Find("Player").GetComponent<Transform>().transform.position ==
                                     GameObject.Find("MovePoint").GetComponent<Transform>().transform.position)
                                {
                                    flag = true;
                                }

                                if (flag)
                                {
                                    GameObject.Find("PPDPoint").SetActive(false);
                                    NextNum();
                                }
                            }
                        }
                        break;
                    case 13:    //1차측~
                        if (!IngAudio.isPlaying)
                        {
                            Gage3.SetActive(true);
                            NextNum();
                        }
                        break;
                    case 14:    //1차측~
                        GameObject.Find("off valve_1").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("off valve_1").GetComponent<BlinkObject>().notblink = true;
                        GameObject.Find("PVBPoint").GetComponent<GageAnimationObject>().enabled = true;
                        GameObject.Find("PVBPoint").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("PVBPoint").GetComponent<GageAnimationObject>().movedone)
                        {
                            if(!stay2)
                            {
                                stay = false;
                                Gage3.SetActive(false);
                                Invoke("StayTime", 2f);
                                stay2 = true;
                            }
                            if(stay)
                            {
                                GameObject.Find("PValve").GetComponent<Animator>().SetBool("PVB_close", false); //애니메이션 돌아가기
                                GameObject.Find("PVBPoint").SetActive(false);
                                GameObject.Find("off valve_1").GetComponent<BlinkObject>().EndBlink();
                                NextNum();
                                stay = false;
                            }
                           
                        }

                        break;
                    case 15:    //2차측~
                        if (!IngAudio.isPlaying)
                        {
                            NextNum();
                        }
                        break;
                    case 16:    //2차측
                        /*외각선 깜박이기*/
                        GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("drain valve").GetComponent<BlinkObject>().notblink = true;

                        GameObject.Find("WVPoint").GetComponent<BoxCollider>().enabled = true;
                        Gage2.SetActive(true);

                        if(GameObject.Find("WVPoint").GetComponent<GageAnimationObject>().GetGageCountDone())
                        {
                            WaterDrop2.SetActive(true); //물나오기

                        }

                        if (GameObject.Find("WVPoint").GetComponent<GageAnimationObject>().movedone)
                        {
                            Gage2.SetActive(false);
                            GameObject.Find("WVPoint").SetActive(false);
                            //하이라이트 없애기
                            GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = false;
                            GameObject.Find("drain valve").GetComponent<BlinkObject>().EndBlink();
                            if (GameObject.Find("Highlighter"))
                            {
                                Debug.Log("destroy!");
                                Destroy(GameObject.Find("Highlighter"));
                            }
                            NextNum();
                        }
                        break;
                    case 17:    //슈퍼비조리판넬~ 자동이동
                        //외곽선 
                        GameObject.Find("Object031").GetComponent<BlinkObject>().enabled = true;

                        GameObject.Find("SPoint").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("SPoint").GetComponent<MoveAnimationObject>().tempdone)
                        {
                            if (stay2)
                            {
                                stay = false;
                                GameObject.Find("YLight").SetActive(false);
                                Invoke("StayTime", 2f);
                                stay2 = false;
                            }
                        }
                        if (GameObject.Find("SPoint").GetComponent<MoveAnimationObject>().movedone)
                        {
                            if (stay)
                            {
                                GameObject.Find("PValve").GetComponent<Animator>().SetBool("WV_open", false); //애니메이션 돌아가기
                                GameObject.Find("Object031").GetComponent<BlinkObject>().EndBlink();
                                NextNum();
                            }

                        }
                        break;
                    case 18:    //밸브 내의~ 자동이동 후(문안)
                        /*외각선 깜박이기*/
                        
                        if (!stay2)    //무지성 반복 방지
                        {
                            stay = false;
                            Invoke("StayTime", 5f);
                            stay2 = true;
                        }

                        if(!IngAudio.isPlaying&&stay&&stay2)
                        {
                            WaterDrop2.SetActive(false); //물끄기
                            GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = true;
                            GameObject.Find("drain valve").GetComponent<BlinkObject>().ReFlag();
                            GameObject.Find("drain valve").GetComponent<BlinkObject>().notblink = true;
                            flag = false;
                            stay = false;

                        }

                        if (GameObject.Find("WVPoint2")&&!stay&&!flag)
                        {
                            GameObject.Find("WVPoint2").GetComponent<BoxCollider>().enabled = true;
                            Gage4.SetActive(true);

                            if (GameObject.Find("WVPoint2").GetComponent<GageAnimationObject>().GetGageCountDone())
                            {
                                

                            }

                            if (GameObject.Find("WVPoint2").GetComponent<GageAnimationObject>().movedone)
                            {
                                Gage4.SetActive(false);
                                GameObject.Find("WVPoint2").SetActive(false);
                                GameObject.Find("drain valve").GetComponent<BlinkObject>().EndBlink();
                                SaveModeIngNum();   //모드 번호 기억하기
                            }
                        }


                        break;
                    case 19:    //수신반 복구가~-------------------------------------------------------------------------------------
                        if (!flag && IngAudio.clip != null)
                        {
                            player.transform.position = startpoint.transform.position;  //시작 위치로 플레이어 이동

                            /*나레이션중 행동 제한*/
                            if (GameObject.Find("PPDPoint") != null)
                                GameObject.Find("PPDPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVTPoint") != null)
                                GameObject.Find("PVTPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("WVPoint") != null)
                                GameObject.Find("WVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPoint") != null)
                                GameObject.Find("SPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPVPoint") != null)
                                GameObject.Find("SPVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVBPoint") != null)
                                GameObject.Find("PVBPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("WVPoint2") != null)
                                GameObject.Find("WVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("SPVPoint2") != null)
                                GameObject.Find("SPVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("CVPoint") != null)
                                GameObject.Find("CVPoint").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("CVPoint2") != null)
                                GameObject.Find("CVPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVBPoint2") != null)
                                GameObject.Find("PVBPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화
                            if (GameObject.Find("PVTPoint2") != null)
                                GameObject.Find("PVTPoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            /*외곽선 전용*/
                            if (GameObject.Find("RRoom_MO02_Valve_02") != null)
                                GameObject.Find("RRoom_MO02_Valve_02").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RR_MO01_ring") != null)
                                GameObject.Find("RR_MO01_ring").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("chamber_front") != null)
                                GameObject.Find("chamber_front").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_1") != null)
                                GameObject.Find("off valve_1").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("off valve_2") != null)
                                GameObject.Find("off valve_2").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("drain valve") != null)
                                GameObject.Find("drain valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("Object031") != null)
                                GameObject.Find("Object031").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("MO_valve") != null)
                                GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("RRM4_Button001") != null)
                                GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().enabled = false;
                            if (GameObject.Find("setting valve") != null)
                                GameObject.Find("setting valve").GetComponent<BlinkObject>().enabled = false;

                            if (GameObject.Find("AVInfo") != null)
                            {
                                AVInfo = GameObject.Find("AVInfo");
                                AVInfo.SetActive(false);
                            }
                            if (GameObject.Find("PVInfo") != null)
                            {
                                PVInfo = GameObject.Find("PVInfo");
                                PVInfo.SetActive(false);
                            }

                            if (GameObject.Find("RockGage1") != null)
                                Gage1 = GameObject.Find("RockGage1");
                            if (GameObject.Find("RockGage2") != null)
                                Gage2 = GameObject.Find("RockGage2");
                            if (GameObject.Find("RockGage3") != null)
                                Gage3 = GameObject.Find("RockGage3");
                            if (GameObject.Find("RockGage4") != null)
                                Gage4 = GameObject.Find("RockGage4");
                            if (GameObject.Find("RockGage5") != null)
                                Gage5 = GameObject.Find("RockGage5");
                            if (GameObject.Find("RockGage6") != null)
                                Gage6 = GameObject.Find("RockGage6");

                            Gage1.SetActive(false);
                            Gage2.SetActive(false);
                            Gage3.SetActive(false);
                            Gage4.SetActive(false);
                            Gage5.SetActive(false);
                            Gage6.SetActive(false);

                            if (GameObject.Find("SVPValvePoint2") != null)
                                GameObject.Find("SVPValvePoint2").GetComponent<BoxCollider>().enabled = false;  //박스콜라이더 비활성화

                            //물 파티클
                            if (GameObject.Find("WaterDrop2") != null)
                            {
                                WaterDrop2 = GameObject.Find("WaterDrop2");
                                WaterDrop2.SetActive(false);
                            }

                            //벨브 애니메이션 꼬임 방지용
                            GameObject.Find("Point_off valve_2_bar").GetComponent<Transform>().Translate(0, -0.12f, 0);
                            GameObject.Find("Point_off valve_1_bar").GetComponent<Transform>().Translate(0, -0.12f, 0);
                            //애니메이션 동시 실행 방지용
                            GameObject.Find("PVBPoint").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVTPoint").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVBPoint2").GetComponent<GageAnimationObject>().enabled = false;
                            GameObject.Find("PVTPoint2").GetComponent<GageAnimationObject>().enabled = false;
                            //GameObject.Find("PreparedValve").GetComponent<Animator>().SetBool("SecondValveOpen", true);

                            stay2 = false;
                            flag = true;    //플레이어 포지션을 고정하지 않도록
                        }

                        if (flag && !IngAudio.isPlaying)
                        {
                            GameObject.Find("PValve").GetComponent<Animator>().SetBool("SPV_aopen", true);//자동으로 애니메이션 실행
                            flag = false;
                            dirflag = false;
                            NextNum();
                        }
                        break;
                    case 20:    //계속해서~
                        GameObject.Find("PPDPoint").GetComponent<BoxCollider>().enabled = true;
                        if (GameObject.Find("PPDPoint").GetComponent<AnimationObject>().movedone)
                        {
                            NextNum();
                            
                        }
                        break;
                    case 21:    //준비작동실 내부~
                        if (!IngAudio.isPlaying)
                        {
                            
                            if (!dirflag)    //천천히 시선 돌리기
                            {
                                Vector3 dir = GameObject.Find("MovePoint").GetComponent<Transform>().transform.position -
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position;

                                GameObject.Find("Player").GetComponent<Transform>().transform.rotation =
                                    Quaternion.LookRotation(Vector3.RotateTowards(
                                        GameObject.Find("Player").GetComponent<Transform>().transform.forward, dir, 0.01f, 0f));

                                if (Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint").GetComponent<Transform>().transform.rotation) <= 8)
                                {
                                    dirflag = true;
                                }
                                Debug.Log("dir:" + dir + "/prot:" + GameObject.Find("Player").GetComponent<Transform>().transform.rotation +
                                    "/angle:" + Quaternion.Angle(GameObject.Find("Player").GetComponent<Transform>().transform.rotation,
                                    GameObject.Find("MovePoint").GetComponent<Transform>().transform.rotation));
                            }
                            else
                            {
                                if (!flag)
                                {
                                    //시선 고정
                                    GameObject.Find("Player").GetComponent<Transform>().transform.LookAt(
                                        new Vector3(GameObject.Find("MovePoint").gameObject.transform.position.x,
                                        GameObject.Find("Player").GetComponent<Transform>().transform.position.y,
                                        GameObject.Find("MovePoint").gameObject.transform.position.z));

                                    //이동
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position =
                                   Vector3.MoveTowards(GameObject.Find("Player").GetComponent<Transform>().transform.position,
                                   GameObject.Find("MovePoint").GetComponent<Transform>().transform.position, 0.005f);

                                }

                                //도착
                                if (GameObject.Find("Player").GetComponent<Transform>().transform.position ==
                                     GameObject.Find("MovePoint").GetComponent<Transform>().transform.position)
                                {
                                    flag = true;
                                }

                                if (flag)
                                {
                                    GameObject.Find("PPDPoint").SetActive(false);
                                    NextNum();
                                }
                            }
                        }
                        break;
                    case 22:    //준비작동식밸브실~
                        //외각선 2개
                        GameObject.Find("MO_valve").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("SPVPoint2").GetComponent<BoxCollider>().enabled = true;

                        if (GameObject.Find("SPVPoint2").GetComponent<AnimationObject>().movedone)  //애니메이션이 끝나면?
                        {
                            GameObject.Find("MO_valve").GetComponent<BlinkObject>().EndBlink();
                            GameObject.Find("RRM4_Button001").GetComponent<BlinkObject>().EndBlink();
                            if(GameObject.Find("PValve").GetComponent<Animator>().GetBool("SPV_sclose"))
                            {
                                GameObject.Find("PValve").GetComponent<Animator>().SetBool("SPV_aopen", false);//자동으로 애니메이션 실행 멈추기
                                NextNum();  //다음
                            }
                        }
                        break;
                    case 23:    //다음은 중간챔버급수밸브를~
                        if (!IngAudio.isPlaying)
                        {
                            //위줄에서 못지운 외곽선 지우기
                            if (GameObject.Find("Highlighter"))
                            {
                                Debug.Log("destroy!");
                                Destroy(GameObject.Find("Highlighter"));
                            }
                            NextNum();
                        }
                        break;
                    case 24:    //준비작동식밸브의~

                        if (!IngAudio.isPlaying)
                        {
                            //위줄에서 못지운 외곽선 지우기
                            if (GameObject.Find("Highlighter"))
                            {
                                Debug.Log("destroy!");
                                Destroy(GameObject.Find("Highlighter"));
                            }
                            GameObject.Find("setting valve").GetComponent<BlinkObject>().enabled = true;
                            NextNum();
                        }
                        break;
                    case 25:    //이제 중간챔버급수밸브를~
                        GameObject.Find("CVPoint").GetComponent<BoxCollider>().enabled = true;  //박스콜라이더 활성화

                        

                        if (GameObject.Find("CVPoint").GetComponent<AnimationObject>().movedone)  //애니메이션이 끝나면?
                        {
                            //GameObject.Find("CVPoint").SetActive(false);  //소리용 막기
                            if (GameObject.Find("CVPoint"))
                            {
                                GameObject.Find("CVPoint").GetComponent<BoxCollider>().enabled = false;
                                GameObject.Find("CVPoint").GetComponent<AnimationObject>().enabled = false;
                            }
                                
                            NextNum();  //다음
                        }
                        break;
                    case 26:    //해당 밸브의 개방으로~
                        

                        //압력계 올라가기
                        if(flag)
                        {
                            GameObject.Find("CVPoint2").GetComponent<BoxCollider>().enabled = true;  //박스콜라이더 비활성화
                            GameObject.Find("Point_pressure gauge01").GetComponent<Animation>().Play();
                            flag = false;
                        }
                        
                        if (GameObject.Find("CVPoint2").GetComponent<AnimationObject>().movedone)  //애니메이션이 끝나면?
                        {
                           
                            GameObject.Find("PValve").GetComponent<Animator>().SetBool("CV_open", false);   //애니메이션 자동 재생 막기
                            //GameObject.Find("CVPoint2").SetActive(false); //소리용 막기
                            if (GameObject.Find("CVPoint2"))
                            {
                                GameObject.Find("CVPoint2").GetComponent<BoxCollider>().enabled = false;
                                GameObject.Find("CVPoint2").GetComponent<AnimationObject>().enabled = false;
                            }
                                
                            GameObject.Find("setting valve").GetComponent<BlinkObject>().EndBlink();
                            Gage5.SetActive(true);
                            NextNum();  //다음
                        }
                        break;

                    case 27:    //1차측 압력계의~
                        //GameObject.Find("CVPoint2").GetComponent<AnimationObject>().movedone = false;
                        GameObject.Find("off valve_1").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("off valve_1").GetComponent<BlinkObject>().notblink = true;
                        GameObject.Find("PVBPoint2").GetComponent<BoxCollider>().enabled = true;
                        GameObject.Find("PVBPoint2").GetComponent<GageAnimationObject>().enabled = true;
                        if (GameObject.Find("PVBPoint2").GetComponent<GageAnimationObject>().movedone&&
                            GameObject.Find("PVBPoint2").GetComponent<GageAnimationObject>().Idle())
                        {
                            if(!stay2)
                            {
                                Gage5.SetActive(false);
                                stay = false;
                                Invoke("StayTime", 2f);

                                stay2 = true;
                            }
                            if(stay)
                            {
                                GameObject.Find("PValve").GetComponent<Animator>().SetBool("PVB_open", false); //애니메이션 돌아가기
                                GameObject.Find("PVBPoint2").SetActive(false);
                                GameObject.Find("off valve_1").GetComponent<BlinkObject>().EndBlink();
                                Gage6.SetActive(true);
                                NextNum();
                                stay = false;
                            }
                        }
                        break;
                    case 28:    //1차측 개폐밸브를~
                        GameObject.Find("off valve_2").GetComponent<BlinkObject>().enabled = true;
                        GameObject.Find("off valve_2").GetComponent<BlinkObject>().notblink = true;
                        if (GameObject.Find("PVTPoint2"))
                        {
                            GameObject.Find("PVTPoint2").GetComponent<BoxCollider>().enabled = true;
                            GameObject.Find("PVTPoint2").GetComponent<GageAnimationObject>().enabled = true;
                        }
                           
                        if (GameObject.Find("PVTPoint2"))
                        {
                            if (GameObject.Find("PVTPoint2").GetComponent<GageAnimationObject>().movedone)
                            {
                                if(stay2)
                                {
                                    stay = false;
                                    Invoke("StayTime", 2f);
                                    stay2 = false;
                                }
                                if(stay)
                                {
                                    Gage6.SetActive(false);
                                    GameObject.Find("PValve").GetComponent<Animator>().SetBool("PVT_open", false); //애니메이션 돌아가기
                                    GameObject.Find("PVTPoint2").SetActive(false);
                                    GameObject.Find("off valve_2").GetComponent<BlinkObject>().EndBlink();
                                    SaveModeIngNum();
                                }

                                //NextNum();
                            }
                        }

                        break;
                }
                break;
            case Mode.Fire:

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
            GameObject.Find("Campus").gameObject.SetActive(false);
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
        /*
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
            GameObject.Find("fire").SetActive(false);
            GameObject.Find("Campus").SetActive(false);
        }
        else if (mode.Equals("Campus"))
        {
            modeNum = Mode.Campus;
            s_narration = campusaudio;
            ModeObject = GameObject.Find("Campus");
            GameObject.Find("Spring1").SetActive(false);
            GameObject.Find("Spring2").SetActive(false);
            GameObject.Find("Pump").SetActive(false);
            GameObject.Find("fire").SetActive(false);
        }*/

        Debug.Log("Here my mode: " + PlayerPrefs.GetString("mode") + "(" + mode + ")");
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

    protected void StopTime()
    {
        flag = true;
    }

    protected void StayTime()
    {
        stay = true;
    }
}

