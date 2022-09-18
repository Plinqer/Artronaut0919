using TMPro;
using UnityEngine;

public class InstantiatedOBJ : MonoBehaviour
{
    public Gamemng manager;
    public GameObject instantiate;
    private GameObject instantiatetxt;
    [SerializeField] DataBaseManager dbm;
    // Start is called before the first frame update
    void Start()
    {
    }


    //행성 버튼 클릭시 그 행성으로 소환되도록 설정 + 행성에 유저의 소원 담기도록 설정
    public void Onclick()
    {
        manager.instantiatedObj = instantiate.gameObject;
        instantiatetxt = instantiate.gameObject.transform.GetChild(0).gameObject;
        instantiatetxt.GetComponent<TextMeshPro>().text = dbm.Mywish;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
