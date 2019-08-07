using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Skills;

public class SkillTooltip : MonoBehaviour
{
    public Text description;

    private void UpdateUI(Skill skill)
    {
        description.text = skill.description;
    }

    public void Show(Vector2 position, Skill skill)
    {
        UpdateUI(skill);
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
