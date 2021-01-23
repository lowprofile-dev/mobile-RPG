////////////////////////////////////////////////////
/*
    File LoadingSceneManager.cs
    class LoadingSceneManager
    
    담당자 : 김기정
    부 담당자 : 안영훈
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI progressTxt;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName) //스타트씬에서 마을가는 용도 ㅎ
    {
        nextScene = sceneName;
        //SceneManager.LoadScene("UIScene");
        if (SceneManager.GetSceneByName("UIScene").isLoaded == false) SceneManager.LoadScene("UIScene");
        SceneManager.LoadScene("LoadingScene" , LoadSceneMode.Additive);
    }

    public static void LoadScene(string current , string target) // 씬이동 cuurent -> target
    {

        nextScene = target;

        if(SceneManager.GetSceneByName("UIScene").isLoaded == false) SceneManager.LoadScene("UIScene");
        else
        {
            GameObject.Find("PlayerUI_View").GetComponent<Canvas>().enabled = false;
        }

        SceneManager.UnloadSceneAsync(current);

        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);

    }

    IEnumerator LoadScene() // 로딩
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene,LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        op.completed += ActivateTransientScene;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                progressTxt.text = string.Format("{0:0.00}%", progressBar.fillAmount);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    SceneManager.UnloadSceneAsync("LoadingScene");

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
  
    }

    private void ActivateTransientScene(AsyncOperation op) // 이동한 씬을 메인씬으로 만듬
    {
        Scene scene = SceneManager.GetSceneByName(nextScene);
        SceneManager.SetActiveScene(scene);        
    }
}