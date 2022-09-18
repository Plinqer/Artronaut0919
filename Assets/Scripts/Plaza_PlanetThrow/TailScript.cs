using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailScript : MonoBehaviour
{
    //위성들 움직임 스크립트
    //위성들은 자전하면서 행성 주위를 돈다


    public Vector3 head;
    [SerializeField] float speed = 5f;
    [SerializeField] float rotation_damping = 4f;
    [SerializeField] float rotation_Speed = 80f;

    [SerializeField] Transform HeadOBJ;
    // Start is called before the first frame update
    void Start()
    {
        rotation_Speed = Random.Range(20, 100);
        GameObject[] HeadOBJs = GameObject.FindGameObjectsWithTag("head"); //head 태그를 단 행성들은 큰 행성들, 즉 위성들이 따라다니는 행성들
        int Chosenobj = Random.Range(0, HeadOBJs.Length);
        HeadOBJ = HeadOBJs[Chosenobj].GetComponent<Transform>();
        //HeadOBJ = GameObject.FindGameObjectWithTag("head").GetComponent<Transform> ();
    }

    // Update is called once per frame
    void Update()
    {
        head = new Vector3(HeadOBJ.transform.position.x, HeadOBJ.transform.position.y + 8, HeadOBJ.transform.position.z);
        var rotation = Quaternion.LookRotation(HeadOBJ.transform.position - transform.position);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotation_damping);

        this.transform.position = Vector3.MoveTowards(transform.position, head, speed * Time.deltaTime);
        transform.RotateAround(HeadOBJ.transform.position, new Vector3(0,1,0), rotation_Speed * Time.deltaTime);
    }
}
