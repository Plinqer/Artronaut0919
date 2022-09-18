using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class uiManager_Gifted : MonoBehaviour
{
    [SerializeField] Vector3 PanelClosePos;
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject MissionComplete;
    [SerializeField] Transform PanelOpenPos;
    [SerializeField] TextMeshProUGUI tokenGivenNum;
    [SerializeField] int GivenNum;
    // Start is called before the first frame update
    void Start()
    {
        PanelClosePos = Panel.transform.position;
        GivenNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(tokenGivenNum.text == "6")
        {
            MissionComplete.SetActive(true);
        }
    }
    public void PanelOpenClose()
    {
        if (Panel.transform.position == PanelOpenPos.position)
        {
            Panel.transform.DOMove(PanelClosePos, 2f);
        }
        else if (Panel.transform.position == PanelClosePos)
        {
            Panel.transform.DOMove(PanelOpenPos.position, 2f);

        }
    }

    public void TokenNum()
    {
        GivenNum++;
        tokenGivenNum.text = GivenNum.ToString();
    }
}
