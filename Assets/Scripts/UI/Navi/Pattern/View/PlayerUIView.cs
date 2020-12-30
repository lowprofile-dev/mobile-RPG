using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIView : View
{
    [SerializeField] private Button _cardTestBtn;
    [SerializeField] private Button _talkTestBtn;

    private void Start()
    {
        _cardTestBtn.onClick.AddListener(delegate { UINavationManager.Instance.ToggleCardUIView(); });
        _talkTestBtn.onClick.AddListener(delegate { UINavationManager.Instance.ToggleTalkView(); }); 
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    public override void UIStart()
    {
        base.UIStart();
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }
}
