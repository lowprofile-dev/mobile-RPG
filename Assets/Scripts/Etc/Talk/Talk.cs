//////////////////////////////////////////////
/*
    File Talk.cs
    class Talk
    
    NPC와의 대화들을 모두 모아둔 클래스

    담당자 : 이신홍
    부담당자 :
*/
////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Talk
{
    private TalkData _talkData; 

    private int _convIndex;         // 현재 진행중인 대화의 인덱스
    private List<string> _speaker;  // 발언자 리스트
    private List<string> _texts;    // 발언 리스트


    // property
    public TalkData talkData { get { return _talkData; } }
    public int convIndex { get { return _convIndex; } }
    public List<string> speaker { get { return _speaker; } }
    public List<string> texts { get { return _texts; } }


    ////////// 베이스 //////////

    public Talk()
    {
        _speaker = new List<string>();
        _texts = new List<string>();
    }



    ////////// 데이터 //////////

    // 대화 내용을 정제한다.
    public void ParsingConvData(TalkData targetTalkData)
    {
        _talkData = targetTalkData;
        string[] split = _talkData.convData.Split('_', '$');
        for (int j = 0; j < split.Length; j++)
        {
            if (j % 2 == 0) _speaker.Add(split[j]);
            else _texts.Add(split[j]);
        }
    }



    ////////// 진행 //////////

    // 다음 대화로 넘어가도록 한다.
    public bool ToNextConv()
    {
        _convIndex++;

        return !IsFinished();
    }

    // 대화가 끝났는지 확인
    public bool IsFinished()
    {
        return _convIndex == speaker.Count;
    }

    // 다음 대화가 존재하는지 확인
    public bool HasNext()
    {
        return _convIndex + 1 == _speaker.Count;
    }

    // 대화 인덱스를 초기로 돌림
    public void InitConvIndex()
    {
        _convIndex = 0;
    }



    ////////// 기타 //////////

    public override string ToString()
    {
        string data = "";
        for (int i = 0; i < _speaker.Count; i++)
        {
            data += _speaker[i] + ": ";
            data += _texts[i] + "\n";
        }

        return data;
    }
}