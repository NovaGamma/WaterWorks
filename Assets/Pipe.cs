using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PipeData;
using static Intersection;

public class Pipe : MonoBehaviour
{
    public int maxVolume;
    public int effectiveVolume;
    public List<Intersection> intersections = new List<Intersection>();
    // Start is called before the first frame update
    public Pipe(){

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
