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

    float angle;
    float velocity;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
    }
    private void Update()
    {
        // transform.rotation = Quaternion.LookRotation(cam.transform.position);

        Vector3 dir = (cam.transform.forward + cam.transform.up);

        float pos = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, pos, ref velocity, 0.0001f);
        transform.rotation = Quaternion.Euler(transform.rotation.x , angle , transform.rotation.z);
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
