using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SimpleAttach : MonoBehaviour
{
    private Interactable interactable;  //상호작용

    void Start()
    {
        interactable = GetComponent<Interactable>();    //상호작용 컴포넌트를 적용
    }

    private void OnHandHoverBegin(Hand hand)
    {
        hand.ShowGrabHint();    //내가 현재 잡고 있는지 확인
    }

    private void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();    //내가 현재 잡았는지를 숨김
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabTypes = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);  //이 오브젝트를 잡고 있는지에 대한 bool값

        /*물체를 잡고 있지않다면*/
        if(interactable.attachedToHand==null&&grabTypes!=GrabTypes.None)
        {
            hand.AttachObject(gameObject, grabTypes);
            hand.HoverLock(interactable);
            hand.HideGrabHint();
        }
        else if(isGrabEnding)   //물체를 잡고 있다고 놓았을때
        {
            hand.DetachObject(gameObject);  //물체를 손에서 분리
            hand.HoverUnlock(interactable);
        }
    }
}
