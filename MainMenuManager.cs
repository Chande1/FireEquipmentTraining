using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("진행중인 행동 번호")]
    [SerializeField] int IngNum = 0;
    
    [Header("메인UI")]
    [SerializeField] GameObject MainUI;
    [Header("서브UI")]
    [SerializeField] GameObject SubUI;
    [Header("서브UI2")]
    [SerializeField] GameObject SubUI2;
    [Header("나레이션")]
    [Tooltip("(확인)재생중인 오디오 소스")]
    [SerializeField] AudioSource IngAudio;                      //진행중인 오디오
    [Tooltip("(필수)나레이션 음성클립")]
    [SerializeField] AudioClip[] m_narration;                   //메뉴 나레이션
    [SerializeField] bool ndone;                                //나레이션 반복 방지
    bool flag;

    void Start()
    {
        MainUI = GameObject.Find("MenuCanvas"); //메인캔버스 받아오기
        SubUI = GameObject.Find("SubCanvas");   //서브캔버스 받아오기
        SubUI2 = GameObject.Find("SubCanvas2");   //서브캔버스 받아오기
        IngAudio = gameObject.GetComponent<AudioSource>();

        if (LoadModeIngNum("SPSpace") == 9)
        {
            IngNum = 3;
            MainUI.SetActive(false);
            SubUI2.SetActive(false);

            //각 씬의 모드 진행 번호 초기화
            SaveModeIngNum("SPSpace", 0);
            SaveModeIngNum("RRoom", 0);
            SaveModeIngNum("AOffice", 0);
            SaveModeIngNum("4FSchool", 0);
        }
        else if(LoadModeIngNum("4FSchool") == 11|| LoadModeIngNum("4FSchool") == 1|| 
            LoadModeIngNum("4FSchool") == 3|| LoadModeIngNum("4FSchool") ==7|| LoadModeIngNum("4FSchool") == 4)
        {
            IngNum = 4;
            MainUI.SetActive(false);
            SubUI.SetActive(false);

            //각 씬의 모드 진행 번호 초기화
            SaveModeIngNum("SPSpace", 0);
            SaveModeIngNum("RRoom", 0);
            SaveModeIngNum("AOffice", 0);
            SaveModeIngNum("4FSchool", 0);
        }
        else
        {
            SubUI.SetActive(false); //서브 창 꺼놓기
            SubUI2.SetActive(false);

            //각 씬의 모드 진행 번호 초기화
            SaveModeIngNum("SPSpace", 0);
            SaveModeIngNum("RRoom", 0);
            SaveModeIngNum("AOffice", 0);
            SaveModeIngNum("4FSchool", 0);
        }


        ndone = false;
        flag = false;
    }

    void Update()
    {
        MainUI = GameObject.Find("MenuCanvas"); //메인캔버스 받아오기
        SubUI = GameObject.Find("SubCanvas");   //서브캔버스 받아오기
        SubUI2 = GameObject.Find("SubCanvas2");   //서브캔버스 받아오기

        switch (IngNum)
        {
            case 0: //오늘은~
                if (!flag&&IngAudio.clip!=null) //오디오 클립이 비어있지 않을때
                {
                    flag = true;
                }
                if(flag&&!IngAudio.isPlaying&&!MainUI)
                {
                    NextNum();
                }
                break;
            case 1: //스프링클러를~
                if (!IngAudio.isPlaying)
                {
                    NextNum();
                }
                break;
            case 2: //화재가 발생한~
                if (!IngAudio.isPlaying)
                {
                    NextNum();
                }
                break;
            case 3: //습식 스프링클러와~
                if (!IngAudio.isPlaying)
                {
                    //NextNum();
                }
                break;
            case 4: //퀴즈를 통해~
                if (!IngAudio.isPlaying && IngAudio.clip != null)
                {
                    Debug.Log("done");
                    NextNum();
                }
                break;
            case 5: //체험할 구역을~
                {
                    
                }
                break;
        }

        //다음 대사로 넘어간 경우 다음대사로 빠르게 교체
        if (IngAudio.clip != null && IngAudio.clip != m_narration[IngNum])              
        {
            IngAudio.Stop();
            ndone = false;
        }
        //오디오가 멈춰있을때
        if (!ndone && !IngAudio.isPlaying)                                            
        {
            IngAudio.clip = m_narration[IngNum];//해당 번호 나레이션 클립에 넣기
            IngAudio.Play();                    //클립 재생
            ndone = true;                       //나레이션 작동중
        }
        
    }

    private void SaveModeIngNum(string _scenename, int _startnum)
    {
        PlayerPrefs.SetInt(_scenename, _startnum);
        PlayerPrefs.Save();
        Debug.Log(_scenename + "(save) : " + _startnum);
    }

    private int LoadModeIngNum(string _scenename)
    {
        int nnum=0;
        Debug.Log(_scenename + "(Load) : " + nnum);

        return PlayerPrefs.GetInt(_scenename, nnum);
    }

    protected void NextNum()
    {
        IngNum++;
    }

    public void ChangeNum(int _num,bool _flag)
    {
        flag = _flag;
        IngNum = _num;
    }

    private void LoadMainNum()
    {
        int mnum=0;
       
    }
}
