using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossSkillRangeFill : MonoBehaviour
{
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject fillArea;
    private bool movePos = false;
    [SerializeField] GameObject target;
    [SerializeField] GameObject parent;

    private float angle;
    private float velocity;
    private float speed;
    private void OnEnable()
    {
        fillArea.GetComponent<Image>().fillAmount = 0f;
    }

    public void RemovedRange(GameObject parent ,GameObject target ,float speed)
    {
        this.parent = parent;
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

        fillArea.GetComponent<Image>().fillAmount = Mathf.Lerp(fillArea.GetComponent<Image>().fillAmount, 1f, Time.deltaTime * speed);

        if (movePos)
        {
          
            transform.position = parent.transform.position;


            Vector3 dir = (parent.transform.forward + parent.transform.right);

            dir.y = 0f;

            float pos = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, pos, ref velocity, 0.001f);


            transform.rotation = Quaternion.Euler(90f, angle - 45f, 0f);
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
