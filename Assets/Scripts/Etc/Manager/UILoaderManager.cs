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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddScene("DungeonScene");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            CloseScene("DungeonScene");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            AddScene("VillageScene");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            CloseScene("VillageScene");
        }
        /*
        if(Input.GetKeyDown(KeyCode.F4))
        {
            if (SceneManager.GetSceneByName("TEST1").isLoaded == false)
                SceneManager.LoadSceneAsync("TEST1" , LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync("TEST1");
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            if (SceneManager.GetSceneByName("QuestCheckScene").isLoaded == false)
                SceneManager.LoadSceneAsync("QuestCheckScene", LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync("QuestCheckScene");
        }
        */

        //if (Input.GetKeyDown(KeyCode.F3))
        //{
        //    if (SceneManager.GetSceneByName("Level2").isLoaded)
        //        SceneManager.UnloadSceneAsync("Level2");

        //    if (SceneManager.GetSceneByName("Level1").isLoaded == false)
        //    {
        //        SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive).completed += operation =>
        //            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));
        //    }
        //}

        //if (Input.GetKeyDown(KeyCode.F4))
        //{
        //    if (SceneManager.GetSceneByName("Level1").isLoaded)
        //        SceneManager.UnloadSceneAsync("Level1");

        //    if (SceneManager.GetSceneByName("Level2").isLoaded == false)
        //    {
        //        SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive)
        //            .completed += HandleLevel2LoadCompleted;
        //    }
        //}
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