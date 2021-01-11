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

    float angle;
    float velocity;

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
        transform.LookAt(2 * transform.position - cam.transform.position);
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
