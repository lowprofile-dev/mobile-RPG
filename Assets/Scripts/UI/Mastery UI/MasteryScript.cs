using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class MasteryScript : MonoBehaviour 
{
    [SerializeField] GameObject parent;
    [SerializeField] public Toggle upSkill; 
    [SerializeField] public Toggle downSkill;
    [SerializeField] GameObject masteryInfo;
    [SerializeField] GameObject upPanel;
    [SerializeField] GameObject downPanel;
    [SerializeField] ToggleGroup toggleGroup;

    MasteryManager masteryManager;
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
        InitCheck();
        ToggleCheck();
    }

    private void InitCheck()
    {
        if(MasteryManager.Instance != null && isInit == false)
        {
            masteryManager = MasteryManager.Instance;
            switch (gameObject.name)
            {
                case "Level5":
                  masteryData[0] = masteryManager.masteryDictionary[1];
                  masteryData[1] = masteryManager.masteryDictionary[2];
                    if (masteryManager.currentMastery.currentMasteryChoices[0] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if(masteryManager.currentMastery.currentMasteryChoices[0] == 1) downSkill.isOn = true ;
                  levelLimit = 5;
                  break;
                case "Level10":
                    masteryData[0] = masteryManager.masteryDictionary[3];
                    masteryData[1] = masteryManager.masteryDictionary[4];
                    if (masteryManager.currentMastery.currentMasteryChoices[1] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[1] == 1) downSkill.isOn = true;
                    levelLimit = 10; 
                    break;           
                case "Level15":      
                    masteryData[0] = masteryManager.masteryDictionary[5];
                    masteryData[1] = masteryManager.masteryDictionary[6];
                    if (masteryManager.currentMastery.currentMasteryChoices[2] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[2] == 1) downSkill.isOn = true;

                    levelLimit = 15; 
                    break;           
                case "Level20":      
                    masteryData[0] = masteryManager.masteryDictionary[7];
                    masteryData[1] = masteryManager.masteryDictionary[8];
                    if (masteryManager.currentMastery.currentMasteryChoices[3] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[3] == 1) downSkill.isOn = true;
                    levelLimit = 20; 
                    break;           
                case "Level25":      
                    masteryData[0] = masteryManager.masteryDictionary[9];
                    masteryData[1] = masteryManager.masteryDictionary[10];
                    if (masteryManager.currentMastery.currentMasteryChoices[4] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[4] == 1) downSkill.isOn = true;

                    levelLimit = 25; 
                    break;           
                case "Level30":      
                    masteryData[0] = masteryManager.masteryDictionary[11];
                    masteryData[1] = masteryManager.masteryDictionary[12];
                    if (masteryManager.currentMastery.currentMasteryChoices[5] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[5] == 1) downSkill.isOn = true;

                    levelLimit = 30; 
                    break;           
                case "Level35":      
                    masteryData[0] = masteryManager.masteryDictionary[13];
                    masteryData[1] = masteryManager.masteryDictionary[14];
                    if (masteryManager.currentMastery.currentMasteryChoices[6] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[6] == 1) downSkill.isOn = true;

                    levelLimit = 35; 
                    break;           
                case "Level40":      
                    masteryData[0] = masteryManager.masteryDictionary[15];
                    masteryData[1] = masteryManager.masteryDictionary[16];
                    if (masteryManager.currentMastery.currentMasteryChoices[7] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[7] == 1) downSkill.isOn = true;

                    levelLimit = 40; 
                    break;           
                case "Level45":      
                    masteryData[0] = masteryManager.masteryDictionary[17];
                    masteryData[1] = masteryManager.masteryDictionary[18];
                    if (masteryManager.currentMastery.currentMasteryChoices[8] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[8] == 1) downSkill.isOn = true;

                    levelLimit = 45; 
                    break;           
                case "Level50":      
                    masteryData[0] = masteryManager.masteryDictionary[19];
                    masteryData[1] = masteryManager.masteryDictionary[20];
                    if (masteryManager.currentMastery.currentMasteryChoices[9] == -1)
                    {
                        upSkill.isOn = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[9] == 1) downSkill.isOn = true;

                    levelLimit = 50;
                    break;
            }

            isInit = true;
        }
    }

    public void ToggleCheck()
    {
        if(SceneManager.GetActiveScene().name == "DungeonScene")
        {
            upPanel.SetActive(true);
            downPanel.SetActive(true);
        }
        else
        {
            if (levelLimit <= MasteryManager.Instance.currentMastery.currentMasteryLevel)
            {
                if(levelLimit >= 10)
                {
                    upPanel.SetActive(false);
                    downPanel.SetActive(false);
                }
                //else if(levelLimit >= 10)
                //{
                //    upPanel.SetActive(false);
                //    downPanel.SetActive(false);
                //}
                if (upSkill.isOn)
                {
                //    downPanel.SetActive(true);
                    masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = -1;
                }
                else if (downSkill.isOn)
                {
                 //   upPanel.SetActive(true);
                    masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = 1;
                }
                else
                {
                 //   upPanel.SetActive(false);
                 //   downPanel.SetActive(false);
                    masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = 0;
                }
                masteryManager.SaveCurrentMastery();
            }
            else
            {
                upPanel.SetActive(true);
                downPanel.SetActive(true);
            }
        }

    }

    public void UpSkillOn()
    {
        masteryInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = masteryData[0].masteryName;
        masteryInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = masteryData[0].masteryDescription;
        //masteryInfo.transform.position = upSkill.transform.position + new Vector3(0,-70);
        masteryInfo.transform.GetChild(1).GetComponent<Image>().sprite = upSkill.transform.GetChild(0).GetComponent<Image>().sprite;
        masteryInfo.SetActive(true);
    }
    public void UpSkillMouseOff()
    {
        masteryInfo.SetActive(false);
    }

    public void DownSkillOn()
    {
        masteryInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = masteryData[1].masteryName;
        masteryInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = masteryData[1].masteryDescription;
        masteryInfo.transform.GetChild(1).GetComponent<Image>().sprite = downSkill.transform.GetChild(0).GetComponent<Image>().sprite;

        //masteryInfo.transform.position = downSkill.transform.position + new Vector3(0, -70);
        masteryInfo.SetActive(true);

    }

    public void DownSkillMouseOff()
    {
        masteryInfo.SetActive(false);
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    ((IPointerClickHandler)upSkill).OnPointerClick(eventData);
    //}
    //
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    ((IPointerClickHandler)downSkill).OnPointerClick(eventData);
    //}
}
