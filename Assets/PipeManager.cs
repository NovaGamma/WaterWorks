using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class PipeManager : MonoBehaviour
{
    public GameObject pipe;
    public GameObject dirtyPipe;
    public List<Pipe> pipes;
    public List<Pipe> dirtyPipes;
    public GameObject intersection;
    public bool mouseClick = false;
    public RaycastHit click1;
    public bool firstClick = false;
    public String type;
    public bool modifiedPipes = false;
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
                        if(this.type == "Pipe") {
                            this.CreatePipe(this.click1, hit);
                        } else {
                            this.CreateDirtyPipe(this.click1, hit);
                        }
                        this.modifiedPipes = true;
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

    public void activatePipe() {
        this.flipMouseClick();
        this.type = "Pipe";
    }

    public void activateDirtyPipe() {
        this.flipMouseClick();
        this.type = "DirtyPipe";
    }

    public void CreatePipe(RaycastHit fromRay, RaycastHit toRay) {
        Vector3 from = fromRay.point;
        Vector3 to = toRay.point;
        GameObject intersectionFrom;
        GameObject intersectionTo;

        switch(fromRay.collider.gameObject.tag) {
            case "EpurationStation":
                return;
            case "Pump":
                intersectionFrom = fromRay.collider.gameObject.GetComponent<Pump>().intersection.gameObject;
                break;
            case "Pipe":
                intersectionFrom = SplitPipe(from, fromRay.collider.gameObject);
                break;
            case "Intersection":
                intersectionFrom = fromRay.collider.gameObject;
                break;
            case "DirtyPipe":
                return;
            default:
                intersectionFrom = (GameObject) Instantiate (intersection, from, Quaternion.Euler (0,0,0), transform);
                break;
        }

        switch(toRay.collider.gameObject.tag) {
            case "EpurationStation":
                return;
            case "Pump":
                intersectionTo = toRay.collider.gameObject.GetComponent<Intersection>().gameObject;
                break;
            case "Pipe":
                intersectionTo = SplitPipe(to, toRay.collider.gameObject);
                break;
            case "Intersection":
                intersectionTo = toRay.collider.gameObject;
                break;
            case "DirtyPipe":
                return;
            default:
                intersectionTo = (GameObject) Instantiate (intersection, to, Quaternion.Euler (0,0,0), transform);
                break;
        }

        InstantiatePipe(intersectionFrom, intersectionTo);
    }

    public void CreateDirtyPipe(RaycastHit fromRay, RaycastHit toRay) {
        Vector3 from = fromRay.point;
        Vector3 to = toRay.point;
        GameObject intersectionFrom;
        GameObject intersectionTo;
        switch(fromRay.collider.gameObject.tag) {
            case "EpurationStation":
                intersectionFrom = fromRay.collider.gameObject.GetComponent<Epuration>().intersection.gameObject;
                break;
            case "Pump":
                return;
            case "Pipe":
                return;
            case "Intersection":
                intersectionFrom = fromRay.collider.gameObject;
                break;
            case "DirtyPipe":
                intersectionFrom = SplitDirtyPipe(from, fromRay.collider.gameObject);
                break;
            default:
                intersectionFrom = (GameObject) Instantiate (intersection, from, Quaternion.Euler (0,0,0), transform);
                break;
        }

        switch(toRay.collider.gameObject.tag) {
            case "EpurationStation":
                intersectionTo = toRay.collider.gameObject.GetComponent<Epuration>().gameObject;
                break;
            case "Pump":
                return;
            case "Pipe":
                return;
            case "Intersection":
                intersectionTo = toRay.collider.gameObject;
                break;
            case "DirtyPipe":
                intersectionTo = SplitPipe(to, toRay.collider.gameObject);
                break;
            default:
                intersectionTo = (GameObject) Instantiate (intersection, to, Quaternion.Euler (0,0,0), transform);
                break;
        }

        InstantiateDirtyPipe(intersectionFrom, intersectionTo);
    }

    public GameObject InstantiateDirtyPipe(GameObject intersectionFrom, GameObject intersectionTo) {
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
        GameObject newPipe = (GameObject) Instantiate (dirtyPipe, Vector3.Lerp(I1Center, I2Center, 0.5f), Quaternion.Euler (0, alignment, 90), transform);
        newPipe.transform.localScale += new Vector3(0, 1, 0) * Vector3.Distance(I1Center, I2Center) / 2;
        newPipe.GetComponent<Pipe>().intersections.Add(intersectionFrom.GetComponent<Intersection>());
        newPipe.GetComponent<Pipe>().intersections.Add(intersectionTo.GetComponent<Intersection>());
        intersectionFrom.gameObject.GetComponent<Intersection>().pipes.Add(newPipe.GetComponent<Pipe>());
        intersectionTo.gameObject.GetComponent<Intersection>().pipes.Add(newPipe.GetComponent<Pipe>());
        dirtyPipes.Add(newPipe.GetComponent<Pipe>());
        return newPipe;
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

    public GameObject SplitDirtyPipe(Vector3 position, GameObject pipe) {
        GameObject newIntersection = (GameObject) Instantiate (intersection, position, Quaternion.Euler (0,0,0), transform);
        Pipe pipeObject = pipe.GetComponent<Pipe>();
        List<Intersection> intersections = pipeObject.intersections;
        GameObject splitPipe1 = InstantiateDirtyPipe(newIntersection, intersections[0].gameObject);
        GameObject splitPipe2 = InstantiateDirtyPipe(newIntersection, intersections[1].gameObject);
        newIntersection.gameObject.GetComponent<Intersection>().pipes.Add(splitPipe1.GetComponent<Pipe>());
        newIntersection.gameObject.GetComponent<Intersection>().pipes.Add(splitPipe2.GetComponent<Pipe>());
        splitPipe1.GetComponent<Pipe>().effectiveVolume = pipeObject.effectiveVolume;
        splitPipe2.GetComponent<Pipe>().effectiveVolume = pipeObject.effectiveVolume;
        dirtyPipes.Remove(pipeObject);
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

    public List<Pipe> GetDirtyPipes()
    {
        return dirtyPipes;
    }

    public bool CheckPositionIsInsideCollider(Vector3 position)
    {
        List<BoxCollider> boxCollidersList = pipes
        .SelectMany(go => go.GetComponentsInChildren<BoxCollider>(true))
        .ToList();
        foreach (var colliderItem in boxCollidersList)
        {
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
