using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 세이브 및 로드, 이런저런 상황에 사용되는 수치들을 종합한다.
/// </summary>
public class StatusManager : SingletonBase<StatusManager>
{
    // 카드 리롤
    public int cardRerollCoin;
    public int needToCardRerollCoin;
    public int rerollCount;

    /// <summary>
    /// 스테이터스 정보들을 초기화한다.
    /// </summary>
    public void InitStatusManager()
    {
        cardRerollCoin = 10000;
        needToCardRerollCoin = 0;
        rerollCount = 0;
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
