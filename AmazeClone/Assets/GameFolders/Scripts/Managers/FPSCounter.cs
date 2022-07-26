using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    public GUIStyle gs;
    public static float fps;

    void Update()
    {

        fps = (1f / Time.smoothDeltaTime);

    }

    private void Start()
    {
        gs.fontSize = 25;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(75, 0, 0, 0), fps.ToString("#,##0.0 fps"), gs);
    }

}
