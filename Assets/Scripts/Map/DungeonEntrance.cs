/*
    File DungeonExit.cs
    class DungeonExit
    
    담당자 : 이신홍
    부 담당자 :

    던전 입구
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") UINaviationManager.Instance.OpenSubUIView("CardUIView");
    }
}
