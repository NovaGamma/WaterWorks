using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    public Intersection intersection;

    public void ProduceWater() {
        List<Pipe> pipes = intersection.pipes;
        if(pipes.Count == 1){
            OverflowSystem(20, intersection, pipes[0]);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        List<Pipe> pipes = intersection.pipes;
        if(pipes.Count == 1){
            OverflowSystem(10, intersection, pipes[0]);
        }
    }*/

    public int OverflowSystem(int addedQuantity, Intersection origin, Pipe pipe) {
        Debug.Log(pipe.GetInstanceID());
        if (pipe.effectiveVolume + addedQuantity <= pipe.maxVolume) {
            pipe.effectiveVolume += addedQuantity;
            Debug.Log("Used all added water " + pipe);
            return 0;
        } else if (pipe.effectiveVolume < pipe.maxVolume) {
            addedQuantity = (pipe.effectiveVolume + addedQuantity) % pipe.maxVolume;
            pipe.effectiveVolume = pipe.maxVolume;
            if (addedQuantity == 0) {
                Debug.Log("Filled "+ pipe);
                return 0;
            }
        } else {
            addedQuantity -= pipe.maxVolume - pipe.effectiveVolume;
            pipe.effectiveVolume = pipe.maxVolume;
        }
        Intersection otherIntersection = pipe.intersections[0].GetInstanceID() == origin.GetInstanceID() ? pipe.intersections[1] : pipe.intersections[0];
        List<Pipe> neighbors = new List<Pipe>();
        foreach(Pipe p in otherIntersection.pipes){
            if(p.GetInstanceID() != pipe.GetInstanceID()) {
                neighbors.Add(p);
            }
        }
        List<(Pipe, int)> map = new List<(Pipe, int)>();
        foreach (Pipe neighbor in neighbors) {
            map.Add((neighbor, OverflowSystem(addedQuantity / neighbors.Count(), otherIntersection, neighbor)));
            Debug.Log(map);
        }
        foreach((Pipe, int) data in map) {
            if(data.Item2 == 0) {
                neighbors.Add(data.Item1);
            }
        }
        int remainingQuantity = 0;
        foreach ((Pipe neighbor, int remaining) in map) {
            remainingQuantity += remaining;
        }
        addedQuantity = remainingQuantity;
        return addedQuantity;
    }
}
