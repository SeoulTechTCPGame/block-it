using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OpenWays
{
    public bool[][] upDown;
    public bool[][] leftRight;
    private int v1;
    private int v2;
    private bool v3;

    public OpenWays(int v1, int v2, bool v3)
    {
        upDown = new bool[v1][];
        for (int i = 0; i < v1; i++)
        {
            upDown[i] = new bool[v2];
            for (int j = 0; j < v2; j++)
            {
                upDown[i][j] = v3;
            }
        }

        leftRight = new bool[v2][];
        for (int i = 0; i < v2; i++)
        {
            leftRight[i] = new bool[v2];
            for (int j = 0; j < v1; j++)
            {
                leftRight[i][j] = v3;
            }
        }
    }
}