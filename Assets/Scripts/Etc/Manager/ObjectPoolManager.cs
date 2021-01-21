////////////////////////////////////////////////////
/*
    File ObjectPoolManager.cs
    class ObjectPoolManager
    
    담당자 : 안영훈
    부 담당자 : 
*/
////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonBase<ObjectPoolManager>
{
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    /// <summary>
    /// 시작과 동시에 온갖 미리 풀 할 친구들을 풀해둔다.
    /// </summary>
    public void InitObjectPoolManager()
    {
         ObjectFastPoolList myFastPoolList = ResourceManager.Instance.Instantiate("Prefab/Etc/Pool/ObjectFastPoolList", transform).GetComponent<ObjectFastPoolList>();

        for(int i=0; i< myFastPoolList.effectList.Length; i++)
        {
            InitObject(myFastPoolList.effectList[i], 2, transform);
        }
    }

    public void PoolBigObjects()
    {

    }

    public void ClearObjectPool() // 오브젝트풀에 적재되어있는 모든 것 없애기
    {
        objectPool.Clear();
    }

    public void ClearObjectPool(string name) // 해당 키의 오브젝트풀 큐에 적재되어있는 모든 것 없애기
    {
        if (objectPool.TryGetValue(name, out Queue<GameObject> objectList))
        {
            objectList.Clear();
        }
    }
    public void InitObject(string path, string name, int cnt) //오브젝트풀에 카운트만큼 미리 만들어두는 함수
    {

        for (int i = 0; i < cnt; i++)
        {
            GameObject obj = Instantiate(Resources.Load("Prefab/" + path) as GameObject);
            obj.name = name;
            ReturnObject(obj);
        }
    }

    public void InitObject(string name, int cnt , Transform parent) //오브젝트풀에 카운트만큼 미리 만들어두는 함수
    {

        for (int i = 0; i < cnt; i++)
        {
            GameObject obj = Instantiate(Resources.Load("Prefab/" + name) as GameObject);
            obj.name = name;
            ReturnObject(obj, parent);
        }
    }
    public void InitObject(GameObject obj , int cnt , Transform tr)
    {
        for (int i = 0; i < cnt; i++)
        {
            GameObject eft = Instantiate(obj);
            eft.name = obj.name;
            eft.transform.position = tr.position;
            ReturnObject(eft);
        }
    }
    /////////////////////////
    /*
         GetObject()  오브젝트풀에 적재되어있는 게임오브젝트 반환 풀에 적재되어있지않으면 자동으로 만들어서 반환해줌
    */
    /////////////////////////

    public GameObject GetObject(GameObject obj)
    {
        if (objectPool.TryGetValue(obj.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
                return CreateNewObject(obj);
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.SetActive(true);
                return _object;
            }
        }
        else
            return CreateNewObject(obj);
    }

    public GameObject GetObject(GameObject obj, Vector3 pos, Quaternion rot)  // 오브젝트풀에 적재되어있는 게임오브젝트 반환
    {
        if (objectPool.TryGetValue(obj.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
                return CreateNewObject(obj, pos, rot);
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.transform.position = pos;
                _object.transform.rotation = rot;
                _object.SetActive(true);
                return _object;
            }
        }
        else
            return CreateNewObject(obj, pos, rot);
    }


    public GameObject GetObject(string name)  // 오브젝트풀에 적재되어있는 게임오브젝트 반환
    {
        if (objectPool.TryGetValue(name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
                return CreateNewObject(Resources.Load("Prefab/" + name) as GameObject);
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.SetActive(true);
                return _object;
            }
        }
        else
            return CreateNewObject(Resources.Load("Prefab/" + name) as GameObject);
    }

    public GameObject GetObject(string name , Vector3 pos , Quaternion rot)  // 오브젝트풀에 적재되어있는 게임오브젝트 반환
    {
        if (objectPool.TryGetValue(name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
                return CreateNewObject(Resources.Load("Prefab/" + name) as GameObject, pos, rot);
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.transform.position = pos;
                _object.transform.rotation = rot;
                _object.SetActive(true);
                return _object;
            }
        }
        else
            return CreateNewObject(Resources.Load("Prefab/" + name) as GameObject);
    }

    private GameObject CreateNewObject(GameObject gameObject) // 게임오브젝트 새로만드는 함수
    {
        GameObject newGO = Instantiate(gameObject);
        newGO.name = gameObject.name;
        return newGO;
    }

    private GameObject CreateNewObject(GameObject gameObject , Vector3 pos , Quaternion rot) // 게임오브젝트 새로만드는 함수
    {
        GameObject newGO = Instantiate(gameObject);
        newGO.name = gameObject.name;
        newGO.transform.position = pos;
        newGO.transform.rotation = rot;
        return newGO;
    }

    public void ReturnObject(GameObject gameObject , Transform parent = null) // 사용하던 게임오브젝트를 오브젝트풀에 반환
    {

        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> newObjectQueue = new Queue<GameObject>();
            newObjectQueue.Enqueue(gameObject);
            objectPool.Add(gameObject.name, newObjectQueue);
        }
        if (parent == null) gameObject.transform.SetParent(transform);
        else gameObject.transform.SetParent(parent);

        gameObject.SetActive(false);

    }

}
