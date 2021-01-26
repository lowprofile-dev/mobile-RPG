using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody arrowRigidbody;
    public void Launch()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        arrowRigidbody.rotation = Quaternion.LookRotation(transform.forward);
        arrowRigidbody.velocity = transform.forward * speed;
    }
    //void Update()
    //{
    //    transform.Translate(transform.forward * speed * Time.deltaTime);
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
