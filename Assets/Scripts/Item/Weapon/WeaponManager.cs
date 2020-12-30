using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponManager : SingletonBase<WeaponManager>
{
    private Weapon _currentWeapon;
    private string _currentWeaponName;
    public Dictionary<string, Weapon> _weaponDic;

    public readonly int[] GradeCriteria = { 0, 9, 18, 25 };
    public readonly int[] SwordNumber = { 21, 37, 31, 34 };
    public readonly int[] DaggerNumber = { 1, 10, 23, 18 };
    public readonly int[] GreatSwordNumber = { 29, 29, 56, 56 };
    public readonly int[] BluntNumber = { 16, 39, 17, 36 };
    public readonly int[] StaffNumber = { 47, 28, 35, 26 };
    public readonly int[] WandNumber = { 4, 14, 13, 11 };

    public void InitWeaponManager()
    {
        _currentWeapon = null;
        _currentWeaponName = null;
        _weaponDic = new Dictionary<string, Weapon>();

        Weapon Sword = new Sword();
        Weapon Dagger = new Dagger();
        Weapon GreatSword = new GreatSword();
        Weapon Blunt = new Blunt();
        Weapon Staff = new Staff();
        Weapon Wand = new Wand();

        AddWeapon("SWORD", Sword as Sword);
        AddWeapon("DAGGER", Dagger as Dagger);
        AddWeapon("GREATSWORD", GreatSword as GreatSword);
        AddWeapon("BLUNT", Blunt as Blunt);
        AddWeapon("STAFF", Staff as Staff);
        AddWeapon("WAND", Wand as Wand);
    }

    public void UpdateWeapon()
    {
        if (_currentWeapon != null) _currentWeapon.Update();
    }

    public void AddWeapon(string weaponName, Weapon weapon)
    {
        _weaponDic.Add(weaponName, weapon);
    }

    public void SetWeapon(string weaponName)
    {
        if (_currentWeapon == _weaponDic[weaponName]) return;
        _currentWeaponName = weaponName;
        _currentWeapon = _weaponDic[_currentWeaponName];
        Player.Instance.MyAnimator.runtimeAnimatorController = _currentWeapon.WeaponAnimation;
    }

    public string GetWeaponName()
    {
        return _currentWeaponName;
    }
    public Weapon GetWeapon()
    {
        return _currentWeapon;
    }

}