using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UILoaderManager : SingletonBase<UILoaderManager>
{
    private GameObject _playerUI = null; public GameObject PlayerUI { get { return _playerUI; } }

    public void InitUILoaderManager()
    {
        _playerUI = GameObject.Find("PlayerUI_View");

    }

    public void LoadUI()
    {      
         _playerUI.GetComponent<Canvas>().enabled = true;
    }

    public void LoadVillage()
    {
        LoadingSceneManager.LoadScene("VillageScene", "DungeonScene");
        SoundManager.Instance.StopEffect("Fire Loop");
        SoundManager.Instance.StopEffect("Cave 1 Loop");
        StartCoroutine(VillageMusicPlay());
    }

    IEnumerator VillageMusicPlay()
    {
        yield return new WaitForSeconds(2);
        SoundManager.Instance.PlayBGM("VillageBGM", 0.55f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Ambient/Fire Loop", 0.15f, 0, true);
    }

    public void LoadDungeon()
    {

        LoadingSceneManager.LoadScene("DungeonScene", "VillageScene");
        SoundManager.Instance.StopEffect("Cave 1 Loop");
        SoundManager.Instance.StopEffect("Fire Loop");
        SoundManager.Instance.PlayBGM("DungeonBGM", 0.6f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Ambient/Cave 1 Loop", 0.25f, 0, true);
    }

    public bool IsSceneDungeon()
    {
        return SceneManager.GetActiveScene().name == "DungeonScene";
    }

}