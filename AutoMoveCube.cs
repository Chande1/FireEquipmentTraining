using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveCube : MonoBehaviour
{
    [SerializeField] bool flag;
    [SerializeField] float movespeed;
    // Start is called before the first frame update
    void Start()
    {
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!flag)
        {
            /*
            Vector3 movere = GameObject.Find("MovePoint").gameObject.transform.position -
                GameObject.Find("Player").GetComponent<Transform>().transform.position;

            Quaternion moverot = Quaternion.LookRotation(movere, Vector3.up);
            GameObject.Find("Player").GetComponent<Transform>().transform.rotation = moverot;
            */
            Vector3 dir = GameObject.Find("MovePoint").GetComponent<Transform>().transform.position -
                                    GameObject.Find("Player").GetComponent<Transform>().transform.position;

            GameObject.Find("Player").GetComponent<Transform>().transform.rotation =
                Quaternion.LookRotation(Vector3.RotateTowards(
                    GameObject.Find("Player").GetComponent<Transform>().transform.forward, dir, 0.01f, 0f));
            /*
            //바라보게
            GameObject.Find("Player").GetComponent<Transform>().transform.LookAt(
                new Vector3(GameObject.Find("MovePoint").gameObject.transform.position.x,
                GameObject.Find("Player").GetComponent<Transform>().transform.position.y,
                GameObject.Find("MovePoint").gameObject.transform.position.z));
            
            //이동
            GameObject.Find("Player").GetComponent<Transform>().transform.position =
           Vector3.MoveTowards(GameObject.Find("Player").GetComponent<Transform>().transform.position,
           GameObject.Find("MovePoint").GetComponent<Transform>().transform.position, movespeed);
            */
        }
       
        if (GameObject.Find("Player").GetComponent<Transform>().transform.position ==
             GameObject.Find("MovePoint").GetComponent<Transform>().transform.position)
        {
            flag = true;
        }

    }

}
