using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pump;
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
        if(ray.collider.gameObject.tag != "PumpArea"){
            return;
        }
        Vector3 position = ray.point;
        GameObject pumpObject = (GameObject) Instantiate (pump, position, Quaternion.Euler (0, 0, 0), transform);
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

    public List<Pump> GetPumps()
    {
        List<Pump> pumpsList = new List<Pump>();
        foreach (var pump in pumps)
        {
            pumpsList.Add(pump.GetComponent<Pump>());
        }
        return pumpsList;
    }
}
