using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BtnFunctions : MonoBehaviour
{
    [SerializeField] GameObject SceneIconPanels;
    [SerializeField] GameObject[] TokensAndInfo;
    public string SceneName;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MenuClicked()
    {
        SceneIconPanels.SetActive(true);
        foreach (GameObject token in TokensAndInfo)
        {
            token.SetActive(false);
        }
    }

    public void BackClicked()
    {
        SceneIconPanels.SetActive(false);
        foreach (GameObject token in TokensAndInfo)
        {
            token.SetActive(true);
        }
       
    }

    public void SceneCalling()
    {
        SceneManager.LoadScene(SceneName);
    }
}
