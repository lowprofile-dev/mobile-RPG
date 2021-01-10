using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MasteryScript : MonoBehaviour
{
    [SerializeField] Toggle upSkill; 
    [SerializeField] Toggle downSkill; 
    // Start is called before the first frame update
    void Start()
    {
        upSkill = transform.GetChild(0).GetChild(0).GetComponent<Toggle>();
        downSkill = transform.GetChild(0).GetChild(1).GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        ToggleCheck();
    }

    public void ToggleCheck()
    {
        if(upSkill.isOn == true)
        {
            downSkill.enabled = false;
        }
        else
        {
            downSkill.enabled = true;
        }

        if(downSkill.isOn == true)
        {
            upSkill.enabled = false;
        }
        else
        {
            upSkill.enabled = true;
        }

    }
}
