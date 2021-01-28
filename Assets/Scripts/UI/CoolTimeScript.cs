using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
////////////////////////////////////////////////////
/*
    File CoolTimeScript.cs
    class CoolTimeScript

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////

public class CoolTimeScript : MonoBehaviour
{
    public Image image;
    public Image lockImage;
    public Image skillImage;

    public Button button;
    public bool isClicked = false;
    float leftTime = 0f;
    public bool isLock;

    private void Start()
    {
        if (gameObject.name == "SkillA")
        {
            isLock = false;
        }

        else if (gameObject.name == "SkillB")
        {
            isLock = true;
            button.enabled = false;
            lockImage.enabled = true;
        }

        else if (gameObject.name == "SkillC")
        {
            isLock = true;
            button.enabled = false;
            lockImage.enabled = true;
        }
    }
    /// <summary>
    /// 무기 매너지에 있는 무기들의 쿨타임 시간을 가져온다.
    /// </summary>
    /// <returns></returns>
    public float GetCoolTime()
    {
        if (WeaponManager.Instance.GetWeapon() != null)
        {
            switch (gameObject.name)
            {

                case "SkillA": return WeaponManager.Instance.GetWeapon().skillACool;
                case "SkillB": return WeaponManager.Instance.GetWeapon().skillBCool;
                case "SkillC": return WeaponManager.Instance.GetWeapon().skillCCool;
            }
        }
        return 0;
    }

    void Update()
    {
        if (Player.Instance != null && Player.Instance.weaponChanged) Start();

        SkillRelease();


        float coolTime = GetCoolTime();
        if (isClicked)
            if (leftTime >= 0)
            {
                leftTime += Time.deltaTime;
                if (leftTime > coolTime)
                {
                    leftTime = coolTime;
                    if (button)
                        button.enabled = true;
                    isClicked = true;
                }

                float ratio = (leftTime / coolTime);
                if (skillImage) skillImage.fillAmount = ratio;
            }
    }

    /// <summary>
    /// 정해진 시간으로 쿨타임을 시작하는 함수
    /// </summary>
    public void StartCoolTime()
    {
        if (isLock) return;
        else
        {
            leftTime = 0f;
            isClicked = true;
            if (button)
                button.enabled = false;
        }
    }
    /// <summary>
    /// 각 스킬의 해금이 풀렸는지 체크하고 이미지와 버튼을 관리한다.
    /// </summary>
    public void SkillRelease()
    {
        if (isLock)
        {
            if (button.enabled == false) { }
            else button.enabled = false;
        } 

        if (gameObject.name != "SkillA")
        {
            if (WeaponManager.Instance.GetWeapon() != null)
            {
                if (gameObject.name == "SkillB" && WeaponManager.Instance.GetWeapon().CheckSkillB() && isLock == true)
                {
                    isLock = false;
                    lockImage.enabled = false;
                    button.enabled = true;
                }
                else if (gameObject.name == "SkillB" && !WeaponManager.Instance.GetWeapon().CheckSkillB() && isLock == false)
                {
                    isLock = true;
                    lockImage.enabled = true;
                    button.enabled = false;
                }

                if (gameObject.name == "SkillC" && WeaponManager.Instance.GetWeapon().CheckSkillC() && isLock == true)
                {
                    isLock = false;
                    lockImage.enabled = false;
                    button.enabled = true;
                }
                else if (gameObject.name == "SkillC" && !WeaponManager.Instance.GetWeapon().CheckSkillC() && isLock == false)
                {
                    isLock = true;
                    lockImage.enabled = true;
                    button.enabled = false;
                }
            }

        }
      
    }
}