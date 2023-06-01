using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Pipe> pipes;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Pipe> GetPipes()
    {
        return pipes;
    }

    public bool CheckPositionIsInsideCollider(Vector3 position)
    {
        var collidersObj = gameObject.GetComponentsInChildren<BoxCollider>();
        for (var index = 0; index < collidersObj.Length; index++)
        {
            var colliderItem = collidersObj[index];
            if(colliderItem.bounds.Contains(position))
            {
                return true;
            }
        }
        return false;
    }
}
