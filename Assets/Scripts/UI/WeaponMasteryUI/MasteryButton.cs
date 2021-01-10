using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasteryButton : MonoBehaviour
{

    public void onButtonClick()
    {
        if (UINaviationManager.Instance.FindTargetIsInNav("SubUI_MasteryView"))
        {
            UINaviationManager.Instance.PopToNav("SubUI_MasteryView");
        }
        else
        {
            UINaviationManager.Instance.PushToNav("SubUI_MasteryView");
        }
    }
}
