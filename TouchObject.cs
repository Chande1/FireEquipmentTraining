using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TouchObject : MonoBehaviour
{
    [SerializeField] bool active;
    
    private Interactable interactable;  //상호작용
    
    void Start()
    {
        interactable = GetComponent<Interactable>();    //상호작용 컴포넌트를 적용
        active = false;
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabTypes = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);  //이 오브젝트를 잡고 있는지에 대한 bool값

        //물체를 잡고 있지않다면
        if (interactable.attachedToHand == null && grabTypes != GrabTypes.None)
        {
            Debug.Log(gameObject + ":grab");
            active = true;
        }
        else if (isGrabEnding)   //물체를 잡고 있다고 놓았을때
        {
            //Debug.Log("bye");
        }
    }

    public bool ActiveObject()
    {
        if (active)
        {
            return true;
        }
        else
            return false;
    }

    public void InActiveObject()
    {
        if (active)
            active = false;
    }
}
