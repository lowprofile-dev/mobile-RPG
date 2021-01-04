using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentItems
{
    //투구
    public int headAccesoriesIndex = 0;
    //장갑
    public int leftElbowIndex = 0;
    public int rightElbowIndex = 0;
    //갑옷
    public int chestIndex = 0;
    public int spineIndex = 0;
    //하체
    public int lowerSpineIndex = 0;
    //신발
    public int leftKneeIndex = 0;
    public int rightKneeIndex = 0;

    public CurrentItems()
    {
        this.headAccesoriesIndex = 0;
        this.leftElbowIndex = 0;
        this.rightElbowIndex = 0;
        this.chestIndex = 0;
        this.spineIndex = 0;
        this.lowerSpineIndex = 0;
        this.leftKneeIndex = 0;
        this.rightKneeIndex = 0;
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