using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PrograssBar : MonoBehaviour
{
    [SerializeField] protected Slider hpSlider;
    [SerializeField] protected Transform obj;

    protected virtual void BarUpdate() { }
}
