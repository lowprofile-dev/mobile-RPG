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
   
    GameObject[] infoWindow;
    PlayerMasteryData[] masteryData;
    private bool isInit = false;
    // Start is called before the first frame update
    void Start()
    {
        upSkill = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>();
        downSkill = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Toggle>();
        masteryData = new PlayerMasteryData[2];

        //infoWindow = new GameObject[2];
        //infoWindow[0] = Instantiate(masteryInfo);
        //infoWindow[0].transform.SetParent(parent.transform);
        //
        //Debug.Log(infoWindow[0].transform.parent.name);
        //infoWindow[0].SetActive(false);
        //infoWindow[0].transform.position = upSkill.transform.position;
        //
        //infoWindow[1] = Instantiate(masteryInfo);
        //infoWindow[1].transform.SetParent(parent.transform);
        //Debug.Log(infoWindow[1].transform.parent.name);
        //
        //infoWindow[0].SetActive(false);
        //infoWindow[1].transform.position = upSkill.transform.position;

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
           // switch (gameObject.name)
           // {
           //     case "Level5":
                    masteryData[0] = MasteryManager.Instance.masteryDictionary[1];
                    masteryData[1] = MasteryManager.Instance.masteryDictionary[2];
           //         break;
           // }
            isInit = true;
        }
    }

    public void ToggleCheck()
    {
        if(upSkill.isOn == true)
        {
            upSkill.enabled = false;
            downSkill.enabled = false;
        }
        else
        {
            //downSkill.enabled = true;
        }
        if(downSkill.isOn == true)
        {
            upSkill.enabled = false;
            downSkill.enabled = false;
        }
        else
        {
            //upSkill.enabled = true;
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
