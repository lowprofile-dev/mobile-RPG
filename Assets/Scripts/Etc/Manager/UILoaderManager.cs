using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UILoaderManager : SingletonBase<UILoaderManager>
{
    private GameObject PlayerUI;
   
    public void InitUILoaderManager()
    {
        PlayerUI = GameObject.Find("PlayerUI_View");
    }

    private void Update()
    {

    }

    public void LoadVillage()
    {
        UILoaderManager.Instance.AddScene("VillageScene");
        UILoaderManager.Instance.CloseScene("DungeonScene");
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
        UILoaderManager.Instance.CloseScene("VillageScene");
        UILoaderManager.Instance.AddScene("DungeonScene");
        SoundManager.Instance.StopEffect("Cave 1 Loop");
        SoundManager.Instance.StopEffect("Fire Loop");
        SoundManager.Instance.PlayBGM("DungeonBGM", 0.6f);
        SoundManager.Instance.PlayEffect(SoundType.EFFECT, "Ambient/Cave 1 Loop", 0.25f, 0, true);
    }

    public bool IsSceneDungeon()
    {
        return SceneManager.GetActiveScene().name == "DungeonScene";
    }

    public void AddScene(string name) // 현재 씬에 다른 씬을 추가하는 함수 ADD 
    {
        if(SceneManager.GetSceneByName(name).isLoaded == false)
        {
            StartCoroutine(Loading(name, LoadSceneMode.Additive));
            //SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        
        }
        else
        {
            Debug.Log("현재 씬은 존재합니다. 디버그 ㄱ");
        }
    }

    public void CloseScene(string name) // 현재 씬에 추가된 다른 씬을 비활성화 시키는 함수
    {
        if (SceneManager.GetSceneByName(name).isLoaded)
        {
            SceneManager.UnloadSceneAsync(name);
        }
        else
        {
            Debug.Log("현재 씬은 존재하지않습니다. 디버그 ㄱ");
        }
    }
    
    public void LoadScene(string currentName , string targetName) // 현재씬에서 다른씬으로 이동하는 함수 Single ex ) 던전 -> 던전 , 마을 -> 던전
    {
        
        if (SceneManager.GetSceneByName(targetName).isLoaded == false)
        {
           
            SceneManager.UnloadSceneAsync(currentName);

            StartCoroutine(Loading(targetName, LoadSceneMode.Single));
        }
        else
        {
            Debug.Log("현재 씬은 존재합니다. 디버그 ㄱ");
            //SceneManager.UnloadSceneAsync(name);
        }
    }

    //private void HandleLevel2LoadCompleted(AsyncOperation obj)
    //{
    //    SceneManager.SetActiveScene(SceneManager.GetSceneByName("CheckScene"));

    //}

    //IEnumerator Loading(string name)
    //{

    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);

    //    while (!asyncOperation.isDone)
    //    {
    //        yield return null;
    //        Debug.Log("Loading  : " + (asyncOperation.progress * 100) + "%");
    //    }

    //}

    IEnumerator Loading(string name , LoadSceneMode mode)
    {
        PlayerUI.SetActive(false);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name , mode);

        while (!asyncOperation.isDone)
        {
            yield return null;
            Debug.Log("Loading  : " + (asyncOperation.progress * 100) + "%");
        }

        PlayerUI.SetActive(true);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}