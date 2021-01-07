using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVReader;

public class Sword : Weapon
{
    // Start is called before the first frame update
    public Sword()
    {
        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;
        outfitGrade = 0;
        masteryLevel=1;

        skillBRelease = false;
        skillCRelease = false;
        
        skillALevel = 0;
        skillBLevel = 0;
        skillCLevel = 0;
        attackLevel = 1;

        attackDamage= 1;
        magicDamage = 0;
        skillSpeed = 0;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;
                  
        skillACool = 0;
        skillBCool = 0;
        skillCCool = 0;

        AttackEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Sword Attack");
        SkillAEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Sword Skill A");
        SkillBEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Sword Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Sword Skill C");
       
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
        //Player.Instance.skillPoint.position = Player.Instance.firePoint.position + new Vector3(0f, 0f, 0f);
        //Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;
        //Player.Instance.skillPoint.Rotate(new Vector3(0f, 0f, -50f));
        //attackDamage = StatusManager.Instance.finalStatus.attackDamage + (StatusManager.Instance.finalStatus.attackDamage * (float)(masteryLevel * 0.02));
        //hitStun = 0.5f;
        //hitFail = 0.5f;
        //hitRigid += (float)(masteryLevel * 0.02);
        //return AttackEffect;

        GameObject obj = ObjectPoolManager.Instance.GetObject(AttackEffect);
        obj.transform.SetParent(Player.Instance.transform);
        obj.transform.position = Player.Instance.skillPoint.position;

        PlayerAttack atk = obj.GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.gameObject);
        atk.PlayAttackTimer(0.02f);

        return null;
        
    }
}
