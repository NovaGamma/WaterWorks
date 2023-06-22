using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ongletsController : MonoBehaviour
{

    private GameObject[] contents;
    // Start is called before the first frame update
    void Start()
    {
        contents = GameObject.FindGameObjectsWithTag("Content");
        foreach (GameObject content in contents)
        {
            content.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeContent(int index)
    {
        foreach (GameObject content in contents)
        {
            
            if (content.name.Equals("Content" + index))
            {
                content.SetActive(true);
                this.GetComponent<ScrollRect>().content = (RectTransform) content.transform;
            }
            else
                content.SetActive(false);

        }
    }
}
