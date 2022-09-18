using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu]
public class TokenCheck : ScriptableObject
{

    //인트로 한번 본 이후는 home으로 돌아갔을 때 다시 보이지 않도록 설정
    //home - tokenmanager 스크립트와 연결
    [SerializeField]
    private bool introFinish;

    public bool _introFinish
    {
        get { return introFinish; }
        set { introFinish = value; }
    }

    // 아래 모두 토큰 획득했는지 확인
    // 토큰 Obtain 여부 easy save 이용, 저장 설정해 두었음

    [SerializeField]
    private bool TheaterToken;

    public bool _TheaterToken
    {
        get { return TheaterToken; }
        set { TheaterToken = value; }
    }

    [SerializeField]
    private bool MusicToken;

    public bool _MusicToken
    {
        get { return MusicToken; }
        set { MusicToken = value; }
    }

    [SerializeField]
    private bool FilmToken;

    public bool _FilmToken
    {
        get { return FilmToken; }
        set { FilmToken = value; }
    }

    [SerializeField]
    private bool DanceToken;

    public bool _DanceToken
    {
        get { return DanceToken; }
        set { DanceToken = value; }
    }

    [SerializeField]
    private bool ArtToken;

    public bool _ArtToken
    {
        get { return ArtToken; }
        set { ArtToken = value; }
    }

    [SerializeField]
    private bool TraditionToken;

    public bool _TraditionToken
    {
        get { return TraditionToken; }
        set { TraditionToken = value; }
    }



    void Awake()
    { 
        introFinish = ES3.Load("introFinish", this.introFinish);
        _DanceToken = ES3.Load("Dance", this._DanceToken);
        _FilmToken = ES3.Load("Film", this._FilmToken);
        _MusicToken = ES3.Load("Music", this._MusicToken);
        _ArtToken = ES3.Load("VisualArt", this._ArtToken);
        _TraditionToken = ES3.Load("Traditionalart", this._TraditionToken);
        _TheaterToken = ES3.Load("Thater", this._TheaterToken);
    }

    public void Save()
    {
        ES3.Save("introFinish", this.introFinish);
        ES3.Save("Dance", this._DanceToken);
        ES3.Save("Film", this._FilmToken);
        ES3.Save("Music", this._MusicToken);
        ES3.Save("VisualArt", this._ArtToken);
        ES3.Save("Traditionalart", this._TraditionToken);
        ES3.Save("Thater", this._TheaterToken);
    }
}
