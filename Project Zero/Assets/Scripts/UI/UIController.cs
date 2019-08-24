using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rpg.Items;
using Rpg.UI;
using Rpg.Items.CraftSystem;

public class UIController : MonoBehaviour, ISlotManager
{
    public static UIController instance;

    internal Unit selectedUnit;

    [Space(10)]
    public EquipmentGroupUI equipmentUI;
    public Sprite itemSlotSprite;
    public Sprite itemSlotBackgroundSprite;
    public Image draggingItemImage;
    public EquipmentTooltip equipmentTooltipPrefab;
    public ShopUI shopUI;
    private Shop shop;
    public SkillTreeUI SkillTreeUI;
    public CraftUI craftUI;
    public CraftManager craftManager;

    private bool draggingItem = false;
    private EquipmentTooltip equipmentTooltip;
    private ItemSlotUI selectedItemSlot;

    private float lastUIClick = 0f;
    private float uiProtectionTime = 0.2f;

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
            equipmentUI.Toggle();
            HideEquipmentTooltip();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            if (shopUI.isActiveAndEnabled)
            {
                shopUI.Close();
                equipmentUI.Close();
                HideEquipmentTooltip();
            }
            else
            {
                shop = Shop.FindShopInRange(PlayerController.localPlayer.MainUnit.transform.position);
                if (shop != null)
                {
                    shopUI.Open(shop);
                    equipmentUI.Open();
                }
                else
                {
                    equipmentUI.Toggle();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            craftUI.Toggle(craftManager);
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

    static public void UIClick()
    {
        instance.lastUIClick = Time.time;
    }

    static public bool UIProtectionTime()
    {
        //Avoids clicks on UI firing commands
        return (instance.lastUIClick + instance.uiProtectionTime > Time.time);
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
                Item item = equipmentManager.GetItem(itemSlotUI.slotType, itemSlotUI.slotNumber);
                
                if (item != null)
                {
                    if (shopUI.isActiveAndEnabled)
                    {
                        //If shop isActive drop the item only if the shop can buy it
                        if (shop.CanBuy(item, PlayerController.localPlayer))
                        {
                            equipmentManager.DropItem(itemSlotUI.slotType, itemSlotUI.slotNumber);
                            shop.Buy(item, PlayerController.localPlayer);
                            shopUI.UpdateUI();
                        }
                    }
                    else
                    {
                        equipmentManager.DropItem(itemSlotUI.slotType, itemSlotUI.slotNumber);
                    }
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

    private void ShowEquipmentTooltip(Equipment equipment, Vector2 position)
    {
        if (draggingItem) return;

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

    public static void ShowItemTooltip(Item item, Vector2 position)
    {
        if (item is Equipment)
        {
            instance.ShowEquipmentTooltip(item as Equipment, position);
        }
    }

    public static void HideItemTooltip()
    {
        instance.HideEquipmentTooltip();
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
