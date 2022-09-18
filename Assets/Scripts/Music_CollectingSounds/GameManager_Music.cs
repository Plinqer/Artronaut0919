using ARLocation;
using ARLocation.MapboxRoutes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class GameManager_Music : MonoBehaviour
{
    
    [SerializeField]
    GameObject[] Conchs;
    [SerializeField] private GameObject Mission;
    [SerializeField] private GameObject LookforTarget;
    [SerializeField] private GameObject CheckUI;
    [SerializeField] GameObject missionCompleteSeq2;
    [SerializeField] GameObject SoundPanel;
    [SerializeField] TokenCheck TokenCheckForHome;
    
    // Start is called before the first frame update
    void Start()
    {
        SoundPanel.SetActive(false);
        LookforTarget.SetActive(true);
        Mission.SetActive(false);
        CheckUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //미션 완료 후 토큰 저장
        if (missionCompleteSeq2.activeSelf)
        {
            TokenCheckForHome._MusicToken = true;
        }


        //고둥 클릭 이벤트
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit Hit = new RaycastHit();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit))
            {
                if (Hit.transform.gameObject.CompareTag("Conch"))
                {
                    GameObject CurConch = Hit.transform.gameObject; //현재 클릭한 고둥
                    CurConch.GetComponent<CollectingEnable>().SoundFound(); //클릭한 고둥 ui창에 표시
                }
            }
        }
    }

    //이미지 타겟 인식 했을시, 미션에 필요한 오브젝트 on + 고둥 랜덤한 공간에 생성
    public void ImgTargetdetect()
    {
        SoundPanel.SetActive(true);
        foreach (GameObject c in Conchs)
        {
            c.SetActive(true);
            c.transform.position = new Vector3(Random.Range(-4.5f, 4.5f),Random.Range(-4.5f, 4.5f),Random.Range(-4.5f, 3.5f));
        }
        LookforTarget.SetActive(false);
        Mission.SetActive(true);
        CheckUI.SetActive(true);
    }
}
