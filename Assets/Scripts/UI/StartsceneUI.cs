using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartsceneUI : MonoBehaviour
{
    public void GoToUIScene()
    {
        LoadingSceneManager.LoadScene("UIScene");
        //UILoaderManager.Instance.LoadScene("StartScene", "UIScene");
    }
}
