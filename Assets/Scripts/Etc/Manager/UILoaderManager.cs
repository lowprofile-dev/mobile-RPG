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

    public bool IsSceneDungeon()
    {
        return SceneManager.GetActiveScene().name == "DungeonScene";
    }

}