using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [HideInInspector] public int Grid_I = 10, Grid_J = 10;
    [HideInInspector] public List<GridItem> RoadList;
    [HideInInspector] public List<GridItem> GridItemList;

    [HideInInspector] public GridItem[,] GridItemArray;

    private void Awake()
    {
        GridItemList = new List<GridItem>();
        RoadList = new List<GridItem>();
        GridItemArray = new GridItem[Grid_I, Grid_J];

        M_Observer.OnGameCreate += GameCreate;
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnGameReady += GameReady;
        M_Observer.OnGamePause += GamePause;
        M_Observer.OnGameContinue += GameContinue;
        M_Observer.OnGameFail += GameFail;
        M_Observer.OnGameComplete += GameComplete;
        M_Observer.OnGameRetry += GameRetry;
        M_Observer.OnGameNextLevel += GameNextLevel;
    }
    private void OnDestroy()
    {
        M_Observer.OnGameCreate -= GameCreate;
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnGameReady -= GameReady;
        M_Observer.OnGamePause -= GamePause;
        M_Observer.OnGameContinue -= GameContinue;
        M_Observer.OnGameFail -= GameFail;
        M_Observer.OnGameComplete -= GameComplete;
        M_Observer.OnGameRetry -= GameRetry;
        M_Observer.OnGameNextLevel -= GameNextLevel;
    }



    private void GameCreate()
    {
        CreateGrid();
        M_Level.I.CurrentBall.transform.SetParent(GridItemArray[1, 1].transform);
        CurrentBall = M_Level.I.CurrentBall;
        GridSetting(CurrentBall, 30);
        CurrentBall.Index_I = 1;
        CurrentBall.Index_J = 1;
        CurrentBall.transform.localScale = new Vector3(1, 10, 1);
    }
    Ball CurrentBall;
    private void GameStart()
    {
        // print("GameStart"); M_Level.I.CurrentBall.transform.SetParent(GridItemArray[1, 1].transform); CurrentBall = M_Level.I.CurrentBall;
    }

    private void GameReady()
    {
        print("GameReady");
    }
    private void GamePause()
    {
        print("GamePause");
    }
    private void GameContinue()
    {
        print("GameContinue");
    }
    private void GameFail()
    {
        print("GameFail");
    }
    private void GameComplete()
    {
        print("GameComplete");
    }
    private void GameRetry()
    {
        print("GameRetry");
    }
    private void GameNextLevel()
    {
        print("GameNextLevel");
    }

    void OnEnable()
    {
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
    }

    void OnDisable()
    {
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
    }

    Vector2 fingerDownFirstVec;
    Vector2 fingerDragVec;
    Vector2 fingerDirection;
    public bool IsMove;
    Vector3 movePos = Vector3.zero;
    float Speed = 20f;

    private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        fingerDownFirstVec = fingerPos;
    }

    private void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
    {

        if (fingerIndex != 0 || M_Level.I.CurrentBall == null) return;
        fingerDragVec = fingerPos;
        fingerDirection = fingerDragVec - fingerDownFirstVec;

        int _katsayý = 50 * Screen.width / 1080;

        int _kontrolhýzýX = Mathf.FloorToInt(fingerDirection.x);
        int _kontrolhýzýY = Mathf.FloorToInt(fingerDirection.y);

        if (!IsMove)
        {
            IsMove = true;

            int _i = CurrentBall.Index_I;
            int _j = CurrentBall.Index_J;

            if (_kontrolhýzýX > _katsayý)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int i = 0; i < Grid_I; i++)
                {
                    if (_i + i <= Grid_I - 1 && _i + i >= 0)
                    {
                        if (GridItemArray[_i + i, _j].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[_i + i, _j].transform);
                            CurrentBall.Index_I = _i + i;
                            GridItemArray[_i + i, _j].IsPainted = true;
                            Destroy(GridItemArray[_i + i, _j].Collider, 0.5f);
                        }
                        else if (!GridItemArray[_i + i, _j].IsRoad) break;
                    }
                }
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; });

            }
            else if (_kontrolhýzýX < -_katsayý)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int i = 0; i < Grid_I; i++)
                {
                    if (_i - i <= Grid_I - 1 && _i - i >= 0)
                    {
                        if (GridItemArray[_i - i, _j].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[_i - i, _j].transform);
                            CurrentBall.Index_I = _i - i;
                        }
                        else if (!GridItemArray[_i - i, _j].IsRoad) break;
                    }
                }
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; });


            }
            else if (_kontrolhýzýY > _katsayý)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int i = 0; i < Grid_J; i++)
                {
                    if (_j + i <= Grid_J - 1 && _j + i >= 0)
                    {
                        if (GridItemArray[_i, _j + i].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[_i, _j + i].transform);
                            CurrentBall.Index_J = _j + i;
                        }
                        else if (!GridItemArray[_i, _j + i].IsRoad) break;
                    }
                }
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; });

            }
            else if (_kontrolhýzýY < -_katsayý)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int i = 0; i < Grid_J; i++)
                {
                    if (_j - i <= Grid_J - 1 && _j - i >= 0)
                    {
                        if (GridItemArray[_i, _j - i].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[_i, _j - i].transform);
                            CurrentBall.Index_J = _j - i;
                        }
                        else if (!GridItemArray[_i, _j - i].IsRoad) break;
                    }
                }

                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; });


            }
            else IsMove = false;

        }
    }

    void CreateGrid()
    {
        for (int j = 0; j < Grid_J; j++)
        {
            for (int i = 0; i < Grid_I; i++)
            {
                GridItem gridItem = Instantiate(M_Level.I.GridItemPrefab, transform);
                gridItem.transform.localPosition = new Vector3(i, 0, j);

                gridItem.Grid_I = i;
                gridItem.Grid_J = j;
                gridItem.IsWall = true;
                gridItem.IsRoad = false;
                gridItem.transform.tag = "Wall";
                gridItem.transform.localScale = new Vector3(1, 0.5f, 1);
                gridItem.GetComponent<MeshRenderer>().material = M_Level.I.WallMat;
                GridItemArray[i, j] = gridItem;


                //if (i == 0 || i == 9 || j == 0 || j == 9)
                //{
                //    gridItem.IsWall = true;
                //    gridItem.IsRoad = false;
                //    gridItem.transform.tag = "Wall";
                //    gridItem.transform.localScale = new Vector3(1, 0.5f, 1);
                //    gridItem.GetComponent<MeshRenderer>().material = M_Level.I.WallMat;
                //}
            }
        }
    }
    void GridSetting(Ball currentBall, int turnSay)
    {
        bool a = false, b = false, c = true, d = true;
        int countSay = 0;
        int sayýtest = 0;
        while (turnSay > countSay)
        {
            sayýtest++;

            int _i = currentBall.Index_I;
            int _j = currentBall.Index_J;


            int count = Random.Range(1, 4);
            int direction = Random.Range(1, 5);

            switch (direction)
            {
                case 1://up  
                    if (a || _j + count >= Grid_J - 1) break;

                    for (int i = 0; i <= count; i++)
                    {
                        SetRoad(GridItemArray[_i, _j + i]);
                    }
                    a = false; b = false; c = true; d = false; countSay++; currentBall.Index_J += count;
                    break;

                case 2://right
                    if (b || _i + count >= Grid_I - 1) break;

                    for (int i = 0; i <= count; i++)
                    {
                        SetRoad(GridItemArray[_i + i, _j]);
                    }
                    a = false; b = false; c = false; d = true; countSay++; currentBall.Index_I += count;
                    break;

                case 3://down
                    if (c || _j - count <= 0) break;

                    for (int i = 0; i <= count; i++)
                    {
                        SetRoad(GridItemArray[_i, _j - i]);
                    }
                    a = true; b = false; c = false; d = false; countSay++; currentBall.Index_J -= count;
                    break;

                case 4://left
                    if (d || _i - count <= 0) break;

                    for (int i = 0; i <= count; i++)
                    {
                        SetRoad(GridItemArray[_i - i, _j]);
                    }
                    a = false; b = true; c = false; d = false; countSay++; currentBall.Index_I -= count;
                    break;
            }
        }
    }

    void SetRoad(GridItem gridItem)
    {
        gridItem.IsRoad = true;
        gridItem.transform.tag = "Road";
        gridItem.IsWall = false;
        gridItem.transform.localScale = new Vector3(1, 0.1f, 1);
        gridItem.GetComponent<MeshRenderer>().material = M_Level.I.RoadMat;
    }
}
