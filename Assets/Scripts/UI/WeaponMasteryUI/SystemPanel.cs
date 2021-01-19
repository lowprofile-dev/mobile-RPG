using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

    public class SystemPanel : MonoBehaviour
{
    public static SystemPanel instance;

    public TextMeshProUGUI text;
    [SerializeField] private GameObject panel;
    private Image image;
    private Sequence sequence;

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
        if(sequence != null) sequence.Kill();

        image.color -= new Color(0, 0, 0, 1);
        text.color -= new Color(0, 0, 0, 1);
        image.transform.position -= new Vector3(0, 15, 0);

        panel.SetActive(true);
        sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0.8f, 1f));
        sequence.Join(text.DOFade(0.8f, 1f));
        sequence.Join(image.transform.DOMoveY(image.transform.position.y + 15, 1.3f));
        sequence.AppendInterval(1);
        sequence.Append(image.DOFade(0, 1f));
        sequence.Join(text.DOFade(0, 1f));
        sequence.OnComplete(delegate { panel.SetActive(false); });

        /*
        panel.SetActive(true);
        StartCoroutine("FadeOut");
        */
    }

    /*
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
    */
}
