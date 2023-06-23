using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpurationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject epurationStation;
    private List<GameObject> epurationStations = new List<GameObject>();
    public bool mouseClick = false;

    public ClockManager clock;
    public int epurationStationPrice = 10;

    // Update is called once per frame
    void Update()
    {
        if(this.mouseClick) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    this.CreateEpurationStation(hit);
                }
            }
        }
    }

    public void flipMouseClick() {
        this.mouseClick = !this.mouseClick;
    }

    public void CreateEpurationStation(RaycastHit ray) {
        if(clock.money >= epurationStationPrice){
            Vector3 position = ray.point;
            GameObject epurationObject = (GameObject) Instantiate (epurationStation, position, Quaternion.Euler (0, 0, 0), transform);
            epurationStations.Add(epurationObject);
            clock.money -= epurationStationPrice;
        }
        this.flipMouseClick();
    }

    public List<Epuration> GetEpurationPlants() {
        List<Epuration> epurationPlants = new List<Epuration>();
        foreach (var epurationPlant in epurationStations)
        {
            epurationPlants.Add(epurationPlant.GetComponent<Epuration>());
        }
        return epurationPlants;
    }
}
