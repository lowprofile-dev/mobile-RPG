using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float alphaSpeed;
    [SerializeField] private float destoryTime;

    TextMeshPro txt;
    CinemachineFreeLook cam;

    float angle;
    float velocity;

    private void OnEnable()
    {

        txt = gameObject.GetComponent<TextMeshPro>();
        txt.transform.localScale = Vector3.one;
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
        cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
        SetDotween();
    }

    private void SetDotween()
    {
        txt.outlineWidth = 0.2f;
        txt.transform.DOScale(0.6f, 2).SetEase(Ease.InOutBack);
        txt.DOFade(0, 2).OnComplete(() => { DestroyObject(); });
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        transform.LookAt(2 * transform.position - cam.transform.position);
    }

    public void PlayRestore(int restore)
    {
        txt.colorGradient = new VertexGradient(GlobalDefine.Instance.textColorGradientRestoreTop, GlobalDefine.Instance.textColorGradientRestoreTop, GlobalDefine.Instance.textColorGradientRestoreBottom, GlobalDefine.Instance.textColorGradientRestoreBottom);
        txt.text = restore.ToString();
    }

    public void PlayDamage(int damage , bool IsCritical)
    {
        txt.colorGradient = new VertexGradient(GlobalDefine.Instance.textColorGradientDamagedTop, GlobalDefine.Instance.textColorGradientDamagedTop, GlobalDefine.Instance.textColorGradientDamagedBottom, GlobalDefine.Instance.textColorGradientDamagedBottom);

        if (IsCritical)
        {
            txt.text = damage.ToString();
        }
        else
        {
            txt.text = damage.ToString();
        }
    }

    public void PlayText(string text , string type)
    {
        txt.colorGradient = new VertexGradient(GlobalDefine.Instance.textColorGradientDamagedTop, GlobalDefine.Instance.textColorGradientDamagedTop, GlobalDefine.Instance.textColorGradientDamagedBottom, GlobalDefine.Instance.textColorGradientDamagedBottom);

        if (type == "player")
        {
            txt.text = text;
        }
        else
        {
            txt.text = text;
        }
    }

    private void DestroyObject()
    {
        ObjectPoolManager.Instance.ReturnObject(gameObject);
    }
}
