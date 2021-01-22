using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

////////////////////////////////////////////////////
/*
    File ToggleClick.cs
    class ToggleClick

    담당자 : 김의겸
    부 담당자 : 
*/
////////////////////////////////////////////////////


public class ToggleClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject skill;
    MasteryScript mastery;
    // Start is called before the first frame update
    void Start()
    {
        mastery = skill.GetComponent<MasteryScript>();
    }

    /// <summary>
    /// 토글키에 마우스 클릭 이벤트가 일어 날 경우 정보창에 출력하기 위한 함수를 호출한다.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.parent.name == "Mastery Card 1")
        {
            mastery.UpSkillOn();
        }
        else mastery.DownSkillOn();
    }

}
