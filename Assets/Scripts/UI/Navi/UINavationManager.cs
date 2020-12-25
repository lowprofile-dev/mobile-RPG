using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavationManager : SingletonBase<UINavationManager>
{
    Dictionary<string, Navigation> navigationDic;
    public GameObject viewObj;

    void Start()
    {
        viewObj = GameObject.FindGameObjectWithTag("View");

        navigationDic = new Dictionary<string, Navigation>();

        navigationDic["PlayerUI"] = new Navigation();
        navigationDic["SubUI"] = new Navigation();
        PushToNav("PlayerUI_View");
        //PushToNav("SubUI_CardUIView");
        //PushToNav("etcNav", "SubUI_View");
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
