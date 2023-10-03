using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalPoint : MonoBehaviour
{
    protected bool flag;
    protected bool dirflag;
    protected bool arvdone;
    protected Transform temptrans;  //플레이어 초기 각도
    protected Transform temppos;  //원하는 방향 각도

    [Header("목표까지 플레이어 회전 속도")]
    [SerializeField] float rotspeed;      //회전 속도
    [Header("목표까지 플레이어 이동 속도")]
    [SerializeField] float transspeed;      //이동 속도

    [SerializeField] GameObject snapturn;
    [SerializeField] GameObject teleport;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform SnapTurn;
    [SerializeField] Transform NonBodyColider;
    [SerializeField] CapsuleCollider capsuleCollider;

    public bool ArriveDone()
    {
        if (arvdone && !flag)
        {
            ReARVPoint();
            return true;
        } 
        else
            return false;

    }

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
        dirflag = false;
        arvdone = false;
    }

    private void Update()
    {
        if(flag)
        {
            GameObject.Find("Player").GetComponent<PlayerMove>().StopPlayer();  //컨트롤러 비활성화
           
            if (dirflag)    //캐릭터 이동
            {
                //이동
                //플레이어와 원하는 좌표 확인용
                //Debug.Log("player(pos) :" + GameObject.Find("Player").transform.position + "\arvpoint(pos) :" + gameObject.transform.position);


                //플레이어가 원하는 지점까지 이동
                GameObject.Find("Player").transform.position = Vector3.MoveTowards(GameObject.Find("Player").transform.position,
                    transform.position, transspeed * Time.deltaTime);

                //플레이어 좌표-원하는 좌표의 절대값이 0.5보다 작을 경우 이동 끝!
                if (Mathf.Abs(GameObject.Find("Player").transform.position.x-gameObject.transform.position.x)<=0.3&&
                    Mathf.Abs(GameObject.Find("Player").transform.position.y - gameObject.transform.position.y) <= 0.3&&
                    Mathf.Abs(GameObject.Find("Player").transform.position.z - gameObject.transform.position.z) <= 0.3)
                {
                    Debug.Log("same pos!");
                    //GameObject.Find("Player").transform.position = gameObject.transform.position;
                    //Debug.Log("player(pos) :" + GameObject.Find("Player").transform.position + "\arvpoint(pos) :" + gameObject.transform.position);
                    arvdone = true;
                    dirflag = false;
                }
            }
            
            
            if (arvdone)    //캐릭터 이동 후 회전
            {
                //플레이어와 원하는 각도 확인용
                //Debug.Log("player(rot) :" + GameObject.Find("Player").transform.eulerAngles + "\arvpoint(rot) :" + gameObject.transform.eulerAngles);

                //float turnamount = GameObject.Find("Player").transform.rotation.y - gameObject.transform.rotation.y;

                //GameObject.Find("Player").transform.Rotate(0, turnamount * rotspeed, 0);


                
                //Slerp회전
                //플레이어를 원하는 각도까지 회전
                GameObject.Find("Player").transform.rotation = Quaternion.Slerp(GameObject.Find("Player").transform.rotation, 
                    gameObject.transform.rotation, rotspeed * Time.deltaTime);
                

                //플레이어 각도=원하는 각도의 절대값이 3보다 작을 경우 회전 끝!
                if (Mathf.Abs(GameObject.Find("Player").transform.eulerAngles.y - gameObject.transform.eulerAngles.y) <= 3)
                {
                    Debug.Log("same rot!");   //거의 같은 각도예요!
                    flag = false;
                }
            }

            //카메라를 따라가는 콜라이더
            float distanceFromFloor = Vector3.Dot(cameraTransform.localPosition, Vector3.up);
            capsuleCollider.height = Mathf.Max(capsuleCollider.radius, distanceFromFloor);

            capsuleCollider.center = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;

            //기타 카메라
            SnapTurn.localPosition = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;
            NonBodyColider.localPosition = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !flag)
        {
            Debug.Log("arrival point: " + gameObject.name);
            GameObject.Find("Player").GetComponent<Rigidbody>().useGravity=false;
            GameObject.Find("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            GameObject.Find("Player").GetComponent<PlayerMove>().StopPlayer();  //컨트롤러 비활성화
            temptrans = GameObject.Find("Player").transform;
            dirflag = true;
            flag = true;
        }
    }

    public void ReARVPoint()
    {
        flag = false;
        dirflag = false;
        arvdone = false;
    }
}
