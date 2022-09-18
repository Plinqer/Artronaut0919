using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstantiateDeco : MonoBehaviour
{
    private Gamemanager_film manager;
    [SerializeField] Camera arCam;
    // Start is called before the first frame update
    void Start()
    {
        manager = this.GetComponent<Gamemanager_film>();
    }


    //버튼 눌렀을시, 소환될 모델 설정
    public void Onclick(GameObject objToInstantiate)
    {
        manager.instantiatedObj = objToInstantiate.gameObject;
    }
    // Start is called before the first frame update


    //버튼 눌렀을시, 바뀔 카메라 필터 설정
    public void LutChange(Texture texture)
    {
        arCam.GetComponent<AmplifyColorEffect>().LutTexture = texture;
    }
}
