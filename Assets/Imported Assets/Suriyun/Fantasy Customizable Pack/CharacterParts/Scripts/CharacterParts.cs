using UnityEngine;
using System.Collections;

public class CharacterParts : MonoBehaviour
{
    public Transform head;
    public Transform hair;
    public Transform headAccesories;
    public Transform rightShoulder;
    public Transform rightElbow;
    public Transform rightWeapon;
    public Transform leftShoulder;
    public Transform leftElbow;
    public Transform leftWeapon;
    public Transform leftShield;
    public Transform chest;
    public Transform spine;
    public Transform lowerSpine;
    public Transform rightHip;
    public Transform rightKnee;
    public Transform leftHip;
    public Transform leftKnee;

    GameObject headObject;
    GameObject hairObject;
    GameObject headAccesoriesObject;
    GameObject rightShoulderObject;
    GameObject rightElbowObject;
    GameObject rightWeaponObject;
    GameObject leftShoulderObject;
    GameObject leftElbowObject;
    GameObject leftWeaponObject;
    GameObject leftShieldObject;
    GameObject chestObject;
    GameObject spineObject;
    GameObject lowerSpineObject;
    GameObject rightHipObject;
    GameObject rightKneeObject;
    GameObject leftHipObject;
    GameObject leftKneeObject;

    public bool rightSideScaleXMinusOne;
    public bool leftSideScaleXMinusOne;

    public GameObject ChangeObject(GameObject oldObject, GameObject newObjectPrefab, Transform container)
    {
        if (newObjectPrefab == null || container == null)
            return null;

        if (oldObject != null)
            Destroy(oldObject);

        GameObject newObject = Instantiate(newObjectPrefab);
        newObject.transform.SetParent(container);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localEulerAngles = Vector3.zero;
        newObject.transform.localScale = Vector3.one;

        return newObject;
    }

    public void ChangeHead(GameObject obj)
    {
        headObject = ChangeObject(headObject, obj, head);
    }

    public void ChangeHair(GameObject obj)
    {
        hairObject = ChangeObject(hairObject, obj, hair);
    }

    public void ChangeHeadAccesories(GameObject obj)
    {
        headAccesoriesObject = ChangeObject(headAccesoriesObject, obj, headAccesories);
    }

    public void ChangeRightShoulder(GameObject obj)
    {
        rightShoulderObject = ChangeObject(rightShoulderObject, obj, rightShoulder);
        if (rightSideScaleXMinusOne && rightShoulderObject != null)
        {
            Vector3 newScale = rightShoulderObject.transform.localScale;
            newScale.x *= -1;
            rightShoulderObject.transform.localScale = newScale;
        }
    }

    public void ChangeRightElbow(GameObject obj)
    {
        rightElbowObject = ChangeObject(rightElbowObject, obj, rightElbow);
        if (rightSideScaleXMinusOne && rightElbowObject != null)
        {
            Vector3 newScale = rightElbowObject.transform.localScale;
            newScale.x *= -1;
            rightElbowObject.transform.localScale = newScale;
        }
    }

    public void ChangeRightWeapon(GameObject obj)
    {
        rightWeaponObject = ChangeObject(rightWeaponObject, obj, rightWeapon);
    }

    public void ChangeLeftShoulder(GameObject obj)
    {
        leftShoulderObject = ChangeObject(leftShoulderObject, obj, leftShoulder);
        if (leftSideScaleXMinusOne && leftShoulderObject != null)
        {
            Vector3 newScale = leftShoulderObject.transform.localScale;
            newScale.x *= -1;
            leftShoulderObject.transform.localScale = newScale;
        }
    }

    public void ChangeLeftElbow(GameObject obj)
    {
        leftElbowObject = ChangeObject(leftElbowObject, obj, leftElbow);
        if (leftSideScaleXMinusOne && leftElbowObject != null)
        {
            Vector3 newScale = leftElbowObject.transform.localScale;
            newScale.x *= -1;
            leftElbowObject.transform.localScale = newScale;
        }
    }

    public void ChangeLeftWeapon(GameObject obj)
    {
        leftWeaponObject = ChangeObject(leftWeaponObject, obj, leftWeapon);
    }

    public void ChangeLeftShield(GameObject obj)
    {
        leftShieldObject = ChangeObject(leftShieldObject, obj, leftShield);
    }

    public void ChangeChest(GameObject obj)
    {
        chestObject = ChangeObject(chestObject, obj, chest);
    }

    public void ChangeSpine(GameObject obj)
    {
        spineObject = ChangeObject(spineObject, obj, spine);
    }

    public void ChangeLowerSpine(GameObject obj)
    {
        lowerSpineObject = ChangeObject(lowerSpineObject, obj, lowerSpine);
    }

    public void ChangeRightHip(GameObject obj)
    {
        rightHipObject = ChangeObject(rightHipObject, obj, rightHip);
        if (rightSideScaleXMinusOne && rightHipObject != null)
        {
            Vector3 newScale = rightHipObject.transform.localScale;
            newScale.x *= -1;
            rightHipObject.transform.localScale = newScale;
        }
    }

    public void ChangeRightKnee(GameObject obj)
    {
        rightKneeObject = ChangeObject(rightKneeObject, obj, rightKnee);
        if (rightSideScaleXMinusOne && rightKneeObject != null)
        {
            Vector3 newScale = rightKneeObject.transform.localScale;
            newScale.x *= -1;
            rightKneeObject.transform.localScale = newScale;
        }
    }

    public void ChangeLeftHip(GameObject obj)
    {
        leftHipObject = ChangeObject(leftHipObject, obj, leftHip);
        if (leftSideScaleXMinusOne && leftHipObject != null)
        {
            Vector3 newScale = leftHipObject.transform.localScale;
            newScale.x *= -1;
            leftHipObject.transform.localScale = newScale;
        }
    }

    public void ChangeLeftKnee(GameObject obj)
    {
        leftKneeObject = ChangeObject(leftKneeObject, obj, leftKnee);
        if (leftSideScaleXMinusOne && leftKneeObject != null)
        {
            Vector3 newScale = leftKneeObject.transform.localScale;
            newScale.x *= -1;
            leftKneeObject.transform.localScale = newScale;
        }
    }

    public void ChangeArmParts(ArmParts parts)
    {
        ChangeLeftShoulder(parts.shoulder);
        ChangeLeftElbow(parts.elbow);
        ChangeRightShoulder(parts.shoulder);
        ChangeRightElbow(parts.elbow);
    }

    public void ChangeLegParts(LegParts parts)
    {
        ChangeLeftHip(parts.hip);
        ChangeLeftKnee(parts.knee);
        ChangeRightHip(parts.hip);
        ChangeRightKnee(parts.knee);
    }

    public void RemoveHead()
    {
        if (headObject != null)
            Destroy(headObject);
    }

    public void RemoveHair()
    {
        if (hairObject != null)
            Destroy(hairObject);
    }

    public void RemoveHeadAccesories()
    {
        if (headAccesoriesObject != null)
            Destroy(headAccesoriesObject);
    }

    public void RemoveRightShoulder()
    {
        if (rightShoulderObject != null)
            Destroy(rightShoulderObject);
    }

    public void RemoveRightElbow()
    {
        if (rightElbowObject != null)
            Destroy(rightElbowObject);
    }

    public void RemoveRightWeapon()
    {
        if (rightWeaponObject != null)
            Destroy(rightWeaponObject);
    }

    public void RemoveLeftShoulder()
    {
        if (leftShoulderObject != null)
            Destroy(leftShoulderObject);
    }

    public void RemoveLeftElbow()
    {
        if (leftElbowObject != null)
            Destroy(leftElbowObject);
    }

    public void RemoveLeftWeapon()
    {
        if (leftWeaponObject != null)
            Destroy(leftWeaponObject);
    }

    public void RemoveLeftShield()
    {
        if (leftShieldObject != null)
            Destroy(leftShieldObject);
    }

    public void RemoveChest()
    {
        if (chestObject != null)
            Destroy(chestObject);
    }

    public void RemoveSpine()
    {
        if (spineObject != null)
            Destroy(spineObject);
    }

    public void RemoveLowerSpine()
    {
        if (lowerSpineObject != null)
            Destroy(lowerSpineObject);
    }

    public void RemoveRightHip()
    {
        if (rightHipObject != null)
            Destroy(rightHipObject);
    }

    public void RemoveRightKnee()
    {
        if (rightKneeObject != null)
            Destroy(rightKneeObject);
    }

    public void RemoveLeftHip()
    {
        if (leftHipObject != null)
            Destroy(leftHipObject);
    }

    public void RemoveLeftKnee()
    {
        if (leftKneeObject != null)
            Destroy(leftKneeObject);
    }

    public void RemoveArmParts()
    {
        RemoveRightShoulder();
        RemoveRightElbow();
        RemoveLeftShoulder();
        RemoveLeftElbow();
    }

    public void RemoveLegParts()
    {
        RemoveRightHip();
        RemoveRightKnee();
        RemoveLeftHip();
        RemoveLeftKnee();
    }
}
