using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public StructureHelper structureHelper;
    public PipeManager pipeManager;
    public PumpManager pumpManager;
    private bool tempCanSpawnHouses = true;
    private int previousTotalNumDay = 1;
    private void OnEnable() {
        TimeManager.OnDateTimeChanged += UpdateDateTime;
    }

    private void UpdateDateTime(DateTime dateTime)
    {
        List<House> houses = structureHelper.GetHouses();
        List<Pump> pumps = pumpManager.GetPumps();
        List<Pipe> normalPipes = pipeManager.GetPipes();

        foreach (Pump pump in pumps)
        {
            pump.ProduceWater();
        }
        foreach (House house in houses)
        {
            house.ConsumeWater();
            house.ProduceWasteWater();
        }
        foreach (Pipe pipe in normalPipes)
        {
            if (dateTime.TotalNumDays != previousTotalNumDay)
            {
                pipe.houseSpawnAuthorized = tempCanSpawnHouses;
                previousTotalNumDay = dateTime.TotalNumDays;
                tempCanSpawnHouses = true;
            } else if (pipe.effectiveVolume < pipe.spawnVolumeLimit)
            {
                tempCanSpawnHouses = false;
            }
        }

        // Place a new house depending on hour
        if(dateTime.isHour(6) || dateTime.isHour(12)) structureHelper.PlaceStructureAroundRoad();
    }
}
