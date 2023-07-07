using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ongletController : MonoBehaviour,
    IPointerClickHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public static int selected = 0;

    private bool hovered = false;

    public int index;

    public Sprite Hover;

    public Sprite Selected;

    public GameObject Barre;

    // Start is called before the first frame update
    void Start()
    {
        if (index == 0)
        {
            this.GetComponent<Image>().sprite = Selected;
            this.GetComponent<Image>().color = new Vector4(255,255,255,255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != index && !hovered)
        {
            this.GetComponent<Image>().color = new Vector4(255,255,255,0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = Selected;
        Barre.GetComponent<ongletsController>().changeContent(index);
        selected = index;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        if (index != selected)
        {
            this.GetComponent<Image>().sprite = Hover;
            this.GetComponent<Image>().color = new Vector4(255,255,255,255);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        if (index != selected)
        {
            this.GetComponent<Image>().color = new Vector4(255,255,255,0);
        }
    }
}
