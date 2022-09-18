using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class SoundCollectCheck : MonoBehaviour
{
    [SerializeField] GameObject MissionCompleteSeq;
    //public AudioSource currentlyPlaying;
    [SerializeField] TextMeshProUGUI CountCollection;
    public int collectCount = 0;
    //public GameObject collectchoice;
    //public CollectingEnable CollectingEnable;
    //[SerializeField] Image collectnum;

    private void Update()
    {
       // CollectingEnable = currentlyPlaying.gameObject.GetComponent<CollectingEnable>();

        //미션 완료시 컴플리트 시퀀스 재생
        if(collectCount == 5)
        {
            MissionCompleteSeq.SetActive(true);
        }
    }

    // 3d 버튼(고둥) 클릭시 미션 숫자 ++ : 자식 오브젝트 중 feedback이벤트에 연결되어있음
    public void collect()
    {
        collectCount++;
        CountCollection.text = collectCount.ToString();
        //collectchoice.SetActive(false);
        //CollectingEnable.collected = true;
    }

}
