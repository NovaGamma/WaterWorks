using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Pipe;
using static PipeData;
using static Intersection;

public class WaterManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PipeManager pipeManager;
    List<Pipe> pipes;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pipes = pipeManager.pipes;
        for(int i=0; i<10; i++){
            this.SmoothingSystem();
        }
    }

    public int OverflowSystem(int addedQuantity, Intersection origin, Pipe pipe) {
        if (pipe.effectiveVolume + addedQuantity <= pipe.maxVolume) {
            pipe.effectiveVolume += addedQuantity;
            return 0;
        } else if (pipe.effectiveVolume < pipe.maxVolume) {
            addedQuantity = pipe.effectiveVolume + addedQuantity % pipe.maxVolume;
            pipe.effectiveVolume = pipe.maxVolume;
            if (addedQuantity == 0) {
                return 0;
            }
        }
        Intersection otherIntersection = pipe.intersections[0] == origin ? pipe.intersections[0] : pipe.intersections[1];
        List<Pipe> neighbors =  (List<Pipe>) (from p in otherIntersection.pipes where p != pipe select p);
        while (addedQuantity > 0 && neighbors.Count() > 0){
            List<(Pipe, int)> map = new List<(Pipe, int)>();
            foreach (Pipe neighbor in neighbors) {
                map.Add((neighbor, OverflowSystem(addedQuantity / neighbors.Count(), otherIntersection, neighbor)));
            }
            neighbors = (List<Pipe>) (from data in map where data.Item2 == 0 select data.Item1);
            int remainingQuantity = 0;
            foreach ((Pipe neighbor, int remaining) in map) {
                remainingQuantity += remaining;
            }
            addedQuantity = remainingQuantity;
        }
        return addedQuantity;
    }

    void SmoothingSystem() {
        List<PipeData> copy = this.CopyPipesData();
        foreach(Pipe pipe in pipes) {
            List<Pipe> neighbors = pipe.GetNeighbors();
            foreach(Pipe neighbor in neighbors){
                Intersection selfDifferentPoint = pipe.intersections.Except(neighbor.intersections).First();
                Intersection neighborDifferentPoint = neighbor.intersections.Except(pipe.intersections).First();
                int heightCoefficent = (int) neighborDifferentPoint.gameObject.transform.position.y - (int) selfDifferentPoint.gameObject.transform.position.y;
                if (pipe.effectiveVolume > neighbor.effectiveVolume && heightCoefficent == 0){
                    int result = (pipe.effectiveVolume - neighbor.effectiveVolume) / (neighbors.Count() + 1);
                    foreach (PipeData p in copy) {
                        if(p.id == pipe.gameObject.GetInstanceID()) {
                            p.effectiveVolume -= result;
                        }
                        if(p.id == neighbor.gameObject.GetInstanceID()) {
                            p.effectiveVolume += result;
                        }
                    }
                }
                if (heightCoefficent <0) {
                    if (neighbor.effectiveVolume + pipe.effectiveVolume > neighbor.maxVolume) {
                        foreach (PipeData p in copy) {
                            if(p.id == pipe.gameObject.GetInstanceID()) {
                                p.effectiveVolume -= neighbor.maxVolume - neighbor.effectiveVolume;
                            }
                            if(p.id == neighbor.gameObject.GetInstanceID()) {
                                p.effectiveVolume = neighbor.maxVolume;
                            }
                        }
                    } else {
                        foreach (PipeData p in copy) {
                            if(p.id == pipe.gameObject.GetInstanceID()) {
                                p.effectiveVolume = 0;
                            }
                            if(p.id == neighbor.gameObject.GetInstanceID()) {
                                p.effectiveVolume += pipe.effectiveVolume;
                            }
                        }
                    }
                }
            }
        }
        foreach (PipeData p in copy) {
            foreach (Pipe pipe in pipes) {
                if (p.id == pipe.gameObject.GetInstanceID()) {
                    pipe.ImportData(p);
                    break;
                }
            }
        }
    }

    List<PipeData> CopyPipesData() {
        List<PipeData> datas = new List<PipeData>();
        foreach(Pipe pipe in pipes) {
            datas.Add(pipe.ExportData());
        }
        return datas;
    }
}
