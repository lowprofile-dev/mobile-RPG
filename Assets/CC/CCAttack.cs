using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAttack : MonoBehaviour
{
  
    [SerializeField]
    private float debuffDuration;   //디버프 지속시간
    [SerializeField]
    private float proc;             //CC가 걸릴 확률

    public float DebuffDuration { get { return debuffDuration; } set { debuffDuration = value; } }
    public float Proc { get { return proc; } set { proc = value; } }

    public void ApplyCC(GameObject unit , float fstun , float ffall , float frig) // CC 적용시키는 코드
    {
        float roll = Random.Range(0, 100);

        float fallRate = ffall * 100f;
        float stunRate = ((100 - fallRate) * fstun);
        float frigRate = ((1 - ffall) * (1 - fstun) * frig) * 100f;
        float none = ((1 - fstun) * (1 - ffall) * (1 - frig)) * 100f;

        Debug.Log(fallRate + " " + stunRate + " " + frigRate + " " + none);
        

        if(roll <= fallRate)
        {
            //넘어짐
            Debug.Log("상태이상 넘어짐");
            unit.GetComponent<LivingEntity>().CCManager.AddCC("fall", new Fall(3, unit.GetComponent<LivingEntity>(), "fall"));
        }
        else if (roll <= stunRate + fallRate)
        {
            //스턴
            Debug.Log("상태이상 스턴");
            unit.GetComponent<LivingEntity>().CCManager.AddCC("stun", new Stun(3, unit.GetComponent<LivingEntity>(), "stun"));
        }
        else if (roll <= frigRate + fallRate + frigRate)
        {
            //경직
            Debug.Log("상태이상 경직");
            unit.GetComponent<LivingEntity>().CCManager.AddCC("rigid", new Rigid(3, unit.GetComponent<LivingEntity>(), "rigid"));
        }
        else
        {
            Debug.Log("안걸림");
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            ApplyCC(other.gameObject, 0.5f, 0.6f, 0.4f);
        }
    }
}
