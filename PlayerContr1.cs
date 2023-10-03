using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerMove : MonoBehaviour
{
    public SteamVR_Action_Vector2 touchpadInput;
    public Transform cameraTransform;
    public Transform SnapTurn;
    public Transform NonBodyColider;
    private CapsuleCollider capsuleCollider;
    //private CharacterController capsuleCollider;
    [SerializeField] bool flag;
    [SerializeField] bool stop;
    [SerializeField] Transform temptrans;
    [SerializeField] GameObject SnapTurnObject;

    void Start()
    {
        stop = false;
        flag = false;
        capsuleCollider = GetComponent<CapsuleCollider>();
        //if (GameObject.Find("Snap Turn") != null)
        //    SnapTurnObject = GameObject.Find("Snap Turn");

        //capsuleCollider = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!stop)   //강제로 멈추지 않는 다면
        {
            //플레이어 이동 구현
            //최소한의 조이스틱값을 받지 않으면 움직이지 않음
            if (Mathf.Abs(touchpadInput.axis.x) * 10 >= 2 || Mathf.Abs(touchpadInput.axis.y) * 10 >= 2)
            {
                flag = true;
            }
            else
                flag = false;

            //조이스틱을 움직이면 값을 받아서 플레이어를 이동
            if (flag)
            {
                //Debug.Log(Player.instance);
                Vector3 movementDir = Player.instance.hmdTransform.TransformDirection(new Vector3(touchpadInput.axis.x, 0, touchpadInput.axis.y));
                transform.position += (Vector3.ProjectOnPlane(Time.deltaTime * movementDir, Vector3.up));
                //Debug.Log(Player.instance+"move");
            }

        }
        else    //강제로 멈춘다면
        {
            gameObject.transform.position=temptrans.position;   //플레이어 위치 고정
        }


        //카메라를 따라가는 콜라이더
        float distanceFromFloor = Vector3.Dot(cameraTransform.localPosition, Vector3.up);
        capsuleCollider.height = Mathf.Max(capsuleCollider.radius, distanceFromFloor);

        capsuleCollider.center = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;

        //기타 카메라
        SnapTurn.localPosition= cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;
        NonBodyColider.localPosition= cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;

    }

    public void StopPlayer()
    {

        flag = false;
        temptrans = gameObject.transform;   //현재위치 저장
        SnapTurnObject.SetActive(false);
        stop = true;                        //멈춰!

    }

    public void PlayPlayer()
    {
        SnapTurnObject.SetActive(true);
        flag = false;
        stop = false;
    }
}
