using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingEnable : MonoBehaviour
{
    [SerializeField] GameObject Conch;
    public bool collected;
    // Start is called before the first frame update
    void Start()
    {
        collected=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundFound()
    {
        if (!collected)
        {
            Conch.SetActive(true);
            collected=true; 
            // ui���� ���� ������Ʈ on - ���/�ִϸ��̼��� feedback�� �����Ǿ�����
        }

    }

}
