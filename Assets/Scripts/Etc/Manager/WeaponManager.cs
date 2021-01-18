using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVReader;
using System;
using System.IO;

public class WeaponManager : SingletonBase<WeaponManager>
{
    private Weapon _currentWeapon;
    private string _currentWeaponName;
    private StatusManager statusManager;
    public Dictionary<string, Weapon> _weaponDic;
    public readonly int[] GradeCriteria = { 0, 9, 18, 25 };
    public readonly int[] SwordNumber = { 21, 37, 31, 34 };
    public readonly int[] DaggerNumber = { 1, 10, 23, 18 };
    public readonly int[] GreatSwordNumber = { 29, 29, 56, 56 };
    public readonly int[] BluntNumber = { 16, 39, 17, 36 };
    public readonly int[] StaffNumber = { 47, 28, 35, 26 };
    public readonly int[] WandNumber = { 4, 14, 13, 11 };

    private void Start()
    {
       statusManager = StatusManager.Instance;
    }

    public void InitWeaponManager()
    {
        _currentWeapon = null;
        _currentWeaponName = null;
        _weaponDic = new Dictionary<string, Weapon>();
        
        Weapon Sword = new Sword();
        Weapon Dagger = new Dagger();
        Weapon Blunt = new Blunt();
        Weapon Staff = new Staff();
        Weapon Wand = new Wand();

        AddWeapon("SWORD", Sword as Sword);
        AddWeapon("DAGGER", Dagger as Dagger);
        AddWeapon("BLUNT", Blunt as Blunt);
        AddWeapon("STAFF", Staff as Staff);
        AddWeapon("WAND", Wand as Wand);
    }

    /// <summary>
    /// 현재 무기에 Exp 추가
    /// </summary>
    public void AddExpToCurrentWeapon(int amount)
    {
        AddExpToSpecificWeapon(_currentWeaponName, amount);
    }

    /// <summary>
    /// 특정 무기에 Exp 추가
    /// </summary>
    public void AddExpToSpecificWeapon(string name, int amount)
    {
        switch (name)
        {
            case "SWORD":
                MasteryManager.Instance.currentMastery.currentSwordMasteryExp += amount;
                break;
            case "WAND":
                MasteryManager.Instance.currentMastery.currentWandMasteryExp += amount;
                break;
            case "DAGGER":
                MasteryManager.Instance.currentMastery.currentWandMasteryExp += amount;
                break;
            case "BLUNT":
                MasteryManager.Instance.currentMastery.currentWandMasteryExp += amount;
                break;
            case "STAFF":
                MasteryManager.Instance.currentMastery.currentWandMasteryExp += amount;
                break;
            default:
                Debug.Log("잘못된 이름입니다.");
                break;
        }
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
        _currentWeaponName = weaponName;
        _currentWeapon = _weaponDic[_currentWeaponName];
        Player.Instance.myAnimator.runtimeAnimatorController = _currentWeapon.WeaponAnimation;
        _weaponDic[weaponName].attackDamage += statusManager.finalStatus.attackDamage;
        _weaponDic[weaponName].magicDamage += statusManager.finalStatus.magicDamage;
        UIManager.Instance.playerUIView.SetWeaponSkillIcon();
    }

    public bool LevelUpCheck()
    {
        return _currentWeapon.LevelUpCheck();
    }

    public string GetWeaponName()
    {
        return _currentWeaponName;
    }
    public Weapon GetWeapon()
    {
        return _currentWeapon;
    } 

    public void WeaponCoolTimeReduce()
    {
        foreach (string key in _weaponDic.Keys)
        {
            _weaponDic[key].skillACool -= 1;
            _weaponDic[key].skillBCool -= 1;
            _weaponDic[key].skillCCool -= 1;
        }
    }
}