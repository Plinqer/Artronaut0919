using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovingTowards : MonoBehaviour
{
    float movingDirection;
    // Start is called before the first frame update
    void Start()
    {

        // 해마, 해파리에 넣어둔 스크립트 - 간단한 움직임
        movingDirection = Random.Range(-1, 2);
        this.transform.DOMove(new Vector3 (this.transform.position.x + movingDirection, this.transform.position.y, this.transform.position.z + movingDirection),50f).SetLoops(-1,LoopType.Incremental);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
