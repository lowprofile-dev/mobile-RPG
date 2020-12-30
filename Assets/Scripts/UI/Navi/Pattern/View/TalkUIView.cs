﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkUIView : View
{
    public override void UIStart()
    {
        base.UIStart();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();

        if(Input.GetMouseButtonDown(0))
        {
            UINavationManager.Instance.ToggleTalkView();
        }
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}