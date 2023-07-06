using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemsController : MonoBehaviour
{
    [Serializable]
    public class ButtonClickedEvent : UnityEvent {}

    [Serializable]
    public struct Item {
        public String name;
        public int price;
        public Sprite sprite;
        public ButtonClickedEvent onClick;
    }
    public Item[] items;

    public GameObject Content;

    public Sprite Background;
    public Sprite HoverBackground;
    public Sprite PressedBackground;

    public GameObject BubbleInfo;
    public GameObject Clock;

    public int Layer = 0;

    public float LocalSize = 112;
    public float Spacing = 5;

    public float OffsetX = 0;
    public float OffsetY = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        var index = 0;
        GameObject temp;
        foreach(Item item in items)
        {
            GameObject newItemFrame = new GameObject(item.sprite.name, typeof(Image), typeof(ItemController));
            newItemFrame.GetComponent<ItemController>().onClick = item.onClick;
            newItemFrame.GetComponent<ItemController>().Background = Background;
            newItemFrame.GetComponent<ItemController>().HoverBackground = HoverBackground;
            newItemFrame.GetComponent<ItemController>().PressedBackground = PressedBackground;
            newItemFrame.GetComponent<ItemController>().Name = item.name;
            newItemFrame.GetComponent<ItemController>().Price = item.price;
            newItemFrame.transform.parent = Content.transform;
            newItemFrame.transform.localPosition = new Vector3(OffsetX + (LocalSize + Spacing)*index,OffsetY,Layer);
            newItemFrame.transform.localScale = new Vector3(LocalSize/100,LocalSize/100,1);

            temp = Instantiate(BubbleInfo, new Vector3(0,0,14), Quaternion.identity);
            newItemFrame.GetComponent<ItemController>().InfoBubble = temp;
            temp.transform.parent = this.transform.parent.parent;
            temp.SetActive(false);

            GameObject newItem = new GameObject("image", typeof(Image));
            newItem.GetComponent<Image>().sprite = item.sprite;
            newItem.transform.parent = newItemFrame.transform;
            newItem.transform.localPosition = new Vector3(0,0,Layer-1);
            index++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO
        //if (this.transform.localPosition >= LocalSize+Spacing)
    }
}
