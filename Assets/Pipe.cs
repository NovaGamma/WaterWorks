using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PipeData;
using static Intersection;

public class Pipe : MonoBehaviour
{
    public int maxVolume;
    public int effectiveVolume;
    public int spawnVolumeLimit = 10;
    private float spawnTimer;
    private float spawnConditionTimeLimit = 1.0f;
    public List<Intersection> intersections = new List<Intersection>();
    public Vector3 Position 
    { 
        get => transform.position; 
    }

    public bool houseSpawnAuthorized = true;

    // Start is called before the first frame update
    public Pipe(){
        
    }
    void Start()
    {
        BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(30f, 2f, 30f);
        boxCollider.enabled = false;
        this.maxVolume = 100;
    }

    // Update is called once per frame
    void Update()
    {
        // Verify if you can spawn house or not depending on effectiveVolume
        if ((houseSpawnAuthorized && effectiveVolume > spawnVolumeLimit) || (!houseSpawnAuthorized && effectiveVolume < spawnVolumeLimit))
        {
            spawnTimer = 0.0f;
            return;
        }

        spawnTimer += Time.deltaTime;
        // If after spawnConditionTimeLimit seconds, the condition above isn't fullfilled, we switch the authorized boolean.
        if (spawnTimer > spawnConditionTimeLimit)
        {
            houseSpawnAuthorized = InverseBoolean(houseSpawnAuthorized);
            spawnTimer = spawnTimer - spawnConditionTimeLimit; // reset timer
        }
    }

    private bool InverseBoolean(bool boolVariable) { return !boolVariable; }

    public List<Pipe> GetNeighbors() {
        List<Pipe> neighbors = new List<Pipe>();
        foreach(Intersection intersection in this.intersections){
            foreach(Pipe pipe in intersection.pipes){
                if(!pipe.Equals(this)){
                    neighbors.Add(pipe);
                }
            }
        }
        return neighbors;
    }

    public void ImportData(PipeData data) {
        this.maxVolume = data.maxVolume;
        this.effectiveVolume = data.effectiveVolume;
        this.intersections = data.intersections;
    }

    public PipeData ExportData() {
        return new PipeData(this.maxVolume, this.effectiveVolume, this.intersections, this.gameObject.GetInstanceID());
    }
}
