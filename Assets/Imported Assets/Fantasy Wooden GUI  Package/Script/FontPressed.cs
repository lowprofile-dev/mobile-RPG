using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FontPressed : MonoBehaviour {

    public Text NormalBtnText;
    public Text PressedBtnText;

    public void OnClickDownBtn()
    {
   
        NormalBtnText.gameObject.SetActive(false);
        PressedBtnText.gameObject.SetActive(true);

    }
    public void OnClickUpBtn()
    {

        NormalBtnText.gameObject.SetActive(true);
        PressedBtnText.gameObject.SetActive(false);
    }
}
