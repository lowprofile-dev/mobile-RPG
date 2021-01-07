using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillRange : MonoBehaviour
{
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject fillArea;
    private bool movePos = false;
    [SerializeField] BossSkeletonPase2 Boss;
    private float angle;
    private float velocity;

    private void OnEnable()
    {
        fillArea.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Boss = GameObject.Find("BossSkeletonPase2").GetComponent<BossSkeletonPase2>();
    }

    void Update()
    {

        fillArea.transform.localScale = Vector3.Lerp(fillArea.transform.localScale, Vector3.one, Time.deltaTime * 2f);

        if (movePos)
        {
            transform.position = Boss.transform.position;

            Vector3 dir = (Boss.transform.forward + Boss.transform.right);
        
            dir.y = 0f;

            float pos = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, pos, ref velocity, 0.001f);
     

            transform.rotation = Quaternion.Euler(90f, angle + 90f, 0f);



           

           // transform.eulerAngles = new Vector3(90f, 0f, Boss.Target.transform.eulerAngles.z);
            //Vector3 dir = Boss.Target.transform.position - Boss.transform.position;

            //Vector3 look = Vector3.Slerp(transform.forward, dir.normalized, Time.deltaTime);
            //look.Normalize();

            //transform.rotation = Quaternion.LookRotation(look, Vector3.forward);


        }
    }
    
    private void OnDisable()
    {
        movePos = false;
    }

    public void setFollow()
    {
        movePos = true;      
    }
}
