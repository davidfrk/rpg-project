using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public UIStatsBar HealthBar;
    public UIStatsBar ManaBar;
    public GameObject StatsTransform;

    [Space(5)]
    public Text Attack;
    public Text AttackSpeed;
    public Text Armor;
    public Text MagicArmor;

    [Space(5)]
    public Text Str;
    public Text Agi;
    public Text Int;
    public Text Will;

    Unit selectedUnit;
    private bool showStats = false;
    
    void Update()
    {
        selectedUnit = PlayerController.localPlayer.selectedUnit;

        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            showStats = !showStats;
        }

        if (selectedUnit != null){
            UpdateUI();
        }

        StatsTransform.SetActive(showStats && selectedUnit != null);
    }

    void UpdateUI()
    {
        HealthBar.Set(selectedUnit.Health, selectedUnit.unitStats.MaxHealth);
        ManaBar.Set(selectedUnit.Mana, selectedUnit.unitStats.MaxMana);

        if (showStats)
        {
            Attack.text = selectedUnit.unitStats.Attack.ToString("F0");
            AttackSpeed.text = selectedUnit.unitStats.AttackSpeed.ToString("F0");
            Armor.text = selectedUnit.unitStats.Armor.ToString("F0");
            MagicArmor.text = selectedUnit.unitStats.MagicResistance.ToString("F0");
            
            Str.text = selectedUnit.unitStats.Str.ToString("F0");
            Agi.text = selectedUnit.unitStats.Agi.ToString("F0");
            Int.text = selectedUnit.unitStats.Int.ToString("F0");
            Will.text = selectedUnit.unitStats.Will.ToString("F0");
        }
    }
}
