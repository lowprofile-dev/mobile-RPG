using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAttack : MonoBehaviour
{

    public void ApplyCC(GameObject unit , float fstun , float ffall , float frig) // CC 적용시키는 코드
    {
        if (unit.GetComponent<LivingEntity>().Hp <= 0) return;

        float roll = Random.Range(0, 100);

        float fallRate = ffall * 100f;
        float stunRate = ((100 - fallRate) * fstun);
        float frigRate = ((1 - ffall) * (1 - fstun) * frig) * 100f;
        float none = ((1 - fstun) * (1 - ffall) * (1 - frig)) * 100f;

        //Debug.log(fallRate + " " + stunRate + " " + frigRate + " " + none);
        

        if(roll <= fallRate)
        {
            //넘어짐
            //Debug.log("상태이상 넘어짐");
            unit.GetComponent<LivingEntity>().CCManager.AddCC("fall", new Fall(3, unit.GetComponent<LivingEntity>(), "fall"));
        }
        else if (roll <= stunRate + fallRate)
        {
            //스턴
            //Debug.log("상태이상 스턴");
            unit.GetComponent<LivingEntity>().CCManager.AddCC("stun", new Stun(3, unit.GetComponent<LivingEntity>(), "stun"));
        }
        else if (roll <= frigRate + fallRate + frigRate)
        {
            //경직
            //Debug.log("상태이상 경직");
            unit.GetComponent<LivingEntity>().CCManager.AddCC("rigid", new Rigid(3, unit.GetComponent<LivingEntity>(), "rigid"));
        }
        else
        {
            //Debug.log("안걸림");
        }
    }
}
