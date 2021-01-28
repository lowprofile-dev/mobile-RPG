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
