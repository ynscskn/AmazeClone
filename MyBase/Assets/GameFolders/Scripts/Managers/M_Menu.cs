using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class M_Menu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject GameMenuPanel;
    public GameObject PausePanel;
    public GameObject GameOverPanel;

    [HideInInspector] public GameObject CurrentPanel;
    [HideInInspector] public bool OnPause;
    private void Awake()
    {
        II = this;
        M_Observer.OnGameCreate += GameCreate;
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnGamePause += GamePause;
        M_Observer.OnGameFail += GameFail;
        M_Observer.OnGameRetry += GameRetry;
        M_Observer.OnGameContinue += GameContinue;
        M_Observer.OnGameMainMenu += GameMainMenu;

    }

    private void OnDestroy()
    {
        M_Observer.OnGameCreate -= GameCreate;
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnGamePause -= GamePause;
        M_Observer.OnGameFail -= GameFail;
        M_Observer.OnGameRetry -= GameRetry;
        M_Observer.OnGameContinue -= GameContinue;
        M_Observer.OnGameMainMenu -= GameMainMenu;
    }

    private void GameCreate()
    {
        SetPanel(MainMenuPanel);
    }

    private void GameStart()
    {
        SetPanel(GameMenuPanel);
    }

    private void GamePause()
    {
        SetPanel(PausePanel);
    }

    private void GameMainMenu()
    {
        SetPanel(MainMenuPanel);
    }

    private void GameFail()
    {
        SetPanel(GameOverPanel);
    }

    private void GameRetry()
    {
        SetPanel(GameMenuPanel);
    }

    private void GameContinue()
    {
        OnPause = false;
        SetPanel(GameMenuPanel);
    }

    void SetPanel(GameObject Panel)
    {
        if (CurrentPanel != null)
        {
            CurrentPanel.SetActive(false);
            CurrentPanel = null;
        }

        CurrentPanel = Panel;
        CurrentPanel.SetActive(true);
    }



    public static M_Menu II;

    public static M_Menu I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Menu");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Menu>();
                }
            }

            return II;
        }
    }
}
