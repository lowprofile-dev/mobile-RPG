using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float GetCoolTime()
    {
        switch (gameObject.name)
        {
            case "SkillA": return WeaponManager.Instance.GetWeapon().skillACool;
            case "SkillB": return WeaponManager.Instance.GetWeapon().skillBCool;
            case "SkillC": return WeaponManager.Instance.GetWeapon().skillCCool;
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

    public void SkillRelease()
    {
        if (isLock)
        {
            if (button.enabled == false) { }
            else button.enabled = false;
        } 

        if (gameObject.name != "SkillA")
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