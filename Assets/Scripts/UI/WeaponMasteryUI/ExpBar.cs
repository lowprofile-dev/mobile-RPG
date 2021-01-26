using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////////////////////////////////////////////////
/*
    File ExpBar.cs
    class ExpBar

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////


public class ExpBar : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private string name;
    public static ExpBar instance;
    int saveExp = 0;
    /// <summary>
    /// 숙련도 경험치 최신화를 위한 업데이트
    /// </summary>

    private void Start()
    {
        instance = this;
    }
    void Update()
    {
        float ratio = ((float)WeaponManager.Instance._weaponDic[name].exp / (float)WeaponManager.Instance._weaponDic[name].expMax);
        if (image)
            image.fillAmount = ratio;
    }
}
