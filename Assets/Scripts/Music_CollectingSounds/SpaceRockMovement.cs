using DG.Tweening;
using UnityEngine;
public class SpaceRockMovement : MonoBehaviour
{

    public float speed;
    // Start is called before the first frame update
    void Start()
    {

    }


    //행성 자전
    // Update is called once per frame
    void Update()
    {
        transform.DORotate(new Vector3(0, -360, 0), speed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

}
