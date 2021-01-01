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
    public void SetValue(float value)
    {
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(value.ToString());
    }
}
