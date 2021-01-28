using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVignette : MonoBehaviour
{
    public static UIVignette Instance;

    [SerializeField] private Color _damagedColor;
    [SerializeField] private Color _initColor;
    [SerializeField] private Image _image;
    UIAnimator _animator;

    private void Start()
    {
        _animator = GetComponent<UIAnimator>();
        _animator.SetupUIAnimator();
        Instance = this;
    }

    public void ShowDamagedAnimation()
    {
        _animator.ResetFinalSequence();
        _animator.AppendSequence(_animator.ColorChange(_damagedColor, _initColor));
        _animator.FinalPlay();
    }
}
