using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MasteryScript : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] Toggle upSkill; 
    [SerializeField] Toggle downSkill;
    [SerializeField] GameObject masteryInfo;
    [SerializeField] GameObject upPanel;
    [SerializeField] GameObject downPanel;
    GameObject[] infoWindow;
    PlayerMasteryData[] masteryData;
    private bool isInit = false;
    private int levelLimit;
    // Start is called before the first frame update
    void Start()
    {
        upSkill = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>();
        downSkill = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Toggle>();
        masteryData = new PlayerMasteryData[2];
        levelLimit = 5;
        upPanel.SetActive(false);
        downPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        ToggleCheck();
        InitCheck();
    }

    private void InitCheck()
    {
        if(MasteryManager.Instance != null && isInit == false)
        {
            switch (gameObject.name)
            {
                case "Level5":
                  masteryData[0] = MasteryManager.Instance.masteryDictionary[1];
                  masteryData[1] = MasteryManager.Instance.masteryDictionary[2];
                  levelLimit = 5;
                  break;
                case "Level10":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[3];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[4];
                    levelLimit = 10;
                    break;
                case "Level15":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[5];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[6];
                    levelLimit = 15;
                    break;
                case "Level20":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[7];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[8];
                    levelLimit = 20;
                    break;
                case "Level25":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[9];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[10];
                    levelLimit = 25;
                    break;
                case "Level30":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[11];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[12];
                    levelLimit = 30;
                    break;
                case "Level35":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[13];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[14];
                    levelLimit = 35;
                    break;
                case "Level40":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[15];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[16];
                    levelLimit = 40;
                    break;
                case "Level45":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[17];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[18];
                    levelLimit = 45;
                    break;
                case "Level50":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[19];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[20];
                    levelLimit = 50;
                    break;
            }

            isInit = true;
        }
    }

    public void ToggleCheck()
    {
        if(levelLimit <= MasteryManager.Instance.currentMastery.currentMasteryLevel)
        {
            if (upSkill.isOn)
            {
                downPanel.SetActive(true);
            }
            else if (downSkill.isOn)
            {
                upPanel.SetActive(true);
            }
            else
            {
                upPanel.SetActive(false);
                downPanel.SetActive(false);
            }
        }
        else
        {
            upPanel.SetActive(true);
            downPanel.SetActive(true);
        }

    }
    public void UpSkillMouseOn()
    {
        masteryInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = masteryData[0].masteryName;
        masteryInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = masteryData[0].masteryDescription;
        masteryInfo.transform.position = upSkill.transform.position + new Vector3(0,-70);
        masteryInfo.SetActive(true);
    }
    public void UpSkillMouseOff()
    {
        masteryInfo.SetActive(false);
    }

    public void DownSkillMouseOn()
    {
        masteryInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = masteryData[1].masteryName;
        masteryInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = masteryData[1].masteryDescription;

        masteryInfo.transform.position = downSkill.transform.position + new Vector3(0, -70);
        masteryInfo.SetActive(true);

    }

    public void DownSkillMouseOff()
    {
        masteryInfo.SetActive(false);
    }
}
