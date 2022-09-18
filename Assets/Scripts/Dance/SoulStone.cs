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
        AdventTime = Random.Range(MinTime, MaxTime); //각 모델들 마다 움직임 지속시간에 맞춰 영혼석 등장 타이밍 설정
        StartCoroutine(SoulStoneFade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //영혼석 등장 (fade in)
    IEnumerator SoulStoneFade()
    {
        yield return new WaitForSeconds(AdventTime - 5);
        this.transform.GetChild(0).gameObject.SetActive(true); //파티클1 영혼석 등장 5초 전 on
        this.transform.GetChild(1).gameObject.SetActive(true); //파티클2 영혼석 등장 5초 전 on
        yield return new WaitForSeconds(5);
        this.GetComponent<MeshRenderer>().material.DOFade(1, 5); //영혼석 페이드인
        this.transform.parent = null; 
        this.transform.DOMove(new Vector3(this.transform.position.x + Random.Range(1, 4), transform.position.y + Random.Range(1,4), transform.position.z + Random.Range(1, 4)),5f).SetEase(Ease.InOutQuad); //영혼석 임의의 공간에 떠오름
        this.transform.DOScale(0.7f, 5f).SetEase(Ease.InOutQuad);
        this.tag = "Soul"; //영혼석 클릭시 반응할 수 있도록 태그 변경)
        if (firstStone)
        {
            //첫번째 영혼석(첫번째 댄서 모델일 경우, 소울스톤 등장 관한 안내 문구 떴다가 5초 후 사라짐)
            SoulStoneInfo.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
            SoulStoneInfo.DOFade(0, 3f);
            SoulStoneInfo.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 3);
        }
    }


    //이전 음악 페이드 아웃
    public void MusicOff()
    {
        if(PreviousMusic != null)
        {
            PreviousMusic.DOFade(0, 5f);

        }
    }
}
