using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerContr : MonoBehaviour
{
    [SerializeField] GameObject snapturn;
    [SerializeField] GameObject teleport;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform SnapTurn;
    [SerializeField] Transform NonBodyColider;
    [SerializeField] CapsuleCollider capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {

        if (GameObject.Find("Snap Turn")!=null)
        {
            snapturn = GameObject.Find("Snap Turn");
        }
        if(GameObject.Find("Teleporting") !=null)
        {
            teleport = GameObject.Find("Teleporting");
        }
    }

    void Update()
    {
        //카메라를 따라가는 콜라이더
        float distanceFromFloor = Vector3.Dot(cameraTransform.localPosition, Vector3.up);
        //capsuleCollider.height = Mathf.Max(capsuleCollider.radius, distanceFromFloor);

        capsuleCollider.center = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;

        //기타 카메라
        SnapTurn.localPosition = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;
        //NonBodyColider.localPosition = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;
    }

    public void StopContr()
    {
        snapturn.SetActive(false);
        teleport.GetComponent<Teleport>().enabled = false;
    }

    public void PlayContr()
    {
        snapturn.SetActive(true);
        teleport.GetComponent<Teleport>().enabled = true;
    }
}
