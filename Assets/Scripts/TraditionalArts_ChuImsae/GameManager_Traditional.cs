using ARLocation.MapboxRoutes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;
using ARLocation;

public class GameManager_Traditional : MonoBehaviour
{
    [SerializeField] GameObject checkUI;
    [SerializeField] GameObject Skybox;
    [SerializeField] GameObject Planets;
    [SerializeField] GameObject LookForTarget;
    [SerializeField] GameObject map;
    [SerializeField] MapboxRoute MapboxRoute;
    [SerializeField] GameObject[] Buttons;
    public GameObject MainPlanet;
    [SerializeField] string NaviMessage;

    string distance;
    [SerializeField] private GameObject Mission;
    [SerializeField] GameObject ContentsPanel;
    [SerializeField] SignPost signpost;
    [SerializeField] GameObject TooFarInfo;
    [SerializeField] GameObject[] navis;
    // Start is called before the first frame update
    void Start()
    {
        MapboxRoute = FindObjectOfType<MapboxRoute>(); 
        LookForTarget.SetActive(false);
        Skybox.SetActive(false);
        Planets.SetActive(false);
        checkUI.SetActive(false);
        Mission.SetActive(false);
        map.SetActive(true);

        //네비 재시작
        ARLocationManager.Instance.ResetARSession((() =>
        {
            Debug.Log("AR+GPS and AR Session were restarted!");
        }));

        //가장 가까운 루트 포인트로 안내
        MapboxRoute.ClosestTarget();


    }

    // Update is called once per frame
    void Update()
    {
        signpost = FindObjectOfType<SignPost>();
        navis = GameObject.FindGameObjectsWithTag("map");
        GpsCheck();

    }

    //네비 위치 안내 : 1000m이상 => 네비 종료 및 너무 멀다는 안내
    //1000m이내라면, 가장 가까운 루트 포인트로 안내
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

    //3d 버튼들 : 가장 큰 행성 중심으로 무작위 position에 배치.
    public void BtnPositioning()
    {

        foreach (GameObject btn in Buttons)
        {
            btn.SetActive(true);
            btn.transform.position = new Vector3(MainPlanet.transform.position.x - Random.Range(-1f, 1f), MainPlanet.transform.position.y - Random.Range(-1f, 0.5f), MainPlanet.transform.position.z - Random.Range(-1f, 1f));
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
        Skybox.SetActive(true);
        Planets.SetActive(true);
        checkUI.SetActive(true);
        Mission.SetActive(true);
        TooFarInfo.SetActive(false);
        BtnPositioning();
    }
}
