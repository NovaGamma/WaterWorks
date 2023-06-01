using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    public Intersection intersection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Pipe> pipes = intersection.pipes;
        if(pipes.Count == 1){
            OverflowSystem(10, intersection, pipes[0]);
        }
    }

    public int OverflowSystem(int addedQuantity, Intersection origin, Pipe pipe) {
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
        }
        Debug.Log(addedQuantity + "water units to add");
        Intersection otherIntersection = pipe.intersections[0] == origin ? pipe.intersections[0] : pipe.intersections[1];
        List<Pipe> neighbors = new List<Pipe>();
        foreach(Pipe p in otherIntersection.pipes){
            if(p != pipe) {
                neighbors.Add(p);
            }
        }
        while (addedQuantity > 0 && neighbors.Count > 0){
            List<(Pipe, int)> map = new List<(Pipe, int)>();
            foreach (Pipe neighbor in neighbors) {
                map.Add((neighbor, OverflowSystem(addedQuantity / neighbors.Count(), otherIntersection, neighbor)));
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
        }
        return addedQuantity;
    }
}
