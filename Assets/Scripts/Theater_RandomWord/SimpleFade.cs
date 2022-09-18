using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SimpleFade : MonoBehaviour
{

    //간단한 페이드 인 스크립트


    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<SpriteRenderer>() != null)
        {
            this.transform.DOShakePosition(3f, 0.7f, 20, 10).SetEase(Ease.InOutQuad);
            this.GetComponent<SpriteRenderer>().material.DOFade(1, 3.5f);
            this.GetComponent<SpriteRenderer>().DOColor(Color.white, 3.5f);
        }
        else if(this.GetComponent<TextMeshPro>() != null)
        {
            this.GetComponent<TextMeshPro>().DOFade(1, 3.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
