using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeDelayBetweenEachMove;
    public float timerBoostValue;

    public float gridUnitSize = 0.641f;
    public int width;
    public int height;
    public float heightOffset;
    public float widthOffset;

    public GameObject[] blockViewTypes;
    public GameObject boardObjectsParent;

    public enum GameState
    {
        TetrominoIsBeingPlaced,
        NextTetrominoInQueue,
        BossIsPassing,
        None
    };

    public GameState state;


    private float timer;
    private BoardManager boardManager;
    private float defaultTimeDelayBetweenEachMove;

    private int lastChosenColor;

    private List<GameObject> temporaryCurrentTetrominoGameObjects;

    private void Awake()
    {
        boardManager = new BoardManager(width, height);
        state = GameState.NextTetrominoInQueue;

        defaultTimeDelayBetweenEachMove = timeDelayBetweenEachMove;
        timer = Time.fixedTime + timeDelayBetweenEachMove;
    }

    private void FixedUpdate()
    {
        if (Time.fixedTime >= timer)
        {
            if (state == GameState.TetrominoIsBeingPlaced)
            {
                if (boardManager.CanKeepMovingThePreset())
                {
                    boardManager.MovePresetDownwards();
                }
                else
                {
                    //
                    temporaryCurrentTetrominoGameObjects.Clear();

                    // check if a full row is complete and shift all rows by one


                    // place the preset on board permentantly
                    // change state
                    state = GameState.NextTetrominoInQueue;
                }
            }

            timer = Time.fixedTime + timeDelayBetweenEachMove;
        }
    }

    void Update()
    {
        if (state == GameState.BossIsPassing)
        {
            // pass the boss


            // after boss has passed
            state = GameState.NextTetrominoInQueue;
        }

        if (state == GameState.NextTetrominoInQueue)
        {
            // choose the next preset
            //boardManager.ChooseNextPreset();
            boardManager.ChooseNextPreset();

            ChooseNewColor();

            temporaryCurrentTetrominoGameObjects = new List<GameObject>();
            RenderDifferences(instantiateForNewTetromino: true);

            state = GameState.TetrominoIsBeingPlaced;
        }

        if (state == GameState.TetrominoIsBeingPlaced)
        {
            if (boardManager.CanKeepMovingThePreset())
            {
                HandlePresetControlByPlayer();
            }

            boardManager.UpdateBoardLogically();
            //boardManager.CheckAndEraseFullRows();

            RenderDifferences();
        }
    }

    private void RenderDifferences(bool instantiateForNewTetromino = false)
    {
        // get tetromino current (default) rotation
        int[,] currentTetromino = boardManager.GetCurrentTetrominoRotation();

        // instnatiate respective color objects in the current places
        var bounds = boardManager.GetCurrentTetrominoBounds();

        int counter = 0;
        for (int i = bounds.Item1; i < bounds.Item3; i++)
        {
            for (int j = bounds.Item2; j < bounds.Item4; j++)
            {
                if (currentTetromino[i - bounds.Item1, j - bounds.Item2] != 0)
                {
                    if (instantiateForNewTetromino)
                    { 
                        var block = Instantiate(blockViewTypes[lastChosenColor], boardObjectsParent.transform);
                        block.transform.position = new Vector3(i * gridUnitSize - widthOffset, (height - j) * gridUnitSize - heightOffset, 0);
                        temporaryCurrentTetrominoGameObjects.Add(block);
                    }
                    else
                    {
                        temporaryCurrentTetrominoGameObjects[counter].transform.position = new Vector3(i * gridUnitSize - widthOffset, (height - j) * gridUnitSize - heightOffset, 0);
                        counter++;
                    }
                }
            }
        }
    }

    private void HandlePresetControlByPlayer()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            boardManager.RotateNextPreset();

            //boardManager.PreviewNextPresetPosition();

            //RenderBoard();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            boardManager.MovePresetToRightAndLeft(true);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            boardManager.MovePresetToRightAndLeft(false);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            timeDelayBetweenEachMove -= timerBoostValue;
        }
        else
        {
            timeDelayBetweenEachMove = defaultTimeDelayBetweenEachMove;
        }
    }
    
    
    private void ChooseNewColor()
    {
        int newColor;
        do
        {
            newColor = UnityEngine.Random.Range(0, blockViewTypes.Length);
        } while (lastChosenColor == newColor);

        lastChosenColor = newColor;
    }
}
