/*
    File CurrentItems.cs
    class CurrentItems
    
    담당자 : 김기정
    부 담당자 :
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentItems
{
    [Header("재화 및 재료")]
    public int gold;
    public int coin;
    //강화석
    public int gem;

    [Header("장비")]
    //투구
    public int headAccesoriesIndex;
    //장갑
    public int leftElbowIndex;
    public int rightElbowIndex;
    public int leftShoulderIndex;
    public int rightShoulderIndex;
    //갑옷
    public int chestIndex;
    public int spineIndex;
    //하체
    public int lowerSpineIndex;
    //신발
    public int leftKneeIndex;
    public int rightKneeIndex;
    public int leftHipIndex;
    public int rightHipIndex;

    public CurrentItems()
    {
        this.gold = 5000;
        this.coin = 50;
        this.gem = 10;
        this.headAccesoriesIndex = 0;
        this.leftElbowIndex = 0;
        this.rightElbowIndex = 0;
        this.leftShoulderIndex = 0;
        this.rightShoulderIndex = 0;
        this.chestIndex = 0;
        this.spineIndex = 0;
        this.lowerSpineIndex = 0;
        this.leftKneeIndex = 0;
        this.rightKneeIndex = 0;
        this.leftHipIndex = 0;
        this.rightHipIndex = 0;
    }
}

[System.Serializable]
public class CurrentItemKeys
{
    public int ArmorKey;
    public int BottomKey;
    public int HelmetKey;
    public int GlovesKey;
    public int BootKey;

    public CurrentItemKeys()
    {
        this.ArmorKey = 1;
        this.BottomKey = 2;
        this.HelmetKey = 3;
        this.GlovesKey = 4;
        this.BootKey = 5;
    }
}