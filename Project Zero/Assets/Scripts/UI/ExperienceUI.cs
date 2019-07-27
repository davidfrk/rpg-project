using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    private Unit selectedUnit;
    private ExperienceManager experienceManager;
    public Text levelText;
    private UIBar expBar;

    void Awake()
    {
        expBar = GetComponent<UIBar>();
    }

    void Update()
    {
        selectedUnit = UIController.instance.selectedUnit;
        if (selectedUnit != null)
        {
            experienceManager = selectedUnit.GetComponent<ExperienceManager>();

            if (experienceManager != null)
            {
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        levelText.text = experienceManager.level.ToString();
        expBar.Set(experienceManager.nextLevelProgression);
    }
}
