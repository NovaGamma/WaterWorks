using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Date & Time Settings")]
    [Range(1,28)]
    public int dateInMonth;
    [Range(1,12)]
    public int month;
    [Range(2000,2100)]
    public int year;
    [Range(0,24)]
    public int hour;
    [Range(0,6)]
    public int minutes;

    private DateTime DateTime;

    [Header("Tick Setting")]
    public int TickMinutesIncrease = 10;
    public float TimeBetweenTicks = 1;
    private float currentTimeBetweenTicks = 0;

    public static UnityAction<DateTime> OnDateTimeChanged;

    public bool pause = false;

    private void Awake() {
        DateTime = new DateTime(dateInMonth, month - 1, year, hour, minutes * 10);
    }

    private void Start() {
        OnDateTimeChanged?.Invoke(DateTime);
    }

    private void Update() {
        if(!pause){
            currentTimeBetweenTicks += Time.deltaTime;

            if (currentTimeBetweenTicks >= TimeBetweenTicks)
            {
                currentTimeBetweenTicks = 0;
                Tick();
            }
        }
    }

    void Tick()
    {
        AdvanceTime();
    }

    void AdvanceTime()
    {
        DateTime.AdvanceMinutes(TickMinutesIncrease);
        OnDateTimeChanged?.Invoke(DateTime);
    }

    public void PauseOrResume()
    {
        pause = !pause;
    }
}

