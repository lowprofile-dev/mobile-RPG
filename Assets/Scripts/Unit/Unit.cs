////////////////////////////////////////////////////
/*
    File Unit.cs
    class Unit
    
    제일 기초가 되는 개체의 단위. id를 가지고 있다.
    
    담당자 : 이신홍
    부 담당자 : 
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected int _id; public int id { get { return _id; } }
}