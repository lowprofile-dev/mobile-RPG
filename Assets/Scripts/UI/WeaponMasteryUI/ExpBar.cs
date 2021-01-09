using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private string name;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        float ratio = ((float) WeaponManager.Instance._weaponDic[name].exp / (float)WeaponManager.Instance._weaponDic[name].expMax);
        if (image)
            image.fillAmount = ratio;
    }
}
