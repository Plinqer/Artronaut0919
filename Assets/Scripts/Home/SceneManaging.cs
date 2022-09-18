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
        //�׺���̼� ���� obj ��� tag = "map" ����
        navis = GameObject.FindGameObjectsWithTag("map");

    }

    //�ε� ������ �ʿ��� ��� ���� ���� �����ִ� �׺���̼� �� �� ��ȯ
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
        yield return new WaitForSeconds(6.2f); //�ε� ������ ���ӽð� �뷫 6��
        SceneManager.LoadScene(SceneName);
    }

}
