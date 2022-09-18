using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ClickAndMoveBtn : MonoBehaviour
{
    bool Expanded;
    [SerializeField] GameObject PlanetSelectPanel;
    [SerializeField] GameObject ClosedPos;
    [SerializeField] GameObject OpenedPos;
    [SerializeField] GameObject wishinputParentPos;
    [SerializeField] GameObject WishInput;
    [SerializeField] GameObject wishinputBtn;
    [SerializeField] GameObject wishinputParent;
    public DataBaseManager DataBaseManager;
    // Start is called before the first frame update
    void Start()
    {
        Expanded =false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //하단 컨텐츠 패널 여닫음
    public void OnClick()
    {
        if (Expanded)
        {
            PlanetSelectPanel.GetComponent<RectTransform>().DOMove(ClosedPos.transform.position, 2f);
            Expanded = false;
        }
        else if (!Expanded)
        {
            PlanetSelectPanel.GetComponent<RectTransform>().DOMove(OpenedPos.transform.position, 2f);
            //wishinputParent.GetComponent<RectTransform>().DOMove(wishinputParentPos.transform.position, 2f);

            Expanded = true;
        }
    }

    //소원을 적었을때 이벤트 : 행성 던지도록 유도
    public void OnWritten()
    {
        if(DataBaseManager.Mywish != "")
        {
            WishInput.gameObject.SetActive(false);
            wishinputBtn.SetActive(false);
            Expanded = true;
            PlanetSelectPanel.GetComponent<RectTransform>().DOMove(OpenedPos.transform.position, 2f);
            //wishinputParent.GetComponent<RectTransform>().DOMove(wishinputParentPos.transform.position, 2f);
        }
        
    }

    //연필 모양 아이콘 클릭시 텍스트 인풋 등장 여부
    public void PencilIconClicked()
    {
        if(wishinputBtn.activeSelf == true)
        {
            WishInput.gameObject.SetActive(false);
            wishinputBtn.SetActive(false);
        }
        else if(wishinputBtn.activeSelf == false)
        {
            WishInput.gameObject.SetActive(true);
            wishinputBtn.SetActive(true);
        }
    }
}
