using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VIEWTYPE
{
    VIEWTYPE_ISAPPEARING, VIEWTYPE_APPEARED, VIEWTYPE_ISDISAPPEARING, VIEWTYPE_DISAPPEARED
}

public class View : MonoBehaviour
{
    protected VIEWTYPE viewtype;
    [SerializeField] private string _viewName; public string viewName { get { return _viewName; } }

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

        // 애니메이션 구현

        // 끝났으면

        viewtype = VIEWTYPE.VIEWTYPE_APPEARED;
    }

    public virtual void UIUpdate()
    {

    }

    public virtual void UIExit()
    {
        viewtype = VIEWTYPE.VIEWTYPE_ISDISAPPEARING;

        // 애니메이션 구현

        // 끝났으면

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
}
