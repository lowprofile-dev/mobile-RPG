using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class InfoUIView : View
{
    [SerializeField] Button ExitBtn;
    // Start is called before the first frame update

    public override void UIStart()
    {
        base.UIStart();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    private void Awake()
    {
        ExitBtn.onClick.AddListener(delegate { OnClick(); });
    }

    private void OnClick()
    {
        UINaviationManager.Instance.PopToNav(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
