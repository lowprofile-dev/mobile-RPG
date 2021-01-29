////////////////////////////////////////////////////
/*
    File Wand.cs
    class Wand

    담당자 : 김의겸
    부 담당자 : 이신홍
*/
////////////////////////////////////////////////////

using UnityEngine;

/// <summary>
/// Weapon을 상속하여 Wand의 데이터와 스킬을 구현해놓은 클래스
/// </summary>
public class Wand : Weapon
{
    // Start is called before the first frame update
    public Wand()
    {
        name = "wand";

        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;

        outfitGrade = 0;

        masteryLevel = MasteryManager.Instance.currentMastery.currentWandMasteryLevel;

        attackDamage = 1;
        skillSpeed = 0;

        skillACoolSave = skillACool = 7;
        skillBCoolSave = skillBCool = 8;
        skillCCoolSave = skillCCool = 11;

        exp = MasteryManager.Instance.currentMastery.currentWandMasteryExp;
        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Wand Animator");

        InitSkillData();
    }


    /// <summary>
    /// 스킬 관련 초기화
    /// </summary>
    public void InitSkillData()
    {
        skillBRelease = MasteryManager.Instance.currentMastery.currentWandSkillBReleased;
        skillCRelease = MasteryManager.Instance.currentMastery.currentWandSkillCReleased;
        
        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;

        // 스킬 쿨타임
        skillACool = 7;
        skillBCool = 9;
        skillCCool = 12;

        skillLevel[0] = MasteryManager.Instance.weaponSkillLevel[3].autoAttackLevel;
        skillLevel[1] = MasteryManager.Instance.weaponSkillLevel[3].skillALevel;
        skillLevel[2] = MasteryManager.Instance.weaponSkillLevel[3].skillBLevel;
        skillLevel[3] = MasteryManager.Instance.weaponSkillLevel[3].skillCLevel;

        // 스킬 프리팹
        AttackEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Attack Effect 1");
        AttackEffect2 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Attack Effect 2");
        AttackEffect3 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Attack Effect 3");
        SkillAEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Skill A");
        SkillBEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Skill C");
    }

    
    public override GameObject SkillA()
    {
        // 어둠 구체를 생성하여 공격
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 1", 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillAEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[3].skillALevel);
        atk.PlayAttackTimer(1.4f);
        atk.OnLoad();

        return SkillAEffect;
    }

    public override GameObject SkillB()
    {
        // 빠른 다중타격 공격
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillBEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[3].skillBLevel);
        atk.PlayAttackTimer(1.0f);
        atk.OnLoad();

        return SkillBEffect;
    }

    public override GameObject SkillC()
    {
        // 제자리에 얼음 벽 생성하여 공격
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 3 Ground", 0.6f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 3 Ice", 0.6f);

        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillCEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[3].skillCLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return SkillCEffect;
    }

    public override GameObject Attack3()
    {
        // 공격 콤보 3
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Attack " + UnityEngine.Random.Range(1, 6), 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect3).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[3].autoAttackLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack2()
    {
        // 공격 콤보 2
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Attack " + UnityEngine.Random.Range(1, 6), 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect2).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[3].autoAttackLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack()
    {
        // 공격 콤보 1
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Attack " + UnityEngine.Random.Range(1, 6), 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject, MasteryManager.Instance.weaponSkillLevel[3].autoAttackLevel);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }
}
