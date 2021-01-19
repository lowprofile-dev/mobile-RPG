using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SystemPanel : MonoBehaviour
{
    public static SystemPanel instance;

    public TextMeshProUGUI text;
    [SerializeField] private GameObject panel;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        image = panel.transform.GetComponent<Image>();
        panel.SetActive(false);
    }

    public void SetText(string inputText)
    {
        text.text = inputText;
    }

    public void FadeOutStart()
    {
        panel.SetActive(true);
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        float time = 0f;
        Color temp = image.color;
        while (image.color.a >= 0.0f)
        {
            time += Time.deltaTime;
            temp.a = Mathf.Lerp(0f, 1f, time);
            image.color = temp;
            text.color = temp;
            yield return null;
            if (image.color.a >= 1f)
            {
                panel.SetActive(false);
                break;
            }
        }
    }
}
