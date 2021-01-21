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
        cam = CameraManager.Instance.PlayerFollowCamera;
        StartCoroutine("LookTarget");
    }

    private IEnumerator LookTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            transform.LookAt(cam.transform);
        }
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
