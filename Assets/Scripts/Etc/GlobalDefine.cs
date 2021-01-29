//////////////////////////////////////////////
/*
    File GlobalDefine.cs
    class GlobalDefine
    
    담당자 : 이신홍
    부담당자 :

    각종 오브젝트 및 데이터의 캐싱에 사용할 공유 데이터 컨테이너 스크립트
*/
////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalDefine : SingletonBase<GlobalDefine>
{
    // 미니맵 퀘스트 아이콘 관련
    public Sprite questStartMinimapIcon;
    public Sprite questExitMinimapIcon;
    public Sprite npcBaseMinimapIcon;

    // 텍스트 색 관련 (Top이 Bottom보다 연하다)
    public Color textColorGradientDamagedTop = new Color(1, 0.7216f, 0.7216f);
    public Color textColorGradientDamagedBottom = new Color(1, 0, 0);
    public Color textColorGradientRestoreTop = new Color(0.7215f, 1, 0.7683f);
    public Color textColorGradientRestoreBottom = new Color(0.1257f, 1, 0.0235f);
    public Color textColorGradientCriticalTop = new Color(0.5f, 0.5f, 1f);
    public Color textColorGradientCriticalBottom = new Color(0.1f, 0.1f, 1f);
}