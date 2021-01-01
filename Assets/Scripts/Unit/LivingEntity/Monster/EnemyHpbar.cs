using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class EnemyHpbar : PrograssBar
{
    [SerializeField] CinemachineFreeLook cam;
    [SerializeField] protected Slider MpSlider;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        BarUpdate();
    }

    protected override void BarUpdate()
    {
        hpSlider.value = obj.GetComponent<LivingEntity>().Hp / obj.GetComponent<LivingEntity>().initHp;
        if(MpSlider != null)
        MpSlider.value = obj.GetComponent<LivingEntity>().Mp / obj.GetComponent<LivingEntity>().initMp;

        transform.rotation = Quaternion.LookRotation(cam.transform.position);
    }
}
