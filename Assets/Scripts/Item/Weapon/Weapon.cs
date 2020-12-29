using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public float damage;
    public float speed;
    public string name;

    public int masteryLevel ;
    public int outfitGrade ;

    public GameObject SkillAEffect;
    public GameObject SkillBEffect;
    public GameObject SkillCEffect;
    public GameObject AttackEffect;
    public Transform EffectPosition;

    public Animator WeaponAnimation;

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
            if (masteryLevel > Player.Instance.weaponManager.GradeCriteria[outfitGrade + 1])
            {
                outfitGrade++;
            }
        }
    }

    //public void AddWeapon(string weaponName, Weapon weapon)
    //{
    //    _weaponDic.Add(weaponName, weapon);
    //}
    //
    //public void SetWeapon(string weaponName)
    //{
    //    if (_currentWeapon == _weaponDic[weaponName]) return;
    //    itemName = weaponName;
    //    _currentWeapon = _weaponDic[itemName];
    //}
    //
    //public string GetWeaponName()
    //{
    //    return itemName;
    //}
    //public Weapon GetWeapon()
    //{
    //    return _currentWeapon;
    //}

}
