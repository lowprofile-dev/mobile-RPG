using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class InfoUIView : View
{
    [SerializeField] Button ExitBtn;

    [SerializeField] GameObject Hat_Slot;
    [SerializeField] GameObject Armor_Slot;
    [SerializeField] GameObject Glove_Slot;
    [SerializeField] GameObject Pant_Slot;
    [SerializeField] GameObject Shoes_Slot;

    [SerializeField] GameObject itemDetail;
    public Transform[] iconList;
    ItemManager itemManager;
    // Start is called before the first frame update

    public override void UIStart()
    {
        base.UIStart();

        FindIconList();
        LoadCurrentEquip();
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/OpenInventory", 0.9f);
    }

    public override void UIUpdate()
    {
        base.UIUpdate();
    }

    public override void UIExit()
    {
        base.UIExit();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Awake()
    {
        itemManager = ItemManager.Instance;
        ExitBtn.onClick.AddListener(delegate { OnClick(); });
    }

    private void OnClick()
    {
        UINaviationManager.Instance.PopToNav(name);
        SoundManager.Instance.PlayEffect(SoundType.UI, "UI/ClickLightBase2", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadCurrentEquip()
    {
        //Hat_Slot.GetComponent<EquipSlot>().SetIcon(ItemManager.Instance.currentItemKeys.HelmetKey)

        EquipSlot hat = Hat_Slot.GetComponent<EquipSlot>();
        hat.SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.HelmetKey]);
        hat.SetItemDetail(itemDetail);
        //itemDetail.transform.position = Hat_Slot.transform.position + new Vector3(20f, 0f, 0f);
        hat.SetIcon(iconList[itemManager.currentItemKeys.HelmetKey-1]);

        EquipSlot armor = Armor_Slot.GetComponent<EquipSlot>();
        armor.SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.ArmorKey]);
        armor.SetItemDetail(itemDetail);
        //itemDetail.transform.position = Armor_Slot.transform.position + new Vector3(30f, 0f, 0f);
        armor.SetIcon(iconList[itemManager.currentItemKeys.ArmorKey - 1]);

        EquipSlot glove = Glove_Slot.GetComponent<EquipSlot>();
        glove.SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.GlovesKey]);
        glove.SetItemDetail(itemDetail);
        //itemDetail.transform.position = Glove_Slot.transform.position + new Vector3(40f, 0f, 0f);
        glove.SetIcon(iconList[itemManager.currentItemKeys.GlovesKey - 1]);

        EquipSlot pant = Pant_Slot.GetComponent<EquipSlot>();
        pant.SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.BottomKey]);
        pant.SetItemDetail(itemDetail);
        //itemDetail.transform.position = Pant_Slot.transform.position + new Vector3(50f, 0f, 0f);
        pant.SetIcon(iconList[itemManager.currentItemKeys.BottomKey - 1]);

        EquipSlot shoes = Shoes_Slot.GetComponent<EquipSlot>();
        shoes.SetItemData(itemManager.itemDictionary[itemManager.currentItemKeys.BootKey]);
        shoes.SetItemDetail(itemDetail);
        //itemDetail.transform.position = Shoes_Slot.transform.position + new Vector3(60f, 0f, 0f);
        shoes.SetIcon(iconList[itemManager.currentItemKeys.BootKey - 1]);
    }

    private void FindIconList()
    {
        int i = 5;
        string[] strarr = new string[5];
        strarr[0] = "Armors";
        strarr[1] = "Boots";
        strarr[2] = "Bottoms";
        strarr[3] = "Gloves";
        strarr[4] = "Helmets";
        iconList = new Transform[125];

        iconList[0] = Resources.Load<GameObject>("Prefab/Items/0Backup/Armors/Basic Armor").transform;
        iconList[1] = Resources.Load<GameObject>("Prefab/Items/0Backup/Bottoms/Basic Bottom").transform;
        iconList[2] = Resources.Load<GameObject>("Prefab/Items/0Backup/Helmets/Basic Helmet").transform;
        iconList[3] = Resources.Load<GameObject>("Prefab/Items/0Backup/Gloves/Basic Gloves").transform;
        iconList[4] = Resources.Load<GameObject>("Prefab/Items/0Backup/Boots/Basic Boot").transform;
        
        while (i < 125)
        {
            GameObject temp = null;
            if (i < 9)
            {
                for (int j = 0; j < 5; j++)
                {

                    temp = Resources.Load<GameObject>("Prefab/Items/" + strarr[j] + "/id0" + (i + 1).ToString());
                    if (temp != null)
                    {
                        iconList[i] = temp.transform;
                        i++;
                        break;
                    }
                }
            }
            else
            {
                for (int j = 0; j < 5; j++)
                {

                    temp = Resources.Load<GameObject>("Prefab/Items/" + strarr[j] + "/id" + (i + 1).ToString());
                    if (temp != null)
                    {
                        iconList[i] = temp.transform;
                        i++;
                        break;
                    }
                }
            }

        }
    }
}
