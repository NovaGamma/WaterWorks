using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureHelper : MonoBehaviour
{
    public PipeManager pipeManager;
    public RoadHelper roadHelper;
    public BuildingType[] buildingTypes;
    public Dictionary<Vector3Int, GameObject> structureDictionary = new Dictionary<Vector3Int, GameObject>();

    private void Update() {
        PlaceStructureAroundRoad(roadHelper.GetRoadPositions());
    }

    public void PlaceStructureAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);
        List<Vector3Int> blockedPositions = new List<Vector3Int>();
        foreach (var freeSpot in freeEstateSpots)
        {
            if(blockedPositions.Contains(freeSpot.Key) || !PlacementHelper.CheckIfPositionAvailable(freeSpot.Key))
            {
                continue;
            }
            var rotation = Quaternion.identity;
            switch (freeSpot.Value) // All house must point to the left
            {
                case Direction.Up:
                    rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case Direction.Down:
                    rotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Direction.Right:
                    rotation = Quaternion.Euler(0, 180, 0);
                    break;
                default: 
                    break;
            }
            for (int i = 0; i < buildingTypes.Length; i++)
            {
                if(buildingTypes[i].IsBuildingAvailable())
                {
                    if (buildingTypes[i].sizeRequired.x > 1)
                    {
                        var halfSizeLength = Mathf.FloorToInt(buildingTypes[i].sizeRequired.x / 2.0f);
                        var halfSizeWidth = Mathf.FloorToInt(buildingTypes[i].sizeRequired.y / 2.0f);
                        List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                        if(VerifyIfBuildingFits(halfSizeLength, halfSizeWidth, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked))
                        {
                            blockedPositions.AddRange(tempPositionsBlocked);
                            var building = SpawnPrefab(buildingTypes[i].GetPrefab(), 
                                                        freeSpot.Key - DirectionHelper.GetOffsetFromDirection(freeSpot.Value) * halfSizeWidth, 
                                                        rotation);
                            structureDictionary.Add(freeSpot.Key, building);
                            foreach(var pos in tempPositionsBlocked)
                            {
                                structureDictionary.Add(pos, building);
                            }
                            break;
                        }
                    }
                    else
                    {
                        var pipe = FindClosestPipe(freeSpot.Key);
                        if (pipe.houseSpawnAuthorized)
                        {
                            var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                            structureDictionary.Add(freeSpot.Key, building);
                        }                        
                    }
                    break;
                }
            }
        }
    }

    private bool VerifyIfBuildingFits(
        int halfSizeLength, 
        int halfSizeWidth,
        Dictionary<Vector3Int, Direction> freeEstateSpots, 
        KeyValuePair<Vector3Int, Direction> freeSpot, 
        List<Vector3Int> blockedPositions,
        ref List<Vector3Int> tempPositionsBlocked)
    {
        Vector3Int direction = Vector3Int.zero;
        if(freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            direction = Vector3Int.right;
        }
        else
        {
            direction = Vector3Int.forward;
        }
        for (int i = 1; i <= halfSizeLength; i++)
        {
            var pos1 = freeSpot.Key + direction * i;
            var pos2 = freeSpot.Key - direction * i;
            if(!freeEstateSpots.ContainsKey(pos1) || !freeEstateSpots.ContainsKey(pos2) || 
                blockedPositions.Contains(pos1) || blockedPositions.Contains(pos2))
            {
                return false;
            }
            tempPositionsBlocked.Add(pos1);
            tempPositionsBlocked.Add(pos2);
        }
        if(freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            direction = Vector3Int.forward;
        }
        else
        {
            direction = Vector3Int.right;
        }
        for (int i = 1; i <= halfSizeWidth; i++)
        {
            var pos1 = freeSpot.Key + direction * i;
            var pos2 = freeSpot.Key - direction * i;
            if(blockedPositions.Contains(pos1) || blockedPositions.Contains(pos2))
            {
                return false;
            }
            tempPositionsBlocked.Add(pos1);
            tempPositionsBlocked.Add(pos2);
        }
        return true;
    }

    private GameObject SpawnPrefab(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        var newStructure = Instantiate(prefab, position, rotation, transform);
        return newStructure;
    }

    public Pipe FindClosestPipe(Vector3 position)
    {
        List<Pipe> pipes = pipeManager.GetPipes();
        if(pipes.Count == 0) return null;
        // This orders the list so the closest object will be the very first entry
        var sorted = pipes.OrderBy(obj => (position - transform.position).sqrMagnitude);
        Pipe closest = sorted.First();
        return closest;
    }

    private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
            var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if(neighbourDirections.Contains(direction) == false)
                {
                    var newPosition = position + DirectionHelper.GetOffsetFromDirection(direction);
                    if(freeSpaces.ContainsKey(newPosition) || structureDictionary.ContainsKey(newPosition))
                    {
                        continue;
                    }
                    freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction));
                }
            }
        }
        return freeSpaces;
    }
}
