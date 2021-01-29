using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseShopUIView : View
{
    [SerializeField] private TextMeshProUGUI _gold;
    [SerializeField] private TextMeshProUGUI _coin;
    [SerializeField] private TextMeshProUGUI _gem;

    [SerializeField] private Button _returnBtn;
    [SerializeField] private Button _purchaseCoinBtn;
    [SerializeField] private Button _purchaseGemBtn;
    [SerializeField] private Button _purchaseLowClassRandItemBtn;
    [SerializeField] private Button _purchaseHighClassRandItemBtn;
    [SerializeField] private Button _purchaseGoldBtn;
    [SerializeField] private Button _purchaseGoldTripleBtn;

    private void Start()
    {
        SetAsset();

        _returnBtn.onClick.AddListener(delegate { ReturnToMain(); } );
        _purchaseCoinBtn.onClick.AddListener(delegate { PurchaseCoin(); } );
        _purchaseGemBtn.onClick.AddListener(delegate { PurchaseGem(); } );
        _purchaseGemBtn.onClick.AddListener(delegate { PurchaseGem(); } );
        _purchaseLowClassRandItemBtn.onClick.AddListener(delegate { PurchaseLowClassItem(); } );
        _purchaseHighClassRandItemBtn.onClick.AddListener(delegate { PurchaseHighClassItem(); } );
        _purchaseGoldBtn.onClick.AddListener(delegate { PurchaseGold(); } );
        _purchaseGoldTripleBtn.onClick.AddListener(delegate { PurchaseGold(); } );
    }

    private void Update()
    {
        SetAsset();
    }

    private void SetAsset()
    {
        _gold.text = ItemManager.Instance.currentItems.gold.ToString();
        _coin.text = ItemManager.Instance.currentItems.coin.ToString();
        _gem.text = ItemManager.Instance.currentItems.gem.ToString();
    }

    private void ReturnToMain()
    {
        UINaviationManager.Instance.PopToNav("SubUI_PurchaseShopUIView");
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
    }

    private void PurchaseCoin()
    {
        if (ItemManager.Instance.currentItems.gold >= 100)
        {
            ItemManager.Instance.AddGold(-100);
            ItemManager.Instance.AddCoin(1);
            SetAsset();
            SystemPanel.instance.SetText( "코인 구매 성공!");
            SystemPanel.instance.FadeOutStart();
        }
        else
        {
            SystemPanel.instance.SetText("골드가 부족합니다");
            SystemPanel.instance.FadeOutStart();
        }
    }

    private void PurchaseGem()
    {
        if (ItemManager.Instance.currentItems.gold >= 500)
        {
            ItemManager.Instance.AddGold(-500);
            ItemManager.Instance.AddGem(1);
            SetAsset();
            SystemPanel.instance.SetText("강화석 구매 성공!");
            SystemPanel.instance.FadeOutStart();
        }
        else
        {
            SystemPanel.instance.SetText("골드가 부족합니다");
            SystemPanel.instance.FadeOutStart();
        }
    }

    private void PurchaseLowClassItem()
    {
        if (ItemManager.Instance.currentItems.gold >= 500)
        {
            ItemManager.Instance.AddGold(-500);
            ItemData itemData = ItemManager.Instance.lowClassItems[UnityEngine.Random.Range(0, ItemManager.Instance.lowClassItems.Count)];
            ItemManager.Instance.AddItem(itemData);
            SetAsset();
            SystemPanel.instance.SetText("일반 아이템 구매 성공!");
            SystemPanel.instance.FadeOutStart();
        }
        else
        {
            SystemPanel.instance.SetText("골드가 부족합니다");
            SystemPanel.instance.FadeOutStart();
        }
    }

    private void PurchaseHighClassItem()
    {
        if (ItemManager.Instance.currentItems.gold >= 1000)
        {
            ItemManager.Instance.AddGold(-1000);
            ItemData itemData = ItemManager.Instance.highClassItems[UnityEngine.Random.Range(0, ItemManager.Instance.highClassItems.Count)];
            ItemManager.Instance.AddItem(itemData);
            SetAsset();
            SystemPanel.instance.SetText("고급 아이템 구매 성공!");
            SystemPanel.instance.FadeOutStart();
        }
        else
        {
            SystemPanel.instance.SetText("골드가 부족합니다");
            SystemPanel.instance.FadeOutStart();
        }
    }

    private void PurchaseGold()
    {
        SystemPanel.instance.SetText("현재 구매가 불가합니다.");
        SystemPanel.instance.FadeOutStart();
    }

    public override void UIStart()
    {
        base.UIStart();
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/OpenShop", 0.9f);
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public override void UIExit()
    {
        base.UIExit();
    }
}
