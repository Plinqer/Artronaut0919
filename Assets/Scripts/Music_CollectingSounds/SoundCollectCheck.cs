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

        //�̼� �Ϸ�� ���ø�Ʈ ������ ���
        if(collectCount == 5)
        {
            MissionCompleteSeq.SetActive(true);
        }
    }

    // 3d ��ư(���) Ŭ���� �̼� ���� ++ : �ڽ� ������Ʈ �� feedback�̺�Ʈ�� ����Ǿ�����
    public void collect()
    {
        collectCount++;
        CountCollection.text = collectCount.ToString();
        //collectchoice.SetActive(false);
        //CollectingEnable.collected = true;
    }

}
