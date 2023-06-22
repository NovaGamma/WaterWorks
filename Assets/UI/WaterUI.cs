using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterUI : MonoBehaviour
{
    private void OnEnable() {
        TimeManager.OnDateTimeChanged += UpdateDateTime;
    }

    // Update every hour
    private void UpdateDateTime(DateTime dateTime)
    {
        
    }
}
