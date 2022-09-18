using ARLocation;
using ARLocation.MapboxRoutes;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gamemng : MonoBehaviour
{
    [SerializeField] string NaviMessage;
    [SerializeField] DataBaseManager DataBaseManager;
    [SerializeField] GameObject TooFarInfo;
    [SerializeField] GameObject LookforTarget;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject GameInfo;
    [SerializeField] GameObject[] navis;
    [SerializeField] GameObject[] Planets;
    [SerializeField] SignPost signpost;
    [SerializeField] MapboxRoute MapboxRoute;
    public GameObject instantiatedObj;
    private GameObject spawnedPos;
    public Camera ARcamera;
    [SerializeField] int Thrownnum;
    [SerializeField] GameObject thrownMark;
    [SerializeField] Text thrownMarkNum;
    public bool Instantiated = false;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    // Start is called before the first frame update
    void Start()
    {
        MapboxRoute = FindObjectOfType<MapboxRoute>();

        GamePanel.SetActive(false);

        ARLocationManager.Instance.ResetARSession((() =>
        {
            Debug.Log("AR+GPS and AR Session were restarted!");
        }));
        MapboxRoute.ClosestTarget();
        Thrownnum = 0;

    }

    // Update is called once per frame
    void Update()
    {
        signpost = FindObjectOfType<SignPost>();
        navis = GameObject.FindGameObjectsWithTag("map");


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("works!");
            Ray ray = ARcamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log(ray);

            if (IsPointOverUI(Input.mousePosition))
            {
                //ui위 클릭시 무효
                Debug.Log("nothing");
            }
            else
            {
                thrownMark.SetActive(true);
                Thrownnum++; // 날린 행성 숫자 ++
                thrownMarkNum.text = Thrownnum.ToString();

                spawnedPos = Instantiate(instantiatedObj, ray.origin, Quaternion.identity);
                spawnedPos.GetComponent<Rigidbody>().AddForce(ray.direction * 55); // 행성 던지기
                if(Thrownnum == 1)
                {
                    GameInfo.SetActive(true); //더 던질 수 있음 안내문구
                    GameInfo.GetComponent<Image>().DOFade(1, 3f); //페이드인
                }
                else
                {
                    GameInfo.GetComponent<Image>().DOFade(0, 3f); //행성 하나 이상 던졌을시 문구 페이드 아웃

                }
            }
        }
        GpsCheck();
        IscloseEnough();

        if (Instantiated)
        {
            DataBaseManager.GetWishes();
        }

    }

    //이미지 타겟 인식 했을시, 네비 종료 및 미션에 필요한 오브젝트 on 
    // 최소 8개, 최대 20개의 행성 불러온 후 각각 행성에 db에서 불러온 소원들 배치 
    public void OnImageFound()
    {
        for(int i =0; i < Random.Range(8, 20); i++)
        {
            GameObject InstantiatedPlanet = Instantiate(Planets[UnityEngine.Random.Range(0, Planets.Length)], new Vector3(Random.Range(-12,12), Random.Range(-12, 12), Random.Range(-12, 12)), transform.rotation);
            InstantiatedPlanet.transform.GetChild(0).gameObject.tag = "WishesOfOthers";
            if (InstantiatedPlanet.GetComponent<HeadScript>() != null)
            {
                //InstantiatedPlanet.GetComponent<HeadScript>().direction = new Vector3(0, 0.1f, 0.2f);
                InstantiatedPlanet.GetComponent<HeadScript>().enabled = false;

            }
            else if(InstantiatedPlanet.GetComponent<TailScript>() != null)
            {
                InstantiatedPlanet.GetComponent<TailScript>().enabled = false;
            }
        }

        if (navis != null)
        {
            foreach (GameObject m in navis)
            {
                m.SetActive(false);
            }

        }
        TooFarInfo.SetActive(false);
        GamePanel.SetActive(true);
        LookforTarget.SetActive(false);
        DataBaseManager.GetWishes();
        Instantiated = true;
    }


    //네비 확인, 너무 먼 거리일 시 안내 문구 + 네비 종료
    public void GpsCheck()
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
                //1000m이내일 시 가장 가까운 루트 포인트로 안내
                TooFarInfo.SetActive(false);
                MapboxRoute.ClosestTarget();

            }
        }

    }

    //충분히 가까운지 판별
    public void IscloseEnough()
    {
        for (int i = 0; i < navis.Length; i++)
        {
            if (navis[i].gameObject.name == "DirectionsLabel")
            {
                GameObject directionlabel = navis[i].gameObject;
                if (directionlabel.GetComponent<TextMeshPro>().text == NaviMessage && signpost.IscloseEnough)
                {
                    LookforTarget.SetActive(true);
                }

            }

            else
            {
                Debug.Log("navi not working");
            }
        }
    }

    private bool IsPointOverUI(Vector2 fingerPosition)
    {
        PointerEventData eventDataPosition = new PointerEventData(EventSystem.current);
        eventDataPosition.position = fingerPosition;
        EventSystem.current.RaycastAll(eventDataPosition, raycastResults);
        return raycastResults.Count > 0;
    }

}
