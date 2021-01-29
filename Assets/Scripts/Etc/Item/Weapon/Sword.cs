////////////////////////////////////////////////////
/*
    File Sword.cs
    class Sword

    담당자 : 김의겸
    부 담당자 : 이신홍
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVReader;

/// <summary>
/// Weapon을 상속하여 Sword의 데이터와 스킬을 구현해놓은 클래스
/// </summary>
public class Sword : Weapon
{
    public Sword()
    {
        name = "sword";

        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;

        outfitGrade = 0;

        masteryLevel = MasteryManager.Instance.currentMastery.currentSwordMasteryLevel;

        skillLevel[0] = MasteryManager.Instance.weaponSkillLevel[0].autoAttackLevel;
        skillLevel[1] = MasteryManager.Instance.weaponSkillLevel[0].skillALevel;
        skillLevel[2] = MasteryManager.Instance.weaponSkillLevel[0].skillBLevel;
        skillLevel[3] = MasteryManager.Instance.weaponSkillLevel[0].skillCLevel;

        attackDamage = 1;
        skillSpeed = 0;

        exp = MasteryManager.Instance.currentMastery.currentSwordMasteryExp;
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Sword Animator");

        InitSkillData();
    }

    /// <summary>
    /// 스킬 관련 초기화
    /// </summary>
    public void InitSkillData()
    {
        skillBRelease = MasteryManager.Instance.currentMastery.currentSwordSkillBReleased;
        skillCRelease = MasteryManager.Instance.currentMastery.currentSwordSkillCReleased;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;

        // 스킬 쿨타임
        skillACoolSave = skillACool = 7;
        skillBCoolSave = skillBCool = 8;
        skillCCoolSave = skillCCool = 11;

        // 스킬 프리팹
        AttackEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 1");
        AttackEffect2 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 2");
        AttackEffect3 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 3");
        SkillAEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill 1 Heal");
        SkillBEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill C");
    }

    public override void Update()
    {
        base.Update();
    }

    public override GameObject SkillA()
    {
        // 체력 회복 스킬
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillAEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[0].skillALevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();
        
        return SkillAEffect;
    }

    public override GameObject SkillB()
    {
        // 돌진 스킬. 돌진이 끝났을때 SkillB2가 자동으로 실행된다.
        Player.Instance.RushEnter();

        return SkillBEffect;
    }

    public override GameObject SkillB2()
    {
        // 부딪힌 자리에서 공격 실행
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 2 Swing", 0.55f);
        CameraManager.Instance.ShakeCamera(10, 3, 0.25f); // 카메라 흔들기 연출
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillBEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[0].skillBLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return SkillBEffect;
    }

    public override GameObject SkillC()
    {
        // 검을 소환하여 큰 데미지
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 3 Swing", 0.55f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Skill 3 Holy", 0.55f);

        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillCEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[0].skillCLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return SkillCEffect;
    }

    public override GameObject Attack3()
    {
        // 공격 콤보 3
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Attack 3", 0.5f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect3).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[0].autoAttackLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack2()
    {
        // 공격 콤보 2
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Attack 2", 0.5f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect2).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[0].autoAttackLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack()
    {
        // 공격 콤보 1
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Sword Attack 1", 0.5f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[0].autoAttackLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();
        
        return null;
    }
}
