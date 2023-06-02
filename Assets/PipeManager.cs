using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PipeManager : MonoBehaviour
{
    public GameObject pipe;
    public List<Pipe> pipes;
    public GameObject intersection;
    public bool mouseClick = false;
    public RaycastHit click1;
    public bool firstClick = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.mouseClick) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    if(!this.firstClick) {
                        this.click1 = hit;
                        this.firstClick = true;
                    } else {
                        this.CreatePipe(this.click1, hit);
                        this.mouseClick = false;
                        this.firstClick = false;
                    }
                }
            }
        }
    }

    public void flipMouseClick() {
        this.mouseClick = !this.mouseClick;
    }

    public void CreatePipe(RaycastHit fromRay, RaycastHit toRay) {
        Vector3 from = fromRay.point;
        Vector3 to = toRay.point;
        GameObject intersectionFrom;
        GameObject intersectionTo;
        if (fromRay.collider.gameObject.tag == "Intersection") {
            intersectionFrom = fromRay.collider.gameObject;
        } else if (fromRay.collider.gameObject.tag == "Pipe") {
            intersectionFrom = SplitPipe(from, fromRay.collider.gameObject);
        } else {
            intersectionFrom = (GameObject) Instantiate (intersection, from, Quaternion.Euler (0,0,0), transform);
        }

        if (toRay.collider.gameObject.tag == "Intersection") {
            intersectionTo = toRay.collider.gameObject;
        } else if (toRay.collider.gameObject.tag == "Pipe") {
            intersectionTo = SplitPipe(to, toRay.collider.gameObject);
        } else {
            intersectionTo = (GameObject) Instantiate (intersection, to, Quaternion.Euler (0,0,0), transform);
        }

        InstantiatePipe(intersectionFrom, intersectionTo);
    }

    public GameObject InstantiatePipe(GameObject intersectionFrom, GameObject intersectionTo) {
        Vector3 I1Center = intersectionFrom.transform.GetComponent<Collider>().bounds.center;
        Vector3 I2Center = intersectionTo.transform.GetComponent<Collider>().bounds.center;

        Vector3 AB = new Vector3(I1Center.x - I2Center.x, I1Center.y - I2Center.y, I1Center.z - I2Center.z);
        Debug.Log(AB);
        float alignment;
        if(AB.x < 0) {
            alignment = Vector3.Angle(AB, new Vector3(0,0,-1)) + 90;
        } else {
            AB = new Vector3(I2Center.x - I1Center.x, I2Center.y - I1Center.y, I2Center.z - I1Center.z);
            alignment = Vector3.Angle(AB, new Vector3(0,0,-1)) + 90;
        }
        Debug.Log(alignment);
        GameObject newPipe = (GameObject) Instantiate (pipe, Vector3.Lerp(I1Center, I2Center, 0.5f), Quaternion.Euler (0, alignment, 90), transform);
        newPipe.transform.localScale += new Vector3(0, 1, 0) * Vector3.Distance(I1Center, I2Center) / 2;
        newPipe.GetComponent<Pipe>().intersections.Add(intersectionFrom.GetComponent<Intersection>());
        newPipe.GetComponent<Pipe>().intersections.Add(intersectionTo.GetComponent<Intersection>());
        intersectionFrom.gameObject.GetComponent<Intersection>().pipes.Add(newPipe.GetComponent<Pipe>());
        intersectionTo.gameObject.GetComponent<Intersection>().pipes.Add(newPipe.GetComponent<Pipe>());
        pipes.Add(newPipe.GetComponent<Pipe>());
        return newPipe;
    }

    public GameObject SplitPipe(Vector3 position, GameObject pipe) {
        GameObject newIntersection = (GameObject) Instantiate (intersection, position, Quaternion.Euler (0,0,0), transform);
        Pipe pipeObject = pipe.GetComponent<Pipe>();
        List<Intersection> intersections = pipeObject.intersections;
        GameObject splitPipe1 = InstantiatePipe(newIntersection, intersections[0].gameObject);
        GameObject splitPipe2 = InstantiatePipe(newIntersection, intersections[1].gameObject);
        newIntersection.gameObject.GetComponent<Intersection>().pipes.Add(splitPipe1.GetComponent<Pipe>());
        newIntersection.gameObject.GetComponent<Intersection>().pipes.Add(splitPipe2.GetComponent<Pipe>());
        splitPipe1.GetComponent<Pipe>().effectiveVolume = pipeObject.effectiveVolume;
        splitPipe2.GetComponent<Pipe>().effectiveVolume = pipeObject.effectiveVolume;
        pipes.Remove(pipeObject);
        foreach(Intersection intersection in intersections) {
            intersection.pipes.Remove(pipeObject);
        }
        Destroy(pipe);
        return newIntersection;
    }

    public List<Pipe> GetPipes()
    {
        return pipes;
    }

    public bool CheckPositionIsInsideCollider(Vector3 position)
    {
        var collidersObj = gameObject.GetComponentsInChildren<BoxCollider>(true);
        for (var index = 0; index < collidersObj.Length; index++)
        {
            var colliderItem = collidersObj[index];
            if(!colliderItem.enabled){
                colliderItem.enabled = true;
                if(colliderItem.bounds.Contains(position))
                {
                    colliderItem.enabled = false;
                    return true;
                }else{
                    colliderItem.enabled = false;
                }
                }
        }
        return false;
    }
}
