using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;

    private Image joystickImage;
    private Image joystickBGImage;
    public Vector2 joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickBackgroundRadius;
    private float joystickRadius;

    ////private bool isCheckTouch = true;
    //private readonly float timeEnable = 0.5f;

    private int? pointerId = null;

    private PlayerController player;

    // Start is called before the first frame update
    private void Start()
    {
        player = PlayerController.Instance;
        joystickOriginalPos = joystickBG.transform.position;
        RectTransform joystickBGRect = joystickBG.GetComponent<RectTransform>();
        joystickBackgroundRadius = joystickBGRect.sizeDelta.y / 2 * joystickBGRect.lossyScale.y;

        RectTransform joystickRect = joystick.GetComponent<RectTransform>();
        joystickRadius = joystickRect.sizeDelta.y / 2 * joystickBGRect.lossyScale.y;

        joystickImage = joystick.GetComponent<Image>();
        joystickBGImage = joystickBG.GetComponent<Image>();
        joyStickVisible(false);
    }

    public void PointerDown(BaseEventData baseEventData)
    {
        //GameController.Instance.isJoyStickActive = true;
        //if (!player.AllowMove)
        //{
        //    CameraController.Instance.SetTarget(player.transform);
        //    player.AllowMove = true;
        //    UIController.Instance.SetActiveStartGameText(false);
        //    UIController.Instance.SetActiveTopUI(true);
        //    UIController.Instance.SetActiveItemsButton(true);
        //}

        if (pointerId == null)
        {
            PointerEventData pointerEventData = baseEventData as PointerEventData;
            pointerId = pointerEventData.pointerId;
            joyStickVisible(true);
            joystick.transform.position = pointerEventData.position;
            joystickBG.transform.position = pointerEventData.position;
            joystickTouchPos = pointerEventData.position;

            StopAllCoroutines();
        }
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (pointerEventData.pointerId == pointerId)
        {
            joyStickVisible(true);
            Vector2 dragPos = pointerEventData.position;
            joystickVec = (dragPos - joystickTouchPos).normalized;
            float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

            if (joystickDist < joystickBackgroundRadius - joystickRadius)
            {
                joystick.transform.position = joystickTouchPos + joystickVec * joystickDist;
            }
            else
            {
                joystick.transform.position = joystickTouchPos + joystickVec * (joystickBackgroundRadius - joystickRadius);
            }

            float speed = Vector2.Distance(joystick.transform.position, joystickTouchPos) / (joystickBackgroundRadius - joystickRadius);
            Vector2 direction = (Vector2)joystick.transform.position - joystickTouchPos;
            direction = direction / joystickBackgroundRadius;

            player.Move(speed, direction);

            StopAllCoroutines();
        }
    }

    public void PointerUp(BaseEventData baseEventData)
    {
        //GameController.Instance.isJoyStickActive = false;

        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (pointerEventData.pointerId == pointerId)
        {
            pointerId = null;
            joyStickVisible(true);
            joystickVec = Vector2.zero;
            joystick.transform.position = joystickOriginalPos;
            joystickBG.transform.position = joystickOriginalPos;
            StopAllCoroutines();
        }
        player.Stop();
        joyStickVisible(false);
        ResetJoyStick();
    }

    public void EndDrag(BaseEventData baseEventData)
    {
        joyStickVisible(false);
        ResetJoyStick();
        player.Stop();
        //StartCoroutine(EnableJoystick(timeEnable));
    }

    public void ResetJoyStick()
    {
        pointerId = null;
        joyStickVisible(false);
        joystickVec = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }

    private IEnumerator EnableJoystick(float time)
    {
        yield return new WaitForSeconds(time);
        joyStickVisible(false);
    }

    private void joyStickVisible(bool isvisible)
    {
        if (isvisible)
        {
            joystickImage.color = new Color(1, 1, 1, 1f);
            joystickBGImage.color = new Color(1, 1, 1, 0.3f);
        }
        else
        {
            joystickImage.color = new Color(1, 1, 1, 0);
            joystickBGImage.color = new Color(1, 1, 1, 0);
        }
    }
}
