using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

////////////////////////////////////////////////////
/*
    File SystemPanel.cs
    class SystemPanel

    담당자 : 김의겸
    부 담당자 : 이신홍
*/
////////////////////////////////////////////////////
///
public class SystemPanel : MonoBehaviour
{
    public static SystemPanel instance;

    public TextMeshProUGUI text;
    [SerializeField] private GameObject panel;
    private Image image;
    private Sequence sequence;

    void Start()
    {
        instance = this;
        image = panel.transform.GetComponent<Image>();
        panel.SetActive(false);
    }
    /// <summary>
    ///  시스템 패널에 출력하고 싶은 텍스트로 값을 변경해준다.
    /// </summary>
    /// <param name="inputText"> 시스템 패널에 출력하고 싶은 텍스트를 입력받는다.</param>
    public void SetText(string inputText)
    {
        text.text = inputText;
    }

    /// <summary>
    /// 시스템 패널을 출력해준다.
    /// DoTween Sequence를 사용하여 백그라운드 이미지와 텍스트를
    /// 천천히 위에서 아래로 나타나게 하고
    /// 1초의 간격 뒤에 천천히 사라지게 하는 함수.
    /// </summary>
    public void FadeOutStart()
    {
        AudioSource sound = SoundManager.Instance.PlayEffect(SoundType.UI, "UI/SpecialText", 0.9f);
        SoundManager.Instance.SetAudioReverbEffect(sound, AudioReverbPreset.Cave);

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
