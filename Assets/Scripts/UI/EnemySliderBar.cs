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
    protected MonsterAction parentAction;

    private void OnEnable()
    {     
        cam = CameraManager.Instance.PlayerFollowCamera;
        StartCoroutine("LookTarget");
        parentAction = parent.GetComponent<MonsterAction>();
    }

    // 카메라 방향으로 bar들을 회전
    private IEnumerator LookTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            transform.LookAt(cam.transform);
        }
    }

    public void HpUpdate() // 해당 오브젝트의 hp 업데이트
    {
        HPSlider.value = parent.Hp / parent.initHp;
    }

    public void CastUpdate() // 해당 오브젝트의 casting 업데이트
    {
        CastSlider.value = parentAction._cntCastTime / parentAction._castTime;
    }
}
