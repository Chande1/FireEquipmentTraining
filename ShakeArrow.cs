using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeArrow : MonoBehaviour
{
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

    Vector3 firstposition;    //처음위치

    void Start()
    {
        firstposition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if(shack)
        {
            if(shackx)
            {
                Vector3 v = firstposition;
                v.x += movemax * Mathf.Sin(Time.time * sspeed);
                transform.position = v;
            }
            else if(shacky)
            {
                Vector3 v = firstposition;
                v.y += movemax * Mathf.Sin(Time.time * sspeed);
                transform.position = v;
            }
            else if(shackz)
            {
                Vector3 v = firstposition;
                v.z += movemax * Mathf.Sin(Time.time * sspeed);
                transform.position = v;
            }
            
        }
    }
}
