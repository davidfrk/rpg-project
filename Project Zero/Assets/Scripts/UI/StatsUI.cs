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
        HealthBar.Set(selectedUnit.Health, selectedUnit.MaxHealth);
        ManaBar.Set(selectedUnit.Mana, selectedUnit.MaxMana);

        if (showStats)
        {
            Attack.text = selectedUnit.stats.Attack.Value.ToString("F0");
            AttackSpeed.text = selectedUnit.stats.AttackSpeed.Value.ToString("F0");
            Armor.text = selectedUnit.stats.Armor.Value.ToString("F0");
            MagicArmor.text = selectedUnit.stats.MagicArmor.Value.ToString("F0");
            
            Str.text = selectedUnit.stats.Str.Value.ToString("F0");
            Agi.text = selectedUnit.stats.Agi.Value.ToString("F0");
            Int.text = selectedUnit.stats.Int.Value.ToString("F0");
            Will.text = selectedUnit.stats.Will.Value.ToString("F0");
        }
    }
}
