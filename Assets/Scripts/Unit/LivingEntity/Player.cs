using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.transform.Translate(new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")) * 3 * Time.deltaTime);
    }

    
}
