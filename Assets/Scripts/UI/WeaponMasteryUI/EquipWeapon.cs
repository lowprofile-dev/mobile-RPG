using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

////////////////////////////////////////////////////
/*
    File EquipWeapon.cs
    class EquipWeapon

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////


public class EquipWeapon : MonoBehaviour
{
    [SerializeField] string name;
    SystemPanel systemPanel;
    Button button;
    Image image;

    void Start()
    {
        button = GetComponent<Button>();
        systemPanel = SystemPanel.instance;
    }

    /// <summary>
    /// 숙련도 창의 장착 버튼을 클릭하였을 경우,
    /// 무기가 장착하고, 시스템 패널에 정보를 출력하는 함수
    /// </summary>
    public void onbuttonClick()
    {
        if (name == "SWORD" || name == "WAND")
        {
            SoundManager.Instance.PlayEffect(SoundType.UI, "UI/WeaponEquip", 0.8f);
            systemPanel.SetText(name + " 착용 !");
            WeaponManager.Instance.SetWeapon(name);
            systemPanel.FadeOutStart();
        }

        else
        {
            SoundManager.Instance.PlayEffect(SoundType.UI, "UI/Locker", 0.8f);
        }
    }
}
