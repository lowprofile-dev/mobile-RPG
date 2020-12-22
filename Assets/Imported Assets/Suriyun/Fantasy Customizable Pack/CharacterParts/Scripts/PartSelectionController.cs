using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PartSelectionController : MonoBehaviour
{
    public CharacterParts characterParts;
    public Text headIndexText;
    public Text hairIndexText;
    public Text headAccesoriesIndexText;
    public Text leftShoulderIndexText;
    public Text leftElbowIndexText;
    public Text leftWeaponIndexText;
    public Text leftShieldIndexText;
    public Text rightShoulderIndexText;
    public Text rightElbowIndexText;
    public Text rightWeaponIndexText;
    public Text chestIndexText;
    public Text spineIndexText;
    public Text lowerSpineIndexText;
    public Text leftHipIndexText;
    public Text leftKneeIndexText;
    public Text rightHipIndexText;
    public Text rightKneeIndexText;
    public Text armPartsIndexText;
    public Text legPartsIndexText;
    public Text allBodyPartsText;
    PartsCollections collections;
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

    public void Init(PartsCollections collections)
    {
        this.collections = collections;
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
        }
    }

    void Update()
    {
        if (headIndexText != null)
            headIndexText.text = (headIndex + 1) + "/" + headKeys.Count;
        if (hairIndexText != null)
            hairIndexText.text = (hairIndex + 1) + "/" + hairKeys.Count;
        if (headAccesoriesIndexText != null)
            headAccesoriesIndexText.text = (headAccesoriesIndex + 1) + "/" + headAccesoriesKeys.Count;
        if (leftShoulderIndexText != null)
            leftShoulderIndexText.text = (leftShoulderIndex + 1) + "/" + shoulderKeys.Count;
        if (leftElbowIndexText != null)
            leftElbowIndexText.text = (leftElbowIndex + 1) + "/" + elbowKeys.Count;
        if (leftWeaponIndexText != null)
            leftWeaponIndexText.text = (leftWeaponIndex + 1) + "/" + weaponKeys.Count;
        if (leftShieldIndexText != null)
            leftShieldIndexText.text = (leftShieldIndex + 1) + "/" + shieldKeys.Count;
        if (rightShoulderIndexText != null)
            rightShoulderIndexText.text = (rightShoulderIndex + 1) + "/" + shoulderKeys.Count;
        if (rightElbowIndexText != null)
            rightElbowIndexText.text = (rightElbowIndex + 1) + "/" + elbowKeys.Count;
        if (rightWeaponIndexText != null)
            rightWeaponIndexText.text = (rightWeaponIndex + 1) + "/" + weaponKeys.Count;
        if (chestIndexText != null)
            chestIndexText.text = (chestIndex + 1) + "/" + chestKeys.Count;
        if (spineIndexText != null)
            spineIndexText.text = (spineIndex + 1) + "/" + spineKeys.Count;
        if (lowerSpineIndexText != null)
            lowerSpineIndexText.text = (lowerSpineIndex + 1) + "/" + lowerSpineKeys.Count;
        if (leftHipIndexText != null)
            leftHipIndexText.text = (leftHipIndex + 1) + "/" + hipKeys.Count;
        if (leftKneeIndexText != null)
            leftKneeIndexText.text = (leftKneeIndex + 1) + "/" + kneeKeys.Count;
        if (rightHipIndexText != null)
            rightHipIndexText.text = (rightHipIndex + 1) + "/" + hipKeys.Count;
        if (rightKneeIndexText != null)
            rightKneeIndexText.text = (rightKneeIndex + 1) + "/" + kneeKeys.Count;
        if (armPartsIndexText != null)
            armPartsIndexText.text = (armPartsIndex + 1) + "/" + armPartsKeys.Count;
        if (legPartsIndexText != null)
            legPartsIndexText.text = (legPartsIndex + 1) + "/" + legPartsKeys.Count;
        if (allBodyPartsText != null)
            allBodyPartsText.text = (allBodyPartsIndex + 1) + "/" + chestKeys.Count;
    }

    public int GetPartKeyIndex(bool isPrev, int currentIndex, List<string> keys)
    {
        if (keys == null || keys.Count == 0)
            return -1;
        if (isPrev)
        {
            --currentIndex;
            if (currentIndex < 0)
                currentIndex = keys.Count - 1;
        }
        else
        {
            ++currentIndex;
            if (currentIndex >= keys.Count)
                currentIndex = 0;
        }
        return currentIndex;
    }

    public void ChangeHeadPart(bool isPrev = false)
    {
        headIndex = GetPartKeyIndex(isPrev, headIndex, headKeys);
        characterParts.ChangeHead(collections.GetHead(headKeys[headIndex]));
    }

    public void ChangeHairPart(bool isPrev = false)
    {
        hairIndex = GetPartKeyIndex(isPrev, hairIndex, hairKeys);
        characterParts.ChangeHair(collections.GetHair(hairKeys[hairIndex]));
    }

    public void ChangeHeadAccesoriesPart(bool isPrev = false)
    {
        headAccesoriesIndex = GetPartKeyIndex(isPrev, headAccesoriesIndex, headAccesoriesKeys);
        characterParts.ChangeHeadAccesories(collections.GetHeadAccesories(headAccesoriesKeys[headAccesoriesIndex]));
    }

    public void ChangeLeftShoulderPart(bool isPrev = false)
    {
        leftShoulderIndex = GetPartKeyIndex(isPrev, leftShoulderIndex, shoulderKeys);
        characterParts.ChangeLeftShoulder(collections.GetShoulder(shoulderKeys[leftShoulderIndex]));
    }

    public void ChangeLeftElbowPart(bool isPrev = false)
    {
        leftElbowIndex = GetPartKeyIndex(isPrev, leftElbowIndex, elbowKeys);
        characterParts.ChangeLeftElbow(collections.GetElbow(elbowKeys[leftElbowIndex]));
    }

    public void ChangeLeftWeaponPart(bool isPrev = false)
    {
        leftWeaponIndex = GetPartKeyIndex(isPrev, leftWeaponIndex, weaponKeys);
        characterParts.ChangeLeftWeapon(collections.GetWeapon(weaponKeys[leftWeaponIndex]));
    }

    public void ChangeLeftShieldPart(bool isPrev = false)
    {
        leftShieldIndex = GetPartKeyIndex(isPrev, leftShieldIndex, shieldKeys);
        characterParts.ChangeLeftShield(collections.GetShield(shieldKeys[leftShieldIndex]));
    }

    public void ChangeRightShoulderPart(bool isPrev = false)
    {
        rightShoulderIndex = GetPartKeyIndex(isPrev, rightShoulderIndex, shoulderKeys);
        characterParts.ChangeRightShoulder(collections.GetShoulder(shoulderKeys[rightShoulderIndex]));
    }

    public void ChangeRightElbowPart(bool isPrev = false)
    {
        rightElbowIndex = GetPartKeyIndex(isPrev, rightElbowIndex, elbowKeys);
        characterParts.ChangeRightElbow(collections.GetElbow(elbowKeys[rightElbowIndex]));
    }

    public void ChangeRightWeaponPart(bool isPrev = false)
    {
        rightWeaponIndex = GetPartKeyIndex(isPrev, rightWeaponIndex, weaponKeys);
        characterParts.ChangeRightWeapon(collections.GetWeapon(weaponKeys[rightWeaponIndex]));
    }

    public void ChangeChestPart(bool isPrev = false)
    {
        chestIndex = GetPartKeyIndex(isPrev, chestIndex, chestKeys);
        characterParts.ChangeChest(collections.GetChest(chestKeys[chestIndex]));
    }

    public void ChangeSpinePart(bool isPrev = false)
    {
        spineIndex = GetPartKeyIndex(isPrev, spineIndex, spineKeys);
        characterParts.ChangeSpine(collections.GetSpine(spineKeys[spineIndex]));
    }

    public void ChangeLowerSpinePart(bool isPrev = false)
    {
        lowerSpineIndex = GetPartKeyIndex(isPrev, lowerSpineIndex, lowerSpineKeys);
        characterParts.ChangeLowerSpine(collections.GetLowerSpine(lowerSpineKeys[lowerSpineIndex]));
    }

    public void ChangeLeftHipPart(bool isPrev = false)
    {
        leftHipIndex = GetPartKeyIndex(isPrev, leftHipIndex, hipKeys);
        characterParts.ChangeLeftHip(collections.GetHip(hipKeys[leftHipIndex]));
    }

    public void ChangeLeftKneePart(bool isPrev = false)
    {
        leftKneeIndex = GetPartKeyIndex(isPrev, leftKneeIndex, kneeKeys);
        characterParts.ChangeLeftKnee(collections.GetKnee(kneeKeys[leftKneeIndex]));
    }

    public void ChangeRightHipPart(bool isPrev = false)
    {
        rightHipIndex = GetPartKeyIndex(isPrev, rightHipIndex, hipKeys);
        characterParts.ChangeRightHip(collections.GetHip(hipKeys[rightHipIndex]));
    }

    public void ChangeRightKneePart(bool isPrev = false)
    {
        rightKneeIndex = GetPartKeyIndex(isPrev, rightKneeIndex, kneeKeys);
        characterParts.ChangeRightKnee(collections.GetKnee(kneeKeys[rightKneeIndex]));
    }

    public void ChangeArmParts(bool isPrev = false)
    {
        armPartsIndex = GetPartKeyIndex(isPrev, armPartsIndex, armPartsKeys);
        characterParts.ChangeArmParts(collections.GetArmParts(armPartsKeys[armPartsIndex]));
    }

    public void ChangeLegParts(bool isPrev = false)
    {
        legPartsIndex = GetPartKeyIndex(isPrev, legPartsIndex, legPartsKeys);
        characterParts.ChangeLegParts(collections.GetLegParts(legPartsKeys[legPartsIndex]));
    }

    public void ChangeAllBodyParts(bool isPrev = false)
    {
        allBodyPartsIndex = GetPartKeyIndex(isPrev, allBodyPartsIndex, chestKeys);
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
