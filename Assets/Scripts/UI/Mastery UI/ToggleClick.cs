using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleClick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject skill;
    MasteryScript mastery;
    // Start is called before the first frame update
    void Start()
    {
        mastery = skill.GetComponent<MasteryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.parent.name == "Mastery Card 1")
        {
            mastery.UpSkillOn();
        }
        else mastery.DownSkillOn();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if (transform.parent.name == "Mastery Card 1") mastery.UpSkillMouseOff();
        //else mastery.DownSkillMouseOff();
    }
}
