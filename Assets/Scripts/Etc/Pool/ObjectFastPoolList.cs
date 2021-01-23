////////////////////////////////////////////////////
/*
    File ObjectFastPoolList.cs
    class ObjectFastPoolList
    
    담당자 : 이신홍
*/
////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

/// <summary>
/// 먼저 풀 할 친구들을 저장해 둘 리스트
/// </summary>
public class ObjectFastPoolList : MonoBehaviour
{
    [SerializeField] private GameObject[] _effectList; public GameObject[] effectList { get { return _effectList; } }
}