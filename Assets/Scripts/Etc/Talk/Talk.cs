using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Talk
{
    private TalkData _talkData; public TalkData talkData { get { return _talkData; } }

    private int _convIndex; public int convIndex { get { return _convIndex; } } // 현재 진행중인 대화의 인덱스
    private List<string> _speaker; public List<string> speaker { get { return _speaker; } } // 발언자 리스트
    private List<string> _texts; public List<string> texts { get { return _texts; } } // 발언 리스트

    public Talk()
    {
        _speaker = new List<string>();
        _texts = new List<string>();
    }

    // 다음 대화로 넘어가도록 한다.
    public bool ToNextConv()
    {
        _convIndex++;

        // 끝까지 다 말했을 경우
        if (_convIndex == _speaker.Count)
        {
            return false;
        }

        // 끝까지 다 말하지 않았을 경우
        return true;
    }

    public bool IsFinished()
    {
        return _convIndex == speaker.Count;
    }

    // 다음 대화가 존재하는지 확인
    public bool HasNext()
    {
        return _convIndex + 1 == _speaker.Count;
    }

    public void InitConvIndex()
    {
        _convIndex = 0;
    }

    // 대화 내용을 정제한다.
    public void RefineConvData(TalkData targetTalkData)
    {
        _talkData = targetTalkData;
        string[] split =  _talkData.convData.Split('_', '$');
        for (int j = 0; j < split.Length; j++)
        {
            if (j % 2 == 0) _speaker.Add(split[j]);
            else _texts.Add(split[j]);
        }
    }

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