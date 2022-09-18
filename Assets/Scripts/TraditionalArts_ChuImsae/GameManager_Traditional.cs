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

        //�׺� �����
        ARLocationManager.Instance.ResetARSession((() =>
        {
            Debug.Log("AR+GPS and AR Session were restarted!");
        }));

        //���� ����� ��Ʈ ����Ʈ�� �ȳ�
        MapboxRoute.ClosestTarget();


    }

    // Update is called once per frame
    void Update()
    {
        signpost = FindObjectOfType<SignPost>();
        navis = GameObject.FindGameObjectsWithTag("map");
        GpsCheck();

    }

    //�׺� ��ġ �ȳ� : 1000m�̻� => �׺� ���� �� �ʹ� �ִٴ� �ȳ�
    //1000m�̳����, ���� ����� ��Ʈ ����Ʈ�� �ȳ�
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

    //3d ��ư�� : ���� ū �༺ �߽����� ������ position�� ��ġ.
    public void BtnPositioning()
    {

        foreach (GameObject btn in Buttons)
        {
            btn.SetActive(true);
            btn.transform.position = new Vector3(MainPlanet.transform.position.x - Random.Range(-1f, 1f), MainPlanet.transform.position.y - Random.Range(-1f, 0.5f), MainPlanet.transform.position.z - Random.Range(-1f, 1f));
        }
    }



    //����� ������� �Ǻ�
    //������ �ȳ� ������ navimessage�� ��ġ �� + 30m�����Ͻ� Ÿ�� ã����� ���� ��

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

    //�̹��� Ÿ�� �ν�������, �׺���̼� ����  + �������࿡ �ʿ��� â�� ��� ��.
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
