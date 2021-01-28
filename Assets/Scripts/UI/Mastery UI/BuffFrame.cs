using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BuffFrame : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private UIBuffInfo buffInfo;
    private Sprite _mySprite;
    private string _myDescription;
    private string _myName;

    private void Start()
    {
        buffInfo = UIManager.Instance.playerUIView.buffInfo;
    }
    
    public void SetData(Sprite sprite, string des, string name)
    {
        _mySprite = sprite;
        _myDescription = des;
        _myName = name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buffInfo.gameObject.SetActive(true);
        buffInfo.transform.position = eventData.position + new Vector2(5, 5);
        buffInfo.SetData(_mySprite, _myName, _myDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buffInfo.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buffInfo.gameObject.SetActive(false);
    }
}
