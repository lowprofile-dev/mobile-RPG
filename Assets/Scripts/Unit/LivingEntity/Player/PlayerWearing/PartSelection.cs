using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PartSelection : MonoBehaviour
{
    [SerializeField] private CharacterParts characterParts;
    
    private PartsCollections collections;
    [SerializeField] bool isPlayer;

    private bool isChanged = false;

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
        GameObject playerAvatar = transform.GetChild(1).gameObject;
        collections = playerAvatar.GetComponent<PartsCollections>();
        characterParts = playerAvatar.GetComponent<CharacterParts>();
        //collections = GameObject.Find("PlayerAvatar").GetComponent<PartsCollections>();
        //characterParts = GameObject.Find("PlayerAvatar").GetComponent<CharacterParts>();
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
        if(isPlayer)
        {
            PlayerWeaponChange();

            if (isChanged)
            {
                Player.Instance.ChangeFaceCamera();
                isChanged = false;
            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                ChangeHeadPart();
                ChangeHairPart();
                ChangeHeadAccesoriesPart();
                ChangeChestPart();
                ChangeSpinePart();
                ChangeLowerSpinePart();
                ChangeArmParts();
                ChangeLegParts();
                isChanged = true;
            }
        }
    }

    private void PlayerWeaponChange()
    {
        if (Player.Instance.weaponManager.GetWeaponName() == "SWORD")
        {
            if (rightWeaponIndex != Player.Instance.weaponManager.SwordNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade])
            {
                ChangeRightWeaponPart(Player.Instance.weaponManager.SwordNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade]);
            }
        }
        else if (Player.Instance.weaponManager.GetWeaponName() == "WAND")
        {
            if (rightWeaponIndex != Player.Instance.weaponManager.WandNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade])
            {
                ChangeRightWeaponPart(Player.Instance.weaponManager.WandNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade]);
            }
        }
        else if (Player.Instance.weaponManager.GetWeaponName() == "GREATSWORD")
        {
            if (rightWeaponIndex != Player.Instance.weaponManager.GreatSwordNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade])
            {
                ChangeRightWeaponPart(Player.Instance.weaponManager.GreatSwordNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade]);
            }
        }
        else if (Player.Instance.weaponManager.GetWeaponName() == "DAGGER")
        {
            if (rightWeaponIndex != Player.Instance.weaponManager.DaggerNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade])
            {
                ChangeRightWeaponPart(Player.Instance.weaponManager.DaggerNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade]);
            }
        }
        else if (Player.Instance.weaponManager.GetWeaponName() == "STAFF")
        {
            if (rightWeaponIndex != Player.Instance.weaponManager.StaffNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade])
            {
                ChangeRightWeaponPart(Player.Instance.weaponManager.StaffNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade]);
            }
        }
        else if (Player.Instance.weaponManager.GetWeaponName() == "BLUNT")
        {
            if (rightWeaponIndex != Player.Instance.weaponManager.BluntNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade])
            {
                ChangeRightWeaponPart(Player.Instance.weaponManager.BluntNumber[Player.Instance.weaponManager.GetWeapon().outfitGrade]);
            }
        }
    }

    public bool PartChanged()
    {
        return isChanged;
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

    public void ChangeHeadPart(int index)
    {
        headIndex = index;
        characterParts.ChangeHead(collections.GetHead(headKeys[headIndex]));
    }

    public void ChangeHeadPart()
    {
        headIndex = GetPartKeyIndex(headIndex, headKeys);
        characterParts.ChangeHead(collections.GetHead(headKeys[headIndex]));
    }

    public void ChangeHairPart(int index)
    {
        hairIndex = index;
        characterParts.ChangeHair(collections.GetHair(hairKeys[hairIndex]));
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

    /// <summary>
    /// 투구 교체
    /// </summary>
    /// <param name="index">headAccesoriesKeys에서 해당 아이템 모델 파일명에 있는 끝의 숫자 세자리가 위치한 인덱스</param>
    public void ChangeHeadAccesoriesPart(int index)
    {
        headAccesoriesIndex = index;
        characterParts.ChangeHeadAccesories(collections.GetHeadAccesories(headAccesoriesKeys[headAccesoriesIndex]));
    }

    public void ChangeLeftShoulderPart()
    {
        leftShoulderIndex = GetPartKeyIndex(leftShoulderIndex, shoulderKeys);
        characterParts.ChangeLeftShoulder(collections.GetShoulder(shoulderKeys[leftShoulderIndex]));
    }

    public void ChangeLeftShoulderPart(int index)
    {
        leftShoulderIndex = index;
        characterParts.ChangeLeftShoulder(collections.GetShoulder(shoulderKeys[leftShoulderIndex]));
    }

    public void ChangeLeftElbowPart()
    {
        leftElbowIndex = GetPartKeyIndex(leftElbowIndex, elbowKeys);
        characterParts.ChangeLeftElbow(collections.GetElbow(elbowKeys[leftElbowIndex]));
    }
    
    /// <summary>
    /// 왼손 장갑 교체
    /// </summary>
    /// <param name="index">leftElbowKeys에서 해당 아이템 모델 파일명에 있는 끝의 숫자 세자리가 위치한 인덱스</param>
    public void ChangeLeftElbowPart(int index)
    {
        leftElbowIndex = index;
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

    public void ChangeLeftWeaponPart(int index)
    {
        leftWeaponIndex = index;
        characterParts.ChangeLeftWeapon(collections.GetWeapon(weaponKeys[leftWeaponIndex]));
    }

    public void ChangeLeftShieldPart(int index)
    {
        leftShieldIndex = index;
        characterParts.ChangeLeftShield(collections.GetShield(shieldKeys[leftShieldIndex]));
    }

    public void ChangeRightShoulderPart()
    {
        rightShoulderIndex = GetPartKeyIndex( rightShoulderIndex, shoulderKeys);
        characterParts.ChangeRightShoulder(collections.GetShoulder(shoulderKeys[rightShoulderIndex]));
    }

    public void ChangeRightShoulderPart(int index)
    {
        rightShoulderIndex = index;
        characterParts.ChangeRightShoulder(collections.GetShoulder(shoulderKeys[rightShoulderIndex]));
    }

    public void ChangeRightElbowPart()
    {
        rightElbowIndex = GetPartKeyIndex(rightElbowIndex, elbowKeys);
        characterParts.ChangeRightElbow(collections.GetElbow(elbowKeys[rightElbowIndex]));
    }

    /// <summary>
    /// 오른손 장갑 교체
    /// </summary>
    /// <param name="index">rightElbowKeys에서 해당 아이템 모델 파일명에 있는 끝의 숫자 세자리가 위치한 인덱스</param>
    public void ChangeRightElbowPart(int index)
    {
        rightElbowIndex = index;
        characterParts.ChangeRightElbow(collections.GetElbow(elbowKeys[rightElbowIndex]));
    }

    public void ChangeRightWeaponPart()
    {
        rightWeaponIndex = GetPartKeyIndex(rightWeaponIndex, weaponKeys);
        characterParts.ChangeRightWeapon(collections.GetWeapon(weaponKeys[rightWeaponIndex]));
    }

    public void ChangeRightWeaponPart(int index)
    {
        if (weaponKeys.Count <= index)
        {
            Debug.Log("Out of Index Range " + weaponKeys.Count + " / " + index);
            return;
        }
        rightWeaponIndex = index;
        characterParts.ChangeRightWeapon(collections.GetWeapon(weaponKeys[rightWeaponIndex]));
    }

    public void ChangeChestPart()
    {
        chestIndex = GetPartKeyIndex( chestIndex, chestKeys);
        characterParts.ChangeChest(collections.GetChest(chestKeys[chestIndex]));
    }

    public void ChangeChestPart(int index)
    {
        chestIndex = index;
        characterParts.ChangeChest(collections.GetChest(chestKeys[chestIndex]));
    }

    public void ChangeSpinePart()
    {
        spineIndex = GetPartKeyIndex(spineIndex, spineKeys);
        characterParts.ChangeSpine(collections.GetSpine(spineKeys[spineIndex]));
    }

    public void ChangeSpinePart(int index)
    {
        spineIndex = index;
        characterParts.ChangeSpine(collections.GetSpine(spineKeys[spineIndex]));
    }

    public void ChangeLowerSpinePart()
    {
        lowerSpineIndex = GetPartKeyIndex(lowerSpineIndex, lowerSpineKeys);
        characterParts.ChangeLowerSpine(collections.GetLowerSpine(lowerSpineKeys[lowerSpineIndex]));
    }

    public void ChangeLowerSpinePart(int index)
    {
        lowerSpineIndex = index;
        characterParts.ChangeLowerSpine(collections.GetLowerSpine(lowerSpineKeys[lowerSpineIndex]));
    }

    public void ChangeLeftHipPart()
    {
        leftHipIndex = GetPartKeyIndex(leftHipIndex, hipKeys);
        characterParts.ChangeLeftHip(collections.GetHip(hipKeys[leftHipIndex]));
    }

    public void ChangeLeftHipPart(int index)
    {
        leftHipIndex = index;
        characterParts.ChangeLeftHip(collections.GetHip(hipKeys[leftHipIndex]));
    }

    public void ChangeLeftKneePart()
    {
        leftKneeIndex = GetPartKeyIndex(leftKneeIndex, kneeKeys);
        characterParts.ChangeLeftKnee(collections.GetKnee(kneeKeys[leftKneeIndex]));
    }

    public void ChangeLeftKneePart(int index)
    {
        leftKneeIndex = index;
        characterParts.ChangeLeftKnee(collections.GetKnee(kneeKeys[leftKneeIndex]));
    }

    public void ChangeRightHipPart()
    {
        rightHipIndex = GetPartKeyIndex(rightHipIndex, hipKeys);
        characterParts.ChangeRightHip(collections.GetHip(hipKeys[rightHipIndex]));
    }

    public void ChangeRightHipPart(int index)
    {
        rightHipIndex = index;
        characterParts.ChangeRightHip(collections.GetHip(hipKeys[rightHipIndex]));
    }

    public void ChangeRightKneePart()
    {
        rightKneeIndex = GetPartKeyIndex(rightKneeIndex, kneeKeys);
        characterParts.ChangeRightKnee(collections.GetKnee(kneeKeys[rightKneeIndex]));
    }

    public void ChangeRightKneePart(int index)
    {
        rightKneeIndex = index;
        characterParts.ChangeRightKnee(collections.GetKnee(kneeKeys[rightKneeIndex]));
    }

    public void ChangeArmParts()
    {
        armPartsIndex = GetPartKeyIndex(armPartsIndex, armPartsKeys);
        characterParts.ChangeArmParts(collections.GetArmParts(armPartsKeys[armPartsIndex]));
    }

    public void ChangeArmParts(int index)
    {
        armPartsIndex = index;
        characterParts.ChangeArmParts(collections.GetArmParts(armPartsKeys[armPartsIndex]));
    }

    public void ChangeLegParts()
    {
        legPartsIndex = GetPartKeyIndex(legPartsIndex, legPartsKeys);
        characterParts.ChangeLegParts(collections.GetLegParts(legPartsKeys[legPartsIndex]));
    }

    public void ChangeLegParts(int index)
    {
        legPartsIndex = index;
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
