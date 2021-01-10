using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHoleMagnetic : MonoBehaviour
{
    [SerializeField] private float forceFactor = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterController>().SimpleMove((transform.position - other.gameObject.transform.position) * forceFactor);
        }
    }
}
