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

    //�ϴ� ���� & ���� �г� ������ ���
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


    //���ڵ� ��ư ��
    public void Rec()
    {
        record.SetActive(true);
    }
    
    //�� ����
    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    

    //���ڵ� ��ư ��������, ȭ�� ���� ��ġ�� ������ ���͹ڽ� ����
    public void letterboxIn()
    {
        upperLetterbox.gameObject.SetActive(true);
        lowerLetterbox.gameObject.SetActive(true);
        upperLetterbox.DOMove(ulb_in.transform.position, 3f);
        lowerLetterbox.DOMove(llb_in.transform.position, 3f);
        StartCoroutine(shootCall());
    }

    //���ڵ� �����, ���͹ڽ� ���ڸ�(ȭ�� ������) ���ư�
    public void letterboxout()
    {
        upperLetterbox.DOMove(ulb_out.transform.position, 3f);
        lowerLetterbox.DOMove(llb_out.transform.position, 3f);
    }

    //���ڵ�
    IEnumerator shootCall()
    {
        Indicator.SetActive(false);
        Indicator.transform.GetChild(0).GetComponent<MeshRenderer>().material.DOFade(0, 2f); //�ε������� ȭ�鿡 ������ �ʵ��� ó��
        Indicator.transform.GetChild(1).GetComponent<TextMeshPro>().DOFade(0, 2f); // �ε������� �ڽ�1 ������Ʈ ���̵�
        Indicator.transform.GetChild(2).GetComponent<SpriteRenderer>().DOFade(0, 2f); // �ε������� �ڽ�2 ������Ʈ ���̵�
        roll.SetActive(true);
        yield return new WaitForSeconds(1f);
        roll.SetActive(false);
        speed.SetActive(true);
        yield return new WaitForSeconds(1f);
        speed.SetActive(false);
        action.SetActive(true);
        yield return new WaitForSeconds(1f);
        action.SetActive(false);
        //���� ��ȭ ����
        ReplayKitManager.StartRecording();
        yield return new WaitForSeconds(10f);
        ReplayKitManager.StopRecording(); 
        letterboxout();
        MissionCompleteSSeq.SetActive(true);
    }


    //���� ����
    public void ShareRecording()
    {
        ReplayKitManager.SharePreview();
    }
}
