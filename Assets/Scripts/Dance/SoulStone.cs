using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.ComponentModel.Design.Serialization;
using UnityEngine.UI;
using TMPro;

public class SoulStone : MonoBehaviour
{
    [SerializeField] Image SoulStoneInfo;
    int AdventTime;
    [SerializeField] int MaxTime;
    [SerializeField] int MinTime;
    [SerializeField] bool firstStone;
    [SerializeField] AudioSource PreviousMusic;
    // Start is called before the first frame update
    void Start()
    {
        AdventTime = Random.Range(MinTime, MaxTime); //�� �𵨵� ���� ������ ���ӽð��� ���� ��ȥ�� ���� Ÿ�̹� ����
        StartCoroutine(SoulStoneFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //��ȥ�� ���� (fade in)
    IEnumerator SoulStoneFade()
    {
        yield return new WaitForSeconds(AdventTime - 5);
        this.transform.GetChild(0).gameObject.SetActive(true); //��ƼŬ1 ��ȥ�� ���� 5�� �� on
        this.transform.GetChild(1).gameObject.SetActive(true); //��ƼŬ2 ��ȥ�� ���� 5�� �� on
        yield return new WaitForSeconds(5);
        this.GetComponent<MeshRenderer>().material.DOFade(1, 5); //��ȥ�� ���̵���
        this.transform.parent = null; 
        this.transform.DOMove(new Vector3(this.transform.position.x + Random.Range(1, 4), transform.position.y + Random.Range(1,4), transform.position.z + Random.Range(1, 4)),5f).SetEase(Ease.InOutQuad); //��ȥ�� ������ ������ ������
        this.transform.DOScale(0.7f, 5f).SetEase(Ease.InOutQuad);
        this.tag = "Soul"; //��ȥ�� Ŭ���� ������ �� �ֵ��� �±� ����)
        if (firstStone)
        {
            //ù��° ��ȥ��(ù��° �� ���� ���, �ҿｺ�� ���� ���� �ȳ� ���� ���ٰ� 5�� �� �����)
            SoulStoneInfo.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            SoulStoneInfo.DOFade(0, 3f);
            SoulStoneInfo.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 3);
        }
    }


    //���� ���� ���̵� �ƿ�
    public void MusicOff()
    {
        if(PreviousMusic != null)
        {
            PreviousMusic.DOFade(0, 5f);

        }
    }
}
