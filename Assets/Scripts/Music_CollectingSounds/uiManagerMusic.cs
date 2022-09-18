using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class uiManagerMusic : MonoBehaviour
{
    [SerializeField] GameObject tokenGet;
    [SerializeField] GameObject homeInfo;
    [SerializeField] GameObject missionInfo;
    [SerializeField] GameObject Panel;
    [SerializeField] Transform OpenPos;
    [SerializeField] Transform ClosePos;
    bool tokenget;
    // Start is called before the first frame update
    void Start()
    {
        tokenget = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //ÇÏ´Ü ÄÁÅÙÃ÷ ÆÐ³Î ¿©´ÝÀ½ ¸ð¼Ç
    public void PanelCloseOpen()
    {
        if (Panel.transform.position == OpenPos.transform.position)
        {
            Panel.transform.DOMove(ClosePos.transform.position, 1.5f).SetEase(Ease.InOutQuad);
        }
        else if(Panel.transform.position == ClosePos.transform.position)
        {
            Panel.transform.DOMove(OpenPos.transform.position, 1.5f).SetEase(Ease.InOutQuad);
        }
    }


}
