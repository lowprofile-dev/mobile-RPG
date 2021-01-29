////////////////////////////////////////////////////
/*
    File UILoaderManager.cs
    class UILoaderManager
    
    담당자 : 안영훈
    부 담당자 : 이신홍 , 김기정
*/
////////////////////////////////////////////////////
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UILoaderManager : SingletonBase<UILoaderManager>
{
    private GameObject _playerUI = null; 
    private TextMeshProUGUI _nameText; 

    // property
    public GameObject PlayerUI { get { return _playerUI; } }
    public TextMeshProUGUI NameText { get { return _nameText; } }

    public void Start()
    {
        StartCoroutine(VillageMusicPlay());
    }

    public void InitUILoaderManager()
    {
        _playerUI = GameObject.Find("PlayerUI_View");
        _nameText = GameObject.Find("NamePanel").transform.Find("Panel").transform.Find("BossNameText").GetComponent<TextMeshProUGUI>();
        _nameText.text = "";
    }

    public void LoadPlayerUI()
    {      
         _playerUI.GetComponent<Canvas>().enabled = true;
    }

    //////////// 로드하는 씬 별 작동 양상 /////////////

    public void LoadVillage() // 마을씬 로드
    {
        LoadingSceneManager.LoadScene("DungeonScene", "VillageScene");
        SoundManager.Instance.StopEffect("Fire Loop");
        SoundManager.Instance.StopEffect("Cave 1 Loop");
        StartCoroutine(VillageMusicPlay());
        //AdManager.Instance.ShowAd();
    }

    IEnumerator VillageMusicPlay() // 마을 음악 재생
    {
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayBGM("VillageBGM", 0.55f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Ambient/Fire Loop", 0.15f, 0, true);
    }

    public void LoadDungeon() // 던전씬 로드
    {
        LoadingSceneManager.LoadScene("VillageScene", "DungeonScene");
        SoundManager.Instance.StopEffect("Cave 1 Loop");
        SoundManager.Instance.StopEffect("Fire Loop");
        SoundManager.Instance.PlayBGM("DungeonBGM", 0.6f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Ambient/Cave 1 Loop", 0.25f, 0, true);
        TalkManager.Instance.SetQuestCondition(3, 0, 1);
    }



    //////////// 기타 /////////////

    public bool IsSceneDungeon() // 지금이 던전씬인지 확인하는 함수
    {
        return SceneManager.GetActiveScene().name == "DungeonScene";
    }

}