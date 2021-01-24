﻿////////////////////////////////////////////////////
/*
    File View.cs
    class View : UI 페이지의 단위. 캔버스 단위로 관리된다.
    enum VIEWTYPE 

    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum VIEWTYPE
{
    VIEWTYPE_ISAPPEARING, VIEWTYPE_APPEARED, VIEWTYPE_ISDISAPPEARING, VIEWTYPE_DISAPPEARED
}

public class View : MonoBehaviour
{
    protected VIEWTYPE viewtype;
    [SerializeField] private string _viewName;

    // 튜토리얼 관련
    [SerializeField] protected Button tutorialButton;
    [SerializeField] protected Button tutorialExitButton;
    [SerializeField] protected GameObject tutorialPage;

    // property
    public string viewName { get { return _viewName; } }



    private void OnEnable()
    {
        _viewName = gameObject.name;
    }

    void Update()
    {
        switch (viewtype)
        {
            case VIEWTYPE.VIEWTYPE_ISAPPEARING:
                break;
            case VIEWTYPE.VIEWTYPE_APPEARED:
                UIUpdate();
                break;
            case VIEWTYPE.VIEWTYPE_ISDISAPPEARING:
                UIUpdate();
                break;
            case VIEWTYPE.VIEWTYPE_DISAPPEARED:
                break;
            default:
                break;
        }
    }

    public virtual void UIStart()
    {
        gameObject.SetActive(true);
        viewtype = VIEWTYPE.VIEWTYPE_ISAPPEARING;
        
        viewtype = VIEWTYPE.VIEWTYPE_APPEARED;
    }

    public virtual void UIUpdate()
    {

    }

    public virtual void UIExit()
    {
        viewtype = VIEWTYPE.VIEWTYPE_ISDISAPPEARING;

        viewtype = VIEWTYPE.VIEWTYPE_DISAPPEARED;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        UIStart();
    }

    public void Hide()
    {
        UIExit();
    }

    protected virtual void TutorialClick(){
        tutorialPage.SetActive(true);
    }
    protected virtual void TutorialExit(){
        tutorialPage.SetActive(false);
    }
}
