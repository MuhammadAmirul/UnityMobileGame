using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackRotationJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    //private Image bgImg;
    //private Image joystickImg;

    //public Vector3 InputDirection { set; get; }

    [HideInInspector]
    public bool Pressed;

    void Start()
    {
        //bgImg = GetComponent<Image>();
        //joystickImg = transform.GetChild(0).GetComponent<Image>();
    }


    public virtual void OnDrag(PointerEventData ped)
    {
        /*Vector2 pos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            //inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            //inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputDirection = new Vector3(x, 0, y);
            InputDirection = (InputDirection.magnitude > 1) ? InputDirection.normalized : InputDirection;

            joystickImg.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (bgImg.rectTransform.sizeDelta.x / 3), InputDirection.z * (bgImg.rectTransform.sizeDelta.y / 3));
        }*/
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        //OnDrag(ped);
        Pressed = true;
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        //InputDirection = Vector3.zero;
        //joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        Pressed = false;
    }

    /*public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
        {
            return inputVector.z;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }*/
}
