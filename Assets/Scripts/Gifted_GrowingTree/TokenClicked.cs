using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TokenClicked : MonoBehaviour
{
    public GameObject Token;
    public GameManager_GrowingTree gameManager;
    [SerializeField] TextMeshProUGUI TokenGivenTxt;
    [SerializeField] Color color;
    [SerializeField] Image Tokenimg;
    [SerializeField] string txtforInfo;
    float position;
    // Start is called before the first frame update
    void Start()
    {
        position = Random.Range(5, 8);   
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onclick()
    {
        TokenGivenTxt.DOFade(1, 2f);
        TokenGivenTxt.text = txtforInfo;
        Tokenimg.DOColor(color, 2f);
        Token.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
