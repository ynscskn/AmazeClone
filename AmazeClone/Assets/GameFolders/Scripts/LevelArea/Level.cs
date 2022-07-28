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
        GridSetting(CurrentBall, 10);
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
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; testtw.Kill(); CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash); });
                testtw = CurrentBall.transform.DOScaleX(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash));

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
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; testtw.Kill(); CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash); });
                testtw = CurrentBall.transform.DOScaleX(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash));



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
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; testtw.Kill(); CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash); });
                testtw = CurrentBall.transform.DOScaleZ(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash));


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

                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; testtw.Kill(); CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash); });
                testtw = CurrentBall.transform.DOScaleZ(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash));



            }
            else IsMove = false;

        }
    }
    Tween testtw;
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
        bool a = true, b = true, c = false, d = false;
        int countSay = 0;
        int sayýtest = 0;
        while (turnSay > countSay)
        {

            sayýtest++;

            int _i = currentBall.Index_I;
            int _j = currentBall.Index_J;


            int count = Random.Range(2, 5);
            int direction = Random.Range(1, 5);
            print("go // direction : " + direction + "   //  count :  " + count);
            switch (direction)
            {
                case 1://up  
                    print("case 1   a : " + a + "  // _i : " + _i + " // _j : " + _j + " // count : " + count + "  // j +1 yol mu ");

                    if (!a || _j + count >= Grid_J - 1) break;

                    if (GridItemArray[_i, _j + 1].IsRoad)
                    {
                        print("yol");

                        int _q = roading(count, _i, _j);
                        currentBall.Index_J += _q;
                    }
                    else
                    {
                        for (int i = 0; i <= count; i++)
                        {
                            print("blob");
                            if (GridItemArray[_i, _j + i].IsRoad && i != 0)
                            {
                                int _q = roading(count, _i, _j);
                                count = _q; print("yol_cýktý");

                            }
                            else
                            {
                                SetRoad(GridItemArray[_i, _j + i]);
                                print("devamke");

                            }
                        }
                        currentBall.Index_J += count;
                    }
                    a = false; b = true; c = false; d = true;

                    countSay++;

                    break;

                case 2://right
                    print("case 2   b : " + b + "  // _i : " + _i + " // _j : " + _j + " // count : " + count + "  // i +1 yol mu ");

                    if (!b || _i + count >= Grid_I - 1) break;

                    if (GridItemArray[_i + 1, _j].IsRoad)
                    {
                        print("yol");
                        int _q = roading(count, _i, _j);

                        currentBall.Index_I += _q;
                    }
                    else
                    {
                        for (int i = 0; i <= count; i++)
                        {
                            print("blob");
                            if (GridItemArray[_i + i, _j].IsRoad && i != 0)
                            {
                                int _q = roading(count, _i, _j);
                                count = _q; print("yol_cýktý");

                            }
                            else
                            {
                                SetRoad(GridItemArray[_i + i, _j]);
                                print("devamke");

                            }
                        }
                        currentBall.Index_I += count;

                    }

                    a = true; b = false; c = true; d = false;

                    countSay++;

                    break;

                case 3://down
                    print("case 3   c : " + c + "  // _i : " + _i + " // _j : " + _j + " // count : " + count + "  // j -1 yol mu ");

                    if (!c || _j - count <= 0) break;
                    if (GridItemArray[_i, _j - 1].IsRoad)
                    {
                        print("yol");

                        int _q = roading(count, _i, _j);

                        currentBall.Index_J -= _q;
                    }
                    else
                    {
                        for (int i = 0; i <= count; i++)
                        {
                            print("blob");
                            if (GridItemArray[_i, _j - i].IsRoad && i != 0)
                            {
                                int _q = roading(count, _i, _j);
                                count = _q; print("yol_cýktý");

                            }
                            else
                            {
                                SetRoad(GridItemArray[_i, _j - i]);
                                print("devamke");

                            }
                        }
                        currentBall.Index_J -= count;
                    }

                    a = false; b = true; c = false; d = true;

                    countSay++;

                    break;

                case 4://left
                    print("case 4   d : " + d + "  // _i : " + _i + " // _j : " + _j + " // count : " + count + "  // i -1 yol mu ");

                    if (!d || _i - count <= 0) break;
                    if (GridItemArray[_i - 1, _j].IsRoad)
                    {
                        print("yol");

                        int _q = roading(count, _i, _j);

                        currentBall.Index_I -= _q;
                    }
                    else
                    {
                        for (int i = 0; i <= count; i++)
                        {
                            print("blob");
                            if (GridItemArray[_i - i, _j].IsRoad && i != 0)
                            {
                                int _q = roading(count, _i, _j);
                                count = _q; print("yol_cýktý");

                            }
                            else
                            {
                                print("devamke");

                                SetRoad(GridItemArray[_i - i, _j]);
                            }

                        }
                        currentBall.Index_I -= count;
                    }

                    a = true; b = false; c = true; d = false;

                    countSay++;

                    break;
            }

            if (currentBall.Index_I == 1) d = false;
            if (currentBall.Index_I == 8) b = false;
            if (currentBall.Index_J == 1) c = false;
            if (currentBall.Index_J == 8) a = false;





        }
    }
    int roading(int count, int _i, int _j)
    {
        int _q = 1;
        while (GridItemArray[_i, _j + _q].IsRoad)
        {
            if (_q == count) break;
            _q++;
        }
        return _q;
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
