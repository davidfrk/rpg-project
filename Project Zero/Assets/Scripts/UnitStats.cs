using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitStats
{
    public float MaxHealth;
    public float MaxMana;
    public float Armor;
    public float Attack;
    public float AttackSpeed;
    public float Str;
    public float Agi;
    public float Int;
    public float Will;
    public float MovementSpeed;
    public float MagicResistance;

    public static void UpdateDerivedStats(ref UnitStats stats)
    {
        stats.MaxHealth        += 10 * stats.Str;
        stats.MaxMana          += 2 * stats.Int + stats.Will;
        stats.Armor            += stats.Agi / 5f;
        stats.Attack           += stats.Str;
        stats.AttackSpeed      += stats.Agi;
        stats.MovementSpeed    += stats.Agi / 20f;
        stats.MagicResistance  += stats.Will;
    }

    public static UnitStats operator+ (UnitStats stats1, UnitStats stats2)
    {
        UnitStats result;
        
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
        result.MagicResistance  = stats1.MagicResistance + stats2.MagicResistance;

        return result;
    }

    public static UnitStats operator *(float mult, UnitStats stats)
    {
        UnitStats result;

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
        result.MagicResistance  = mult * stats.MagicResistance;

        return result;
    }
}
