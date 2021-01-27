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

public class UIAnimator : MonoBehaviour
{
    [SerializeField] protected GameObject _targetObj;
    [SerializeField] protected CanvasGroup _canvasGroup;

    private RectTransform _transform;
    private Vector3 _initPosition;

    // 시퀀스 목록
    private Sequence fadeInWithUpSequence;


    private void OnEnable()
    {
        _transform = _targetObj.GetComponent<RectTransform>();
        _initPosition = _transform.position;
        _canvasGroup.alpha = 0;
    }

    public void FadeInWithUp()
    {
        _canvasGroup.alpha = 0;
        _transform.position = _initPosition;

        fadeInWithUpSequence = DOTween.Sequence();
        fadeInWithUpSequence.Join(_transform.DOMoveY(_initPosition.y - 50, 0.0f));
        fadeInWithUpSequence.Append(_canvasGroup.DOFade(1.0f, 1.0f));
        fadeInWithUpSequence.Join(_transform.DOMoveY(_transform.position.y, 1.0f));
        fadeInWithUpSequence.SetEase(Ease.InSine);
        fadeInWithUpSequence.Play();
    }
}
