using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;
    List<Vector3> positions = new List<Vector3>();

    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    private int length = 10;
    private float angle = 90;

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
        
    }

    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                var sequence = lsystem.GenerateSentence();
                length = 10;
                VisualizeSequence(sequence, hit.point);
            }
        }
    }

    private void VisualizeSequence(string sequence, Vector3 currentPosition)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        Vector3 direction = Vector3.forward;
        Vector3 tempPosition = Vector3.zero;

        positions.Add(currentPosition);

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
                    roadHelper.PlaceStreetPositions(tempPosition, Vector3Int.RoundToInt(direction), length, ref structureHelper.structureDictionary);
                    Length -= 2;
                    positions.Add(currentPosition);
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
        }
        roadHelper.FixRoad();
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

