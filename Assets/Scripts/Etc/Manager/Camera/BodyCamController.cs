using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BodyCamController : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    bool isButtonDown;
    [SerializeField] GameObject bodyAvata;

    [SerializeField] float rotateSpeed = 10f;

    private void OnEnable()
    {
        Invoke("LoadAvata", 1f);
    }
    private void LoadAvata()
    {
        bodyAvata = Player.Instance.FaceCam.BodyAvata;
    }

    private void Update()
    {
        if (isButtonDown && bodyAvata != null)
        {

            Vector3 rot = bodyAvata.transform.rotation.eulerAngles;
#if UNITY_EDITOR
            rot.y -= Input.GetAxis("Mouse X") * rotateSpeed;
#endif

#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            rot.y += Input.touches[0].deltaPosition.x * rotateSpeed;
#endif
            Quaternion q = Quaternion.Euler(rot);
            bodyAvata.transform.rotation = Quaternion.Slerp(bodyAvata.transform.rotation, q, 2f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonDown = false;
    }
}
