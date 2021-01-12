using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTButton : MonoBehaviour
{
    public Button button;
    public string map = "CheckScene";

    private void Start()
    {
        //Button btn = button.GetComponent<Button>();
        //btn.onClick.AddListener(fClick);
        button.onClick.AddListener(fClick);
    }
    void fClick()
    {
        //UILoaderManager.Instance.LoadScene("TEST1", map);
    }
}
