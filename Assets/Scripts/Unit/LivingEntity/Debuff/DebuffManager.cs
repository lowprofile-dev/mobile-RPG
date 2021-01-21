////////////////////////////////////////////////////
/*
    File DebuffManager.cs
    class DebuffManager
    
    담당자 : 안영훈
    부 담당자 : 

    디버프를 관리하는 매니저 스크립트 유닛마다 선언해서 가지고 있으면 됨

*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    // Start is called before the first frame update

    private List<Debuff> debuffs = new List<Debuff>(); //디버프 리스트

    private List<Debuff> debuffsToRemove = new List<Debuff>(); //디버프 삭제 리스트

    private List<Debuff> newDebuffs = new List<Debuff>(); //새로운 디버프 리스트

    public void Update()
    {
        HandleDebuffs();
    }

    public void Release() // 디버프 클리어
    {
        debuffs.Clear();
    }

    public void AddDebuff(Debuff debuff) // 디버프 적용
    {
        newDebuffs.Add(debuff);  
    }
    public void RemoveDebuff(Debuff debuff) //디버프 삭제
    {
        debuffsToRemove.Add(debuff);
    }

    private void HandleDebuffs()
    {
        
        if (newDebuffs.Count > 0) //새로운 디버프 리스트 카운트가 0보다 크면
        {
            debuffs.AddRange(newDebuffs); //디버프에 new 디버프 추가

            newDebuffs.Clear(); //새로운 디버프 리스트 삭제 
        }

        foreach (Debuff debuff in debuffsToRemove) //디버프 삭제 리스트 순회
        {
            debuffs.Remove(debuff);

        }

        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs) //디버프 리스트 순회
        {
            debuff.Update();
        }

    }
}
