using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
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
        M_Observer.OnGameMainMenu?.Invoke();
    }
}
