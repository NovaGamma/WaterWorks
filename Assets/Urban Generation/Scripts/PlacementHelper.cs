using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementHelper
{
    public static List<Direction> FindNeighbour(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<Direction> neighbourDirections = new List<Direction>();
            if (collection.Contains(position + Vector3Int.right))
            {
                neighbourDirections.Add(Direction.Right);
            }
            if (collection.Contains(position - Vector3Int.right))
            {
                neighbourDirections.Add(Direction.Left);
            }
            if (collection.Contains(position + new Vector3Int(0, 0, 1)))
            {
                neighbourDirections.Add(Direction.Up);
            }
            if (collection.Contains(position - new Vector3Int(0, 0, 1)))
            {
                neighbourDirections.Add(Direction.Down);
            }
            return neighbourDirections;
    }

    public static Direction GetReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default: 
                break;
        }
        throw new System.Exception("No direction such as " + direction);
    }

    public static bool CheckIfPositionInWater(Vector3 position)
    {
        GameObject[] waters = GameObject.FindGameObjectsWithTag("Water");
        foreach (var gameObject in waters)
        {
            var colliderItem = gameObject.GetComponent<Collider>();
            if(colliderItem.bounds.Contains(position)) return true;
        }
        return false;
    }

    public static bool CheckIfPositionOffLimit(Vector3 position)
    {
        GameObject[] mapLimits = GameObject.FindGameObjectsWithTag("MapLimit");
        foreach (var gameObject in mapLimits)
        {
            var colliderItem = gameObject.GetComponent<Collider>();
            if(colliderItem.bounds.Contains(position)) return true;
        }
        return false;
    }

    public static bool CheckIfPositionAvailable(Vector3 position)
    {
        return !(CheckIfPositionInWater(position) && CheckIfPositionOffLimit(position));
    }
}
