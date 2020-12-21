using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoaderManager : SingletonBase<UILoaderManager>
{
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    if (SceneManager.GetSceneByName("UIScene").isLoaded == false)
        //        SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
        //    else
        //        SceneManager.UnloadSceneAsync("UIScene");
        //}

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (SceneManager.GetSceneByName("CheckScene").isLoaded == false)
                SceneManager.LoadSceneAsync("CheckScene", LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync("CheckScene");
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            if (SceneManager.GetSceneByName("TEST1").isLoaded == false)
                SceneManager.LoadSceneAsync("TEST1" , LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync("TEST1");
        }

        if(Input.GetKeyDown(KeyCode.F4))
        {
            if (SceneManager.GetSceneByName("QuestCheckScene").isLoaded == false)
                SceneManager.LoadSceneAsync("QuestCheckScene", LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync("QuestCheckScene");
        }

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

    private void HandleLevel2LoadCompleted(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level2"));
    }
}