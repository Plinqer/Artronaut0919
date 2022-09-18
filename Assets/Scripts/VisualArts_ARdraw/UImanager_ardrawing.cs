using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class UImanager_ardrawing : MonoBehaviour
{
    int LastBrush;
    int BrushToRedo = 0;
    [SerializeField] ARSessionOrigin arOrigin;
    [SerializeField] GameObject StatusChoice;
    [SerializeField] Transform OpenedPos;
    [SerializeField] Vector3 ClosedPos;
    int sliderval;
    [SerializeField] LineSettings lineSettings;
    [SerializeField] Text sliderpx;
    [SerializeField] Slider slider;
    bool PlanetMode;
    bool DrawingMode;
    [SerializeField] GameObject tool;
    int brushnum;
    [SerializeField] List<GameObject> brushList;
    // Start is called before the first frame update
    void Start()
    {
        brushnum = 1;
        ClosedPos = StatusChoice.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //���� �������� �߰��� �귯�� ����
        LastBrush = arOrigin.transform.childCount - brushnum;
    }

    //������ �г� ���� ���� ���
    public void OnPanelClick()
    {
        if (StatusChoice.transform.position == OpenedPos.transform.position)
        {
            StatusChoice.transform.DOMove(ClosedPos, 2f).SetEase(Ease.InOutQuad);
        }
        else if(StatusChoice.transform.position == ClosedPos)
        {
            StatusChoice.transform.DOMove(OpenedPos.transform.position, 2f).SetEase(Ease.InOutQuad);
        }


    }

    //slider�� �귯�� ������ ����
    public void BrushSize()
    {
        sliderval = Mathf.RoundToInt(slider.value);
        sliderpx.text = sliderval.ToString() + "px";
        lineSettings.startWidth = slider.value / 1000;
        lineSettings.endWidth = slider.value / 1000;
    }

   
    //�귯�� ���ֱ�
    public void undo()
    {
        Debug.Log(arOrigin.transform.GetChild(LastBrush).gameObject.name);
        if(arOrigin.transform.GetChild(LastBrush).gameObject.CompareTag("Line"))
        {
            arOrigin.transform.GetChild(LastBrush).gameObject.SetActive(false);
            brushList.Add(arOrigin.transform.GetChild(LastBrush).gameObject);
            brushnum++;
        }
    }

    //�귯�� ����
    public void redo()
    {
        Debug.Log(BrushToRedo);
        brushList[BrushToRedo].gameObject.SetActive(true);
        brushList.Remove(brushList[BrushToRedo]);
        brushnum--;
        BrushToRedo++;
    }

}
