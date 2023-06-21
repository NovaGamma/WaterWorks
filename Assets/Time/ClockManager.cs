using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    private void OnEnable() {
        TimeManager.OnDateTimeChanged += UpdateDateTime;
    }

    private void UpdateDateTime(DateTime dateTime)
    {
        Debug.Log(dateTime);
    }
}
