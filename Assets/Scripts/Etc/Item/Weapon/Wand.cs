using UnityEngine;
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

        skillBRelease = MasteryManager.Instance.currentMastery.currentWandSkillBReleased;
        skillCRelease = MasteryManager.Instance.currentMastery.currentWandSkillCReleased;

        skillLevel[0] = MasteryManager.Instance.weaponSkillLevel[3].autoAttackLevel;
        skillLevel[1] = MasteryManager.Instance.weaponSkillLevel[3].skillALevel;
        skillLevel[2] = MasteryManager.Instance.weaponSkillLevel[3].skillBLevel;
        skillLevel[3] = MasteryManager.Instance.weaponSkillLevel[3].skillCLevel;

        exp = MasteryManager.Instance.currentMastery.currentWandMasteryExp;


        attackDamage = 0;
        magicDamage = 1;
        skillSpeed = 0;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;

        skillACool = 4;
        skillBCool = 6;
        skillCCool = 9;

        AttackEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Attack Effect 1");
        AttackEffect2 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Attack Effect 2");
        AttackEffect3 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Attack Effect 3");
        SkillAEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Skill A");
        SkillBEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Wand Skill C");

        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Wand Animator");

    }

    public override GameObject SkillA()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 1", 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillAEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(1.4f);
        atk.OnLoad();

        return SkillAEffect;
    }

    public override GameObject SkillB()
    {
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillBEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(1.0f);
        atk.OnLoad();

        return SkillBEffect;
    }

    public override GameObject SkillC()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 3 Ground", 0.6f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Skill 3 Ice", 0.6f);

        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillCEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return SkillCEffect;
    }

    public override GameObject Attack3()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Attack " + UnityEngine.Random.Range(1, 6), 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect3).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack2()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Attack " + UnityEngine.Random.Range(1, 6), 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect2).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }

    public override GameObject Attack()
    {
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "SkillEffect/Wand Attack " + UnityEngine.Random.Range(1, 6), 0.6f);
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(AttackEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

        return null;
    }
}
