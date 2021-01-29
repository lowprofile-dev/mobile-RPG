using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSVReader;
using System;
using System.IO;

////////////////////////////////////////////////////
/*
    File WeaponManager.cs
    class WeaponManager

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////

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
        MasteryManager.Instance.weaponManager = this;
    }
    /// <summary>
    /// 무기매니저 초기화
    /// </summary>
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

        MasteryManager.Instance.MasteryApply();
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
        MasteryManager.Instance.UpdateCurrentExp();
    }

    /// <summary>
    /// 현재 무기의 Update 함수를 실행해준다.
    /// </summary>
    public void UpdateWeapon()
    {
        if (_currentWeapon != null) _currentWeapon.Update();
    }

    /// <summary>
    /// 무기 딕셔너리에 무기를 추가한다.
    /// </summary>
    /// <param name="weaponName"> 입력할 무기의 이름</param>
    /// <param name="weapon">입력할 무기의 클래스</param>
    public void AddWeapon(string weaponName, Weapon weapon)
    {
        _weaponDic.Add(weaponName, weapon);
    }

    /// <summary>
    /// 현재 무기를 입력받은 무기의 이름을 통해 변경해준다. 
    /// </summary>
    /// <param name="weaponName"> 바꿀 무기의 이름을 입력받는다 </param>
    public void SetWeapon(string weaponName)
    {
        _currentWeaponName = weaponName;
        _currentWeapon = _weaponDic[_currentWeaponName];
        Player.Instance.myAnimator.runtimeAnimatorController = _currentWeapon.WeaponAnimation;
        _weaponDic[weaponName].attackDamage += statusManager.finalStatus.attackDamage;
        UIManager.Instance.playerUIView.SetWeaponSkillIcon();
    }

    /// <summary>
    /// 현재 무기의 이름을 리턴받는 함수
    /// </summary>
    /// <returns></returns>
    public string GetWeaponName()
    {
        return _currentWeaponName;
    }

    /// <summary>
    /// 현재 무기의 클래스를 리턴받는 함수
    /// </summary>
    /// <returns></returns>
    public Weapon GetWeapon()
    {
        return _currentWeapon;
    } 

    /// <summary>
    /// 전체 무기 스킬의 쿨타임을 1씩 줄여준다
    /// </summary>
    public void WeaponCoolTimeReduce()
    {
        foreach (string key in _weaponDic.Keys)
        {
            _weaponDic[key].skillACool = _weaponDic[key].skillACoolSave - StatusManager.Instance.finalStatus.attackCooldown;
            _weaponDic[key].skillBCool = _weaponDic[key].skillBCoolSave - StatusManager.Instance.finalStatus.attackCooldown;
            _weaponDic[key].skillCCool = _weaponDic[key].skillCCoolSave - StatusManager.Instance.finalStatus.attackCooldown;
        }
    }
    /// <summary>
    /// 전체 무기 스킬의 쿨타임을 1씩 늘려준다
    /// </summary>
    public void WeaponCoolTimeincrease()
    {
        foreach (string key in _weaponDic.Keys)
        {
            _weaponDic[key].skillACool += StatusManager.Instance.finalStatus.attackCooldown;
            _weaponDic[key].skillBCool += StatusManager.Instance.finalStatus.attackCooldown;
            _weaponDic[key].skillCCool += StatusManager.Instance.finalStatus.attackCooldown;
        }
    }
}