using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillRange : MonoBehaviour
{
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject fillArea;
    private bool movePos = false;
    [SerializeField] GameObject target;
    private float angle;
    private float velocity;
    private float speed;
    private void OnEnable()
    {
        fillArea.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void RemovedRange(GameObject target , float speed)
    {
        this.target = target;
        this.speed = speed;
        StartCoroutine(Remove());

    }
    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(speed);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
    void Update()
    {

        fillArea.transform.localScale = Vector3.Lerp(fillArea.transform.localScale, Vector3.one, Time.deltaTime * speed);

        if (movePos)
        {
            transform.position = target.transform.position;

            Vector3 dir = (target.transform.forward + target.transform.right);
        
            dir.y = 0f;

            float pos = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, pos, ref velocity, 0.001f);
     
            transform.rotation = Quaternion.Euler(90f, angle + 90f, 0f);

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
