using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    // Start is called before the first frame update
    public Sword()
    {        
        damage = 10f;
        speed = 1.5f;
        masteryLevel = 1;
        outfitGrade = 0;

        coolTimeA = 4f;
        coolTimeB = 6f;
        coolTimeC = 7f;

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

        return SkillAEffect;
    }
    public override GameObject SkillB()
    {
        dir = Player.Instance.firePoint.position - Player.Instance.transform.position;
        dir = dir.normalized;
        Player.Instance.skillPoint.position = dir * 3f + Player.Instance.transform.position + new Vector3(0f, -1f, 0f);
        Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;

        return SkillBEffect;
    }
    public override GameObject SkillC()
    {
        dir = Player.Instance.firePoint.position - Player.Instance.transform.position;
        dir = dir.normalized;
        Player.Instance.skillPoint.position = dir * 8f + Player.Instance.transform.position + new Vector3(0f,-1f,0f);
        Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;
        return SkillCEffect;
    }
    public override GameObject Attack()
    {
        Player.Instance.skillPoint.position = Player.Instance.firePoint.position + new Vector3(-0.2f, -0.5f, 0f);
        Player.Instance.skillPoint.rotation = Player.Instance.transform.rotation;
        Player.Instance.skillPoint.Rotate(new Vector3(0f, 0f, -50f));
        return AttackEffect;
    }
}
