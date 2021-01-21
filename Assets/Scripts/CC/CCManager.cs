////////////////////////////////////////////////////
/*
    File CCManager.cs
    class CCManager
    
    담당자 : 안영훈
    부 담당자 : 
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
        ccControl["fall"] = null;
        ccControl["stun"] = null;
        ccControl["rigid"] = null;
    }

    public void Update()
    {
        HandleCC();
    }

    public void Release()
    {
        ccControl.Clear();
        currentCC = null;
    }
    
    public void AddCC(string type, CwordControl cc , GameObject obj)
    {
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
    public void RemoveCC(string type)
    {
        ccControl.Remove(type);
        ccControl[type] = null;
        currentCC = null;

        if (currentType == "monster")
            mons.ChangeState(MONSTER_STATE.STATE_IDLE);
        else if (currentType == "player")
            player.ChangeState(PLAYERSTATE.PS_IDLE);
    }

    private void HandleCC()
    {
        if(currentCC != null)
        {
            ccControl[currentCC].Updata();
        }
    }


}
