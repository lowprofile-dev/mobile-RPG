using System.Collections.Generic;
using UnityEngine;

public class UINaviationManager : SingletonBase<UINaviationManager>
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
    }

    public void UpdateNavigationManager()
    {
        navigationDic["PlayerUI"].UpdateNavigation();
        navigationDic["SubUI"].UpdateNavigation();
    }

    public void ToggleCardUIView()
    {
        if(FindTargetIsInNav("SubUI_CardUIView"))
        {
            PopToNav("SubUI_CardUIView");
        }
        else
        {
            PushToNav("SubUI_CardUIView");
        }
    }

    public void ToggleTalkView()
    {
        if (FindTargetIsInNav("PlayerUI_TalkUIView"))
        {
            Debug.Log("HELLO");
            PopToNav("PlayerUI_TalkUIView");
        }
        else
        {
            Debug.Log("NO HELLO");
            PushToNav("PlayerUI_TalkUIView");
        }
    }

    public bool FindTargetIsInNav(string viewName)
    {
        string[] key = viewName.Split('_');
        return navigationDic[key[0]].Find(viewName);
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
