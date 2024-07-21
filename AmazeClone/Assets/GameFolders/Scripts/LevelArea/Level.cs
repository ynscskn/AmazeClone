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
        // Placeholder for GameStart logic
    }

    private void GameReady()
    {
        Debug.Log("GameReady");
    }

    private void GamePause()
    {
        Debug.Log("GamePause");
    }

    private void GameContinue()
    {
        Debug.Log("GameContinue");
    }

    private void GameFail()
    {
        Debug.Log("GameFail");
    }

    private void GameComplete()
    {
        Debug.Log("GameComplete");
    }

    private void GameRetry()
    {
        Debug.Log("GameRetry");
    }

    private void GameNextLevel()
    {
        Debug.Log("GameNextLevel");
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
    Tween scaleTw;

    private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        fingerDownFirstVec = fingerPos;
    }

    private void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
    {
        if (fingerIndex != 0 || M_Level.I.CurrentBall == null) return;

        fingerDragVec = fingerPos;
        fingerDirection = fingerDragVec - fingerDownFirstVec;

        int multiplier = 50 * Screen.width / 1080;

        int controlSpeedX = Mathf.FloorToInt(fingerDirection.x);
        int controlSpeedY = Mathf.FloorToInt(fingerDirection.y);

        if (!IsMove)
        {
            IsMove = true;

            int i = CurrentBall.Index_I;
            int j = CurrentBall.Index_J;

            if (controlSpeedX > multiplier)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int k = 0; k < Grid_I; k++)
                {
                    if (i + k <= Grid_I - 1 && i + k >= 0)
                    {
                        if (GridItemArray[i + k, j].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[i + k, j].transform);
                            CurrentBall.Index_I = i + k;
                            GridItemArray[i + k, j].IsPainted = true;
                            Destroy(GridItemArray[i + k, j].Collider, 0.5f);
                        }
                        else if (!GridItemArray[i + k, j].IsRoad) break;
                    }
                }
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; scaleTw.Kill(); CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash); });
                scaleTw = CurrentBall.transform.DOScaleX(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash));
            }
            else if (controlSpeedX < -multiplier)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int k = 0; k < Grid_I; k++)
                {
                    if (i - k <= Grid_I - 1 && i - k >= 0)
                    {
                        if (GridItemArray[i - k, j].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[i - k, j].transform);
                            CurrentBall.Index_I = i - k;
                        }
                        else if (!GridItemArray[i - k, j].IsRoad) break;
                    }
                }
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; scaleTw.Kill(); CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash); });
                scaleTw = CurrentBall.transform.DOScaleX(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleX(1, 0.1f).SetEase(Ease.Flash));
            }
            else if (controlSpeedY > multiplier)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int k = 0; k < Grid_J; k++)
                {
                    if (j + k <= Grid_J - 1 && j + k >= 0)
                    {
                        if (GridItemArray[i, j + k].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[i, j + k].transform);
                            CurrentBall.Index_J = j + k;
                        }
                        else if (!GridItemArray[i, j + k].IsRoad) break;
                    }
                }
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; scaleTw.Kill(); CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash); });
                scaleTw = CurrentBall.transform.DOScaleZ(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash));
            }
            else if (controlSpeedY < -multiplier)
            {
                fingerDownFirstVec = fingerPos;
                IsMove = true;
                for (int k = 0; k < Grid_J; k++)
                {
                    if (j - k <= Grid_J - 1 && j - k >= 0)
                    {
                        if (GridItemArray[i, j - k].IsRoad)
                        {
                            CurrentBall.transform.SetParent(GridItemArray[i, j - k].transform);
                            CurrentBall.Index_J = j - k;
                        }
                        else if (!GridItemArray[i, j - k].IsRoad) break;
                    }
                }
                CurrentBall.transform.DOLocalMove(movePos, Speed).SetSpeedBased().SetEase(Ease.Flash).OnComplete(() => { IsMove = false; scaleTw.Kill(); CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash); });
                scaleTw = CurrentBall.transform.DOScaleZ(1.5f, 0.25f).SetEase(Ease.OutExpo).OnComplete(() => CurrentBall.transform.DOScaleZ(1, 0.1f).SetEase(Ease.Flash));
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
            }
        }
    }

    void GridSetting(Ball currentBall, int turnCount)
    {
        bool canMoveUp = true, canMoveRight = true, canMoveDown = false, canMoveLeft = false;
        int movesCount = 0;

        while (turnCount > movesCount)
        {
            int i = currentBall.Index_I;
            int j = currentBall.Index_J;

            int count = Random.Range(2, 5);
            int direction = Random.Range(1, 5);

            switch (direction)
            {
                case 1: // up
                    if (!canMoveUp || j + count >= Grid_J - 1) break;

                    for (int k = 0; k <= count; k++)
                    {
                        if (GridItemArray[i, j + k].IsColon)
                        {
                            count = k - 1;
                            break;
                        }
                        else SetRoad(GridItemArray[i, j + k]);
                    }

                    if (GridItemArray[i, j + count + 1].IsRoad)
                    {
                        while (GridItemArray[i, j + count].IsRoad)
                        {
                            if (j + count + 1 >= Grid_J - 1) break;
                            count++;
                        }
                    }

                    currentBall.Index_J += count;
                    SetColon(GridItemArray[i, j + count + 1]);

                    canMoveUp = false;
                    canMoveRight = true;
                    canMoveDown = false;
                    canMoveLeft = true;
                    movesCount++;
                    break;

                case 2: // right
                    if (!canMoveRight || i + count >= Grid_I - 1) break;

                    for (int k = 0; k <= count; k++)
                    {
                        if (GridItemArray[i + k, j].IsColon)
                        {
                            count = k - 1;
                            break;
                        }
                        else SetRoad(GridItemArray[i + k, j]);
                    }

                    if (GridItemArray[i + count + 1, j].IsRoad)
                    {
                        while (GridItemArray[i + count, j].IsRoad)
                        {
                            if (i + count + 1 >= Grid_I - 1) break;
                            count++;
                        }
                    }

                    currentBall.Index_I += count;
                    SetColon(GridItemArray[i + count + 1, j]);

                    canMoveUp = true;
                    canMoveRight = false;
                    canMoveDown = true;
                    canMoveLeft = false;
                    movesCount++;
                    break;

                case 3: // down
                    if (!canMoveDown || j - count <= 0) break;

                    for (int k = 0; k <= count; k++)
                    {
                        if (GridItemArray[i, j - k].IsColon)
                        {
                            count = k - 1;
                            break;
                        }
                        else SetRoad(GridItemArray[i, j - k]);
                    }

                    if (GridItemArray[i, j - count - 1].IsRoad)
                    {
                        while (GridItemArray[i, j - count].IsRoad)
                        {
                            if (j - count - 1 <= 0) break;
                            count++;
                        }
                    }

                    currentBall.Index_J -= count;
                    SetColon(GridItemArray[i, j - count - 1]);

                    canMoveUp = false;
                    canMoveRight = true;
                    canMoveDown = false;
                    canMoveLeft = true;
                    movesCount++;
                    break;

                case 4: // left
                    if (!canMoveLeft || i - count <= 0) break;

                    for (int k = 0; k <= count; k++)
                    {
                        if (GridItemArray[i - k, j].IsColon)
                        {
                            count = k - 1;
                            break;
                        }
                        else SetRoad(GridItemArray[i - k, j]);
                    }

                    if (GridItemArray[i - count - 1, j].IsRoad)
                    {
                        while (GridItemArray[i - count, j].IsRoad)
                        {
                            if (i - count - 1 <= 0) break;
                            count++;
                        }
                    }

                    currentBall.Index_I -= count;
                    SetColon(GridItemArray[i - count - 1, j]);

                    canMoveUp = true;
                    canMoveRight = false;
                    canMoveDown = true;
                    canMoveLeft = false;
                    movesCount++;
                    break;
            }

            if (currentBall.Index_I == 1) canMoveLeft = false;
            if (currentBall.Index_I == 8) canMoveRight = false;
            if (currentBall.Index_J == 1) canMoveDown = false;
            if (currentBall.Index_J == 8) canMoveUp = false;
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

    void SetColon(GridItem gridItem)
    {
        if (gridItem.IsColon) return;
        gridItem.IsColon = true;
        gridItem.transform.tag = "Colon"; // Make sure this tag is defined in Unity
        gridItem.GetComponent<MeshRenderer>().material.color = Color.red;
    }
}
