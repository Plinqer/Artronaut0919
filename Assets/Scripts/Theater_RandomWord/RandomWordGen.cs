using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class RandomWordGen : MonoBehaviour
{
    [SerializeField] Uifadeinout uifade;
    [SerializeField] TokenCheck tokencheck;
    [SerializeField] TextMeshPro textPaste;
    public bool Selected;
    List<NameSave> namess = new List<NameSave>();
    public TextMeshPro SelectedTxt;
    public TextAsset WordsData;
    private NameSave n;
    string[] data;
    public string selectedWord;
    [SerializeField] GameObject Hands;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Selected = false; //스크립트 켜질 때마다 false, 선택할때 true될 수 있도록
        System.Random random = new System.Random();
        int number = random.Next(1, 10);
        data = WordsData.text.Split(new char[] { '\n' });
        List<string> names = new List<string>();
        string[] row = data[number].Split(new char[] { ',' });
        n = new NameSave();
        n.RandomName = row[0];
        namess.Add(n);

        StartCoroutine(wordsRotate());

    }
    void Start()
    {

    }

    //단어 선택 전까지 단어 회전 애니메이션 + txt파일에서 단어 리스트 가져와서 로테이션
    IEnumerator wordsRotate()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!Selected)
            {
                SelectedTxt.transform.DORotate(new Vector3(360,0 , 0), 0.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
                SelectedTxt.text = data[i];
                if(SelectedTxt.text == data[9])
                {
                    i = 0;
                }
                yield return new WaitForSeconds(0.5f);
                
            }
            else
            {
                DOTween.Kill(SelectedTxt.transform);
                selectedWord = data[Random.Range(0, data.Length)];
                SelectedTxt.text = selectedWord;
                
            }
        }

    }

    //단어 선택시 다음장으로 버튼 on
    public void BtnClick()
    {
        Debug.Log("clicked");

        Hands.gameObject.SetActive(false);
        Selected = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
