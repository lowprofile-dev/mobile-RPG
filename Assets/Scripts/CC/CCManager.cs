////////////////////////////////////////////////////
/*
    File CCManager.cs
    class CCManager
    
    담당자 : 안영훈
    부 담당자 : 

    CC 는 한번에 하나의 CC만 걸릴 수 있기 때문에 Dictionary 현재의 키값의 CC만 업데이트 하도록 설계되어있음.
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCManager : MonoBehaviour
{
    private Dictionary<string, CwordControl> ccControl = new Dictionary<string, CwordControl>();

    private MonsterAction mons = null;
    private Player player = null;
    private string currentType = null;
    private string currentCC = null; public string CurrentCC { get { return currentCC; } set { currentCC = value; } }

    public CCManager(ref MonsterAction mon , string type)
    {
        this.mons = mon;
        this.currentType = type;
    }

    public CCManager(ref Player player , string type)
    {
        this.player = player;
        this.currentType = type;
    }

    private void OnEnable()
    {
        currentCC = null;
        ccControl["fall"] = null;
        ccControl["stun"] = null;
        ccControl["rigid"] = null;
    }

    public void Update()
    {
        HandleCC();
    }

    public void Release() // CC 삭제
    {
        ccControl.Clear();
        currentCC = null;
    }
    
    public void AddCC(string type, CwordControl cc , GameObject obj)
    {
        /*
         * CC 우선순위
         * 넘어짐 > 스턴 > 경직
         * 우선순위순으로 현재 CC가 다음 CC보다 우선순위가 낮은 경우 우선순위가 높은 CC가 적용됨
         * ex ) 넘어짐 발동중에는 스턴 , 경직 적용안됨
         * ex2 ) 스턴이나 경직 발동중 넘어짐상태가 들어오면 스턴 , 경직 취소 후 넘어짐 상태로 적용
         */
        if(type == "fall")
        {
            GameObject eft = ObjectPoolManager.Instance.GetObject("Effect/CCEffect/FallEffect");
            eft.transform.SetParent(obj.transform);
            eft.transform.localPosition = Vector3.zero;
            eft.transform.rotation = Quaternion.identity;

            ccControl.Clear();
            ccControl.Add(type, cc);
            currentCC = type;
            if (currentType == "monster" && !mons.monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                mons.ChangeState(MONSTER_STATE.STATE_FALL);
            else if (currentType == "player")
                player.ChangeState(PLAYERSTATE.PS_FALL);
        }
        else if (type == "stun")
        {
            
                if (currentCC != "fall")
                {                      
                GameObject eft = ObjectPoolManager.Instance.GetObject("Effect/CCEffect/StunEffect");
                eft.transform.SetParent(obj.transform);
                eft.transform.localPosition = new Vector3(0f, obj.transform.lossyScale.y, 0f);
                eft.transform.rotation = Quaternion.identity;

                ccControl.Clear();
                    ccControl.Add(type, cc);
                    currentCC = type;
                    if (currentType == "monster" && !mons.monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
                        mons.ChangeState(MONSTER_STATE.STATE_STUN);
                    else if (currentType == "player")
                        player.ChangeState(PLAYERSTATE.PS_STUN);
                }
            
        }
        else if(type == "rigid")
        {
            
                if (currentCC != "fall" && currentCC != "stun")
                {

                    ccControl.Clear();
                    ccControl.Add(type, cc);
                    currentCC = type;
                    if (currentType == "monster" && !mons.monster.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rigid") )
                        mons.ChangeState(MONSTER_STATE.STATE_RIGID);
                    else if (currentType == "player")
                        player.ChangeState(PLAYERSTATE.PS_RIGID);
                }
            
        }
        
    }
    public void RemoveCC(string type) // CC 지속시간이 끝나면 호출되는 함수
    {
        ccControl.Remove(type);
        ccControl[type] = null;
        currentCC = null;

        if (currentType == "monster")
            mons.ChangeState(MONSTER_STATE.STATE_IDLE);
        else if (currentType == "player")
            player.ChangeState(PLAYERSTATE.PS_IDLE);
    }

    private void HandleCC() // key값의 CC만 업데이트
    {
        if(currentCC != null)
        {
            ccControl[currentCC].Updata();
        }
    }


}
