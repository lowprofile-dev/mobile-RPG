////////////////////////////////////////////////////
/*
    File ManagerHolder.cs
    class ManagerHolder
    
    담당자 : 이신홍
    부 담당자 : 

    전체 매니저를 관리하고 프로젝트의 세팅을 진행하는 클래스
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHolder : SingletonBase<ManagerHolder>
{
    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public CardManager cardManager;
    [HideInInspector] public TalkManager talkManager;
    [HideInInspector] public SoundManager soundManager;
    [HideInInspector] public CameraManager cameraManager;
    [HideInInspector] public StatusManager statusManager;
    [HideInInspector] public ResourceManager resourceManager;
    [HideInInspector] public UILoaderManager uiLoaderManager;
    [HideInInspector] public ObjectPoolManager objectPoolManager;
    [HideInInspector] public UINaviationManager uiNavationManager;
    [HideInInspector] public MasteryManager masteryManager;
    [HideInInspector] public WeaponManager weaponManager;
    [HideInInspector] public ItemManager itemManager;
    [HideInInspector] public GlobalDefine globalDefine;
    [HideInInspector] public MonsterManager monsterManager;
    [HideInInspector] public AdManager adManager;

    private void Start()
    {
        SetProjectSettings();
        AddManagersToStartGame();
        InitManagers();
    }

    /// <summary>
    /// 프로젝트에서 사용될 세팅들
    /// </summary>
    private void SetProjectSettings()
    {
        Screen.SetResolution(848, 480, true);
        Application.targetFrameRate = 60;
        Random.InitState((int)(System.DateTime.Now.Ticks));
    }

    /// <summary>
    /// ManagerHolder의 자식으로 매니저를 생성해 넣는다.
    /// </summary>
    private Component AddManager<T>()
    {
        GameObject objectRtn = new GameObject(typeof(T).ToString());
        objectRtn.transform.SetParent(transform);
        return objectRtn.AddComponent(typeof(T));
    }

    /// <summary>
    /// 매니저들을 생성한다.
    /// </summary>
    private void AddManagersToStartGame()
    {
        resourceManager = (ResourceManager)AddManager<ResourceManager>();
        objectPoolManager = (ObjectPoolManager)AddManager<ObjectPoolManager>();
        masteryManager = (MasteryManager)AddManager<MasteryManager>();
        weaponManager = (WeaponManager)AddManager<WeaponManager>();
        uiLoaderManager = (UILoaderManager)AddManager<UILoaderManager>();
        talkManager = (TalkManager)AddManager<TalkManager>();
        cardManager = (CardManager)AddManager<CardManager>();
        cameraManager = (CameraManager)AddManager<CameraManager>();
        soundManager = (SoundManager)AddManager<SoundManager>();
        uiNavationManager = (UINaviationManager)AddManager<UINaviationManager>();
        uiManager = (UIManager)AddManager<UIManager>();
        statusManager = (StatusManager)AddManager<StatusManager>();
        itemManager = (ItemManager)AddManager<ItemManager>();
        monsterManager = (MonsterManager)AddManager<MonsterManager>();
        adManager = (AdManager)AddManager<AdManager>();
    }

    /// <summary>
    /// 매니저들을 순서에 맞춰 초기화한다.
    /// </summary>
    private void InitManagers()
    {
        resourceManager.InitResourceManager();
        objectPoolManager.InitObjectPoolManager();
        masteryManager.InitMasteryManager();
        weaponManager.InitWeaponManager();
        statusManager.InitStatusManager();
        uiLoaderManager.InitUILoaderManager();
        talkManager.InitTalkManager();
        cardManager.InitCardManager();
        soundManager.InitSoundManager();
        uiNavationManager.InitUINavigationManager();
        uiManager.InitUIManager();
        statusManager.InitStatusManager();
        itemManager.InitItemManager();
        monsterManager.InitMonsterManager();
    }

    /// <summary>
    /// Update가 존재하는 매니저들에 대해서 업데이트들을 순서대로 실시한다.
    /// </summary>
    private void Update()
    {
        uiNavationManager.UpdateNavigationManager();
        weaponManager.UpdateWeapon();
    }
}
