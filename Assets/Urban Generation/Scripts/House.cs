using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Pipe pipe;
    public int consumeAmount = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pipe.ConsumeWater(consumeAmount))
        {
            Debug.Log("Consumed " + consumeAmount);
        }
        else
        {
            Debug.Log("Not enough water in Pipe");
        }
    }
}
