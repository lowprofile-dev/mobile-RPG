using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    Image Bar;
    float maxHp = 100f;
    public bool barSet = false;
    // Start is called before the first frame update
    void Start()
    {
        Bar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "HP")
        {
            if (Player.Instance != null && barSet == false)
            {
                maxHp = Player.Instance.initHp;
                barSet = true;
            }
            if (barSet) Bar.fillAmount = Player.Instance.Hp / maxHp;

        }
        else
        {
            if (Player.Instance != null && barSet == false)
            {
                maxHp = Player.Instance.initMp;
                barSet = true;
            }
            if (barSet) Bar.fillAmount = Player.Instance.Mp / maxHp;
        }

    }
}
