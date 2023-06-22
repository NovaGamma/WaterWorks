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
    public int intervalSpawn = 1;
    public float spawnTimer = 2.0f;
    private float time = 0.0f;

    private void Update() {
        if(pipeManager.modifiedPipes)
        {
            Debug.Log("Updated pipes");
            pipeManager.modifiedPipes = false;
            Invoke("UpdateAllPipes", 1f);
        }
    }

    public void PlaceStructureAroundRoad()
    {
        List<Vector3Int> roadPositions = roadHelper.GetRoadPositions();
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);
        List<Vector3Int> blockedPositions = new List<Vector3Int>();
        foreach (var freeSpot in freeEstateSpots)
        {
            if(blockedPositions.Contains(freeSpot.Key) || !PlacementHelper.CheckIfPositionAvailable(freeSpot.Key))
            {
                continue;
            }
            Pipe pipe = FindClosestPipe(freeSpot.Key);
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
                Debug.Log("Check building type " + i);
                if(buildingTypes[i].IsBuildingAvailable())
                {
                    if (buildingTypes[i].sizeRequired.x > 1)
                    {
                        var halfSizeLength = Mathf.FloorToInt(buildingTypes[i].sizeRequired.x / 2.0f);
                        var halfSizeWidth = Mathf.FloorToInt(buildingTypes[i].sizeRequired.y / 2.0f);
                        List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();
                        if(VerifyIfBuildingFits(halfSizeLength, halfSizeWidth, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked))
                        {
                            if (pipe.houseSpawnAuthorized)
                            {
                                var building = SpawnPrefab(buildingTypes[i].GetPrefab(), 
                                                            freeSpot.Key - DirectionHelper.GetOffsetFromDirection(freeSpot.Value) * halfSizeWidth, 
                                                            rotation);
                                building.GetComponent<House>().pipe = pipe;
                                structureDictionary.Add(freeSpot.Key, building);
                                blockedPositions.AddRange(tempPositionsBlocked);
                                foreach(var pos in tempPositionsBlocked)
                                {
                                    if(!structureDictionary.ContainsKey(pos)){
                                        structureDictionary.Add(pos, building);
                                    }
                                }
                                return;
                            }
                        }
                        break;
                    }
                    else
                    {
                        if (pipe.houseSpawnAuthorized)
                        {
                            var building = SpawnPrefab(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation);
                            House house = building.GetComponent<House>();
                            house.pipe = pipe;
                            house.dirtyPipe = FindClosestDirtyPipe(freeSpot.Key);
                            structureDictionary.Add(freeSpot.Key, building);
                            return;
                        }         
                        break;               
                    }
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
        return pipes.OrderBy(pipe => Vector3.Distance(pipe.transform.position, position)).FirstOrDefault();;
    }

    public Pipe FindClosestDirtyPipe(Vector3 position)
    {
        List<Pipe> pipes = pipeManager.GetDirtyPipes();
        if(pipes.Count == 0) return null;
        return pipes.OrderBy(pipe => Vector3.Distance(pipe.transform.position, position)).FirstOrDefault();;
    }

    public void UpdateAllPipes()
    {
        foreach (var structure in structureDictionary)
        {
            House house = structure.Value.GetComponent<House>();
            house.pipe = FindClosestPipe(structure.Key);
            house.dirtyPipe = FindClosestDirtyPipe(structure.Key);
        }
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

    public List<House> GetHouses()
    {
        HashSet<House> uniqueHouses = new HashSet<House>();
        foreach (var pair in structureDictionary)
        {
            if (!uniqueHouses.Contains(pair.Value.GetComponent<House>()))
            {
                uniqueHouses.Add(pair.Value.GetComponent<House>());
            }
        }
        return uniqueHouses.ToList<House>();
    }
}
