using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentItems
{
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
        this.headAccesoriesIndex = 7;
        this.leftElbowIndex = 48;
        this.rightElbowIndex = 48;
        this.leftShoulderIndex = 48;
        this.rightShoulderIndex = 48;
        this.chestIndex = 54;
        this.spineIndex = 54;
        this.lowerSpineIndex = 54;
        this.leftKneeIndex = 51;
        this.rightKneeIndex = 51;
        this.leftHipIndex = 51;
        this.rightHipIndex = 51;
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