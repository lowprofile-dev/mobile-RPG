using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

////////////////////////////////////////////////////
/*
    File MasteryScript.cs
    class MasteryScript

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////


public class MasteryScript : MonoBehaviour
{
    public static int offLevel;
    [SerializeField] GameObject parent;
    [SerializeField] public Toggle upSkill; 
    [SerializeField] public Toggle downSkill;
    [SerializeField] GameObject masteryInfo;
    [SerializeField] GameObject upPanel;
    [SerializeField] GameObject downPanel;
    [SerializeField] ToggleGroup toggleGroup;
    private bool clicked = false;
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
        upSkill.onValueChanged.AddListener(delegate { ToggleChange(upSkill); });
        downSkill.onValueChanged.AddListener(delegate { ToggleChange(downSkill); });
    }

    public void ToggleChange(Toggle temp)
    {
        if(temp != null && isInit == true)
        {
            if (temp == upSkill && temp.isOn) masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = -1;
            else if (temp == downSkill && temp.isOn) masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = 1;
            else masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = 0;
            MasteryManager.Instance.MasteryApply();
        }
    }
    // Update is called once per frame
    void Update()
    {
        InitCheck();
        ToggleCheck();
    }

    /// <summary>
    /// 각 마스터리 스킬 별 이름과 내용, 해금 레벨을 초기화 해주는 함수  
    /// </summary>
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
                        MasteryManager.Instance.masterySet[0, 0] = true;
                        MasteryManager.Instance.masterySet[0, 1] = false;
                        upSkill.isOn = true;
                        clicked = true;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[0] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[0, 0] = false;
                        MasteryManager.Instance.masterySet[0, 1] = true;
                    }
                    else clicked = false;
                  levelLimit = 5;
                  break;
                case "Level10":
                    masteryData[0] = masteryManager.masteryDictionary[3];
                    masteryData[1] = masteryManager.masteryDictionary[4];
                    if (masteryManager.currentMastery.currentMasteryChoices[1] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[1, 0] = true;
                        MasteryManager.Instance.masterySet[1, 1] = false;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[1] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[1, 0] = false;
                        MasteryManager.Instance.masterySet[1, 1] = true;
                    }
                    else clicked = false;
                        levelLimit = 10; 
                    break;           
                case "Level15":      
                    masteryData[0] = masteryManager.masteryDictionary[5];
                    masteryData[1] = masteryManager.masteryDictionary[6];
                    if (masteryManager.currentMastery.currentMasteryChoices[2] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[2, 0] = true;
                        MasteryManager.Instance.masterySet[2, 1] = false;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[2] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[2, 0] = false;
                        MasteryManager.Instance.masterySet[2, 1] = true;
                    }
                    else clicked = false;
                    levelLimit = 15; 
                    break;           
                case "Level20":      
                    masteryData[0] = masteryManager.masteryDictionary[7];
                    masteryData[1] = masteryManager.masteryDictionary[8];
                    if (masteryManager.currentMastery.currentMasteryChoices[3] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[3, 0] = true;
                        MasteryManager.Instance.masterySet[3, 1] = false;

                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[3] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[3, 0] = false;
                        MasteryManager.Instance.masterySet[3, 1] = true;

                    }
                    else clicked = false;
                        levelLimit = 20; 
                    break;           
                case "Level25":      
                    masteryData[0] = masteryManager.masteryDictionary[9];
                    masteryData[1] = masteryManager.masteryDictionary[10];
                    if (masteryManager.currentMastery.currentMasteryChoices[4] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[4, 0] = true;
                        MasteryManager.Instance.masterySet[4, 1] = false;

                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[4] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[4, 0] = false;
                        MasteryManager.Instance.masterySet[4, 1] = true;
                    }
                    else clicked = false;
                    levelLimit = 25; 
                    break;           
                case "Level30":      
                    masteryData[0] = masteryManager.masteryDictionary[11];
                    masteryData[1] = masteryManager.masteryDictionary[12];
                    if (masteryManager.currentMastery.currentMasteryChoices[5] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[5, 0] = true;
                        MasteryManager.Instance.masterySet[5, 1] = false;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[5] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[5, 0] = false;
                        MasteryManager.Instance.masterySet[5, 1] = true;
                    }
                    else clicked = false;
                    levelLimit = 30; 
                    break;           
                case "Level35":      
                    masteryData[0] = masteryManager.masteryDictionary[13];
                    masteryData[1] = masteryManager.masteryDictionary[14];
                    if (masteryManager.currentMastery.currentMasteryChoices[6] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[6, 0] = true;
                        MasteryManager.Instance.masterySet[6, 1] = false;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[6] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[6, 0] = false;
                        MasteryManager.Instance.masterySet[6, 1] = true;
                    }
                    else clicked = false;
                    levelLimit = 35; 
                    break;           
                case "Level40":      
                    masteryData[0] = masteryManager.masteryDictionary[15];
                    masteryData[1] = masteryManager.masteryDictionary[16];
                    if (masteryManager.currentMastery.currentMasteryChoices[7] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[7, 0] = true;
                        MasteryManager.Instance.masterySet[7, 1] = false;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[7] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[7, 0] = false;
                        MasteryManager.Instance.masterySet[7, 1] = true;
                    }
                    else clicked = false;
                    levelLimit = 40; 
                    break;           
                case "Level45":      
                    masteryData[0] = masteryManager.masteryDictionary[17];
                    masteryData[1] = masteryManager.masteryDictionary[18];
                    if (masteryManager.currentMastery.currentMasteryChoices[8] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[8, 0] = true;
                        MasteryManager.Instance.masterySet[8, 1] = false;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[8] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[8, 0] = false;
                        MasteryManager.Instance.masterySet[8, 1] = true;
                    }
                    else clicked = false;
                    levelLimit = 45; 
                    break;           
                case "Level50":      
                    masteryData[0] = masteryManager.masteryDictionary[19];
                    masteryData[1] = masteryManager.masteryDictionary[20];
                    if (masteryManager.currentMastery.currentMasteryChoices[9] == -1)
                    {
                        upSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[9, 0] = true;
                        MasteryManager.Instance.masterySet[9, 1] = false;
                    }
                    else if (masteryManager.currentMastery.currentMasteryChoices[9] == 1)
                    {
                        downSkill.isOn = true;
                        clicked = true;
                        MasteryManager.Instance.masterySet[9, 0] = false;
                        MasteryManager.Instance.masterySet[9, 1] = true;
                    }
                    else clicked = false;
                    levelLimit = 50;
                    break;
            }
            isInit = true;
        }
    }

    /// <summary>
    /// 해금 레벨이 안되었을 경우 버튼의 입력을 패널을 추가하여 막아주고,
    /// 해금 레벨이 되었을 경우 패널을 없애서 입력을 받을 수 있도록 해주는 함수 
    /// </summary>
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
                if(levelLimit >= 5)
                {
                    upPanel.SetActive(false);
                    downPanel.SetActive(false);
                }

                if (upSkill.isOn)
                {
                    //if (masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] == 1)
                    //{
                    //    //Player.Instance.masterySet[(levelLimit - 1) / 5] = false;
                    //}
                    //masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = -1;
                }
                else if (downSkill.isOn)
                {
                    //if (masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] == -1)
                    //{
                    //    //Player.Instance.masterySet[(levelLimit - 1) / 5] = false;
                    //}
                    //masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = 1;
                }
                else
                {
                    //masteryManager.currentMastery.currentMasteryChoices[(levelLimit - 1) / 5] = 0;
                    //Player.Instance.masterySet[(levelLimit - 1) / 5] = false;
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

    /// <summary>
    /// 마스터리 스킬 중 위쪽에 위치하는 스킬들을 클릭했을경우
    /// 마스터리 정보창에 스킬 관련 정보가 출력된다.
    /// </summary>
    public void UpSkillOn()
    {

        masteryInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = masteryData[0].masteryName;
        masteryInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = masteryData[0].masteryDescription;
        masteryInfo.transform.GetChild(1).GetComponent<Image>().sprite = upSkill.transform.GetChild(0).GetComponent<Image>().sprite;
        masteryInfo.SetActive(true);

    }
    
    //public void UpSkillMouseOff()
    //{
    //    masteryInfo.SetActive(false);
    //}

    /// <summary>
    /// 마스터리 스킬 중 아래쪽에 위치하는 스킬들을 클릭했을경우
    /// 마스터리 정보창에 스킬 관련 정보가 출력된다.
    /// </summary>
    public void DownSkillOn()
    {
       
        masteryInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = masteryData[1].masteryName;
        masteryInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = masteryData[1].masteryDescription;
        masteryInfo.transform.GetChild(1).GetComponent<Image>().sprite = downSkill.transform.GetChild(0).GetComponent<Image>().sprite;
        masteryInfo.SetActive(true);

    }

    //public void DownSkillMouseOff()
    //{
    //    masteryInfo.SetActive(false);
    //}
}
