﻿using System.Collections;
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

        attackLevel = 1;
        skillALevel = 1;
        skillBLevel = 0;
        skillCLevel = 0;

        attackDamage= 1;
        magicDamage = 0;
        skillSpeed = 0;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;
                  
        skillACool = 0;
        skillBCool = 0;
        skillCCool = 0;

        AttackEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 1");
        AttackEffect2 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 2");
        AttackEffect3 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 3");
        SkillAEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill 1 Heal");
        SkillBEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill C");
       
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Sword Animator");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    masteryLevel++;
        //}
    }

    public override GameObject SkillA()
    {
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillAEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();
        
        return SkillAEffect;
    }

    public override GameObject SkillB()
    {
        Player.Instance.RushEnter();

        return SkillBEffect;
    }

    public override GameObject SkillB2()
    {
        CameraManager.Instance.ShakeCamera(10, 3, 0.25f); // 카메라 흔들기 연출
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillBEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

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

    public override GameObject Attack3()
    {
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect3).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack2()
    {
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect2).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack()
    {
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();
        
        return null;
        
    }
}
