using ARLocation;
using ARLocation.MapboxRoutes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class GameManager_Theater : MonoBehaviour
{
    [SerializeField] GameObject LookForTarget;
    [SerializeField] GameObject CheckUI;
    [SerializeField] string NaviMessage;
    [SerializeField] MapboxRoute MapboxRoute;
    public bool SpaceSecured;
    [SerializeField] GameObject TooFarInfo;
    [SerializeField] GameObject SecureSpace;
    [SerializeField] GameObject SpaceConfirm;
    [SerializeField] private GameObject Mission;
    string distance;
    [SerializeField] GameObject[] navis;
    [SerializeField] SignPost signpost;
    // Start is called before the first frame update
    void Start()
    {
        SpaceSecured = true;
        MapboxRoute = FindObjectOfType<MapboxRoute>();
        LookForTarget.SetActive(false);
        Mission.SetActive(false);
        CheckUI.SetActive(false);
        ARLocationManager.Instance.ResetARSession((() =>
        {
            Debug.Log("AR+GPS and AR Session were restarted!");
        }));
        MapboxRoute.ClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {

        signpost = FindObjectOfType<SignPost>();
        navis = GameObject.FindGameObjectsWithTag("map");
        //StartCoroutine(GpsUpdate());

        GpsCheck();

        if (!SpaceSecured && Input.GetMouseButtonDown(0))
        {
            SpaceConfirm.SetActive(true);
        }
    }


    //네비 1000m이상일 경우 네비 종료 및 너무 거리 멀다 안내 
    //아닐 경우(1000m이하일 경우 가장 가까운 포인트로 안내)
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
                TooFarInfo.SetActive(false);
                MapboxRoute.ClosestTarget();

            }
        }

    }

    //이미지 타겟 인식했을 때 네비 종료 + 컨텐츠 수행에 필요한 오브젝트 on
    public void mapCloseOndetect()
    {
        foreach (GameObject m in navis)
        {
            m.SetActive(false);
        }
        Mission.SetActive(true);
        TooFarInfo.SetActive(false);
        CheckUI.SetActive(true);
        SpaceSecured = false;
    }

    //충분히 가까운지 판별
    //map 태그 가진 오브젝트 중 direationalsLabel 찾아서 네비 메시지랑 비교 + 현재 거리 30 m 이하일시 타겟 주위에서 찾으시오 안내
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

}
