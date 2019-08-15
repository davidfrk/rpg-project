using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rpg.Items;
using Rpg.UI;

public class UIController : MonoBehaviour, ISlotManager
{
    public static UIController instance;

    internal Unit selectedUnit;

    [Space(10)]
    public GameObject equipmentUI;
    bool showEquipmentUI = false;
    public Sprite itemSlotSprite;
    public Sprite itemSlotBackgroundSprite;
    public Image draggingItemImage;
    public EquipmentTooltip equipmentTooltipPrefab;
    public ShopUI shopUI;
    private Shop shop;
    public SkillTreeUI SkillTreeUI;

    private bool draggingItem = false;
    private EquipmentTooltip equipmentTooltip;
    private ItemSlotUI selectedItemSlot;

    internal float lastUIClick = 0f;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (selectedUnit != PlayerController.localPlayer.selectedUnit)
        {
            selectedUnit = PlayerController.localPlayer.selectedUnit;
            //Callback selecionou nova unidade
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ToggleEquipmentUI();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            if (shopUI.isActiveAndEnabled)
            {
                shopUI.Close();
                equipmentUI.SetActive(showEquipmentUI);
            }
            else
            {
                shop = Shop.FindShopInRange(PlayerController.localPlayer.MainUnit.transform.position);
                shopUI.Open(shop);
                equipmentUI.SetActive(true);
            }
        }

        if(Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.T))
        {
            SkillTreeUI.Toggle();
        }

        if (Input.GetMouseButtonUp(0)){
            selectedItemSlot = null;
            draggingItemImage.gameObject.SetActive(false);
        }
    }

    public void ToggleEquipmentUI()
    {
        showEquipmentUI = !showEquipmentUI;

        equipmentUI.SetActive(showEquipmentUI);
    }

    public void OnMouseLeftClickDown(ItemSlotUI itemSlotUI)
    {
        lastUIClick = Time.time;
    }

    public void OnMouseRightClickDown(ItemSlotUI itemSlotUI)
    {
        lastUIClick = Time.time;

        if (selectedUnit != null)
        {
            EquipmentManager equipmentManager = selectedUnit.GetComponent<EquipmentManager>();
            if (equipmentManager != null)
            {
                Item item = equipmentManager.DropItem(itemSlotUI.slotType, itemSlotUI.slotNumber);

                if (shopUI.isActiveAndEnabled)
                {
                    shop.Buy(item, PlayerController.localPlayer);
                }
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
            draggingItem = false;
        }
        else
        {
            draggingItemImage.sprite = selectedItemSlot.item.sprite;
            draggingItemImage.rectTransform.position = position;
            draggingItemImage.gameObject.SetActive(true);
            draggingItem = true;
        }
    }

    public void OnPointerEnter(ItemSlotUI itemSlotUI, PointerEventData eventData)
    {
        if (itemSlotUI.item != null && itemSlotUI.item.itemType == Item.ItemType.Equipment)
        {
            if (!draggingItem)
            {
                Equipment equipment = itemSlotUI.item as Equipment;
                ShowEquipmentTooltip(equipment, eventData.position);
            }
        }
        else
        {
            HideEquipmentTooltip();
        }
    }

    public void OnPointerExit(ItemSlotUI itemSlotUI, PointerEventData eventData)
    {
        HideEquipmentTooltip();
    }

    private void ShowEquipmentTooltip(Equipment equipment, Vector2 position)
    {
        if (equipmentTooltip != null)
        {
            if (equipmentTooltip.equipment == equipment) return;
            else HideEquipmentTooltip();
        }

        equipmentTooltip = Instantiate<EquipmentTooltip>(equipmentTooltipPrefab, position, Quaternion.identity, transform);
        equipmentTooltip.UpdateUI(equipment);
    }

    private void HideEquipmentTooltip()
    {
        if (equipmentTooltip != null)
        {
            Destroy(equipmentTooltip.gameObject);
            equipmentTooltip = null;
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
