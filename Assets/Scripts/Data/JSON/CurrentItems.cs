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
        headAccesoriesIndex = 0;
        leftElbowIndex = 0;
        rightElbowIndex = 0;
        chestIndex = 0;
        spineIndex = 0;
        lowerSpineIndex = 0;
        leftKneeIndex = 0;
        rightKneeIndex = 0;
    }
}