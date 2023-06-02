using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrbanGenerationManager : MonoBehaviour
{
    // Start is called before the first frame update

    public LSystemGenerator lsystem;
    public PipeManager pipeManager;
    public RoadHelper roadHelper;
    public StructureHelper structureHelper;

    public List<Direction> blockedDirections;

    public void SetVisualizerAttributes(Visualizer visualizer)
    {
        visualizer.lsystem = lsystem;
        visualizer.pipeManager = pipeManager;
        visualizer.roadHelper = roadHelper;
        visualizer.structureHelper = structureHelper;
        visualizer.blockedDirections = blockedDirections;
    }
}
