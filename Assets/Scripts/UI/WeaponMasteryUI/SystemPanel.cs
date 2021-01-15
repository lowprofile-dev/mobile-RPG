using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SystemPanel : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] private GameObject panel;
    private Image image;
    private bool isCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        image = panel.transform.GetComponent<Image>();
        panel.SetActive(false);
    }

    public void Update()
    {
        if(WeaponManager.Instance != null && WeaponManager.Instance.GetWeapon() != null)
        {
            if (WeaponManager.Instance.LevelUpCheck() && isCheck == false)
            {
                FadeOutStart();
                isCheck = true;
                text.text = WeaponManager.Instance.GetWeapon().name.ToUpper() + " Level UP !!";
            }
        }
    }

    public void FadeOutStart()
    {
        panel.SetActive(true);
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        float time = 0f;
        Color temp = image.color;
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
                isCheck = false;
                break;
            }
        }
    }
}
