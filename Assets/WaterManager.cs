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
    List<Pipe> dirtyPipes;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pipes = pipeManager.pipes;
        for(int i=0; i<10; i++){
            this.SmoothingSystem(pipes);
        }

        dirtyPipes = pipeManager.dirtyPipes;
        for(int i=0; i<10; i++){
            this.SmoothingSystem(dirtyPipes);
        }
    }

    void SmoothingSystem(List<Pipe> pipes) {
        List<PipeData> copy = this.CopyPipesData(pipes);
        foreach(Pipe pipe in pipes) {
            List<Pipe> neighbors = pipe.GetNeighbors();
            foreach(Pipe neighbor in neighbors){
                Intersection selfDifferentPoint = pipe.intersections.Except(neighbor.intersections).First();
                Intersection neighborDifferentPoint = neighbor.intersections.Except(pipe.intersections).First();
                int heightCoefficent = (int) neighborDifferentPoint.gameObject.transform.position.y - (int) selfDifferentPoint.gameObject.transform.position.y;
                if (pipe.effectiveVolume > neighbor.effectiveVolume && heightCoefficent == 0){
                    int result = (pipe.effectiveVolume - neighbor.effectiveVolume) / (neighbors.Count() + 1);
                    if (neighbor.effectiveVolume + result > neighbor.maxVolume) {
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
                                p.effectiveVolume -= result;
                            }
                            if(p.id == neighbor.gameObject.GetInstanceID()) {
                                p.effectiveVolume += result;
                            }
                        }
                    }
                }
                if (heightCoefficent < 0) {
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

    List<PipeData> CopyPipesData(List<Pipe> pipes) {
        List<PipeData> datas = new List<PipeData>();
        foreach(Pipe pipe in pipes) {
            datas.Add(pipe.ExportData());
        }
        return datas;
    }
}
