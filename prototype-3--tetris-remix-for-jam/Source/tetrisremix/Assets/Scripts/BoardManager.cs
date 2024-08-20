using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager
{
    private int height;
    private int width;

    private List<Tetromino> presets;
    public int[,] gameBoard;
    private Tetromino nextTetrominoToPlace;

    private Vector2 nextTetrominoCurrentPosition;
    private Vector2 defaultInitialPosition;
    private List<Vector2> nextPresetVisitedPositions;

    public int presetIndexForNextNEXT;

    public BoardManager(int width, int height)
    {
        this.height = height;
        this.width = width;
        defaultInitialPosition = new Vector2(width / 2, 0);
        nextTetrominoCurrentPosition = defaultInitialPosition;

        gameBoard = new int[width, height];

        nextPresetVisitedPositions = new List<Vector2>();

        _InitPresets();
    }
    
    private void _InitPresets()
    {
        presets = new List<Tetromino>();
        
        presets.Add(TetrominosContainer.GetPreset_L());
        presets.Add(TetrominosContainer.GetPreset_O());
        presets.Add(TetrominosContainer.GetPreset_I());
        presets.Add(TetrominosContainer.GetPreset_T());
        presets.Add(TetrominosContainer.GetPreset_S());

        ChooseNextNextPresetIndex();
    }
    
    public void DrawBoardLogically()
    {
        string board = "0, ";
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                board += gameBoard[j, i];
            }
            board += "\n" + i + ", " ;
        }

        Debug.Log(board);
    }
    
    public void InitializeNextPreset()
    {
        int index = presetIndexForNextNEXT;

        nextTetrominoToPlace = presets[index];
        
        nextTetrominoCurrentPosition = defaultInitialPosition;
        
        nextPresetVisitedPositions.Clear();
    }

    public void ChooseNextNextPresetIndex()
    {
        presetIndexForNextNEXT = UnityEngine.Random.Range(0, presets.Count);
    }

    public string GetNextNextPresetName() => presets[presetIndexForNextNEXT].name;

    public void UpdateBoardLogically()
    {
        int length = nextTetrominoToPlace.GetLength();

        int x = (int)nextTetrominoCurrentPosition.x;
        int y = (int)nextTetrominoCurrentPosition.y;

        WipePreviosulyVisitedPlacesByNextPresetLogically();

        for (int i = x; i < x + length; i++)
        {
            for (int j = y; j < y + length; j++)
            {
                if (nextTetrominoToPlace.rotations[nextTetrominoToPlace.GetCurrentRotationIndex()][i-x ,j - y] != 0)
                { 
                    gameBoard[i, j] = 1;
                    nextPresetVisitedPositions.Add(new Vector2(i, j));
                }
            }
        }
    }
    
    public void MovePresetDownwards()
    {
        nextTetrominoCurrentPosition.y++;

        int[] lowerBounds = nextTetrominoToPlace.FindLowerBoundsOfPreset();
        int maxY = lowerBounds.Max();

        // check collision witht previoulsy placed presets
        if (nextTetrominoCurrentPosition.y + maxY >= height)
            nextTetrominoCurrentPosition.y = height - 1 - maxY;
    }

    public void MovePresetToRightAndLeft(bool isRight)
    {
        
        if (isRight)
        {
            if (nextTetrominoCurrentPosition.x + nextTetrominoToPlace.GetHighestBoundRight() + 1 >= width)
                return;

            nextTetrominoCurrentPosition.x++;
        }
        else
        {
            Debug.Log("b ~ " + nextTetrominoToPlace.GetHighestBoundLeft());
            Debug.Log("x ~ " + nextTetrominoCurrentPosition.x);


            if (nextTetrominoCurrentPosition.x + nextTetrominoToPlace.GetHighestBoundLeft() - 1 < 0)
                return;

            nextTetrominoCurrentPosition.x--;
        }
        
    }

    public void RotateNextPreset()
    {
        nextTetrominoToPlace.Rotate();
    }

    public void WipePreviosulyVisitedPlacesByNextPresetLogically()
    {
        // clean previous locations
        foreach (Vector2 pos in nextPresetVisitedPositions)
        {
            gameBoard[(int)pos.x, (int)pos.y] = 0;
        }

        nextPresetVisitedPositions.Clear();
    }

    public bool CanKeepMovingThePreset()
    {
        
        int length = nextTetrominoToPlace.GetLength();
        int x = (int)nextTetrominoCurrentPosition.x;
        int y = (int)nextTetrominoCurrentPosition.y;
        int[] lowerBounds = nextTetrominoToPlace.FindLowerBoundsOfPreset();

        if (nextTetrominoCurrentPosition.y + nextTetrominoToPlace.GetHighestBoundY(lowerBounds) + 1 >= height - 1)
            return false;

        for (int i = x; i < x + length; i++)
        {
            if (lowerBounds[i-x] != -1 && 
                gameBoard[i, y + lowerBounds[i-x]] != 0 && 
                gameBoard[i, y + lowerBounds[i-x] + 1] != 0)
                return false;
        }

        return true;
    }

    public void SetColorForCurrentPreset(int colorIndex)
    {
        nextTetrominoToPlace.SetColor(colorIndex);
    }

    public bool[] CheckAndEraseFullRows()
    {
        bool[] rowBlocksAreFull = new bool[height];
        for (int j = 0; j < height; j++)
        {
            rowBlocksAreFull[j] = true;

            for (int i = 0; i < width; i++)
            {
                if (gameBoard[i, j] == 0)
                {
                    rowBlocksAreFull[j] = false;
                    break;
                }
            }
        }

        for (int j = height - 1; j >= 0; j--)
        {
            if (rowBlocksAreFull[j])
            {
                Debug.Log("Full row detected");

                for (int k = j; k > 0; k--)
                {
                    for (int i = 0; i < width; i++)
                    {
                        gameBoard[i, k] = gameBoard[i, k - 1];
                    }
                }
            }
        }

        return rowBlocksAreFull;
    }

    public int[,] GetCurrentTetrominoRotation()
    {
        return nextTetrominoToPlace.rotations[nextTetrominoToPlace.GetCurrentRotationIndex()];
    }

    public (int, int, int , int) GetCurrentTetrominoBounds()
    {
        int length = nextTetrominoToPlace.GetLength();

        return ((int)nextTetrominoCurrentPosition.x, (int)nextTetrominoCurrentPosition.y,
            (int)nextTetrominoCurrentPosition.x + length, (int)nextTetrominoCurrentPosition.y + length);
    }

    public int GetCurrentTetrominoLength() => nextTetrominoToPlace.GetLength();
}
