using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        button.onClick.AddListener(ButtonClicked);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonClicked);
    }
    void ButtonClicked()
    {
        if (M_Menu.I.OnPause) M_Observer.OnGameContinue?.Invoke();

        else M_Observer.OnGameStart?.Invoke();

    }
}
