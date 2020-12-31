using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeScript : MonoBehaviour
{
    public Image image;
    public Button button;
    public float coolTime = 10.0f;
    public bool isClicked = false;
    float leftTime = 0f;

    void Update()
    {
        if (isClicked)
            if (leftTime >= 0)
            {
                leftTime += Time.deltaTime;
                if (leftTime > coolTime)
                {
                    leftTime = coolTime;
                    if (button)
                        button.enabled = true;
                    isClicked = true;
                }

                float ratio = (leftTime / coolTime);
                if (image)
                    image.fillAmount = ratio;
            }
    }

    public void StartCoolTime()
    {
        leftTime = 0f;
        isClicked = true;
        if (button)
            button.enabled = false;
    }
}