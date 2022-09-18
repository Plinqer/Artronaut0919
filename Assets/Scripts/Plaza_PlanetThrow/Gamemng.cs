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
                //ui�� Ŭ���� ��ȿ
                Debug.Log("nothing");
            }
            else
            {
                thrownMark.SetActive(true);
                Thrownnum++; // ���� �༺ ���� ++
                thrownMarkNum.text = Thrownnum.ToString();

                spawnedPos = Instantiate(instantiatedObj, ray.origin, Quaternion.identity);
                spawnedPos.GetComponent<Rigidbody>().AddForce(ray.direction * 55); // �༺ ������
                if(Thrownnum == 1)
                {
                    GameInfo.SetActive(true); //�� ���� �� ���� �ȳ�����
                    GameInfo.GetComponent<Image>().DOFade(1, 3f); //���̵���
                }
                else
                {
                    GameInfo.GetComponent<Image>().DOFade(0, 3f); //�༺ �ϳ� �̻� �������� ���� ���̵� �ƿ�

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

    //�̹��� Ÿ�� �ν� ������, �׺� ���� �� �̼ǿ� �ʿ��� ������Ʈ on 
    // �ּ� 8��, �ִ� 20���� �༺ �ҷ��� �� ���� �༺�� db���� �ҷ��� �ҿ��� ��ġ 
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


    //�׺� Ȯ��, �ʹ� �� �Ÿ��� �� �ȳ� ���� + �׺� ����
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
                //1000m�̳��� �� ���� ����� ��Ʈ ����Ʈ�� �ȳ�
                TooFarInfo.SetActive(false);
                MapboxRoute.ClosestTarget();

            }
        }

    }

    //����� ������� �Ǻ�
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
