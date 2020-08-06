using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3,
}

public static class DirectionExtension
{
    public static Vector2Int ToCoordinate(this Direction dir)
    {
        var result = Vector2Int.zero;

        switch (dir)
        {
            case Direction.Left:
                result = new Vector2Int(-1, 0);
                break;
            case Direction.Right:
                result = new Vector2Int(1, 0);
                break;
            case Direction.Up:
                result = new Vector2Int(0, 1);
                break;
            case Direction.Down:
                result = new Vector2Int(0, -1);
                break;
        }

        return result;
    }

    public static Vector2 GetVector2(this Direction dir)
    {
        var result = Vector2.zero;

        switch(dir)
        {
            case Direction.Left:
                result = new Vector3(-1, 0);
                break;
            case Direction.Right:
                result = new Vector3(1, 0);
                break;
            case Direction.Up:
                result = new Vector3(0, 1);
                break;
            case Direction.Down:
                result = new Vector3(0, -1);
                break;
        }

        return result;
    }
}
