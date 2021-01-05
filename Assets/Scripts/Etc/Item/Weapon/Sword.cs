using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVReader;

public class Sword : Weapon
{
    // Start is called before the first frame update
    public Sword(WeaponData swordData)
    {
        id = swordData.id;
        weaponModelIndex = swordData.weaponModelIndex;
        weaponType = swordData.weaponType;

        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;

        attackDamage = swordData.attackDamage;
        attackSpeed = swordData.attackSpeed;
        magicDamage = swordData.magicDamage;

        skillACoef = swordData.skillACoef;
        skillBCoef = swordData.skillBCoef;
        skillCCoef = swordData.skillCCoef;

        skillACool = swordData.skillACool;
        skillBCool = swordData.skillBCool;
        skillCCool = swordData.skillCCool;

        masteryLevel = 1;
        skillBRelease = false;
        skillCRelease = false;

        AttackEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Attack");
        SkillAEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Skill A");
        SkillBEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/PlayerEffect/Sword Skill C");
 
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Sword Animator");
    }

    // Update is called once per frame
    public override void Update()
    {
        OutfitGradeCheck();
        if(Input.GetKeyDown(KeyCode.P))
        {
            masteryLevel++;
        }
    }

    public override GameObject SkillA()
    {
        Player.Instance.skillPoint.position = new Vector3(0f, -1f, 0f) + Player.Instance.gameObject.transform.position;
        Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;
        attackDamage = StatusManager.Instance.finalStatus.attackDamage + (StatusManager.Instance.finalStatus.attackDamage * (float)((masteryLevel / 3) * 0.05)) + skillACoef;
        Player.Instance.Hp += attackDamage * (0.15f + (float)(masteryLevel * 0.01));
        hitFail = 0.5f;
        hitStun = 0.5f;
        hitRigid = 0.5f;
        return SkillAEffect;
    }
    public override GameObject SkillB()
    {
        dir = Player.Instance.firePoint.position - Player.Instance.transform.position;
        dir = dir.normalized;
        Player.Instance.skillPoint.position = dir * 3f + Player.Instance.transform.position + new Vector3(0f, -1f, 0f);
        Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;
        attackDamage = 1.3f*StatusManager.Instance.finalStatus.attackDamage + (StatusManager.Instance.finalStatus.attackDamage * (float)((masteryLevel/5) * 0.15)) + skillBCoef;
        hitFail = 0.8f;
        hitStun = 0.5f;
        hitRigid = 0.5f;
        if (masteryLevel % 5 == 0) skillBCool -= 0.5f;

        return SkillBEffect;
    }
    public override GameObject SkillC()
    {
        dir = Player.Instance.firePoint.position - Player.Instance.transform.position;
        dir = dir.normalized;
        Player.Instance.skillPoint.position = dir * 8f + Player.Instance.transform.position + new Vector3(0f,-1f,0f);
        Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;
        attackDamage = 2*StatusManager.Instance.finalStatus.attackDamage + (StatusManager.Instance.finalStatus.attackDamage * (float)((masteryLevel / 10) * 0.5f)) + skillCCoef;

        return SkillCEffect;
    }
    public override GameObject Attack()
    {
        Player.Instance.skillPoint.position = Player.Instance.firePoint.position + new Vector3(-0.2f, -0.5f, 0f);
        Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;
        Player.Instance.skillPoint.Rotate(new Vector3(0f, 0f, -50f));
        attackDamage = StatusManager.Instance.finalStatus.attackDamage + (StatusManager.Instance.finalStatus.attackDamage * (float)(masteryLevel * 0.02));
        hitStun = 0.5f;
        hitFail = 0.5f;
        hitRigid += (float)(masteryLevel * 0.02);
        return AttackEffect;
    }
}
