﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rpg.Skills;

public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text cost;
    public Text key;
    Image image;
    ISkillManagerUI skillManagerUI;
    public Skill Skill { get; private set; }

    void Awake()
    {
        image = GetComponent<Image>();
        skillManagerUI = GetComponentInParent<ISkillManagerUI>();
    }

    public void UpdateUI(Skill skill, KeyCode keyCode)
    {
        Skill = skill;
        image.sprite = skill.icon;
        cost.text = skill.manaCost.ToString("F0");
        key.text = keyCode.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillManagerUI.OnPointerEnter(eventData, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillManagerUI.OnPointerExit(eventData, this);
    }
}
