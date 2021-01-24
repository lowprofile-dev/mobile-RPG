////////////////////////////////////////////////////
/*
    File PartSelectionNpc.cs
    class PartSelectionNpc
    
    NPC중 Partselection을 사용해 부품을 조립하는 형식의 NPC
    
    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using UnityEngine;

public class PartSelectionNpc : NonLivingEntity
{
    private PartSelection _myPartSelection;

    [Header("부품 ID 선택")]
    [SerializeField] private int headId;
    [SerializeField] private int hairId;
    [SerializeField] private int headAccId;
    [SerializeField] private int chestId;
    [SerializeField] private int spineId;
    [SerializeField] private int spine2Id;
    [SerializeField] private int armsId;
    [SerializeField] private int legsId;
    [SerializeField] private int rWeaponId;
    [SerializeField] private int lWeaponId;
    [SerializeField] private int lShieldId;

    protected override void Start()
    {
        base.Start();
        _myPartSelection = GetComponent<PartSelection>();

        _myPartSelection.Start();
        _myPartSelection.Init();

        SetParts();
    }

    /// <summary>
    /// 파트를 변경해준다.
    /// </summary>
    private void SetParts()
    {
        if (headId != 0) _myPartSelection.ChangeHeadPart(headId);
        if (hairId != 0) _myPartSelection.ChangeHairPart(hairId);
        if (headAccId != 0) _myPartSelection.ChangeHeadAccesoriesPart(headAccId);
        if (chestId != 0) _myPartSelection.ChangeChestPart(chestId);
        if (spineId != 0) _myPartSelection.ChangeSpinePart(spineId);
        if (spine2Id != 0) _myPartSelection.ChangeLowerSpinePart(spine2Id);
        if (armsId != 0) _myPartSelection.ChangeArmParts(armsId);
        if (legsId != 0) _myPartSelection.ChangeLegParts(legsId);
        if (rWeaponId != 0) _myPartSelection.ChangeRightWeaponPart(rWeaponId);
        if (lWeaponId != 0) _myPartSelection.ChangeLeftWeaponPart(lWeaponId);
        if (lShieldId != 0) _myPartSelection.ChangeLeftShieldPart(lShieldId);
    }
}
