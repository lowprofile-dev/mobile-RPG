using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeScript : MonoBehaviour
{
    public Image image;
    public Image lockImage;
    public Button button;
    public float coolTime = 10.0f;
    public bool isClicked = false;
    float leftTime = 0f;
    public bool isLock;

    private void Start()
    {
        if (gameObject.name == "SkillA")
        {
            isLock = false;
            if(WeaponManager.Instance != null) coolTime = WeaponManager.Instance.GetWeapon().skillACool;
        }
        else if (gameObject.name == "SkillB")
        {
            isLock = true;
            button.enabled = false;
            if (WeaponManager.Instance != null) coolTime = WeaponManager.Instance.GetWeapon().skillBCool;
            lockImage.enabled = true;

        }
        else if (gameObject.name == "SkillC")
        {
            isLock = true;
            button.enabled = false;
            if (WeaponManager.Instance != null) coolTime = WeaponManager.Instance.GetWeapon().skillCCool;
            lockImage.enabled = true;
        }
    }

    void Update()
    {
        if (Player.Instance != null && Player.Instance.weaponChanged) Start();

        SkillRelease();

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
                if (image)
                    image.fillAmount = ratio;
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