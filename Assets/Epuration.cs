using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epuration : MonoBehaviour
{
    public Intersection intersection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int toRemove = 20 / intersection.pipes.Count;
        foreach (Pipe pipe in intersection.pipes) {
            if(pipe.effectiveVolume >= toRemove) {
                pipe.effectiveVolume -= toRemove;
            } else {
                pipe.effectiveVolume = 0;
            }
        }
    }
}
