//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraCollision : MonoBehaviour
//{
//    [SerializeField] Transform obstruction;
//    [SerializeField] Transform target;

//    private void Start()
//    {
//        obstruction = target;
//    }

//    private void LateUpdate()
//    {
//        ViewObstructed();
//    }

//    void ViewObstructed()
//    {
//        RaycastHit hit;

//        if (Physics.Raycast(transform.position, target.position - transform.position, out hit))
//        {
//            if (hit.collider.gameObject.tag != "Player")
//            {
//                obstruction = hit.transform;
//                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
//            }
//            else
//            {
//                obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
//            }
//        }
//    }

//}
