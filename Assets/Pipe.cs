using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PipeData;
using static Point;

public class Pipe : MonoBehaviour
{
    public int maxVolume;
    public int effectiveVolume;
    public List<Point> points = new List<Point>();
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
        foreach(Point point in this.points){
            foreach(Pipe pipe in point.pipes){
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
        this.points = data.points;
    }

    public PipeData ExportData() {
        return new PipeData(this.maxVolume, this.effectiveVolume, this.points, this.gameObject.GetInstanceID());
    }
}
