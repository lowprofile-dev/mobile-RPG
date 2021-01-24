using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 사용되지 않음.
/// </summary>
public class UINavigation : MonoBehaviour
{
    Stack<GameObject> ViewManager;

    private void OnEnable()
    {
        
    }

    public void PushView(GameObject obj) 
    {
        ViewManager.Push(obj);
        obj.SetActive(true);
    }
}
