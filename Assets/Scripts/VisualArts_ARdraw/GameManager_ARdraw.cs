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
        //�׺���̼� ��Ʈ �ȳ� ������Ʈ
        MapboxRoute = FindObjectOfType<MapboxRoute>();
        //���������� �Ÿ� 30m������ �� ��Ÿ���� �˸�
        LookForTarget.SetActive(false);
        //�ϴ� �귯��, ��ƼĿ ���� �г�â
        ContentsPanel.SetActive(false);
        //���� �ϴ� ��ū & �̼� ������
        TokenMissionCheck.SetActive(false);
        //�� ���۽�, �׺���̼� Ŀ���� ��Ʈ - ���� ��ġ���� ���� ����� ����Ʈ�� �ȳ����ֱ�
        MapboxRoute.ClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //�׺� �ȳ�â & ��ũ��Ʈ
        signpost = FindObjectOfType<SignPost>();
        navis = GameObject.FindGameObjectsWithTag("map");


        if (CompleteSeq.activeSelf)
        {
            TokenCheckForHome._ArtToken = true;
            //token check(ScriptableObj)�� ��Ʈ ��ū ȹ�� ó��
        }
   
        //�༺ ��ȯ
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

        //�귯�� 3�� �׷��� ��, �̴�� �ϼ��ϱ� ���� ��ư ��
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
            //���� ��ġ�� 1000m�̻��̸�, �ʹ� �ִٴ� �������̼��� ��
            if (signpost.IsTooFar)
            {
                TooFarInfo.SetActive(true);
                //��ġ�� �ʹ� �� ���, �׺� ��� �ڵ� �����
                foreach (GameObject m in navis)
                {
                    m.SetActive(false);
                }
                //�̼� â ���� �����
                Mission.SetActive(false);
            }
            else
            {
                //��ġ�� 1000m �̳��� ��� �׺� �ȳ� �̾���, ���� ����� ��Ʈ ����Ʈ�� �ȳ�.
                TooFarInfo.SetActive(false);
                MapboxRoute.ClosestTarget();

            }
        }

    }


    //�̹��� Ÿ�� �ν�������, �׺���̼� ���� �� �������࿡ �ʿ��� â�� ��� ��.
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


    //����� ������� �Ǻ�
    public void IscloseEnough()
    {
        for(int i=0; i < navis.Length; i++)
        {
            //������ �ȳ� ������ navimessage�� ��ġ �� + 30m�����Ͻ� Ÿ�� ã����� ���� ��
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



    //ui ������ Ŭ���� Ŭ�� ���� �ʵ���
    private bool IsPointOverUI(Vector2 fingerPosition)
    {
        PointerEventData eventDataPosition = new PointerEventData(EventSystem.current);
        eventDataPosition.position = fingerPosition;
        EventSystem.current.RaycastAll(eventDataPosition, raycastResults);
        return raycastResults.Count > 0;
    }
}
