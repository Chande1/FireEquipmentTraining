using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleButton : MonoBehaviour
{
    [SerializeField] GameObject maincanvas;
    [SerializeField] GameObject subcanvas;
    [SerializeField] GameObject subcanvas2;

    public void CanvasChange()
    {
        if (maincanvas.activeInHierarchy)
        {
            maincanvas.SetActive(false);
            subcanvas.SetActive(true);
            gameObject.GetComponent<MainMenuManager>().ChangeNum(1, true);    //다시 시작 나레이션 재생
        }
        else if (subcanvas.activeInHierarchy)
        {
            subcanvas.SetActive(false);
            maincanvas.SetActive(true);
            gameObject.GetComponent<MainMenuManager>().ChangeNum(0, false);    //다시 시작 나레이션 재생
        }

        Debug.Log("canvas change!");
    }
    public void CanvasChange2()
    {
        if (maincanvas.activeInHierarchy)
        {
            maincanvas.SetActive(false);
            subcanvas2.SetActive(true);
            gameObject.GetComponent<MainMenuManager>().ChangeNum(4, true);    //다시 시작 나레이션 재생
        }
        else if (subcanvas2.activeInHierarchy)
        {
            subcanvas2.SetActive(false);
            maincanvas.SetActive(true);
            gameObject.GetComponent<MainMenuManager>().ChangeNum(0, false);    //다시 시작 나레이션 재생
        }

        Debug.Log("canvas change!");
    }

    public void SceneChange(string SceneName)
    {
        //SceneManager.LoadScene(SceneName);
        Valve.VR.SteamVR_LoadLevel.Begin(SceneName);
        Debug.Log("scene change!");
    }

    public void ModeChange(string ModeName)
    {
        PlayerPrefs.SetString("mode", ModeName);        //현재 모드 저장
        Debug.Log("mode change!my mode: "+PlayerPrefs.GetString("mode"));
    }
}
