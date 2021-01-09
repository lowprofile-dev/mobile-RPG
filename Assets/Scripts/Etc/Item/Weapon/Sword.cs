using UnityEngine;

public class Sword : Weapon
{
    // Start is called before the first frame update
    public Sword()
    {
        hitStun = 0.5f;
        hitRigid = 0.5f;
        hitFail = 0.5f;
        outfitGrade = 0;
        masteryLevel = 1;

        skillBRelease = false;
        skillCRelease = false;

        skillLevel[0] = 1;
        skillLevel[1] = 1;
        skillLevel[2] = 0;
        skillLevel[3] = 0;

        attackDamage = 1;
        magicDamage = 0;
        skillSpeed = 0;

        skillACoef = 0;
        skillBCoef = 0;
        skillCCoef = 0;

        skillACool = 5;
        skillBCool = 6;
        skillCCool = 7;

        AttackEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 1");
        AttackEffect2 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 2");
        AttackEffect3 = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Attack Effect 3");
        SkillAEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill 1 Heal");
        SkillBEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill B");
        SkillCEffect = Resources.Load<GameObject>("Prefab/Effect/SkillEffect/Player/Attacks/Sword Skill C");

        WeaponAnimation = Resources.Load<RuntimeAnimatorController>("Animation/Animator/Player/Sword Animator");
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
        PlayerAttack atk = ObjectPoolManager.Instance.GetObject(SkillCEffect).GetComponent<PlayerAttack>();
        atk.SetParent(Player.Instance.skillPoint.gameObject);
        atk.PlayAttackTimer(0.4f);
        atk.OnLoad();

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
