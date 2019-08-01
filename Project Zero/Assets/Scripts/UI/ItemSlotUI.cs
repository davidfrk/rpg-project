using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rpg.Items;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropHandler
{
    public Item item;
    public Image itemImage;
    internal int slotNumber;
    internal Item.ItemType slotType;

    private ISlotManager slotManager;
    Image backgroundImage;
    Sprite backgroundSprite;
    Sprite itemSlotSprite;

    void Awake()
    {
        backgroundImage = GetComponent<Image>();
        slotManager = GetComponentInParent<ISlotManager>();
    }

    void Start()
    {
        UpdateSprites();
    }

    void UpdateSprites()
    {
        backgroundSprite = UIController.instance.itemSlotBackgroundSprite;
        itemSlotSprite = UIController.instance.itemSlotSprite;

        backgroundImage.sprite = backgroundSprite;
        UpdateItemSlot(item);
    }

    public void UpdateItemSlot(Item item)
    {
        this.item = item;
        if (item != null)
        {
            itemImage.sprite = item.sprite;
        }
        else
        {
            itemImage.sprite = itemSlotSprite;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            slotManager.OnMouseRightClickDown(this);
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            slotManager.OnMouseLeftClickDown(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        /*
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            slotManager.OnMouseLeftClick(this, eventData);
        }*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            slotManager.OnMouseLeftClickDrag(this, eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            slotManager.OnMouseLeftClickDrop(this, eventData);
        }
    }
}
