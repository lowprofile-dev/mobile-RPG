using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavationManager : SingletonBase<UINavationManager>
{
    Dictionary<string, Navigation> navigationDic;
    public GameObject viewObj;

    public void InitUINavigationManager()
    {
        viewObj = GameObject.FindGameObjectWithTag("View");

        navigationDic = new Dictionary<string, Navigation>();

        navigationDic["PlayerUI"] = new Navigation();
        navigationDic["SubUI"] = new Navigation();
        PushToNav("PlayerUI_View");
        //PushToNav("etcNav", "SubUI_View");
    }

    public void UpdateNavigationManager()
    {
        ControlCardUIView();
    }

    public void ControlCardUIView()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            PushToNav("SubUI_CardUIView");
        }

        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            PopToNav("SubUI_CardUIView");
        }
    }

    public View PushToNav(string viewName)
    {
        string[] key = viewName.Split('_');
        return navigationDic[key[0]].Push(viewName);
    }

    public View PopToNav(string viewName)
    {
        string[] key = viewName.Split('_');
        return navigationDic[key[0]].Pop(viewName);
    }

    public Navigation returnNav(string name)
    {
        return navigationDic[name];
    }
}
