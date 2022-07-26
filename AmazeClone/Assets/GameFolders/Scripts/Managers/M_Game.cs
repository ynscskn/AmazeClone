using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class M_Game : MonoBehaviour
{
    private void Awake()
    {
        II = this;
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    private void Update()
    {

    }

    public static M_Game II;

    public static M_Game I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Game");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Game>();
                }
            }

            return II;
        }
    }

}
