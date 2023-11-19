using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidNextPosition
{
    public bool[][] _validNextPosition;

    public ValidNextPosition(int v1, int v2, bool v3, Pawn_ turn)
    {
        _validNextPosition = new bool[v1][];
        for(int i=0; i<v1; i++)
        {
            _validNextPosition[i] = new bool[v2];
            for(int j=0 ;j<v2; j++)
            {

                if (turn.position.row == i + 1 && turn.position.col == j)
                {
                    _validNextPosition[i][j] = !v3;
                    continue;
                }
                if (turn.position.row == i - 1 && turn.position.col == j)
                {
                    _validNextPosition[i][j] = !v3;
                    continue;
                }
                if (turn.position.row == i && turn.position.col == j + 1)
                {
                    _validNextPosition[i][j] = !v3;
                    continue;
                }
                if (turn.position.row == i && turn.position.col == j - 1)
                {
                    _validNextPosition[i][j] = !v3;
                    continue;
                }
                _validNextPosition[i][j] = v3;
            }
        }
    }
}
