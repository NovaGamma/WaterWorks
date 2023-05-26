using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PipeData
{
    public int maxVolume;
    public int effectiveVolume;
    public List<Intersection> intersections = new List<Intersection>();
    public int id;

    public PipeData(int maxVolume, int effectiveVolume, List<Intersection> intersections, int id) {
        this.maxVolume = maxVolume;
        this.effectiveVolume = effectiveVolume;
        this.intersections = intersections;
        this.id = id;
    }
}
