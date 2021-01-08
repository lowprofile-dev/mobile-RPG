using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCManager : MonoBehaviour
{

    private Dictionary<string, CwordControl> ccControl = new Dictionary<string, CwordControl>();

    private string currentCC = null; public string CurrentCC { get { return currentCC; } set { currentCC = value; } }

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
            ccControl.Clear();
            ccControl.Add(type, cc);
            currentCC = type;
        }
        else if (type == "stun")
        {
            if(ccControl.TryGetValue("fall" , out CwordControl cwControl)){
                if (cwControl == null)
                {
                    ccControl.Clear();
                    ccControl.Add(type, cc);
                    currentCC = type;
                }
                else
                {
                    Debug.Log("넘어짐 상태라 스턴안걸림");
                }
            }
        }
        else if(type == "rigid")
        {
            if (ccControl.TryGetValue("fall", out CwordControl ccFall) && ccControl.TryGetValue("stun" , out CwordControl ccStun))
            {
                if (ccFall == null && ccStun == null)
                {
                    ccControl.Clear();
                    ccControl.Add(type, cc);
                    currentCC = type;
                }
                else
                {
                    Debug.Log("넘어짐 상태 혹은 스턴 상태라 경직 안걸림");
                }
            }
        }
        
    }
    public void RemoveCC(string type)
    {
        ccControl.Remove(type);
    }

    private void HandleCC()
    {
        if(currentCC != null)
        {
            Debug.Log(currentCC);
            ccControl[currentCC].Updata();
        }
    }


}
