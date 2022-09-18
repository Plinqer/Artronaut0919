using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using DG.Tweening;

public class uiManager_Traditional : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject Chuimsae;
    RotationSpeedAdjust rotationspeedadjust;
    [SerializeField] int MissionNum;
    [SerializeField] TextMeshProUGUI Mission;
    [SerializeField] GameObject missionCompleteSeq;
    [SerializeField] GameObject missionCompleteSeq2;
    [SerializeField] TokenCheck TokenCheckForHome;
    [SerializeField] GameObject info;
    // Start is called before the first frame update
    void Start()
    {
        MissionNum = 0;

    }


    // Update is called once per frame
    void Update()
    {
        //�̼� �Ϸ�� �Ϸ� ������ �÷���
        if(MissionNum == 8)
        {
            missionCompleteSeq.SetActive(true);
        }
        //�̼� �Ϸ�� ��ū ȹ�� 
        if (missionCompleteSeq2.activeSelf)
        {
            TokenCheckForHome._TraditionToken = true;
        }

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit Hit = new RaycastHit();
            //��ư Ŭ���� �༺�� ��� ����, ��ī�̹ڽ� �ؽ��ĺ���, ���� ����, ���ӻ� �ؽ�Ʈ ����
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit))
            {
                if (Hit.transform.gameObject.CompareTag("btn") && Hit.transform.gameObject.GetComponent<RotationSpeedAdjust>() != null)
                {
                    GameObject btnobj = Hit.transform.gameObject;
                    rotationspeedadjust = btnobj.GetComponent<RotationSpeedAdjust>();
                    rotationspeedadjust.onclick();
                    foreach(MMFeedbacks f in rotationspeedadjust.feedbacks)
                    {
                        f.StopFeedbacks();
                    }
                    rotationspeedadjust.feedback.GetComponent<MMFeedbacks>().PlayFeedbacks();
                    ChuimsaeClicked();
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    GameObject InstantiatedTxt = Instantiate(Chuimsae, ray.origin, Quaternion.identity);
                    InstantiatedTxt.GetComponent<TextMeshPro>().text = rotationspeedadjust.ChuImSae;
                    InstantiatedTxt.transform.DOMove(target.position, 5f).SetEase(Ease.InOutQuart);
                    InstantiatedTxt.GetComponent<TextMeshPro>().DOFade(0, 15f);
                    //��ư �ۿ� 
                    Debug.Log("3D btn pressed");
                }
                else
                {
                    Debug.Log("btn not here");
                }
            }
            else
            {
                Debug.Log("raycast not working");
            }
        }
    }


    //��ư Ŭ���� �̼� ���� ���� ���� 
    public void ChuimsaeClicked()
    {
        MissionNum++;
        Mission.text = MissionNum.ToString();
        if (info.activeSelf == false)
        {
            info.SetActive(true);
        }
    }

}
