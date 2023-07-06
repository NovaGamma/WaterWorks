using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ToggleQuest1;
    private bool completed1 = false;

    public GameObject ToggleQuest2;
    private bool completed2 = false;

    public GameObject ToggleQuest3;
    private bool completed3 = false;

    public static int pumpNb = 0;

    public static int epurNb = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (completed1 == false && pumpNb > 0)
        {
            ToggleQuest1.GetComponent<Toggle>().isOn = true;
            completed1 = true;
        }

        if (completed2 == false && epurNb > 0)
        {
            ToggleQuest2.GetComponent<Toggle>().isOn = true;
            completed2 = true;
        }

        if (completed3 == false && ClockManager.population > 99)
        {
            ToggleQuest3.GetComponent<Toggle>().isOn = true;
            completed3 = true;
        }
    }
}
