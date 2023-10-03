using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] Material pastMT;   //과거 메테리얼
    [SerializeField] Material newMT;    //새로운 메테리얼

    private void Start()
    {   
        //현재 메테리얼 저장
        pastMT = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
    }
}
