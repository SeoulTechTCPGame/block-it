using System;
using UnityEngine;


class Pawn
{
        private Vector2Int mCoordinate;
        private int mPlankNum = 10;
        

        public Vector2Int GetCoordinate()
        {
            return mCoordinate;
        }

        public int GetPlankNum()
        {
            return mPlankNum;
        }

        public void SetCoordinate(Vector2Int coordinate)
        {
            mCoordinate = coordinate;
        }

        public void UsePlank()
        {
            mPlankNum--;
        }
    }