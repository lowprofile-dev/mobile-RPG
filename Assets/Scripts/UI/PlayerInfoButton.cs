using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoButton : MonoBehaviour
{
    
    public void onCilckInfo()
    {
     //   Debug.Log("정보창클릭");
        UIManager.Instance.PlayerInfoClick();
    }
}
