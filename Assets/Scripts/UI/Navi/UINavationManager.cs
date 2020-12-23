using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavationManager : SingletonBase<UINavationManager>
{
    Dictionary<string, Navigation> navigationDic;
    public Canvas canvas;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("View").GetComponent<Canvas>();

        navigationDic = new Dictionary<string, Navigation>();

        navigationDic["playerInfoNav"] = new Navigation();
        navigationDic["etcNav"] = new Navigation();

        PushToNav("playerInfoNav", "PlayerUI_View");
        //PushToNav("etcNav", "SubUI_View");
    }

    public View PushToNav(string name, string viewName)
    {
        return navigationDic[name].Push(viewName);
    }

    public View PopToNav(string name, string viewName)
    {
        return navigationDic[name].Pop(viewName);
    }

    public Navigation returnNav(string name)
    {
        return navigationDic[name];
    }
}
