using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRemover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float time = 4f;
    float count = 0;

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if(count >= time)
        {
            count = 0;
            Destroy(gameObject);
        }

    }
}
