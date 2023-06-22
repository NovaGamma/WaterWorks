using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBuildingManager : MonoBehaviour
{

    public GameObject reservoir;
    private List<GameObject> reservoirs = new List<GameObject>();
    public bool reservoirMouseClick = false;

    public int reservoirPrice = 5;

    public GameObject sewerReject;
    private List<GameObject> sewerRejects = new List<GameObject>();
    public bool sewerRejectMouseClick = false;

    public int sewerRejectPrice = 5;

    public GameObject treatmentStation;
    private List<GameObject> treatmentStations = new List<GameObject>();
    public bool treatmentStationMouseClick = false;
    public int treatmentStationPrice = 15;

    public GameObject waterCastle;
    private List<GameObject> waterCastles = new List<GameObject>();
    public bool waterCastleMouseClick = false;
    public int waterCastlePrice = 10;

    public ClockManager clock;

    // Update is called once per frame
    void Update()
    {
        if(this.reservoirMouseClick) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    this.CreateReservoir(hit);
                }
            }
        }
        if(this.sewerRejectMouseClick) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    this.CreateSewerReject(hit);
                }
            }
        }
        if(this.treatmentStationMouseClick) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    this.CreateTreatmentStation(hit);
                }
            }
        }
        if(this.waterCastleMouseClick) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    this.CreateWaterCastle(hit);
                }
            }
        }
    }

    public void CreateReservoir(RaycastHit ray) {
        if(clock.money >= reservoirPrice){
            Vector3 position = ray.point;
            GameObject reservoirObject = (GameObject) Instantiate (reservoir, position, Quaternion.Euler (0, 0, 0), transform);
            reservoirs.Add(reservoirObject);
            clock.money -= reservoirPrice;
        }
        this.flipReservoirMouseClick();
    }

    public void flipReservoirMouseClick() {
        this.reservoirMouseClick = !this.reservoirMouseClick;
    }

    public void CreateSewerReject(RaycastHit ray) {
        if(clock.money >= sewerRejectPrice){
            Vector3 position = ray.point;
            GameObject sewerRejectObject = (GameObject) Instantiate (sewerReject, position, Quaternion.Euler (0, 0, 0), transform);
            sewerRejects.Add(sewerRejectObject);
            clock.money -= sewerRejectPrice;
        }
        this.flipSewerRejectMouseClick();
    }

    public void flipSewerRejectMouseClick() {
        this.sewerRejectMouseClick = !this.sewerRejectMouseClick;
    }

    public void CreateTreatmentStation(RaycastHit ray) {
        if(clock.money >= treatmentStationPrice){
            Vector3 position = ray.point;
            GameObject treatmentStationObject = (GameObject) Instantiate (treatmentStation, position, Quaternion.Euler (0, 0, 0), transform);
            treatmentStations.Add(treatmentStationObject);
            clock.money -= treatmentStationPrice;
        }
        this.flipTreatmentStationMouseClick();
    }

    public void flipTreatmentStationMouseClick() {
        this.treatmentStationMouseClick = !this.treatmentStationMouseClick;
    }

    public void CreateWaterCastle(RaycastHit ray) {
        if(clock.money >= waterCastlePrice){
            Vector3 position = ray.point;
            GameObject waterCastleObject = (GameObject) Instantiate (waterCastle, position, Quaternion.Euler (0, 0, 0), transform);
            waterCastles.Add(waterCastleObject);
            clock.money -= waterCastlePrice;
        }
        this.flipWaterCastleMouseClick();
    }

    public void flipWaterCastleMouseClick() {
        this.waterCastleMouseClick = !this.waterCastleMouseClick;
    }
}
