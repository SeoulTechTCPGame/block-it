using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Walls
{
    public bool[][] horizontal;
    public bool[][] vertical;

    public Walls(int v1, int v2, bool v3)
    {
        horizontal = new bool[v1][];
        vertical = new bool[v1][];
        for (int i = 0; i < v1; i++)
        {
            horizontal[i] = new bool[v2];
            vertical[i] = new bool[v2];
            for (int j = 0; j < v2; j++)
            {
                horizontal[i][j] = v3;
                vertical[i][j] = v3;
            }
        }
    }
}
