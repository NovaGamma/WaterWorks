using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;
    public PipeManager pipeManager;
    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    public List<Direction> blockedDirections;

    private int length = 5;
    private float angle = 90;
    private int maxSpawnLength;

    public Vector3 startingPosition;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }
        set => length = value;
    }

    private void Start()
    {
        maxSpawnLength = Length * 2 * lsystem.iterationLimit;
    }

    private void Update() {
        int randomIndex = UnityEngine.Random.Range(1, 4);
        var sequence = lsystem.GenerateSentence();
        if(randomIndex == 1 && !blockedDirections.Contains(Direction.Up)){
            CheckAndVisualize(sequence, startingPosition, Direction.Up);
        }
        if(randomIndex == 2 && !blockedDirections.Contains(Direction.Down)){
            CheckAndVisualize(sequence, startingPosition, Direction.Down);
        }
        if(randomIndex == 3 && !blockedDirections.Contains(Direction.Right)){
            CheckAndVisualize(sequence, startingPosition, Direction.Right);
        }
        if(randomIndex == 4 && !blockedDirections.Contains(Direction.Left)){
            CheckAndVisualize(sequence, startingPosition, Direction.Left);
        }      
    }

    private void CheckAndVisualize(string sequence, Vector3 position, Direction direction)
    {
        if(CheckIfInFreeZone(position, direction))
        {
            VisualizeSequence(sequence, position, DirectionHelper.GetOffsetFromDirection(direction));
            blockedDirections.Add(direction);
        }
    }

    private bool CheckIfInFreeZone(Vector3 position, Direction direction){
        Vector3[] offsets = CreateDirectionnalOffsets(position, maxSpawnLength, direction);
        return 
            pipeManager.CheckPositionIsInsideCollider(offsets[0]) &&
            pipeManager.CheckPositionIsInsideCollider(offsets[1]) &&
            pipeManager.CheckPositionIsInsideCollider(offsets[2]) &&
            pipeManager.CheckPositionIsInsideCollider(offsets[3]);
    }

    private Vector3[] CreateDirectionnalOffsets(Vector3 position, int length, Direction direction)
    {
        Vector3[] offsets = new Vector3[4];
        switch (direction)
        {
             case Direction.Left:
                offsets[0] = position + new Vector3(-length, 0f, -length);
                offsets[1] = position + new Vector3(-length, 0f, length);
                offsets[2] = position + new Vector3(0f, 0f, -length);
                offsets[3] = position + new Vector3(0f, 0f, length);
                break;

            case Direction.Right:
                offsets[0] = position + new Vector3(length, 0f, -length);
                offsets[1] = position + new Vector3(length, 0f, length);
                offsets[2] = position + new Vector3(0f, 0f, -length);
                offsets[3] = position + new Vector3(0f, 0f, length);
                break;

            case Direction.Down:
                offsets[0] = position + new Vector3(-length, 0f, 0f);
                offsets[1] = position + new Vector3(length, 0f, 0f);
                offsets[2] = position + new Vector3(length, 0f, -length);
                offsets[3] = position + new Vector3(-length, 0f, -length);
                break;

            case Direction.Up:
                offsets[0] = position + new Vector3(-length, 0f, 0f);
                offsets[1] = position + new Vector3(length, 0f, 0f);
                offsets[2] = position + new Vector3(length, 0f, length);
                offsets[3] = position + new Vector3(-length, 0f, length);
                break;
            default:
                Debug.LogError("Invalid direction specified.");
                break;
        }
        return offsets;
    }

    private void VisualizeSequence(string sequence, Vector3 currentPosition, Vector3 direction)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        Vector3 tempPosition = Vector3.zero;
        bool exitLoop = false;

        foreach (var letter in sequence)
        {
            EncodingLetters encoding = (EncodingLetters)letter;
            switch (encoding)
            {
                case EncodingLetters.save:
                    savePoints.Push(new AgentParameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case EncodingLetters.load:
                    if (savePoints.Count > 0)
                    {
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("Dont have saved point in our stack");
                    }
                    break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * length;
                    for (int i = 0; i < length; i++)
                    {
                        var position = Vector3Int.RoundToInt(tempPosition + Vector3Int.RoundToInt(direction) *i);
                        if(!PlacementHelper.CheckIfPositionAvailable(position))
                        {
                            exitLoop = true;
                        }
                    }
                    roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length, ref structureHelper.structureDictionary);
                    break;
                case EncodingLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction;
                    break;
                case EncodingLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction;
                    break;
                default:
                    break;
            }
            if (exitLoop) break;
        }
        roadHelper.FixRoad(lsystem, roadHelper, structureHelper, pipeManager);
        structureHelper.PlaceStructureAroundRoad(roadHelper.GetRoadPositions());
    }

    public enum EncodingLetters
    {
        unknown = '1',
        save = '[',
        load = ']',
        draw = 'F',
        turnRight = '+',
        turnLeft = '-'
    }
}

