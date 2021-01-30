////////////////////////////////////////////////////
/*
    File Monster.cs
    class Monster
    
    담당자 : 이신홍
    부 담당자 : 

    UI의 다양한 Animation 연출들을 모아둠
*/
////////////////////////////////////////////////////

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] protected GameObject _targetObj;
    [SerializeField] protected CanvasGroup _canvasGroup;

    private RectTransform _transform;
    private Vector3 _initTransform;

    // 시퀀스 목록
    public Sequence finalSequence;
    private Sequence _fadeInWithUpSequence; 
    private Sequence _fadeoutWithDownSequence;
    private Sequence _fadeout;
    private Sequence _xScaleUpSequence;
    private Sequence _colorChangeSequence;

    public void SetupUIAnimator()
    {
        _transform = _targetObj.GetComponent<RectTransform>();
        _initTransform = transform.position;
        finalSequence = DOTween.Sequence();
    }

    public void FinalPlay()
    {
        finalSequence.Play();
    }

    public void ResetFinalSequence()
    {
        if(finalSequence != null)
        {
            finalSequence.Kill();
            finalSequence = DOTween.Sequence();
        }
    }

    public void AppendSequence(Sequence sequence, float interval = 0.0f)
    {
        if(interval != 0) finalSequence.AppendInterval(interval);
        finalSequence.Append(sequence);
    }

    public void JoinSequence(Sequence sequence)
    {
        finalSequence.Join(sequence);
    }

    public Sequence FadeInWithUp()
    {
        if(_fadeInWithUpSequence != null) _fadeInWithUpSequence.Kill();
        _fadeInWithUpSequence = DOTween.Sequence();
        _fadeInWithUpSequence.Append(_transform.DOMoveY(_initTransform.y, 1.0f).From(_initTransform.y - 20, isRelative:false));
        _fadeInWithUpSequence.Join(_canvasGroup.DOFade(1.0f, 1.0f).From(0.0f));
        _fadeInWithUpSequence.SetEase(Ease.InSine);

        return _fadeInWithUpSequence;
    }

    public Sequence FadeOutWithDown()
    {
        if (_fadeoutWithDownSequence != null) _fadeoutWithDownSequence.Kill();
        _fadeoutWithDownSequence = DOTween.Sequence();
        _fadeoutWithDownSequence.Append(_transform.DOMoveY(_initTransform.y - 20, 1.0f).From(_initTransform.y, isRelative: false));
        _fadeoutWithDownSequence.Join(_canvasGroup.DOFade(0.0f, 1.0f).From(1.0f));
        _fadeoutWithDownSequence.SetEase(Ease.InSine);

        return _fadeoutWithDownSequence;
    }


    public Sequence FadeOut()
    {
        if (_fadeout != null) _fadeout.Kill();
        _fadeout = DOTween.Sequence();
        _fadeout.Append(_canvasGroup.DOFade(0.0f, 1.0f).From(1.0f));
        _fadeout.SetEase(Ease.InSine);

        return _fadeout;
    }


    public Sequence XScaleUp()
    {
        if (_xScaleUpSequence != null) _xScaleUpSequence.Kill();
        _xScaleUpSequence = DOTween.Sequence();
        _xScaleUpSequence.Append(_transform.DOScaleX(1, 0.7f).From(0, isRelative: false));
        _xScaleUpSequence.SetEase(Ease.OutExpo).easeOvershootOrAmplitude = 1.5f;

        return _xScaleUpSequence;
    }

    public Sequence ColorChange(Color color, Color initColor)
    {
        if (_colorChangeSequence != null) _colorChangeSequence.Kill();
        _colorChangeSequence = DOTween.Sequence();
        _colorChangeSequence.Append(GetComponent<Image>().DOColor(color, 0.1f).From(initColor));
        _colorChangeSequence.Append(GetComponent<Image>().DOColor(initColor, 0.8f).From(color));
        _colorChangeSequence.SetEase(Ease.InQuad);

        return _colorChangeSequence;
    }
}
