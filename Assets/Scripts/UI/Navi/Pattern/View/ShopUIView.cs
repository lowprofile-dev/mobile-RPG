using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum Shoptype
{
    DUNGEONSHOP = 0,        //스킨 구매
    BLACKMARKET,            //재료(아이템?) 랜덤 가격 판매
    IMPERIALMARKET,         //재료 고정가격 판매
    ALCHEMIST               //강화석 판매
}

public class ShopUIView : View
{
    [SerializeField] public GameObject itemSlot;
    [SerializeField] public GameObject ShopTitle;
    [SerializeField] public GameObject PriceOrBudgetPanel;
    [SerializeField] public GameObject PurchaseOrSellPanel;
    [SerializeField] public GameObject itemBag;
    [SerializeField] public GameObject shopItemCardPrefab;
    [SerializeField] public GameObject[] cardslots;

    [SerializeField] public InventoryUIView inventoryUI;
    [SerializeField] public Transform content;
    [SerializeField] public Button PurchaseOrSellBtn;
    [SerializeField] public Button ExitShopButton;
    [SerializeField] public Shoptype currentShopType = Shoptype.IMPERIALMARKET;

    List<GameObject> itemSlots = new List<GameObject>();
    ItemManager itemManager;
    Transform[] iconList;

    public int currentCardIndex;

    private void Awake()
    {
        itemManager = ItemManager.Instance;
        iconList = inventoryUI.iconList;
        ExitShopButton.onClick.AddListener( delegate { OnClickExitShotButton(); });
    }

    private void Start()
    {
        itemManager = ItemManager.Instance;
        iconList = inventoryUI.iconList;
        EnterShopUI(Shoptype.IMPERIALMARKET);
    }

    private void Update()
    {
        if (itemManager == null) itemManager = ItemManager.Instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        itemBag.SetActive(false);
        itemManager.ResetCart();
        LoadPlayerInventory();
        LoadItemCards();
    }

    public void LoadItemCards()
    {
        for (int i = 0; i < cardslots.Length; i++)
        {
            GameObject card;
            if (cardslots[i].transform.childCount == 0)
            {
                card = Instantiate(shopItemCardPrefab, cardslots[i].transform);
                card.GetComponent<ShopItemCard>().shopUIView = this;
                card.GetComponent<ShopItemCard>().index = i;
                card.GetComponent<ShopItemCard>().InitShopItemCard();
            }
            else
            {
                card = cardslots[i].transform.GetChild(0).gameObject;
            }
            //var card = ObjectPoolManager.Instance.GetObject(shopItemCardPrefab, cardslots[i].transform.position, Quaternion.identity);

            if (itemManager.itemCart[i] != null)
            //if (itemManager.itemCart[i].id != 0)
            {
                card.GetComponent<ShopItemCard>().itemData = itemManager.itemCart[i];
                if (itemManager.itemCart[i].id > 0)
                    card.GetComponent<ShopItemCard>().SetIcon(iconList[itemManager.itemCart[i].id - 1]);
            }
            else
            {
                card.GetComponent<ShopItemCard>().ResetItemCard();
            }
        }
        SetTotalPrice();
    }

    private void OnClickExitShotButton()
    {
        UINaviationManager.Instance.PopToNav("SubUI_ShopBase");
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
    }

    public void LoadPlayerInventory()
    {
        ClearInventoryCache();
        if (itemManager == null) itemManager = ItemManager.Instance;
        foreach (var item in itemManager.playerInventory)
        {
            ItemData itemData = itemManager.itemDictionary[item.Key];
            ShopItemSlot slot = Instantiate(itemSlot, content).GetComponent<ShopItemSlot>();
            slot.InitShopItemSlot(this);
            slot.SetIcon(iconList[item.Key - 1]);
            slot.SetQuantity(item.Value);
            //slot.GetComponent<ShopItemSlot>().SetItemData(itemManager.itemDictionary[item.Key]);
            slot.GetComponent<ShopItemSlot>().SetItemData(itemManager.itemDictionary[item.Key]);
            itemSlots.Add(slot.gameObject);
            switch (itemData.itemgrade)
            {
                case 1:
                    slot.SetItemGrade(new Color(1, 1, 1, 0.15f));
                    break;
                case 2:
                    slot.SetItemGrade(new Color(0, 1, 0, 0.15f));
                    break;
                case 3:
                    slot.SetItemGrade(new Color(0, 0, 1, 0.15f));
                    break;
                case 4:
                    slot.SetItemGrade(new Color(153, 50, 204, 0.15f));
                    break;
                case 5:
                    slot.SetItemGrade(new Color(1, 0.92f, 0.016f, 0.15f));
                    break;
            }

        }
    }

    private void ClearInventoryCache()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Destroy(itemSlots[i]);
        }
        itemSlots.Clear();
    }

    // TODO : 상점 데이터 연동
    private void OnClickPurchaseInDungeon()
    {

    }

    private void OnClickSellInBlack()
    {

    }

    private void OnClickSellInImperial()
    {
        if (itemManager == null) itemManager = ItemManager.Instance;
        itemManager.SellItem();
        LoadItemCards();
        SetTotalPrice();
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/SellItem", 0.9f);
    }

    private void OnClickExtract()
    {

    }

    public void SetTotalPrice()
    {
        if (itemManager == null)
            itemManager = ItemManager.Instance;
        PriceOrBudgetPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = itemManager.GetTotalCartPrice().ToString();
    }

    public void EnterShopUI(Shoptype type)
    {
        PriceOrBudgetPanel.SetActive(true);
        PurchaseOrSellBtn.onClick.RemoveAllListeners();
        currentShopType = type;

        switch (type)
        {
            case Shoptype.DUNGEONSHOP:
                ShopTitle.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "던전 상점";
                PriceOrBudgetPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "보유 골드";
                PriceOrBudgetPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ItemManager.Instance.currentItems.gold.ToString();
                PurchaseOrSellPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "구매하기";
                PurchaseOrSellBtn.onClick.AddListener(delegate { OnClickPurchaseInDungeon(); });
                break;
            case Shoptype.BLACKMARKET:
                ShopTitle.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "암시장";
                PriceOrBudgetPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "판매 골드";
                PurchaseOrSellPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "판매하기";
                PurchaseOrSellBtn.onClick.AddListener(delegate { OnClickSellInBlack(); });
                break;
            case Shoptype.IMPERIALMARKET:
                ShopTitle.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "제국 상점";
                PriceOrBudgetPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "판매 골드";
                PurchaseOrSellPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "판매하기";
                PurchaseOrSellBtn.onClick.AddListener(delegate { OnClickSellInImperial(); });
                break;
            case Shoptype.ALCHEMIST:
                ShopTitle.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "연금술사";
                PriceOrBudgetPanel.SetActive(false);
                PurchaseOrSellPanel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "강화석 추출";
                PurchaseOrSellBtn.onClick.AddListener(delegate { OnClickExtract(); });
                break;
            default:
                break;
        }
    }

    public override void UIStart()
    {
        base.UIStart();
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickMedium01", 0.9f);
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
