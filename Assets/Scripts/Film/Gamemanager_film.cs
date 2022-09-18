using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.XR;
using System;
using ARLocation.MapboxRoutes;
using DG.Tweening;
using ARLocation;
using TMPro;

public class Gamemanager_film : MonoBehaviour
{
    [SerializeField] string NaviMessage;
    [SerializeField] GameObject LookForTarget;
    [SerializeField] GameObject CheckUI;
    [SerializeField]MapboxRoute MapboxRoute;
    public ARRaycastManager raymanager;
    string distance;
    [SerializeField] GameObject map;
    ScrollOpenClose soc;
    [SerializeField] GameObject[] navis;
    [SerializeField] GameObject FindImgTarget;
    [SerializeField] GameObject missionCompleteSeq2;
    [SerializeField] TokenCheck TokenCheckForHome;
    [SerializeField] GameObject TokenMission;
    [SerializeField] GameObject TooFarInfo;
    [SerializeField] GameObject Mission;
    [SerializeField] GameObject GamePanel;
    [SerializeField] SignPost signpost;
    public GameObject placementIndicator;
    public bool placementPoseIsValid = false;
    private Pose placementPose;
    [SerializeField] ARSessionOrigin arOrigin;
    public GameObject instantiatedObj;
    private GameObject spawnedPos;
    public Camera ARcamera;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    // Start is called before the first frame update
    void Start()
    {
        MapboxRoute = FindObjectOfType<MapboxRoute>();

        LookForTarget.SetActive(false);
        Mission.SetActive(false);
        TokenMission.SetActive(false);
        CheckUI.SetActive(false);
        GamePanel.SetActive(false);
        soc = FindObjectOfType<ScrollOpenClose>();
        MapboxRoute.ClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();


        navis = GameObject.FindGameObjectsWithTag("map");
        signpost = FindObjectOfType<SignPost>();

        // 최초 모델 소환 이후 record 버튼 및 안내 뜸
        if (placementPoseIsValid && Input.GetMouseButtonDown(0))
        {
            Debug.Log("works!");
            //ui position 클릭시 반응 x
            if (IsPointOverUI(Input.mousePosition))
            {
                Debug.Log("nothing");
            }
            else
            {
                //소환 + 패널 닫음(열기) + 레코딩 버튼 뜸
                spawnedPos = Instantiate(instantiatedObj, placementPose.position, placementPose.rotation);
                soc.PanelOpenClose();
                soc.Rec();
            }
        }


        GpsCheck();

        if (missionCompleteSeq2.activeSelf)
        {
            TokenCheckForHome._FilmToken = true;
        }

        IscloseEnough();

    }

    //네비 1000m이상일 경우 네비 종료 및 너무 거리 멀다 안내 
    //아닐 경우(1000m이하일 경우 가장 가까운 포인트로 안내)
    void GpsCheck()
    {

        if (signpost != null)
        {
            if (signpost.IsTooFar)
            {
                TooFarInfo.SetActive(true);
                foreach (GameObject m in navis)
                {
                    m.SetActive(false);
                }
            }
            else
            {
                TooFarInfo.SetActive(false);
                MapboxRoute.ClosestTarget();

            }
        }

    }


  
    public void IndicatorInactive()
    {
        placementPoseIsValid = false;
    }

    //plane 인식 후 indicator position설정

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && instantiatedObj != null)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            Debug.Log("Valid");
        }
        else
        {
            placementIndicator.SetActive(false);
            Debug.Log("Invalid");

        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = ARcamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        //arOrigin.GetComponent<ARRaycastManager>().Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        if (raymanager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes))
        {
            placementPoseIsValid = true;
        }
        //placementPoseIsValid = hits.Count > 0 || Input.GetMouseButtonDown(0);
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }


    //충분히 가까운지 판별
    //마지막 안내 문구가 navimessage와 일치 시 + 30m이하일시 타겟 찾으라는 문구 뜸

    public void IscloseEnough()
    {
        for (int i = 0; i < navis.Length; i++)
        {
            if (navis[i].gameObject.name == "DirectionsLabel")
            {
                GameObject directionlabel = navis[i].gameObject;
                if (directionlabel.GetComponent<TextMeshPro>().text == NaviMessage && signpost.IscloseEnough)
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

    //이미지 타겟 인식했을시, 네비게이션 종료  + 게임진행에 필요한 창들 모두 뜸.
    public void mapCloseOndetect()
    {
        foreach (GameObject m in navis)
        {
            m.SetActive(false);
        }
        Mission.SetActive(true);
        TooFarInfo.SetActive(false);
        TokenMission.SetActive(true);
        CheckUI.SetActive(true);
        GamePanel.SetActive(true);
        LookForTarget.SetActive(false);
    }

    //ui위에서 클릭시 무효
    private bool IsPointOverUI(Vector2 fingerPosition)
    {
        PointerEventData eventDataPosition = new PointerEventData(EventSystem.current);
        eventDataPosition.position = fingerPosition;
        EventSystem.current.RaycastAll(eventDataPosition, raycastResults);
        return raycastResults.Count > 0;
    }
}
