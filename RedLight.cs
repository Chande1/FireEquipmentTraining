using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedLight : MonoBehaviour
{
    [Header("오브젝트")]
    [SerializeField] bool redobject;
    [SerializeField] GameObject redlight2;

    [Header("이미지")]
    public Image[] redlight;
    Color color;
    bool flag;

    private void Start()
    {
        if(redobject)
        {

        }
        else
        {
            for (int i = 0; i < redlight.Length; i++)
            {
                color = redlight[i].color;
            }

            color.a = 0f;

        }

        flag = false;
    }
    private void Update()
    {
        if(!flag)
            Invoke("RedHightLight", 1f);
    }

    void RedHightLight()
    {
        if(redobject)
        {
            redlight2.SetActive(true);
        }
        else
        {
            color.a = 255f;
            for (int i = 0; i < redlight.Length; i++)
            {
                redlight[i].color = color;
            }
        }
        
        Invoke("NonLight", 1f);
        flag = true;
    }
    void NonLight()
    {
        if (redobject)
        {
            redlight2.SetActive(false);
        }
        else
        {
            color.a = 0f;
            for (int i = 0; i < redlight.Length; i++)
            {
                redlight[i].color = color;
            }
        }
       
        flag = false;
    }
}
