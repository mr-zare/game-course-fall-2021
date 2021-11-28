using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino
{
    public List<int[,]> rotations;
    private int currentRotationIndex;
    private int colorCode;
    public string name;

    public int GetLength() => rotations[0].GetLength(0);

    public Tetromino(string name)
    {
        currentRotationIndex = 0;
        
        this.name = name;
    }

    public void Rotate()
    {
        currentRotationIndex++;
        if (currentRotationIndex >= rotations.Count)
            currentRotationIndex = 0;
    }
    
    public int GetCurrentRotationIndex() => currentRotationIndex;

    public int[] FindLowerBoundsOfPreset()
    {
        int length = GetLength();

        int[] bounds = new int[length];

        for (int i = 0; i < length; i++)
            bounds[i] = -1;

        for (int j = length - 1; j >= 0; j--)
        {
            for (int i = 0; i < length; i++)
            {
                if (rotations[GetCurrentRotationIndex()][i, j] != 0)
                {
                    if (bounds[i] < j)
                    {
                        bounds[i] = j;
                    }
                }
            }
        }
        return bounds;
    }
    
    public int GetHighestBoundY(int[] bounds)
    {
        int max = -2;
        for (int i = 0; i < bounds.Length; i++)
        {
            if (bounds[i] > max)
                max = bounds[i];
        }
        return max;
    }

    public int GetHighestBoundRight()
    {
        int length = GetLength();

        int max = -100;

        for (int j = 0; j < length; j++)
        {
            for (int i = length-1; i > 0 ; i--)
            {
                if (rotations[GetCurrentRotationIndex()][i, j] != 0)
                {
                    if (i > max)
                        max = i;
                }
            }
        }
        return max;
    }

    public int GetHighestBoundLeft()
    {
        int length = GetLength();

        int min = 100;

        for (int j = 0; j < length; j++)
        {
            for (int i = 0; i < length; i++)
            {
                if (rotations[GetCurrentRotationIndex()][i, j] != 0)
                {
                    if (i < min)
                        min = i;
                }
            }
        }
        return min;
    }

    public void SetColor(int colorIndex) => colorCode = colorIndex;

    public int GetColor() => colorCode;
}

public static class TetrominosContainer
{
    public static Tetromino GetPreset_L()
    {
        Tetromino preset = new Tetromino("L");
        preset.rotations = new List<int[,]>();

        preset.rotations.Add(new int[3, 3]
        {
            {0, 0, 1},
            {1, 1, 1},
            {0, 0, 0}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 1, 0},
            {0, 1, 0},
            {0, 1, 1}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 0, 0},
            {1, 1, 1},
            {1, 0, 0}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {1, 1, 0},
            {0, 1, 0},
            {0, 1, 0}
        });

        return preset;
    }

    public static Tetromino GetPreset_O()
    {
        Tetromino preset = new Tetromino("O");
        preset.rotations = new List<int[,]>();

        preset.rotations.Add(new int[2, 2]
        {
            {1, 1},
            {1, 1},
        });

        return preset;
    }

    public static Tetromino GetPreset_I()
    {
        Tetromino preset = new Tetromino("I");
        preset.rotations = new List<int[,]>();

        preset.rotations.Add(new int[4, 4]
        {
            {0, 0, 1, 0},
            {0, 0, 1, 0},
            {0, 0, 1, 0},
            {0, 0, 1, 0},
        });
        preset.rotations.Add(new int[4, 4]
        {
            {0, 0, 0, 0},
            {1, 1, 1, 1},
            {0, 0, 0, 0},
            {0, 0, 0, 0},
        });

        return preset;
    }

    public static Tetromino GetPreset_T()
    {
        Tetromino preset = new Tetromino("T");
        preset.rotations = new List<int[,]>();

        preset.rotations.Add(new int[3, 3]
        {
            {0, 1, 0},
            {1, 1, 1},
            {0, 0, 0}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 1, 0},
            {0, 1, 1},
            {0, 1, 0}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 0, 0},
            {1, 1, 1},
            {0, 1, 0}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 1, 0},
            {1, 1, 0},
            {0, 1, 0}
        });

        return preset;
    }
    
    public static Tetromino GetPreset_S()
    {
        Tetromino preset = new Tetromino("S");
        preset.rotations = new List<int[,]>();

        preset.rotations.Add(new int[3, 3]
        {
            {0, 1, 1},
            {1, 1, 0},
            {0, 0, 0}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 1, 0},
            {0, 1, 1},
            {0, 0, 1}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 0, 0},
            {0, 1, 1},
            {1, 1, 0}
        });
        preset.rotations.Add(new int[3, 3]
        {
            {0, 1, 0},
            {1, 1, 0},
            {1, 0, 0}
        });

        return preset;
    }
}