using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class RotationSpeedAdjust : MonoBehaviour
{
    public string ChuImSae;
    public GameObject feedback;
    [SerializeField] Texture tex;
    [SerializeField] GameObject skybox;
    public GameObject[] rotateAround;
    public SpaceRockMovement SelfRotation;
    [SerializeField] GameObject[] TalAu;
    [SerializeField] AudioSource AuToPlay;
    [SerializeField] AudioSource VoiceToPlay;
    [SerializeField] float rotateAroundSpeed;
    [SerializeField] float SelfrotationSpeed;
    public MMFeedbacks[] feedbacks;

    // Start is called before the first frame update
    void Start()
    {
        TalAu = GameObject.FindGameObjectsWithTag("Tal"); //��� ����
        rotateAround = GameObject.FindGameObjectsWithTag("RotateAroundPlanet");//������
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //��� ��ư ���� ��ī�̹ڽ� �ؽ���, ���� ����, ���� - ���� ���ǵ� ����
    public void onclick()
    {
        skybox.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tex);
        transform.DORotate(new Vector3(0, 2, 360), 1, RotateMode.FastBeyond360);
        foreach (GameObject au in TalAu)
        {
            //��� ��� ���̵� �ƿ� �� ����
            au.GetComponent<AudioSource>().DOFade(0, 2f);
            au.GetComponent<AudioSource>().Stop();
        }
        AuToPlay.Play();
        AuToPlay.DOFade(1, 5f);
        VoiceToPlay.Play();
        foreach (GameObject p in rotateAround)
        {
            if (p.GetComponent<PlanetRotateMovement>() != null)
            {
                p.GetComponent<PlanetRotateMovement>().rotateSpeed = rotateAroundSpeed;
            }
        }
        SelfRotation.speed = SelfrotationSpeed;
    }

}
