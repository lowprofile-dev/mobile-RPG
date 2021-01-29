////////////////////////////////////////////////////
/*
    File UIBuffInfo.cs
    class UIBuffInfo
    
    담당자 : 이신홍
    부 담당자 : 

    버프를 표현하는 툴팁 클래스
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuffInfo : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;

    public void SetData(Sprite sprite, string name, string description)
    {
        _img.sprite = sprite;
        _name.text = name;
        _description.text = description;
    }
}
