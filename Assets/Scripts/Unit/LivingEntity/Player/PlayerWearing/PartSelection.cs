using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartSelection : MonoBehaviour
{
    private CharacterParts characterParts;
    
    private PartsCollections collections;

    int headIndex = -1;
    int hairIndex = -1;
    int headAccesoriesIndex = -1;
    int leftShoulderIndex = -1;
    int leftElbowIndex = -1;
    int leftWeaponIndex = -1;
    int leftShieldIndex = -1;
    int rightShoulderIndex = -1;
    int rightElbowIndex = -1;
    int rightWeaponIndex = -1;
    int chestIndex = -1;
    int spineIndex = -1;
    int lowerSpineIndex = -1;
    int leftHipIndex = -1;
    int leftKneeIndex = -1;
    int rightHipIndex = -1;
    int rightKneeIndex = -1;
    int armPartsIndex = -1;
    int legPartsIndex = -1;
    int allBodyPartsIndex = -1;

    List<string> headKeys = new List<string>();
    List<string> hairKeys = new List<string>();
    List<string> headAccesoriesKeys = new List<string>();
    List<string> shoulderKeys = new List<string>();
    List<string> elbowKeys = new List<string>();
    List<string> weaponKeys = new List<string>();
    List<string> shieldKeys = new List<string>();
    List<string> chestKeys = new List<string>();
    List<string> spineKeys = new List<string>();
    List<string> lowerSpineKeys = new List<string>();
    List<string> hipKeys = new List<string>();
    List<string> kneeKeys = new List<string>();
    List<string> armPartsKeys = new List<string>();
    List<string> legPartsKeys = new List<string>();

    public void Start()
    {
        collections = GameObject.Find("PlayerAvatar").GetComponent<PartsCollections>();
        characterParts = GameObject.Find("PlayerAvatar").GetComponent<CharacterParts>();
    }

    public void Init()
    {
        if (collections != null)
        {
            headKeys.AddRange(collections.headObjects.Keys);
            hairKeys.AddRange(collections.hairObjects.Keys);
            headAccesoriesKeys.AddRange(collections.headAccesoriesObjects.Keys);
            shoulderKeys.AddRange(collections.shoulderObjects.Keys);
            elbowKeys.AddRange(collections.elbowObjects.Keys);
            weaponKeys.AddRange(collections.weaponObjects.Keys);
            shieldKeys.AddRange(collections.shieldObjects.Keys);
            chestKeys.AddRange(collections.chestObjects.Keys);
            spineKeys.AddRange(collections.spineObjects.Keys);
            lowerSpineKeys.AddRange(collections.lowerSpineObjects.Keys);
            hipKeys.AddRange(collections.hipObjects.Keys);
            kneeKeys.AddRange(collections.kneeObjects.Keys);
            armPartsKeys.AddRange(collections.armParts.Keys);
            legPartsKeys.AddRange(collections.legParts.Keys);
            // Change parts to make parts object active
            ChangeHeadPart();
            ChangeHairPart();
            ChangeHeadAccesoriesPart();
            ChangeChestPart();
            ChangeSpinePart();
            ChangeLowerSpinePart();
            ChangeArmParts();
            ChangeLegParts();
            ChangeRightWeaponPart();
            ChangeLeftShieldPart();
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ChangeHeadPart();
            ChangeHairPart();
            ChangeHeadAccesoriesPart();
            ChangeChestPart();
            ChangeSpinePart();
            ChangeLowerSpinePart();
            ChangeArmParts();
            ChangeLegParts();
            ChangeRightWeaponPart();
        }
    }

    public int GetPartKeyIndex( int currentIndex, List<string> keys)
    {
        if (keys == null || keys.Count == 0)
            return -1;
        ++currentIndex;
        if (currentIndex >= keys.Count)
            currentIndex = 0;
        return currentIndex;
    }

    public void ChangeHeadPart()
    {
        headIndex = GetPartKeyIndex(headIndex, headKeys);
        characterParts.ChangeHead(collections.GetHead(headKeys[headIndex]));
    }

    public void ChangeHairPart()
    {
        hairIndex = GetPartKeyIndex(hairIndex, hairKeys);
        characterParts.ChangeHair(collections.GetHair(hairKeys[hairIndex]));
    }

    public void ChangeHeadAccesoriesPart()
    {
        headAccesoriesIndex = GetPartKeyIndex(headAccesoriesIndex, headAccesoriesKeys);
        characterParts.ChangeHeadAccesories(collections.GetHeadAccesories(headAccesoriesKeys[headAccesoriesIndex]));
    }

    public void ChangeLeftShoulderPart()
    {
        leftShoulderIndex = GetPartKeyIndex(leftShoulderIndex, shoulderKeys);
        characterParts.ChangeLeftShoulder(collections.GetShoulder(shoulderKeys[leftShoulderIndex]));
    }

    public void ChangeLeftElbowPart()
    {
        leftElbowIndex = GetPartKeyIndex(leftElbowIndex, elbowKeys);
        characterParts.ChangeLeftElbow(collections.GetElbow(elbowKeys[leftElbowIndex]));
    }

    public void ChangeLeftWeaponPart()
    {
        leftWeaponIndex = GetPartKeyIndex(leftWeaponIndex, weaponKeys);
        characterParts.ChangeLeftWeapon(collections.GetWeapon(weaponKeys[leftWeaponIndex]));
    }

    public void ChangeLeftShieldPart()
    {
        leftShieldIndex = GetPartKeyIndex(leftShieldIndex, shieldKeys);
        characterParts.ChangeLeftShield(collections.GetShield(shieldKeys[leftShieldIndex]));
    }

    public void ChangeRightShoulderPart()
    {
        rightShoulderIndex = GetPartKeyIndex( rightShoulderIndex, shoulderKeys);
        characterParts.ChangeRightShoulder(collections.GetShoulder(shoulderKeys[rightShoulderIndex]));
    }

    public void ChangeRightElbowPart()
    {
        rightElbowIndex = GetPartKeyIndex(rightElbowIndex, elbowKeys);
        characterParts.ChangeRightElbow(collections.GetElbow(elbowKeys[rightElbowIndex]));
    }

    public void ChangeRightWeaponPart()
    {
        rightWeaponIndex = GetPartKeyIndex(rightWeaponIndex, weaponKeys);
        characterParts.ChangeRightWeapon(collections.GetWeapon(weaponKeys[rightWeaponIndex]));
    }

    public void ChangeChestPart()
    {
        chestIndex = GetPartKeyIndex( chestIndex, chestKeys);
        characterParts.ChangeChest(collections.GetChest(chestKeys[chestIndex]));
    }

    public void ChangeSpinePart()
    {
        spineIndex = GetPartKeyIndex(spineIndex, spineKeys);
        characterParts.ChangeSpine(collections.GetSpine(spineKeys[spineIndex]));
    }

    public void ChangeLowerSpinePart()
    {
        lowerSpineIndex = GetPartKeyIndex(lowerSpineIndex, lowerSpineKeys);
        characterParts.ChangeLowerSpine(collections.GetLowerSpine(lowerSpineKeys[lowerSpineIndex]));
    }

    public void ChangeLeftHipPart()
    {
        leftHipIndex = GetPartKeyIndex(leftHipIndex, hipKeys);
        characterParts.ChangeLeftHip(collections.GetHip(hipKeys[leftHipIndex]));
    }

    public void ChangeLeftKneePart()
    {
        leftKneeIndex = GetPartKeyIndex(leftKneeIndex, kneeKeys);
        characterParts.ChangeLeftKnee(collections.GetKnee(kneeKeys[leftKneeIndex]));
    }

    public void ChangeRightHipPart()
    {
        rightHipIndex = GetPartKeyIndex(rightHipIndex, hipKeys);
        characterParts.ChangeRightHip(collections.GetHip(hipKeys[rightHipIndex]));
    }

    public void ChangeRightKneePart()
    {
        rightKneeIndex = GetPartKeyIndex(rightKneeIndex, kneeKeys);
        characterParts.ChangeRightKnee(collections.GetKnee(kneeKeys[rightKneeIndex]));
    }

    public void ChangeArmParts()
    {
        armPartsIndex = GetPartKeyIndex(armPartsIndex, armPartsKeys);
        characterParts.ChangeArmParts(collections.GetArmParts(armPartsKeys[armPartsIndex]));
    }

    public void ChangeLegParts()
    {
        legPartsIndex = GetPartKeyIndex(legPartsIndex, legPartsKeys);
        characterParts.ChangeLegParts(collections.GetLegParts(legPartsKeys[legPartsIndex]));
    }

    public void ChangeAllBodyParts()
    {
        allBodyPartsIndex = GetPartKeyIndex(allBodyPartsIndex, chestKeys);
        var key = chestKeys[allBodyPartsIndex];

        var newIndex = chestKeys.IndexOf(key);
        chestIndex = newIndex == -1 ? chestIndex : newIndex;

        newIndex = spineKeys.IndexOf(key);
        spineIndex = newIndex == -1 ? spineIndex : newIndex;

        newIndex = lowerSpineKeys.IndexOf(key);
        lowerSpineIndex = newIndex == -1 ? lowerSpineIndex : newIndex;

        newIndex = armPartsKeys.IndexOf(key);
        armPartsIndex = newIndex == -1 ? armPartsIndex : newIndex;

        newIndex = legPartsKeys.IndexOf(key);
        legPartsIndex = newIndex == -1 ? legPartsIndex : newIndex;

        characterParts.ChangeChest(collections.GetChest(chestKeys[chestIndex]));
        characterParts.ChangeSpine(collections.GetSpine(spineKeys[spineIndex]));
        characterParts.ChangeLowerSpine(collections.GetLowerSpine(lowerSpineKeys[lowerSpineIndex]));
        characterParts.ChangeArmParts(collections.GetArmParts(armPartsKeys[armPartsIndex]));
        characterParts.ChangeLegParts(collections.GetLegParts(legPartsKeys[legPartsIndex]));
    }

    
}
