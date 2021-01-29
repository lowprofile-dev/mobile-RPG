////////////////////////////////////////////////////
/*
    File DamageText.cs
    class DamageText
    
    담당자 : 안영훈
    부 담당자 : 이신홍

    플레이어 , 몬스터가 입는 데미지를 보여주는 텍스트
*/
////////////////////////////////////////////////////

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
        // 애니메이션 다시 재생시 Dotween 초기화를 위한 작업
        txt = gameObject.GetComponent<TextMeshPro>(); 
        txt.transform.localScale = Vector3.one;
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
        txt.fontSize = 11;

        cam = GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineFreeLook>();
        SetDotween();
    }

    /// <summary>
    /// Dotween 애니메이션을 설정한다.
    /// </summary>
    private void SetDotween()
    {
        txt.outlineWidth = 0.2f; // 표면 아웃라인
        txt.transform.DOScale(0.6f, 2).SetEase(Ease.InOutBack); // 점점 작아지도록 설정, EaseType 설정
        txt.DOFade(0, 2).OnComplete(() => { DestroyObject(); }); // 점점 사라지도록 하고 끝난 뒤 파괴
    }

    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0)); // 점점 위로 이동하도록
        transform.LookAt(2 * transform.position - cam.transform.position); // 카메라를 바라보도록
    }

    /// <summary>
    /// 회복 텍스트
    /// </summary>
    public void PlayRestore(int restore)
    {
        txt.colorGradient = new VertexGradient(GlobalDefine.Instance.textColorGradientRestoreTop, GlobalDefine.Instance.textColorGradientRestoreTop, GlobalDefine.Instance.textColorGradientRestoreBottom, GlobalDefine.Instance.textColorGradientRestoreBottom);
        txt.text = restore.ToString();
    }

    /// <summary>
    /// 데미지 텍스트
    /// </summary>
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

    /// <summary>
    /// 데미지 텍스트
    /// </summary>
    public void PlayCriticalDamage(int damage, bool IsCritical)
    {
        txt.colorGradient = new VertexGradient(GlobalDefine.Instance.textColorGradientCriticalBottom, GlobalDefine.Instance.textColorGradientCriticalTop, GlobalDefine.Instance.textColorGradientCriticalBottom, GlobalDefine.Instance.textColorGradientCriticalTop);
        txt.fontSize += 3.0f;

        if (IsCritical)
        {
            txt.text = damage.ToString();
        }
        else
        {
            txt.text = damage.ToString();
        }
    }



    /// <summary>
    /// 기타 텍스트
    /// </summary>
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
