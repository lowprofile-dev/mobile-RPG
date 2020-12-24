using UnityEngine;
using System.Collections;

public class CardUIView : View
{
    public override void UIExit()
    {
        base.UIExit();
    }

    public override void UIStart()
    {
        base.UIStart();
        CardManager.Instance.SetNewCard();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
    }
}
