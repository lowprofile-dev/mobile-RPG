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
    }
    
    public void AddCC(string type, CwordControl cc)
    {
        if(type == "fall")
        {
         //   Debug.Log("CCMANAGER 넘어짐");
            ccControl.Clear();
            ccControl.Add(type, cc);
            currentCC = type;
            if (currentType == "monster")
                mons.ChangeState(MONSTER_STATE.STATE_FALL);
            else if (currentType == "player")
                player.ChangeState(PLAYERSTATE.PS_FALL);
        }
        else if (type == "stun")
        {
            
                if (currentCC != "fall")
                {
             //       Debug.Log("CCMANAGER 스턴");
                    ccControl.Clear();
                    ccControl.Add(type, cc);
                    currentCC = type;
                    if (currentType == "monster")
                        mons.ChangeState(MONSTER_STATE.STATE_STUN);
                    else if (currentType == "player")
                        player.ChangeState(PLAYERSTATE.PS_STUN);
                }
                else
                {
           //         Debug.Log("넘어짐 상태라 스턴안걸림");
                }
            
        }
        else if(type == "rigid")
        {
            
                if (currentCC != "fall" && currentCC != "stun")
                {
               //     Debug.Log("CCMANAGER 경직");
                    ccControl.Clear();
                    ccControl.Add(type, cc);
                    currentCC = type;
                    if (currentType == "monster")
                        mons.ChangeState(MONSTER_STATE.STATE_RIGID);
                    else if (currentType == "player")
                        player.ChangeState(PLAYERSTATE.PS_RIGID);
                }
                else
                {
           //         Debug.Log("넘어짐 상태 혹은 스턴 상태라 경직 안걸림");
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
