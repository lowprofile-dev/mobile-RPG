using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    Image hpBar;
    float maxHp = 100f;
    public static float hp;
    // Start is called before the first frame update
    void Start()
    {
        hpBar = GetComponent<Image>();
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = hp / maxHp;
    }
}
