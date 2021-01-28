////////////////////////////////////////////////////
/*
    File UINavigationManager.cs
    class UINavigationManager
    
    담당자 : 이신홍
    부 담당자 : 

    Navigation 구조를 위한 UINavigation 매니저
    네비게이션 하나는 하나의 View만 표출할 수 있으며, 이는 스택으로 쌓여, 닫을 시에는 이전에 열어놓았던 뷰가 열리게 된다.
*/
////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

public class UINaviationManager : SingletonBase<UINaviationManager>
{
    Dictionary<string, Navigation> navigationDic;   // Navigation 목록
    public GameObject viewObj;                      // View들이 모여있는 오브젝트

    
    public void InitUINavigationManager()
    {
        viewObj = GameObject.FindGameObjectWithTag("View");

        navigationDic = new Dictionary<string, Navigation>();

        navigationDic["PlayerUI"] = new Navigation();   // 메인이 되는 UI이다. (항상 떠있음)
        navigationDic["SubUI"] = new Navigation();      // 서브가 되는 다양한 UI이다.

        PushToNav("PlayerUI_View");
    }

    /// <summary>
    /// 해당 네비게이션의 뷰를 업데이트한다.
    /// </summary>
    public void UpdateNavigationManager()
    {
        navigationDic["PlayerUI"].UpdateNavigation();
        navigationDic["SubUI"].UpdateNavigation();
    }


    /// <summary>
    /// 네비게이션 안에 해당 뷰가 있는지 찾는다.
    /// </summary>
    public bool FindTargetIsInNav(string viewName)
    {
        string[] key = viewName.Split('_'); // 네비게이션이름_뷰이름으로 입력된다.
        return navigationDic[key[0]].Find(viewName);
    }

    /// <summary>
    /// 네비게이션에 해당 뷰를 넣는다.
    /// </summary>
    public View PushToNav(string viewName)
    {
        string[] key = viewName.Split('_');
        return navigationDic[key[0]].Push(viewName);
    }

    /// <summary>
    /// 네비게이션에서 해당 뷰를 뺀다.
    /// </summary>
    public View PopToNav(string viewName)
    {
        string[] key = viewName.Split('_');
        return navigationDic[key[0]].Pop(viewName);
    }

    public Navigation returnNav(string name)
    {
        return navigationDic[name];
    }



    //////////// 씬 별 UI 토글 /////////////

    /// <summary>
    /// 해당 SubUIView를 켠다. (다만 이미 네비게이션에 존재할 시엔 무시)
    /// </summary>
    public void OpenSubUIView(string name)
    {
        string viewName = "SubUI_" + name;
        if (FindTargetIsInNav(viewName)) return;
        else PushToNav(viewName);
    }

    public void ToggleCardUIView()
    {
        if (FindTargetIsInNav("SubUI_CardUIView")) PopToNav("SubUI_CardUIView");
        else PushToNav("SubUI_CardUIView");
    }

    internal void ToggleShopView()
    {
        if (FindTargetIsInNav("SubUI_ShopBase")) PopToNav("SubUI_ShopBase");
        else PushToNav("SubUI_ShopBase");

    }

    internal void TogglePurchaseShopView()
    {
        if (FindTargetIsInNav("SubUI_PurchaseShopUIView"))
            PopToNav("SubUI_PurchaseShopUIView");
        else PushToNav("SubUI_PurchaseShopUIView");
    }

    internal void ToggleWeaponView()
    {
        if (FindTargetIsInNav("SubUI_WeaponMasteryView")) PopToNav("SubUI_WeaponMasteryView");
        else PushToNav("SubUI_WeaponMasteryView");
    }

    internal void ToggleMasteryView()
    {
        if (FindTargetIsInNav("SubUI_MasteryView")) PopToNav("SubUI_MasteryView");
        else PushToNav("SubUI_MasteryView");
    }

    public void ToggleTalkView()
    {
        if (FindTargetIsInNav("PlayerUI_TalkUIView")) PopToNav("PlayerUI_TalkUIView");
        else PushToNav("PlayerUI_TalkUIView");
    }

}
