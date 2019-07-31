using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Skills;

public class SkillSlotUI : MonoBehaviour
{
    public Text cost;
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void UpdateUI(Skill skill)
    {
        image.sprite = skill.icon;
        cost.text = skill.manaCost.ToString("F0");
    }
}
