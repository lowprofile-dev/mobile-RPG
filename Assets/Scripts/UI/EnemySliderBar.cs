using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class EnemySliderBar : MonoBehaviour
{
    CinemachineFreeLook cam;
    [SerializeField] protected Slider HPSlider;
    [SerializeField] protected Slider CastSlider;
    [SerializeField] protected Monster parent;
    
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.position);
    }

    public void HpUpdate()
    {
        HPSlider.value = parent.Hp / parent.initHp;
    }

    public void CastUpdate()
    {
        CastSlider.value = parent.GetComponent<MonsterAction>()._cntCastTime / parent.GetComponent<MonsterAction>()._castTime;

    }
}
