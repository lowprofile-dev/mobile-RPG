using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] string name;
    //[SerializeField] GameObject panel;
    //[SerializeField] TextMeshProUGUI text;
    SystemPanel systemPanel;
    Button button;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        systemPanel = SystemPanel.instance;
    }

    // Update is called once per frame

    public void onbuttonClick()
    {
        if (name == "SWORD" || name == "WAND")
        {

            systemPanel.SetText(name + " 착용 !");
            WeaponManager.Instance.SetWeapon(name);
        }
        else
        {
            systemPanel.SetText("아직 준비 중입니다.");
        }
        systemPanel.FadeOutStart();
    }
}
