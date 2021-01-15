using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI text;
    Button button;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        image = panel.GetComponent<Image>();
        panel.SetActive(false);
    }

    // Update is called once per frame

    public void onbuttonClick()
    {
        //if (name == "DAGGER" || name == "STAFF" || name == "BLUNT")
        //{
        //    StartCoroutine("FadeOut");
        //    return;
        //}
        if (name == "SWORD" || name == "WAND")
        {
            text.text = name + " 착용 !";
            WeaponManager.Instance.SetWeapon(name);
        }
        else
        {
            text.text = "아직 준비 중입니다.";
        }
        StartCoroutine("FadeOut");
    }


    IEnumerator FadeOut()
    {
        float time = 0f;
        Color temp = image.color;
        panel.SetActive(true);
        while (image.color.a >= 0.0f)
        {
            time += Time.deltaTime;
            temp.a = Mathf.Lerp(0f, 1f, time);
            image.color = temp;
            text.color = temp;
            yield return null;
            if (image.color.a >= 1f)
            {
                panel.SetActive(false);
                break;
            }
        }

    }
}
