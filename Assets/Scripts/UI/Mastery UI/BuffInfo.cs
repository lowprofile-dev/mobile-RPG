using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffInfo : MonoBehaviour
{
    [SerializeField] GameObject buffInfo;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI buffName;
    [SerializeField] TextMeshProUGUI buffText;
    [SerializeField] float point;
    // Start is called before the first frame update

    private void Start()
    {
        image = buffInfo.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        buffName = buffInfo.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        buffText = buffInfo.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void ShowBuffInfo()
    {
        buffInfo.SetActive(true);
        buffInfo.transform.position = transform.position + new Vector3(0f, point, 0f);
    }

    public void OffBuffInfo()
    {
        buffInfo.SetActive(false);
    }
}
