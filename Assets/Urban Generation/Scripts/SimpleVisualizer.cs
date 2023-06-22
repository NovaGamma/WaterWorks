using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVisualizer : MonoBehaviour
{
    public LSystemGenerator lsystem;
    public PipeManager pipeManager;
    List<Vector3> positions = new List<Vector3>();
    public GameObject prefab;
    public Material lineMaterial;

    private int length = 8;
    private int maxSpawnLength;
    private float angle = 90;

    public int Length 
    { 
        get
        {
            if(length > 0){
                return length;
            }else{
                return 1;
            }
        } 
        set => length = value; 
    }

    private void Start() 
    {
        maxSpawnLength = length * 2 * lsystem.iterationLimit;
        var currentPosition = Vector3.zero;
        positions.Add(currentPosition);
    }

    private void Update() {
        var sequence = lsystem.GenerateSentence();
        Debug.Log("Vérification position : " + positions[positions.Count - 1]);
        CheckAndVisualize(sequence, Direction.Up, Vector3.forward);
    }

    private void CheckAndVisualize(string sequence, Direction direction, Vector3 vectorDirection)
    {
        if(CheckIfInFreeZone(positions[positions.Count - 1], direction))
        {
            VisualizeSequence(sequence, positions[positions.Count - 1], vectorDirection);
        }
    }

    private bool CheckIfInFreeZone(Vector3 position, Direction direction){
        Debug.Log("Vérification point de spawn : " + position + " " + pipeManager.CheckPositionIsInsideCollider(position));
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
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>(); // Last in First out

        Vector3 tempPosition = Vector3.zero;

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
                    if(savePoints.Count > 0){
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Length = agentParameter.length;
                    }else{
                        throw new System.Exception("Don't have saved point in our stack");
                    }
                    break;
                case EncodingLetters.draw:
                    tempPosition = currentPosition;
                    currentPosition += direction * Length;
                    DrawLine(tempPosition, currentPosition, Color.red);
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

        foreach (var position in positions)
        {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("line");
        line.transform.position = start;
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
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
