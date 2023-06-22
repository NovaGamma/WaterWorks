using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadHelper : MonoBehaviour
{
    public GameObject roadStraight, roadCorner, road3way, road4way, roadEnd;
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>();
    public PumpManager pumpManager;

    public List<Vector3Int> GetRoadPositions()
    {
        return roadDictionary.Keys.ToList();
    }

    public void PlaceStreetPositions(Vector3 startPosition, Vector3Int direction, int length, ref Dictionary<Vector3Int, GameObject> structureDictionary)
    {
        var rotation = Quaternion.identity;
        if(direction.x == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(startPosition + direction *i);
            // Special case to avoid adding an existing.
            if (roadDictionary.ContainsKey(position))
            {
                continue;
            }
            // Special case to destroy a building.
            if (structureDictionary.ContainsKey(position))
            {
                Destroy(structureDictionary[position]);
                structureDictionary.Remove(position);
            }
            var road = Instantiate(roadStraight, position, rotation, transform);
            roadDictionary.Add(position, road);
        }
    }

    public void FixRoad(LSystemGenerator lSystem, RoadHelper roadHelper, StructureHelper structureHelper, PipeManager pipeManager)
    {
        foreach (Vector3Int key in roadDictionary.Keys)
        {
            if(pumpManager.GetPumpsPositions().Contains(key)){
                continue;
            }
            fixRoadCandidates.Add(key);
        }
        foreach (var position in fixRoadCandidates)
        {
            List<Direction> neighbourDirections = PlacementHelper.FindNeighbour(position, roadDictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            if (neighbourDirections.Count == 1)
            {
                Destroy(roadDictionary[position]);
                Direction blockedDirection = Direction.Right;
                Vector3 visualizerPosition = position + Vector3.left;
                if (neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                    blockedDirection = Direction.Down;
                    visualizerPosition = position + Vector3.forward;
                } else if (neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                    blockedDirection = Direction.Left;
                    visualizerPosition = position + Vector3.right;
                }
                else if (neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                    blockedDirection = Direction.Up;
                    visualizerPosition = position + Vector3.back;
                }
                roadDictionary[position] = Instantiate(roadEnd, position, rotation, transform);
                Visualizer visualizerRoadEnd = roadDictionary[position].GetComponent<Visualizer>();
                visualizerRoadEnd.lsystem = lSystem;
                visualizerRoadEnd.roadHelper = roadHelper;
                visualizerRoadEnd.structureHelper = structureHelper;
                visualizerRoadEnd.pipeManager = pipeManager;
                visualizerRoadEnd.startingPosition = visualizerPosition;
                visualizerRoadEnd.blockedDirections.Add(blockedDirection);
            }
            else if (neighbourDirections.Count == 2)
            {
                Destroy(roadDictionary[position]);
                if(neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                    roadDictionary[position] = Instantiate(roadStraight, position, rotation, transform);
                    continue;
                }
                if (neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.identity;
                    roadDictionary[position] = Instantiate(roadStraight, position, rotation, transform);
                    continue;
                }
                if (neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down) && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[position] = Instantiate(roadCorner, position, rotation, transform);
            }
            else if(neighbourDirections.Count == 3)
            {
                Destroy(roadDictionary[position]);
                if (neighbourDirections.Contains(Direction.Right) 
                    && neighbourDirections.Contains(Direction.Down) 
                    && neighbourDirections.Contains(Direction.Left)
                    )
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down) 
                    && neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left) 
                    && neighbourDirections.Contains(Direction.Up)
                    && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                roadDictionary[position] = Instantiate(road3way, position, rotation, transform);
            }
            else
            {
                Destroy(roadDictionary[position]);
                roadDictionary[position] = Instantiate(road4way, position, rotation, transform);
            }
        }
    }

    public bool CheckPositionIsInsideCollider(Vector3 position)
    {
        var collidersObj = gameObject.GetComponentsInChildren<BoxCollider>();
        for (var index = 0; index < collidersObj.Length; index++)
        {
            var colliderItem = collidersObj[index];
            if(colliderItem.bounds.Contains(position))
            {
                return true;
            }
        }
        return false;
    }
}

