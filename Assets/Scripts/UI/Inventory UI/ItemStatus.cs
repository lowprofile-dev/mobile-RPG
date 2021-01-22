/*
    File ItemDetailNew.cs
    class ItemDetailNew
    
    담당자 : 김기정
    부 담당자 : 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemStatus : MonoBehaviour
{
    public void SetPanel(string statusType)
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(statusType);
    }
    public void SetValue(string value)
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(value);
    }

    public void SetData(string dataType, string value)
    {
        SetPanel(dataType);
        SetValue(value);
    }
}
