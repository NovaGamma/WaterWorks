using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PipeData : MonoBehaviour
{
    public int maxVolume;
    public int effectiveVolume;
    public List<Point> points = new List<Point>();
    public int id;

    public PipeData(int maxVolume, int effectiveVolume, List<Point> points, int id) {
        this.maxVolume = maxVolume;
        this.effectiveVolume = effectiveVolume;
        this.points = points;
        this.id = id;
    }
}
