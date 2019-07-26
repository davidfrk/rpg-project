using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour, ISlotManager
{
    public static UIController instance;

    public Unit selectedUnit;

    [Space(10)]
    public GameObject equipmentUI;
    bool show = false;
    public Sprite itemSlotSprite;
    public Sprite itemSlotBackgroundSprite;
    public Image draggingItemImage;
    private ItemSlotUI selectedItemSlot;

    internal float lastUIClick;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Toggle();
        }

        if (Input.GetMouseButtonUp(0)){
            selectedItemSlot = null;
            draggingItemImage.gameObject.SetActive(false);
        }
    }

    public void Toggle()
    {
        show = !show;

        equipmentUI.SetActive(show);
    }

    public void OnMouseRightClickDown(ItemSlotUI itemSlotUI)
    {
        lastUIClick = Time.time;

        if (selectedUnit != null)
        {
            EquipmentManager equipmentManager = selectedUnit.GetComponent<EquipmentManager>();
            if (equipmentManager != null)
            {
                equipmentManager.DropItem(itemSlotUI.slotType, itemSlotUI.slotNumber);
            }
        }
    }

    public void OnMouseLeftClickDrag(ItemSlotUI itemSlotUI, PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            selectedItemSlot = itemSlotUI;

            DragUpdate(eventData.position);
        }
    }

    public void OnMouseLeftClickDrop(ItemSlotUI itemSlotUI, PointerEventData eventData)
    {
        if (selectedItemSlot.item != null && selectedItemSlot != itemSlotUI)
        {
            SwapItems(selectedItemSlot, itemSlotUI);
        }

        selectedItemSlot = null;
        DragUpdate(eventData.position);
    }

    public void DragUpdate(Vector2 position)
    {
        if (selectedItemSlot == null || selectedItemSlot.item == null)
        {
            draggingItemImage.gameObject.SetActive(false);
        }
        else
        {
            draggingItemImage.sprite = selectedItemSlot.item.sprite;
            draggingItemImage.rectTransform.position = position;
            draggingItemImage.gameObject.SetActive(true);
        }
    }

    public void SwapItems(ItemSlotUI slot1, ItemSlotUI slot2)
    {
        if (selectedUnit != null)
        {
            EquipmentManager equipmentManager = selectedUnit.GetComponent<EquipmentManager>();
            if (equipmentManager != null)
            {
                equipmentManager.SwapItems(slot1.slotType, slot1.slotNumber, slot2.slotType, slot2.slotNumber);
            }
        }
        
        //Debug.Log("Swap " + slot1.slotType + " " + slot1.slotNumber + " to " + slot2.slotType + " " + slot2.slotNumber);
    }
}
