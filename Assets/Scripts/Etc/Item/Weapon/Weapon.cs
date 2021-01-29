using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////
/*
    File Weapon.cs
    class Weapon

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////


public class Weapon
{
    public string name;

    public int outfitGrade;
    public int masteryLevel;

    public bool skillBRelease = false;
    public bool skillCRelease = false;

    public float hitStun;
    public float hitRigid;
    public float hitFail;

    public int[] skillLevel = new int[4];

    public float skillACoef;
    public float skillBCoef;
    public float skillCCoef;

    public float skillACoolSave;
    public float skillBCoolSave;
    public float skillCCoolSave;

    public float skillACool;
    public float skillBCool;
    public float skillCCool;

    public int expMax= 100;
    public int exp =0;

    public float attackDamage;
    public float magicDamage;
    public float skillSpeed;

    private bool levelUp = false;

    public GameObject SkillAEffect;
    public GameObject SkillBEffect;
    public GameObject SkillCEffect;
    public GameObject SkillAEffect2;
    public GameObject SkillBEffect2;
    public GameObject SkillCEffect2;
    public GameObject AttackEffect;
    public GameObject AttackEffect2;
    public GameObject AttackEffect3;

    public Vector3 dir;
    
    public RuntimeAnimatorController WeaponAnimation;

    /// <summary>
    /// 무기의 외형과 레벨이 변경됐는지 체크한다.
    /// </summary>
    public virtual void Update()
    {
        OutfitGradeCheck();
        MasteryLevelUp();
        SKillCoolTime();
    }

    private void SKillCoolTime()
    {
        skillACool = skillACoolSave - StatusManager.Instance.finalStatus.attackCooldown;
        skillBCool = skillBCoolSave - StatusManager.Instance.finalStatus.attackCooldown;
        skillCCool = skillCCoolSave - StatusManager.Instance.finalStatus.attackCooldown;
    }




    /// <summary>
    /// 각 스킬의 이팩트를 반환해주는 함수들
    /// </summary>

    public virtual GameObject SkillA()
    {
        return SkillAEffect;
    }
    public virtual GameObject SkillB()
    {
        return SkillBEffect;

    }
    public virtual GameObject SkillC()
    {
        return SkillCEffect;
    }

    public virtual GameObject SkillA2()
    {
        return SkillAEffect2;
    }

    public virtual GameObject SkillB2()
    {
        return SkillBEffect2;
    }

    public virtual GameObject SkillC2()
    {
        return SkillCEffect2;
    }

    public virtual GameObject Attack3()
    {
        return AttackEffect3;
    }

    public virtual GameObject Attack2()
    {
        return AttackEffect2;
    }

    public virtual GameObject Attack()
    {
        return AttackEffect;
    }

    /// <summary>
    /// 무기의 숙련도 레벨에 따라 외형을 바꿔주는 함수.
    /// </summary>
    public void OutfitGradeCheck()
    {
        if (outfitGrade <= 2)
        {
            if (masteryLevel > Player.Instance.weaponManager.GradeCriteria[outfitGrade + 1])
            {
                outfitGrade++;
            }
        }
    }

    /// <summary>
    /// 무기의 레벨이 올랐는지 체크하며 최신화 해주는 함수 
    /// </summary>
    public void MasteryLevelUp()
    {
        if (exp >= expMax)
        {
            exp = exp - expMax;
            masteryLevel++;
            SkillRelease();
            MasteryManager.Instance.incrementMasteryLevel(this.name);
            levelUp = true;
            SystemPanel.instance.SetText("Level UP !!");
            SystemPanel.instance.FadeOutStart();
            
        }
        else levelUp = false;
    }

    /// <summary>
    /// 레벨에 따른 스킬의 해금을 관리하는 함수
    /// </summary>
    public void SkillRelease()
    {
        if (masteryLevel >= 19 && skillCRelease == false)
        {
            switch (this.name)
            {
                case "sword":
                    MasteryManager.Instance.currentMastery.currentSwordSkillCReleased = true;
                    break;
                case "dagger":
                    MasteryManager.Instance.currentMastery.currentDaggerSkillCReleased = true;
                    break;
                case "blunt":
                    MasteryManager.Instance.currentMastery.currentBluntSkillCReleased = true;
                    break;
                case "wand":
                    MasteryManager.Instance.currentMastery.currentWandSkillCReleased = true;
                    break;
                case "staff":
                    MasteryManager.Instance.currentMastery.currentStaffSkillCReleased = true;
                    break;
            }

            skillCRelease = true;
            MasteryManager.Instance.incrementSkillLevel(this.name, "skillC");
            skillLevel[3] = 1;
        }
        else if (masteryLevel >= 10 && skillBRelease == false)
        {
            switch (this.name)
            {
                case "sword":
                    MasteryManager.Instance.currentMastery.currentSwordSkillBReleased = true;
                    break;
                case "dagger":
                    MasteryManager.Instance.currentMastery.currentDaggerSkillBReleased = true;
                    break;
                case "blunt":
                    MasteryManager.Instance.currentMastery.currentBluntSkillBReleased = true;
                    break;
                case "wand":
                    MasteryManager.Instance.currentMastery.currentWandSkillBReleased = true;
                    break;
                case "staff":
                    MasteryManager.Instance.currentMastery.currentStaffSkillBReleased = true;
                    break;
            }
            skillBRelease = true;
            MasteryManager.Instance.incrementSkillLevel(this.name, "skillB");
            skillLevel[2] = 1;
        }
    }
    /// <summary>
    /// 스킬의 해금 여부를 리턴해주는 함수
    /// </summary>
    /// <returns></returns>
    public bool CheckSkillB()
    {
        return skillBRelease;
    }

    public bool CheckSkillC()
    {
        return skillCRelease;
    }
}
