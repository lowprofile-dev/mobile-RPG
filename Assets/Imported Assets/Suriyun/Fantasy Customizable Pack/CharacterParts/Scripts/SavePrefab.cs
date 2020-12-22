#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SavePrefab : MonoBehaviour
{
    public GameObject saveObject;
    public Text savePathText;
    public void Save()
    {
#if UNITY_EDITOR
        if (savePathText == null || saveObject == null || string.IsNullOrEmpty(savePathText.text))
            return;
        // The paths to the mesh/prefab assets.
        string prefabPath = "Assets/" + savePathText.text + ".prefab";

        // Delete the assets if they already exist.
        AssetDatabase.DeleteAsset(prefabPath);

        // Save the transform's GameObject as a prefab asset.
        PrefabUtility.CreatePrefab(prefabPath, saveObject);
#endif
    }
}
