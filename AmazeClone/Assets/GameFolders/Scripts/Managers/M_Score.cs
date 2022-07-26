using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class M_Score : MonoBehaviour
{
    public TextMeshProUGUI CurrentScoreText, BestScoreText, GameOverScoreText, MenuBestScoreText;

    [HideInInspector] public int Score;

    private void Awake()
    {
        II = this;

        M_Observer.OnGameCreate += GameCreate;
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnGameFail += GameFail;
        M_Observer.OnGameRetry += GameRetry;


    }

    private void OnDestroy()
    {
        M_Observer.OnGameCreate -= GameCreate;
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnGameFail -= GameFail;
        M_Observer.OnGameRetry -= GameRetry;

    }

    private void GameRetry()
    {
        GameStart();
    }

    private void GameFail()
    {
        GameOverScoreText.text = Score.ToString();
        if (PlayerPrefs.GetInt("bestscore") < Score)
        {
            PlayerPrefs.SetInt("bestscore", Score);
            BestScoreText.text = Score.ToString();
            MenuBestScoreText.text = Score.ToString();
        }
    }

    private void GameStart()
    {
        Score = 0;
        SetScore();
    }

    private void GameCreate()
    {
        string _bestScore = PlayerPrefs.GetInt("bestscore").ToString();
        MenuBestScoreText.text = _bestScore;
        BestScoreText.text = _bestScore;
    }

    public void SetScore()
    {
        CurrentScoreText.text = Score.ToString();
    }

    public static M_Score II;

    public static M_Score I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Score");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Score>();
                }
            }

            return II;
        }
    }
}
