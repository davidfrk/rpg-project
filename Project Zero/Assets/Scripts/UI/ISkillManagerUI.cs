using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISkillManagerUI
{
    void OnPointerEnter(PointerEventData data, SkillSlotUI skillTooltip);
    void OnPointerExit(PointerEventData data, SkillSlotUI skillTooltip);
}
