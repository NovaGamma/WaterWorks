using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pump;
    public GameObject intersection;
    public bool mouseClick = false;
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
                    this.CreatePump(hit);
                }
            }
        }
    }

    public void flipMouseClick() {
        this.mouseClick = !this.mouseClick;
    }

    public void CreatePump(RaycastHit ray) {
        Vector3 position = ray.point;
        GameObject intersectionObject = (GameObject) Instantiate (intersection, position + new Vector3(0, 0, 0.5f), Quaternion.Euler (0,0,0), transform);
        GameObject pumpObject = (GameObject) Instantiate (pump, position, Quaternion.Euler (0, 0, 0), transform);
        pumpObject.GetComponent<Pump>().intersection = intersectionObject.GetComponent<Intersection>();
        flipMouseClick();
    }
}
