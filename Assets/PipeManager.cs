using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PipeManager : MonoBehaviour
{
    public GameObject pipe;
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
        } else {
            intersectionFrom = (GameObject) Instantiate (intersection, from + new Vector3(0,3,0), Quaternion.Euler (0,0,0));
            intersectionFrom.transform.parent = transform;
        }

        if (toRay.collider.gameObject.tag == "Intersection") {
            intersectionTo = toRay.collider.gameObject;
        } else {
            intersectionTo = (GameObject) Instantiate (intersection, to + new Vector3(0,3,0), Quaternion.Euler (0,0,0));
            intersectionTo.transform.parent = transform;
        }

        Vector3 I1Center = intersectionFrom.transform.GetComponent<Collider>().bounds.center;
        Vector3 I2Center = intersectionTo.transform.GetComponent<Collider>().bounds.center;

        Vector3 AB = new Vector3(I1Center.x - I2Center.x, I1Center.y - I2Center.y, I1Center.z - I2Center.z);

        float alignment = Vector3.Angle(AB, new Vector3(0,0,-1));
        if(I1Center.x < I2Center.x && I1Center.z < I2Center.z){
            Debug.Log("Bottom Left");
            alignment -= 0;
        } else if (I1Center.x < I2Center.x && I1Center.z < I2Center.z) {
            Debug.Log("Top Right");
            alignment -= 0;
        } else if(I1Center.x < I2Center.x && I1Center.z > I2Center.z){
            Debug.Log("Bottom Right");
            alignment += 0;
        } else {
            Debug.Log("Top Left");
            alignment += 0;
        }
        Debug.Log(alignment);
        GameObject newPipe = (GameObject) Instantiate (pipe, Vector3.Lerp(I1Center, I2Center, 0.5f), Quaternion.Euler (0, alignment + 90, 90));
        newPipe.transform.localScale += new Vector3(0, 1, 0) * Vector3.Distance(I1Center, I2Center) / 2;
        newPipe.transform.parent = transform;
    }
}
