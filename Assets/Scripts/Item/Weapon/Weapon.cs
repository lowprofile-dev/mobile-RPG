using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    // Start is called before the first frame update
    private Weapon _currentWeapon;
    public Dictionary<string, Weapon> _weaponDic;

    public float damage;
    public float speed;

    public readonly int[] GradeCriteria = { 0, 9, 18, 25 };
    public readonly int[] SwordNumber = { 21, 37, 31, 34 };
    public readonly int[] DaggerNumber = { 1, 10, 23, 18 };
    public readonly int[] GreatSwordNumber = { 29, 29, 56, 56 };
    public readonly int[] BluntNumber = { 16, 39, 17, 36 };
    public readonly int[] StaffNumber = { 47, 28, 35, 26 };
    public readonly int[] WandNumber = { 4, 14, 13, 11 };

    public int masteryLevel ;
    public int outfitGrade ;

    public GameObject SkillAEffect;
    public GameObject SkillBEffect;
    public GameObject SkillCEffect;
    public GameObject AttackEffect;
    public Transform EffectPosition;

    public Animator WeaponAnimation;

    public Weapon()
    {
        _currentWeapon = null;
        itemName = null;
        _weaponDic = new Dictionary<string, Weapon>();
    }

    public virtual void Update()
    {
     
    }

    public virtual void SkillA()
    {

    }
    public virtual void SkillB()
    {

    }
    public virtual void SkillC()
    {

    }
    public virtual void Attack()
    {

    }

    public void OutfitGradeCheck()
    {
        if (outfitGrade <= 3)
        {
            if (masteryLevel > GradeCriteria[outfitGrade + 1])
            {
                outfitGrade++;
            }
        }
    }

    public void AddWeapon(string weaponName, Weapon weapon)
    {
        _weaponDic.Add(weaponName, weapon);
    }

    public void SetWeapon(string weaponName)
    {
        if (_currentWeapon == _weaponDic[weaponName]) return;
        itemName = weaponName;
        _currentWeapon = _weaponDic[itemName];
    }

    public string GetWeaponName()
    {
        return itemName;
    }
    public Weapon GetWeapon()
    {
        return _currentWeapon;
    }

}
