using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Uifadeinout : MonoBehaviour
{
    public ARRaycastManager raymanager;
    private bool placementPoseIsValid;
    bool IsReading;
    public RandomWordGen wordsGen;
    int PageNum = 0;
    int randomWordsGen = 0;
    GameManager_Theater gamemanager;
    [SerializeField] GameObject MissionCompleteSeq;
    [SerializeField] GameObject MissionCompleteSeq2;
    [SerializeField] GameObject TouchIcon;
    [SerializeField] TokenCheck TokenCheckForHome;
    [SerializeField] GameObject GobackTxt;
    [SerializeField] GameObject wordSelectBtn;
    [SerializeField] GameObject PageFlipBtn;
    [SerializeField] Camera Cam;
    [SerializeField] GameObject[] txts;
    public TextAsset[] RandomWordsTxts;
    [SerializeField] TextMeshPro[] SelectTxt;
    private Pose placementPose;

    private void Start()
    {
        IsReading = false;
        gamemanager = FindObjectOfType<GameManager_Theater>();
    }

    public void page()
    {
        StartCoroutine(PageFlip());
    }

    public void TokenIn()
    {
        Sequence mySequence = DOTween.Sequence();
        MissionCompleteSeq.gameObject.SetActive(true);
    }

    public void SecureSpace()
    {
        TouchIcon.SetActive(true);
        gamemanager.SpaceSecured = true;
    }

    public IEnumerator PageFlip()
    {
        for (PageNum = 0; PageNum < txts.Length - 1; PageNum++)
        {
            if (txts[PageNum].activeSelf)
            {

                txts[PageNum].gameObject.GetComponent<TextMeshPro>().DOFade(0, 3.5f);
                int children = txts[PageNum].transform.childCount;
                for (int i = 0; i < children; ++i)
                {
                    if (txts[PageNum].transform.GetChild(i).GetComponent<TextMeshPro>() != null)
                    {
                        txts[PageNum].transform.GetChild(i).GetComponent<TextMeshPro>().DOFade(0, 3.5f);
                    }
                    else if (txts[PageNum].transform.GetChild(i).GetComponent<SpriteRenderer>() != null)
                    {
                        txts[PageNum].transform.GetChild(i).GetComponent<SpriteRenderer>().material.DOFade(0, 3.5f);
                    }


                }
                yield return new WaitForSeconds(3.5f);
                txts[PageNum].gameObject.SetActive(false);


                //StartCoroutine(PageOff());


                txts[PageNum + 1].SetActive(true);
                if (txts[PageNum + 1].transform.Find("SelectedTxt") != null)
                {
                    wordSelectBtn.SetActive(true);
                    PageFlipBtn.SetActive(false);
                    wordsGen.WordsData = RandomWordsTxts[randomWordsGen];
                    wordsGen.SelectedTxt = SelectTxt[randomWordsGen];
                    wordsGen.gameObject.SetActive(true);
                    randomWordsGen++;
                }

                else
                {
                    wordSelectBtn.SetActive(false);
                    PageFlipBtn.SetActive(true);
                    wordsGen.gameObject.SetActive(false);
                    wordsGen.Selected = false;

                }
                PageNum++;
            }
        }

    }

    private void Update()
    {
        if (wordsGen.Selected)
        {
            DOTween.Kill(wordsGen.SelectedTxt.transform);
            wordsGen.SelectedTxt.transform.DORotate(new Vector3(0, 0, 0), 2f);
        }

        if (txts[6].activeSelf == true && wordsGen.Selected)
        {
            DOTween.Kill(wordsGen.SelectedTxt.transform);
            wordsGen.SelectedTxt.transform.DORotate(new Vector3(0, 0, 0), 2f);
            PageFlipBtn.SetActive(false);
            wordSelectBtn.SetActive(false);
            StartCoroutine(EndingSeq());
        }

        if (TouchIcon.activeSelf == true && Input.GetMouseButtonDown(0) && !IsReading)
        {
            TouchIcon.SetActive(false);
            Vector3 mousePosition = Cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y + 1.5f, Input.mousePosition.z + 2f));
            foreach (GameObject t in txts)
            {
                t.transform.position = mousePosition;
            }
            txts[0].SetActive(true);
            PageFlipBtn.SetActive(true);
            IsReading = true;
        }

        if (MissionCompleteSeq2.activeSelf)
        {
            TokenCheckForHome._TheaterToken = true;
        }
    }
    private void UpdatePlacementPose()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raymanager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes))
        {
            placementPoseIsValid = true;
        }
        //placementPoseIsValid = hits.Count > 0 || Input.GetMouseButtonDown(0);
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }
    IEnumerator EndingSeq()
    {
        yield return new WaitForSeconds(4.5f);
        TokenIn();

    }
}
