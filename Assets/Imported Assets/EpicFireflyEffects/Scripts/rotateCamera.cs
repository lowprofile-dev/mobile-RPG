using UnityEngine;
using System.Collections;

public class rotateCamera : MonoBehaviour {

    public float turnSpeed = 10f;

    void Update () {
		transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
	}
}