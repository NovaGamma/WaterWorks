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
    public List<Intersection> intersections = new List<Intersection>();
    public Vector3 Position 
    { 
        get => transform.position; 
    }

    public bool houseSpawnAuthorized = false;

    // Start is called before the first frame update
    public Pipe(){
        
    }
    void Start()
    {
        BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(30f, 2f, 110f);
        boxCollider.enabled = false;
        this.maxVolume = 100;
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

    // Return False if it couldn't consume
    public bool ConsumeWater(int Amount)
    {
        if(effectiveVolume - Amount > 0)
        {
            effectiveVolume -= Amount;
            return true;
        }else
        {
            return false;
        }
    }

    public bool AddWater(int Amount)
    {
        if(effectiveVolume + Amount <= maxVolume)
        {
            effectiveVolume += Amount;
            return true;
        }else
        {
            return false;
        }
    }
}
