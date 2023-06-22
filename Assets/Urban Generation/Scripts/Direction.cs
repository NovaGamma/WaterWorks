using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class DirectionHelper
{
    public static Vector3Int GetOffsetFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3Int.forward;
            case Direction.Down:
                return Vector3Int.back;
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            default: 
                break;
        }
        throw new System.Exception("No direction such as " + direction);
    }
}

