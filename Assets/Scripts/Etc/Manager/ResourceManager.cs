////////////////////////////////////////////////////
/*
    File ResourceManager.cs
    class ResourceManager
    
    담당자 : 이신홍
    부 담당자 :

    경로를 통해 리소스를 불러올 수 있는 매니저
    Resources를 사용하므로 Resource 폴더에 있는 에셋만 불러올 수 있다.
*/
////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : SingletonBase<ResourceManager>
{
    public Dictionary<string, Object> resourceContainer = new Dictionary<string, Object>(); // 중복된 리소스는 저장해두었다가 사용하도록 하여 Load 부하를 줄임.

    public void InitResourceManager()
    {
        // DO SOMETHING
    }

    /// <summary>
    /// 경로를 받아 오브젝트를 생성한다.
    /// </summary>
    public static Object Load(string path)
    {
        Object obj = Resources.Load(path);
        if (obj != null) return obj;

        else
        {
            Debug.LogError("Resource path " + path + " is not valided");
            return null;
        }
    }



    // 다양한 Instantiate 방법 //

    // 이름을 입력해 생성하는 방법.
    public GameObject Instantiate(string name, string path, Transform parent = null)
    {
        GameObject obj = Instantiate(path, parent);
        obj.name = name;
        
        return obj;
    }

    // 위치, 각도를 입력해 생성하는 방법.
    public GameObject Instantiate(string path, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject obj = Instantiate(path, parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        return obj;
    }

    // 이름, 위치, 각도를 입력해 생성하는 방법.
    public GameObject Instantiate(string name, string path, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject obj = Instantiate(path, parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.name = name;

        return obj;
    }

    // 가장 베이직한 생성법. 경로만 입력한다.
    public GameObject Instantiate(string path, Transform parent = null)
    {
        Object source = null;

        // LOAD 
        if (resourceContainer.ContainsKey(path)) // 기존 로딩한 적이 있다면
        {
            source = resourceContainer[path]; // 바로 가져온다.
        }

        else // 처음 본다면
        {
            source = Load(path);
            resourceContainer.Add(path, source); // load하여 등록
        }

        // INSTANTIATE
        if (source != null) // object가 존재하면
        {
            GameObject newObject = Instantiate(source, parent) as GameObject; // 생성 후
            return newObject; // 리턴
        }

        else // 존재하지 않으면
        {
            Debug.LogWarning("Please check path resource load failed : " + path);
            return null;
        }
    }
}
