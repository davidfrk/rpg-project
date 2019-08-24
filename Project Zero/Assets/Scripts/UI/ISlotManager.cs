using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISlotManager
{
    void OnMouseLeftClickDown(ItemSlotUI itemSlotUI);
    void OnMouseRightClickDown(ItemSlotUI itemSlotUI);
    void OnMouseLeftClickDrag(ItemSlotUI itemSlotUI, PointerEventData eventData);
    void OnMouseLeftClickDrop(ItemSlotUI itemSlotUI, PointerEventData eventData);
}
