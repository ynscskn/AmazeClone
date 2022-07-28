using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // M_Observer.OnGameRetry?.Invoke();
    }
}
