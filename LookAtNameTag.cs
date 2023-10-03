using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtNameTag : MonoBehaviour
{
    [SerializeField] bool onlyxz;

    // Update is called once per frame
    void Update()
    {
        if(onlyxz)
        {
            transform.LookAt(new Vector3(GameObject.Find("VRCamera").transform.position.x,transform.position.y,
                GameObject.Find("VRCamera").transform.position.z));
        }
        else
            transform.LookAt(GameObject.Find("VRCamera").transform);       
    }
}
