using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponButton : MonoBehaviour
{

    public void onButtonClick()
    {

       UINaviationManager.Instance.PushToNav("SubUI_WeaponMasteryView");

    }
}
