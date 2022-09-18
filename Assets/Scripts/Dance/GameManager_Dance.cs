using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager_Dance : MonoBehaviour
{
    [SerializeField] TokenCheck tokencheckforHome;
    [SerializeField] GameObject Soundinfo;
    [SerializeField] GameObject Mission;
    [SerializeField] GameObject Move1;
    [SerializeField] GameObject Move2;
    [SerializeField] GameObject Move3;
    [SerializeField] GameObject Move4;
    [SerializeField] GameObject CheckUI;
    [SerializeField] GameObject LookforTarget;
    [SerializeField] bool ExhibitionStart = false;
    int SoulCollect =0;
    [SerializeField] TextMeshProUGUI CollectCountText;
    [SerializeField] GameObject CompleteSeq;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    // Start is called before the first frame update
    void Start()
    {
        Soundinfo.SetActive(false) ;
        LookforTarget.SetActive(true);
        Mission.SetActive(false);
        CheckUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit Hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit))
            {
                //영혼석 클릭시, 유저 포지션으로 다가오면서 페이드아웃
                if (Hit.transform.gameObject.CompareTag("Soul"))
                {
                    GameObject btnobj = Hit.transform.gameObject;
                    btnobj.transform.DOMove(Camera.main.transform.position, 7f).SetEase(Ease.InOutQuint);
                    btnobj.GetComponent<SoulStone>().MusicOff();
                    btnobj.GetComponent<MeshRenderer>().material.DOFade(0, 5f);
                    btnobj.transform.parent = Camera.main.transform;
                    //버튼 작용 
                    Debug.Log("btnobj name is " + btnobj.gameObject.name);
                    SoulCollect++;
                    CollectCountText.text = SoulCollect.ToString();
                    Debug.Log("3D btn pressed");
                    //다음 움직임 on
                    if(btnobj.gameObject.name == "Diamond1")
                    {
                        Move2.SetActive(true);
                    }
                    else if (btnobj.gameObject.name == "Diamond2")
                    {
                        Move3.SetActive(true);
                    }
                    else if (btnobj.gameObject.name == "Diamond3")
                    {
                        Move4.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log("btn not here");
                }
            }
        }

        //미션 완료 조건 충족시, 토큰 획득 저장 및 컴플리트 시퀀스 재생
        if(SoulCollect == 4)
        {
            CompleteSeq.SetActive(true);
            tokencheckforHome._DanceToken = true;
        }

        //이미지 스캔 후 화면 한번 클릭하면 첫번째 댄서 모델 등장
        if (ExhibitionStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Move1.SetActive(true);
                Soundinfo.SetActive(false);
            }
        }
    }

    //이미지 타겟 인식시 필요한 오브젝트 on
    public void OnImageDetect()
    {
        Mission.SetActive(true);
        CheckUI.SetActive(true);
        LookforTarget.SetActive(false);
        Soundinfo.SetActive(true);
        Move1.SetActive(true);
        ExhibitionStart = true;
    }
}
