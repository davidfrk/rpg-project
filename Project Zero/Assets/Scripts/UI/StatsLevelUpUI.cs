using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;

public class StatsLevelUpUI : MonoBehaviour
{
    public GameObject statsLevelUpTransform;

    void Update()
    {
        Unit selectedUnit = UIController.instance.selectedUnit;
        if (selectedUnit != null)
        {
            statsLevelUpTransform.SetActive(selectedUnit.unitController.isPlayerUnit && selectedUnit.stats.HasStatsPointsAvailable());
        }
    }

    public void LevelUp(int stat)
    {
        UIController.instance.selectedUnit?.stats.LevelUp(GetStatType(stat));
        AudioManager.instance.PlaySound(AudioManager.UISound.StatUpgrade);
    }

    private StatType GetStatType(int stat)
    {
        switch (stat)
        {
            case 0: return StatType.Str;
            case 1: return StatType.Dex;
            case 2: return StatType.Int;
            case 3: return StatType.Will;
            default:
                {
                    Debug.LogError("Invalid stat id");
                    return StatType.Str;
                }
        }
    }
}
