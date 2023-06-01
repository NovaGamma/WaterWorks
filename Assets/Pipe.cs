using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 Position 
    { 
        get => transform.position; 
    }
    void Start()
    {
        BoxCollider boxCollider = transform.gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(20f, 20f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
