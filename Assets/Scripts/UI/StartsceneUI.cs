using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartsceneUI : MonoBehaviour
{
    public void GoToUIScene()
    {
        SceneManager.LoadScene("UIScene");
    }
}
