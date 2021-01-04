using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DamageText : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float alphaSpeed;
    [SerializeField] private float destoryTime;

    TextMesh txt;
    CinemachineFreeLook cam;

    //private void Start()
    //{
    //    txt = gameObject.GetComponent<TextMesh>();
    //    //cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
    //}

    private void OnEnable()
    {
        txt = gameObject.GetComponent<TextMesh>();
        StartCoroutine(DestroyObject());
        cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        transform.rotation = Quaternion.LookRotation(cam.transform.position);
        //alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        //text.color = alpha;

    }

    public void PlayDamage(float damage , bool IsCritical)
    { 
        if (IsCritical)
        {
            txt.text = "<color=#ff0000>" + "-" + damage + "</color>";
        }
        else
        {
            txt.text = "<color=#ffffff>" + "-" + damage + "</color>";
        }
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destoryTime);
        //Destroy(gameObject);
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
