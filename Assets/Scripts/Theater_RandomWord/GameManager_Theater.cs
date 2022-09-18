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


    //�׺� 1000m�̻��� ��� �׺� ���� �� �ʹ� �Ÿ� �ִ� �ȳ� 
    //�ƴ� ���(1000m������ ��� ���� ����� ����Ʈ�� �ȳ�)
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

    //�̹��� Ÿ�� �ν����� �� �׺� ���� + ������ ���࿡ �ʿ��� ������Ʈ on
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

    //����� ������� �Ǻ�
    //map �±� ���� ������Ʈ �� direationalsLabel ã�Ƽ� �׺� �޽����� �� + ���� �Ÿ� 30 m �����Ͻ� Ÿ�� �������� ã���ÿ� �ȳ�
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
