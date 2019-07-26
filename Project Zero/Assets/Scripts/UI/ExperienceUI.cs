using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    public ExperienceManager selectedUnit;
    public Text levelText;
    private UIBar expBar;

    void Awake()
    {
        expBar = GetComponent<UIBar>();
    }

    void Update()
    {
        if (selectedUnit != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        levelText.text = selectedUnit.level.ToString();
        expBar.Set(selectedUnit.nextLevelProgression);
    }
}
