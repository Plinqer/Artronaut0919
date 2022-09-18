using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tokenmanager : MonoBehaviour
{
    [SerializeField] TokenCheck tokencheck;
    [SerializeField] GameObject theatertoken;
    [SerializeField] GameObject arttoken;
    [SerializeField] GameObject filmtoken;
    [SerializeField] GameObject dancetoken;
    [SerializeField] GameObject musictoken;
    [SerializeField] GameObject traditiontoken;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tokencheck.Save();

        //tokencheck���� token ȹ�� Ȯ�� �� ��ū �÷� ó��

        if(tokencheck._TheaterToken == true)
        {
            theatertoken.GetComponent<Image>().color = Color.white;
        }

        if (tokencheck._FilmToken == true)
        {
            filmtoken.GetComponent<Image>().color = Color.white;
        }

        if (tokencheck._ArtToken == true)
        {
            arttoken.GetComponent<Image>().color = Color.white;
        }

        if (tokencheck._DanceToken == true)
        {
            dancetoken.GetComponent<Image>().color = Color.white;
        }

        if (tokencheck._MusicToken == true)
        {
            musictoken.GetComponent<Image>().color = Color.white;
        }

        if (tokencheck._TraditionToken == true)
        {
            traditiontoken.GetComponent<Image>().color = Color.white;
        }
    }

}
