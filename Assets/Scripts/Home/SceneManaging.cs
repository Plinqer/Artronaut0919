using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManaging : MonoBehaviour
{
    public string SceneName;
    [SerializeField] GameObject loadPage;
    [SerializeField] bool loadPageNeed;
    [SerializeField] GameObject[] navis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //네비게이션 관련 obj 모두 tag = "map" 상태
        navis = GameObject.FindGameObjectsWithTag("map");

    }

    //로딩 페이지 필요한 경우 현재 씬에 켜져있는 네비게이션 끈 후 전환
    public void SceneClick()
    {
        foreach (GameObject navi in navis)
        {
            navi.gameObject.SetActive(false);
        }
        if (loadPageNeed)
        {
            StartCoroutine(loading());

        }

        else
        {
            foreach (GameObject navi in navis)
            {
                navi.gameObject.SetActive(false);
            }
            SceneManager.LoadScene(SceneName);

        }
    }
    IEnumerator loading()
    {
        loadPage.SetActive(true);
        yield return new WaitForSeconds(6.2f); //로딩 페이지 지속시간 대략 6초
        SceneManager.LoadScene(SceneName);
    }

}
