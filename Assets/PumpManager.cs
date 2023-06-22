using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pump;
    public GameObject intersection;
    public UrbanGenerationManager urbanGenerationManager;
    private List<GameObject> pumps = new List<GameObject>();
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
        if(ray.collider.gameObject.tag != "Water"){
            return;
        }
        Vector3 position = ray.point;
        //GameObject intersectionObject = (GameObject) Instantiate (intersection, position + new Vector3(0, 0, 0.5f), Quaternion.Euler (0,0,0), transform);
        GameObject pumpObject = (GameObject) Instantiate (pump, position, Quaternion.Euler (0, 0, 0), transform);
        //pumpObject.GetComponent<Pump>().intersection = pumpObject.GetComponentInChildren<Intersection>();
        pumps.Add(pumpObject);
        this.flipMouseClick();
        pumpObject.GetComponent<Visualizer>().startingPosition = position;
        urbanGenerationManager.SetVisualizerAttributes(pumpObject.GetComponent<Visualizer>());
    }

    public List<Vector3Int> GetPumpsPositions()
    {
        List<Vector3Int> pumpsPositions = new List<Vector3Int>();
        foreach (var pump in pumps)
        {
            pumpsPositions.Add(Vector3Int.RoundToInt(pump.transform.position));
        }
        return pumpsPositions;
    }
}
