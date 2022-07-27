using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [HideInInspector] public int Index_I, Index_J;
    [HideInInspector] public Material BallMat;

    private void Awake()
    {
        BallMat = GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            other.GetComponent<MeshRenderer>().material = BallMat;
        }
    }

}
