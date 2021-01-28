//////////////////////////////////////////////
/*
    File SingletonBase.cs
    class SingletonBase
    
    담당자 : 이신홍
    부담당자 :
    
    싱글톤의 베이스가 될 클래스
*/
////////////////////////////////////////////////////

using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool shuttingDown = false;
    private static object lockCheck = new object();
    private static T instance;

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this.GetComponent<T>();
            this.name = typeof(T).ToString() + " (Singleton)";
        }
    }

    public static T Instance
    {
        get
        {
            if (shuttingDown)
            {
                Debug.Log("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                return null;
            }
            
            lock (lockCheck) // Thread Safe
            {
                if (instance == null)
                {
                    // 인스턴스가 없으면 새롭게 생성한다.
                    GameObject singletonObj = new GameObject();
                    instance = singletonObj.AddComponent<T>();
                    singletonObj.name = typeof(T).ToString() + " (Singleton)";

                    // 싱글톤이 씬이 바뀌어도 유지되도록 한다.
                    DontDestroyOnLoad(singletonObj);
                }

                return instance;
            }
        }
    }

    // 게임 종료시 Object보다 싱글톤의 메소드가 먼저 실행 될 경우의 대비.
    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

    private void OnDestroy()
    {
        shuttingDown = true;
    }
}

