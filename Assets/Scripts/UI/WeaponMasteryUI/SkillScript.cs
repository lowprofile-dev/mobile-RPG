using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    void Start()
    {
        if(isLock)
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
        ok = window.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<Button>();
        cancle = window.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetComponent<Button>();

        outofWindow.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { testoff(); });
        ok.onClick.AddListener(delegate { testok(); });
        cancle.onClick.AddListener(delegate { testoff(); });
        skillButton.onClick.AddListener(delegate { teston(); });


    }

    private void testok()
    {
        skillLevel++;
        testoff();
    }

    public void teston()
    {
        outofWindow.SetActive(true);
        window.SetActive(true);
        upGradeContents.SetText(skillLevel, 5000 + skillLevel * 1000);
    }

    public void testoff()
    {
        outofWindow.SetActive(false);
        window.SetActive(false);
    }

    public void SkillLevelPrint()
    {
        skillLevelText.text = "Lv." + skillLevel;
    }

    public void SkillUnLock()
    {
        skillLockImage.gameObject.SetActive(false);
        skillLevelText.gameObject.SetActive(true);
        skillReleased = true;
        isLock = false;
        skillButton.enabled = true;
    }
}
