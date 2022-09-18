using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlanetRotateMovement : MonoBehaviour
{
    //위성들 자전 + 큰 행성 주위 공전

    [SerializeField] Transform Target;
    public float rotateSpeed;
    public float Movingrange =0.5f; 
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0, -360, 0), 20f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        {
            var rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3f);
            transform.RotateAround(Target.transform.position, new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
        }
    }

}
