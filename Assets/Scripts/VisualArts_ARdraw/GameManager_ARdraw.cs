using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using ARLocation.MapboxRoutes;
using System.Reflection;
using ARLocation;
using TMPro;
using UnityEngine.XR.ARSubsystems;

public class GameManager_ARdraw : MonoBehaviour
{
    [SerializeField] GameObject TokenMissionCheck;
    [SerializeField] string NaviMessage;
    public ARRaycastManager raymanager;
    [SerializeField] MapboxRoute MapboxRoute;
    [SerializeField] TokenCheck TokenCheckForHome;
    [SerializeField] GameObject CompleteSeq;
    string distance;
    [SerializeField] GameObject map;
    [SerializeField] SignPost signpost;
    [SerializeField] GameObject TooFarInfo;
    [SerializeField] GameObject LookForTarget;
    private Pose placementPose;
    [SerializeField] ARSessionOrigin arOrigin;
    public bool EnableSpawn = false;
    public Camera ARcamera;
    private bool spawned = false;
    ARDrawManager ardrawManager;
    public GameObject instantiatedObj;
    public GameObject spawnedPos;
    [SerializeField] GameObject[] navis;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    private bool placementPoseIsValid;
    [SerializeField] GameObject ContentsPanel;
    [SerializeField] GameObject PlacementInfo;
    [SerializeField] GameObject Confirm;
    [SerializeField] private GameObject Mission;

    // Start is called before the first frame update
    void Start()
    {
        //네비게이션 루트 안내 오브젝트
        MapboxRoute = FindObjectOfType<MapboxRoute>();
        //목적지에서 거리 30m이하일 시 나타나는 알림
        LookForTarget.SetActive(false);
        //하단 브러쉬, 스티커 담은 패널창
        ContentsPanel.SetActive(false);
        //좌측 하단 토큰 & 미션 아이콘
        TokenMissionCheck.SetActive(false);
        //씬 시작시, 네비게이션 커스텀 루트 - 현재 위치에서 가장 가까운 포인트로 안내해주기
        MapboxRoute.ClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //네비 안내창 & 스크립트
        signpost = FindObjectOfType<SignPost>();
        navis = GameObject.FindGameObjectsWithTag("map");


        if (CompleteSeq.activeSelf)
        {
            TokenCheckForHome._ArtToken = true;
            //token check(ScriptableObj)에 아트 토큰 획득 처리
        }
   
        //행성 소환
        if (EnableSpawn && Input.GetMouseButtonDown(0))
        {
            Debug.Log("works!");

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 4f));
            if (IsPointOverUI(Input.mousePosition))
            {
                Debug.Log("nothing");
            }
            else if (!spawned)
            {
                spawnedPos = Instantiate(instantiatedObj, mousePosition, Quaternion.identity);
                spawned = true;
                ContentsPanel.SetActive(true);
                PlacementInfo.SetActive(false);
                EnableSpawn = false;
            }
            else if (spawned)
            {
                Debug.Log("Already Spawned");
            }
        }

        //브러쉬 3번 그렸을 시, 이대로 완성하기 컨펌 버튼 뜸
        if (arOrigin.transform.childCount > 5)
        {
            Confirm.SetActive(true);
        }

        GpsCheck();

        IscloseEnough();



    }

    void GpsCheck()
    {

        if (signpost != null)
        {
            //나의 위치가 1000m이상이면, 너무 멀다는 인포메이션이 뜸
            if (signpost.IsTooFar)
            {
                TooFarInfo.SetActive(true);
                //위치가 너무 멀 경우, 네비 모두 자동 종료됨
                foreach (GameObject m in navis)
                {
                    m.SetActive(false);
                }
                //미션 창 또한 사라짐
                Mission.SetActive(false);
            }
            else
            {
                //위치가 1000m 이내일 경우 네비 안내 이어짐, 가장 가까운 루트 포인트로 안내.
                TooFarInfo.SetActive(false);
                MapboxRoute.ClosestTarget();

            }
        }

    }


    //이미지 타겟 인식했을시, 네비게이션 종료 및 게임진행에 필요한 창들 모두 뜸.
    public void mapCloseOndetect()
    {
        if(navis != null)
        {
            foreach (GameObject m in navis)
            {
                m.SetActive(false);
            }

        }
        Mission.SetActive(true);
        PlacementInfo.SetActive(true);
        TooFarInfo.SetActive(false);
        ContentsPanel.SetActive(true);
        TokenMissionCheck.SetActive(true);
        EnableSpawn = true;
        LookForTarget.SetActive(false);

    }


    //충분히 가까운지 판별
    public void IscloseEnough()
    {
        for(int i=0; i < navis.Length; i++)
        {
            //마지막 안내 문구가 navimessage와 일치 시 + 30m이하일시 타겟 찾으라는 문구 뜸
            if (navis[i].gameObject.name == "DirectionsLabel")
            {
                GameObject directionlabel = navis[i].gameObject;
                if(directionlabel.GetComponent<TextMeshPro>().text == NaviMessage && signpost.IscloseEnough) 
                {
                    LookForTarget.SetActive(true);
                }

            }

            else
            {
                Debug.Log("navi not working");
            }
        }
    }



    //ui 위에서 클릭시 클릭 되지 않도록
    private bool IsPointOverUI(Vector2 fingerPosition)
    {
        PointerEventData eventDataPosition = new PointerEventData(EventSystem.current);
        eventDataPosition.position = fingerPosition;
        EventSystem.current.RaycastAll(eventDataPosition, raycastResults);
        return raycastResults.Count > 0;
    }
}
