using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
