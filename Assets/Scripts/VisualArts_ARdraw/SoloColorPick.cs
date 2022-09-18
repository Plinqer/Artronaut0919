using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoloColorPick : MonoBehaviour
{
    public Color color;
    public float brushSize;
    [SerializeField]
    private LineSettings lineSettings;

    [SerializeField] Material brushMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //브러쉬 텍스쳐, 컬러, 브러쉬 사이즈 설정 스크립트
    // ui 컨텐츠 패널에서 조정
    public void BrushTexture()
    {
        lineSettings.defaultMaterial = brushMat;
    }
    public void OnColorPick()
    {
        color = this.GetComponent<Image>().color;
        lineSettings.endColor = color;
        lineSettings.startColor = color;
    }

    public void onBrushSizePick()
    {
        lineSettings.startWidth = brushSize;
        lineSettings.endWidth = brushSize;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
