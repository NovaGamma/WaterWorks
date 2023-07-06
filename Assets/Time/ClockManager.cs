using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClockManager : MonoBehaviour
{
    public StructureHelper structureHelper;
    public PipeManager pipeManager;
    public PumpManager pumpManager;
    public EpurationManager epurationManager;

    public static int money = 200;

    public static int population = 0;

    public TextMeshProUGUI  waterUI;
    public TextMeshProUGUI  moneyUI;
    public TextMeshProUGUI  popUI;
    public TextMeshProUGUI  timeUI;
    public TextMeshProUGUI  needMoreWaterUI;

    private bool tempCanSpawnHouses = true;
    private int previousTotalNumDay = 1;
    private void OnEnable() {
        TimeManager.OnDateTimeChanged += UpdateDateTime;
    }

    // Update every hour
    private void UpdateDateTime(DateTime dateTime)
    {
        List<House> houses = structureHelper.GetHouses();
        List<Pump> pumps = pumpManager.GetPumps();
        List<Pipe> normalPipes = pipeManager.GetPipes();
        List<Epuration> epurationPlants = epurationManager.GetEpurationPlants();

        foreach (Pump pump in pumps)
        {
            pump.ProduceWater();
        }
        foreach (House house in houses)
        {
            house.ConsumeWater();
            house.ProduceWasteWater();
        }

        foreach (Epuration epuration in epurationPlants) {
            epuration.cleanWater();
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
        if(dateTime.isHour(6) || dateTime.isHour(12) || dateTime.isHour(18)) structureHelper.PlaceStructureAroundRoad();

        if(dateTime.isMidnight()) UpdateMoney();
        UpdateMoneyText();
        UpdateWaterText();
        UpdatePopulationText();
        UpdateTimeText(dateTime);
    }

    public void UpdateMoney()
    {
        List<House> houses = structureHelper.GetHouses();
        int profitJournalier = 0;
        foreach (var house in houses)
        {
            profitJournalier += house.revenu;
        }
        money += profitJournalier;
    }

    public void UpdateNeedMoreWaterText()
    {
        needMoreWaterUI.enabled = !needMoreWaterUI.enabled;
    }

    public void UpdateTimeText(DateTime dateTime)
    {
        timeUI.text = dateTime.ToString();
    }

    public void UpdateMoneyText()
    {
        moneyUI.text = money * 100 + " $";
    }

    public void UpdateWaterText()
    {
        List<House> houses = structureHelper.GetHouses();
        List<Pipe> normalPipes = pipeManager.GetPipes();
        int consommation = 0;
        int effectiveWater = 0;

        foreach (var house in houses)
        {
            consommation += house.consumeAmount;
        }
        foreach (var pipe in normalPipes)
        {
            effectiveWater += pipe.effectiveVolume;
        }
        waterUI.text = consommation * 100 + " m³ / " + effectiveWater * 100 + " m³";
        
        if((needMoreWaterUI.enabled && consommation <= effectiveWater) || (!needMoreWaterUI.enabled && consommation > effectiveWater))
        {
            UpdateNeedMoreWaterText();
        }

    }

    public void UpdatePopulationText()
    {
        List<House> houses = structureHelper.GetHouses();
        population = 0;
        foreach (var house in houses)
        {
            population += house.population;
        }
        popUI.text = population + "";

    }
}
