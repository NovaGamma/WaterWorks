using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemController : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public ItemsController.ButtonClickedEvent onClick;

    public Sprite Background;
    public Sprite HoverBackground;

    public Sprite PressedBackground;

    public GameObject InfoBubble;

    private bool hovered = false;

    public String Name;

    public int Price;

    private bool affordable;

    private TextMeshProUGUI NameText;

    private TextMeshProUGUI PriceText;

    private Animator anim;

    //public int index;

    //public static GameObject selected;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().sprite = Background;
        NameText = InfoBubble.transform.Find("Bubble/Name").gameObject.GetComponent<TextMeshProUGUI>();
        NameText.text = Name;
        PriceText = InfoBubble.transform.Find("Bubble/Price").gameObject.GetComponent<TextMeshProUGUI>();
        PriceText.text = "Price : " + (100*Price) + " $";
        anim = InfoBubble.GetComponent<Animator>();
        affordable = ClockManager.money >= Price;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hovered || InfoBubble.activeSelf)
        {
            InfoBubble.transform.position = Input.mousePosition + new Vector3(InfoBubble.transform.localScale.x*50+1,InfoBubble.transform.localScale.y*50+1,0);
            if ((ClockManager.money < Price) && (affordable == true))
            {
                affordable = false;
                PriceText.color = Color.red;
                NameText.color = Color.red;
            }
            else if (ClockManager.money >= Price && affordable == false)
            {
                affordable = true;
                PriceText.color = Color.white;
                NameText.color = Color.white;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //selected = this.gameObject;
        onClick.Invoke();
        this.GetComponent<Image>().sprite = HoverBackground;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        InfoBubble.SetActive(true);
        anim.Play("BubbleOpen");
        this.GetComponent<Image>().sprite = HoverBackground;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        anim.Play("BubbleClose");
        this.GetComponent<Image>().sprite = Background;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = PressedBackground;
    }

}
