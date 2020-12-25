﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHolder : SingletonBase<ManagerHolder>
{
   [HideInInspector] public UIManager uiManager;
   [HideInInspector] public GameManager gameManager;
   [HideInInspector] public CardManager cardManager;
   [HideInInspector] public TalkManager talkManager;
   [HideInInspector] public SoundManager soundManager;
   [HideInInspector] public CameraManager cameraManager;
   [HideInInspector] public ResourceManager resourceManager;
   [HideInInspector] public UILoaderManager uiLoaderManager;
   [HideInInspector] public ObjectPoolManager objectPoolManager;
   [HideInInspector] public UINavationManager uiNavationManager;

    private void Start()
    {
        Screen.SetResolution(Screen.width, (int)(Screen.width * (9f / 16f)), true);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        Random.InitState((int)(System.DateTime.Now.Ticks));

        AddManagersToStartGame();
        InitManagers();
    }

    private Component AddManager<T>()
    {
        GameObject objectRtn = new GameObject(typeof(T).ToString());
        objectRtn.transform.SetParent(transform);
        return objectRtn.AddComponent(typeof(T));
    }

    private void AddManagersToStartGame()
    {
        gameManager = (GameManager)AddManager<GameManager>();
        resourceManager = (ResourceManager)AddManager<ResourceManager>();
        objectPoolManager = (ObjectPoolManager)AddManager<ObjectPoolManager>();
        uiLoaderManager = (UILoaderManager)AddManager<UILoaderManager>();
        talkManager = (TalkManager)AddManager<TalkManager>();
        cardManager = (CardManager)AddManager<CardManager>();
        cameraManager = (CameraManager)AddManager<CameraManager>();
        soundManager = (SoundManager)AddManager<SoundManager>();
        uiNavationManager = (UINavationManager)AddManager<UINavationManager>();
        uiManager = (UIManager)AddManager<UIManager>();
    }

    private void InitManagers()
    {
        gameManager.InitGameManager();
        resourceManager.InitResourceManager();
        objectPoolManager.InitObjectPoolManager();
        cameraManager.InitCameraManager();
        uiLoaderManager.InitUILoaderManager();
        talkManager.InitTalkManager();
        cardManager.InitCardManager();
        soundManager.InitSoundManager();
        uiNavationManager.InitUINavigationManager();
        uiManager.InitUIManager();
    }

    private void Update()
    {
        soundManager.UpdateSoundManager();
        uiNavationManager.UpdateNavigationManager();
    }
}