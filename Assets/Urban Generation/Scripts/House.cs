using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public Pipe pipe;
    public Pipe dirtyPipe;
    public int consumeAmount = 5;
    public int wastewaterAmount = 5;
    public int population = 4;
    public int revenu = 1;


    public void ConsumeWater()
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
    
    public void ProduceWasteWater()
    {
        if (dirtyPipe is not null)
        {
            if(dirtyPipe.AddWater(wastewaterAmount))
            {
                Debug.Log("Produced " + wastewaterAmount);
            }
            else
            {
                Debug.Log("Not enough space in Pipe");
            }
        }
        else
        {
            Debug.Log("No waste pipe");
        }
    }
}
