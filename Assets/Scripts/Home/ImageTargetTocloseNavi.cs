using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetTocloseNavi : MonoBehaviour
{
    [SerializeField] GameObject[] navis; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        navis = GameObject.FindGameObjectsWithTag("map");

    }

    public void mapCloseOndetect()
    {
        foreach(GameObject m in navis)
        {
            m.SetActive(false);
        }
    }
}
