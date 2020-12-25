using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    public bool isInteracting;

    public void InitGameManager()
    {
        isInteracting = false;
    }

    void Start()
    {
    }
    
    void Update()
    {
    }
}
