using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;

[System.Serializable]
public class Stats
{
    public CharacterStat MaxHealth;
    public CharacterStat MaxMana;
    public CharacterStat Armor;
    public CharacterStat MagicArmor;
    public CharacterStat Attack;
    public CharacterStat AttackSpeed;
    public CharacterStat Str;
    public CharacterStat Agi;
    public CharacterStat Int;
    public CharacterStat Will;
    public CharacterStat MovementSpeed;

    [Space(10)]
    public BaseStats baseStats;

    [Space(10)]
    public float StrGain;
    public float AgiGain;
    public float IntGain;
    public float WillGain;

    private int level = 0;

    public void UpdateBaseStats()
    {
        MaxHealth.BaseValue     = baseStats.MaxHealth;
        MaxMana.BaseValue       = baseStats.MaxMana;
        Armor.BaseValue         = baseStats.Armor;
        MagicArmor.BaseValue    = baseStats.MagicArmor;
        Attack.BaseValue        = baseStats.Attack;
        AttackSpeed.BaseValue   = baseStats.AttackSpeed;
        Str.BaseValue           = baseStats.Str;
        Agi.BaseValue           = baseStats.Agi;
        Int.BaseValue           = baseStats.Int;
        Will.BaseValue          = baseStats.Will;
        MovementSpeed.BaseValue = baseStats.MovementSpeed;
    }

    public void UpdateLevel(int level)
    {
        if (level != this.level)
        {
            Str.BaseValue = baseStats.Str + level * StrGain;
            Agi.BaseValue = baseStats.Agi + level * AgiGain;
            Int.BaseValue = baseStats.Int + level * IntGain;
            Will.BaseValue = baseStats.Will + level * WillGain;

            UpdateDerivedStats();
        }
    }

    public void UpdateDerivedStats()
    {
        MaxHealth.BaseValue += 10f * Str.Value;
        MaxMana.BaseValue += 2f * Int.Value + Will.Value;
        Armor.BaseValue += Agi.Value / 5f;
        MagicArmor.BaseValue += Will.Value;
        Attack.BaseValue += Str.Value;
        AttackSpeed.BaseValue += Agi.Value;
        MovementSpeed.BaseValue += Agi.Value / 20f;
    }
}

[System.Serializable]
public struct BaseStats
{
    public float MaxHealth;
    public float MaxMana;
    public float Armor;
    public float MagicArmor;
    public float Attack;
    public float AttackSpeed;
    public float Str;
    public float Agi;
    public float Int;
    public float Will;
    public float MovementSpeed;
    
    public static void UpdateDerivedStats(ref BaseStats stats)
    {
        stats.MaxHealth        += 10 * stats.Str;
        stats.MaxMana          += 2 * stats.Int + stats.Will;
        stats.Armor            += stats.Agi / 5f;
        stats.Attack           += stats.Str;
        stats.AttackSpeed      += stats.Agi;
        stats.MovementSpeed    += stats.Agi / 20f;
        stats.MagicArmor       += stats.Will;
    }

    public static BaseStats operator+ (BaseStats stats1, BaseStats stats2)
    {
        BaseStats result;
        
        result.MaxHealth        = stats1.MaxHealth       + stats2.MaxHealth;
        result.MaxMana          = stats1.MaxMana         + stats2.MaxMana;
        result.Armor            = stats1.Armor           + stats2.Armor;
        result.Attack           = stats1.Attack          + stats2.Attack;
        result.AttackSpeed      = stats1.AttackSpeed     + stats2.AttackSpeed;
        result.Str              = stats1.Str             + stats2.Str;
        result.Agi              = stats1.Agi             + stats2.Agi;
        result.Int              = stats1.Int             + stats2.Int;
        result.Will             = stats1.Will            + stats2.Will;
        result.MovementSpeed    = stats1.MovementSpeed   + stats2.MovementSpeed; 
        result.MagicArmor       = stats1.MagicArmor + stats2.MagicArmor;

        return result;
    }

    public static BaseStats operator *(float mult, BaseStats stats)
    {
        BaseStats result;

        result.MaxHealth        = mult * stats.MaxHealth;
        result.MaxMana          = mult * stats.MaxMana;
        result.Armor            = mult * stats.Armor;
        result.Attack           = mult * stats.Attack;
        result.AttackSpeed      = mult * stats.AttackSpeed;
        result.Str              = mult * stats.Str;
        result.Agi              = mult * stats.Agi;
        result.Int              = mult * stats.Int;
        result.Will             = mult * stats.Will;
        result.MovementSpeed    = mult * stats.MovementSpeed;
        result.MagicArmor       = mult * stats.MagicArmor;

        return result;
    }
}
