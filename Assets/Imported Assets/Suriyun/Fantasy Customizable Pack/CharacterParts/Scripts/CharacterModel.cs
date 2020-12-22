using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public Transform rightHandContainer;
    public Transform leftHandContainer;
    public Transform shieldContainer;
    public Transform headContainer;
    private Animator tempAnimator;
    public Animator TempAnimator
    {
        get
        {
            if (tempAnimator == null)
                tempAnimator = GetComponent<Animator>();
            return tempAnimator;
        }
    }
    private GameObject headModel;
    private readonly List<GameObject> weaponModels = new List<GameObject>();

    public void SetHeadModel(GameObject model)
    {
        if (headModel != null)
            Destroy(headModel);
        headModel = AddModel(model, headContainer, null);
    }

    public void SetWeaponModel(GameObject rightHandModel, GameObject leftHandModel, GameObject shieldModel)
    {
        ClearGameObjects(weaponModels);
        AddModel(rightHandModel, rightHandContainer, weaponModels);
        AddModel(leftHandModel, leftHandContainer, weaponModels);
        AddModel(shieldModel, shieldContainer, weaponModels);
    }

    private void ClearGameObjects(List<GameObject> list)
    {
        foreach (var entry in list)
            Destroy(entry);
        list.Clear();
    }

    private GameObject AddModel(GameObject model, Transform transform, List<GameObject> list)
    {
        if (model == null)
            return null;
        var newModel = Instantiate(model, transform);
        newModel.transform.localPosition = Vector3.zero;
        newModel.transform.localEulerAngles = Vector3.zero;
        newModel.transform.localScale = Vector3.one;
        newModel.gameObject.SetActive(true);
        if (list != null)
            list.Add(newModel);
        return newModel;
    }
}
