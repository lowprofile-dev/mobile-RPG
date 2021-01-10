using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButton : MonoBehaviour
{

    public void onButtonClick()
    {
        if (UINaviationManager.Instance.FindTargetIsInNav("SubUI_WeaponMasteryView"))
        {
            UINaviationManager.Instance.PopToNav("SubUI_WeaponMasteryView");
        }
        else
        {
            UINaviationManager.Instance.PushToNav("SubUI_WeaponMasteryView");
        }
    }
}
