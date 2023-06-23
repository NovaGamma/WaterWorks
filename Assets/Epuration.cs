using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epuration : MonoBehaviour
{
    public Intersection intersection;

    public void cleanWater() {
        int toRemove = 0;
        if(intersection.pipes.Count != 0) toRemove = 20 / intersection.pipes.Count;
        foreach (Pipe pipe in intersection.pipes) {
            if(pipe.effectiveVolume >= toRemove) {
                pipe.effectiveVolume -= toRemove;
            } else {
                pipe.effectiveVolume = 0;
            }
        }
    }
}
