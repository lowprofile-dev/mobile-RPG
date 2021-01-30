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
    UIAnimator animator;

    void Start()
    {
        instance = this;
        image = panel.transform.GetComponent<Image>();
        panel.SetActive(false);
        animator = GetComponent<UIAnimator>();
        animator.SetupUIAnimator();
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
    /// </summary>
    public void FadeOutStart()
    {
        SoundManager.Instance.StopEffect("SpecialText");
        AudioSource sound = SoundManager.Instance.PlayEffect(SoundType.UI, "UI/SpecialText", 0.5f);
        SoundManager.Instance.SetAudioReverbEffect(sound, AudioReverbPreset.Cave);

        
        panel.SetActive(true);
        animator.ResetFinalSequence();
        animator.AppendSequence(animator.XScaleUp().AppendInterval(0.7f));
        animator.AppendSequence(animator.FadeOut().OnComplete(delegate { panel.SetActive(false); }));
        animator.FinalPlay();
    }
}
