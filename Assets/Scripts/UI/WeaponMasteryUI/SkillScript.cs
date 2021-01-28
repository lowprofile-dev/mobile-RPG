using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

////////////////////////////////////////////////////
/*
    File SkillScript.cs
    class SkillScript

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////
///
public class SkillScript : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private bool isLock;
    [SerializeField] private Button skillButton;
    [SerializeField] private TextMeshProUGUI skillLevelText;
    [SerializeField] private RawImage skillLockImage;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private GameObject upgradeWindow;
    [SerializeField] GameObject outofWindow;

    private GameObject window;
    private Button ok;
    private Button cancle;
    private UpGradeContents upGradeContents;
    public bool skillReleased = false;

    public int skillLevel = 0;

    // Start is called before the first frame update
    /// <summary>
    /// 시작할때 스킬의 해금 여부를 체크한 후에
    /// 스킬 잠금 이미지와 텍스트 및 버튼의 동작 여부를 초기화 해준다.
    /// </summary>
    void Start()
    {
        if (isLock)
        {
            skillLevelText.gameObject.SetActive(false);
            skillLockImage.gameObject.SetActive(true);
            skillButton.enabled = false;
        }
        else
        {
            skillLevelText.gameObject.SetActive(true);
            skillLockImage.gameObject.SetActive(false);
            skillButton.enabled = true;

        }
        window = Instantiate(upgradeWindow, parent.transform);
        upGradeContents = window.GetComponent<UpGradeContents>();
        ok = window.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(0).GetComponent<Button>();
        cancle = window.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(1).GetComponent<Button>();

        outofWindow.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { testoff(); });
        ok.onClick.AddListener(delegate { testok(); });
        cancle.onClick.AddListener(delegate { testoff(); });
        skillButton.onClick.AddListener(delegate { teston(); });
    }

    /// <summary>
    /// 
    /// 강화창에서 OK 버튼을 눌렀을 경우에 동작을 임의로 작성한 함수
    /// 
    /// </summary>
    private void testok()
    {
        if(500+skillLevel*100 <= ItemManager.Instance.currentItems.gold)
        {
            ItemManager.Instance.currentItems.gold -= 500 + skillLevel * 100;
            upGradeContents.SetPlayerMoneyText(ItemManager.Instance.currentItems.gold);
            skillLevel++;
            testoff();
        }
        else
        {
            SystemPanel.instance.SetText("강화 할 돈이 부족합니다.");
            SystemPanel.instance.FadeOutStart();
        }
    }

    /// <summary>
    /// 
    /// 스킬의 이미지를 클릭했을 경우
    /// 강화창이 팝업되고, 그에 따른 텍스트들을 임의로 결정해주는 함수.
    /// 
    /// </summary>
    public void teston()
    {
        outofWindow.SetActive(true);
        window.SetActive(true);
        upGradeContents.SetUpgradeMoneyText(skillLevel, 500 + skillLevel * 100);
        upGradeContents.SetPlayerMoneyText(ItemManager.Instance.currentItems.gold);
    }

    /// <summary>
    /// 
    /// 강화창을 닫아주는 함수
    /// 
    /// </summary>
    public void testoff()
    {
        outofWindow.SetActive(false);
        window.SetActive(false);
    }

    /// <summary>
    /// 
    /// 스킬의 레벨을 출력해주는 함수
    /// 
    /// </summary>
    public void SkillLevelPrint()
    {
        skillLevelText.text = "Lv." + skillLevel;
    }

    /// <summary>
    /// 
    /// 스킬의 해금 조건이 만족되었을 경우 해금을 풀어주는 함수 
    /// 
    /// </summary>
    public void SkillUnLock()
    {
        skillLockImage.gameObject.SetActive(false);
        skillLevelText.gameObject.SetActive(true);
        skillReleased = true;
        isLock = false;
        skillButton.enabled = true;
    }
}
