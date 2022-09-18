using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using VoxelBusters.CoreLibrary;
using VoxelBusters.ReplayKit;
using TMPro;

public class ScrollOpenClose : MonoBehaviour
{
    [SerializeField] GameObject MissionCompleteSSeq;
    [SerializeField] Transform upperLetterbox;
    [SerializeField] Transform lowerLetterbox;
    [SerializeField] Transform ulb_in;
    [SerializeField] Transform ulb_out;
    [SerializeField] Transform llb_in;
    [SerializeField] Transform llb_out;
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Indicator;
    [SerializeField] GameObject share;
    [SerializeField] GameObject roll;
    [SerializeField] GameObject speed;
    [SerializeField] GameObject action;
    [SerializeField] Transform PanelOpenPos;
    [SerializeField] Transform PanelClosePos;
    public GameObject record;

    // Start is called before the first frame update
    void Start()
    {
        upperLetterbox.gameObject.SetActive(false);
        lowerLetterbox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //하단 데코 & 필터 패널 여닫음 모션
    public void PanelOpenClose()
    {
        if(Panel.transform.position == PanelOpenPos.position)
        {
            Panel.transform.DOMove(PanelClosePos.position, 2f).SetEase(Ease.InOutQuad);
        }
        else if(Panel.transform.position == PanelClosePos.position)
        {
            Panel.transform.DOMove(PanelOpenPos.position, 2f).SetEase(Ease.InOutQuad);
        }

    }    


    //레코딩 버튼 뜸
    public void Rec()
    {
        record.SetActive(true);
    }
    
    //씬 변경
    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    

    //레코딩 버튼 눌렀을시, 화면 상하 위치에 검은색 레터박스 들어옴
    public void letterboxIn()
    {
        upperLetterbox.gameObject.SetActive(true);
        lowerLetterbox.gameObject.SetActive(true);
        upperLetterbox.DOMove(ulb_in.transform.position, 3f);
        lowerLetterbox.DOMove(llb_in.transform.position, 3f);
        StartCoroutine(shootCall());
    }

    //레코딩 종료시, 레터박스 제자리(화면 밖으로) 돌아감
    public void letterboxout()
    {
        upperLetterbox.DOMove(ulb_out.transform.position, 3f);
        lowerLetterbox.DOMove(llb_out.transform.position, 3f);
    }

    //레코딩
    IEnumerator shootCall()
    {
        Indicator.SetActive(false);
        Indicator.transform.GetChild(0).GetComponent<MeshRenderer>().material.DOFade(0, 2f); //인디케이터 화면에 보이지 않도록 처리
        Indicator.transform.GetChild(1).GetComponent<TextMeshPro>().DOFade(0, 2f); // 인디케이터 자식1 오브젝트 페이드
        Indicator.transform.GetChild(2).GetComponent<SpriteRenderer>().DOFade(0, 2f); // 인디케이터 자식2 오브젝트 페이드
        roll.SetActive(true);
        yield return new WaitForSeconds(1f);
        roll.SetActive(false);
        speed.SetActive(true);
        yield return new WaitForSeconds(1f);
        speed.SetActive(false);
        action.SetActive(true);
        yield return new WaitForSeconds(1f);
        action.SetActive(false);
        //영상 녹화 시작
        ReplayKitManager.StartRecording();
        yield return new WaitForSeconds(10f);
        ReplayKitManager.StopRecording(); 
        letterboxout();
        MissionCompleteSSeq.SetActive(true);
    }


    //영상 공유
    public void ShareRecording()
    {
        ReplayKitManager.SharePreview();
    }
}
