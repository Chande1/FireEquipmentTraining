using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCard : MonoBehaviour
{
    [Header("회전 속도")]
    [SerializeField] float rotspeed;
    [Header("회전할 각도")]
    [SerializeField] float wantrot;

    [SerializeField] bool rot; //회전하는지 검사
    [SerializeField] float pastrot; //처음 각도
    [SerializeField] float temprot; //회전중인 각도

    [Header("나타날 오브젝트 위치")]
    [SerializeField] GameObject objectpos;

    public bool changedone;

    void Start()
    {
        changedone = false;
        rot = false;
        pastrot= transform.eulerAngles.y+90;
        temprot = transform.eulerAngles.y+90;
    }

    private void Update()
    {
        if(rot)
        {
            temprot += Time.deltaTime * rotspeed;

            if(temprot>=wantrot+pastrot)
            {
                //temprot = pastrot;
                changedone = true;
                rot = false;
            }

        }

        transform.localRotation = Quaternion.Euler(0, temprot, 0);
    }

    public void ChangeCard(GameObject _changeobject)
    {
        rot = true;
        //오브젝트 소환
        GameObject Temp=Instantiate(_changeobject, objectpos.transform.position, objectpos.transform.rotation);
        Temp.transform.parent = objectpos.transform;

    }
}
