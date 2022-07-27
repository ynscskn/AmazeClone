using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Level : MonoBehaviour
{
    public Level[] Levels;
    public Ball BallPrefab;
    public GridItem GridItemPrefab;
    public Material WallMat, RoadMat;

    [HideInInspector] public Ball CurrentBall;
    [HideInInspector] public Level CurrentLevel;
    [HideInInspector] public int LevelIndex;


    private void Awake()
    {
        II = this;

        if (PlayerPrefs.HasKey("LevelIndex"))
        {
            LevelIndex = PlayerPrefs.GetInt("LevelIndex");
        }
        else
        {
            LevelIndex = 0;
        }
    }
    private void Start()
    {
        CurrentLevel = Instantiate(Levels[LevelIndex], transform);
        CurrentBall = Instantiate(BallPrefab, new Vector3(1, 0, 1), Quaternion.identity, transform);
        CurrentBall.Index_I = 1;
        CurrentBall.Index_J = 1;
    }

    public static M_Level II;

    public static M_Level I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Level");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Level>();
                }
            }

            return II;
        }
    }
}
