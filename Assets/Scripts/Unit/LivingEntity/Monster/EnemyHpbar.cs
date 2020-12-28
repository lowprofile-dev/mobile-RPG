using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class EnemyHpbar : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] CinemachineFreeLook cam;
    [SerializeField] private Transform enemy;
    // Start is called before the first frame update
   
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        Vector3 dir = cam.transform.position - transform.forward;
        transform.rotation = Quaternion.LookRotation(dir);
        //transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

        hpSlider.value = enemy.GetComponent<LivingEntity>().Hp / enemy.GetComponent<LivingEntity>().initHp;
    }
}
