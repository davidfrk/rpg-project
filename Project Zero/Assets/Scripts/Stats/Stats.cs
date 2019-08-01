using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Stats {

    [System.Serializable]
    public class Stats
    {
        public CharacterStat MaxHealth;
        public CharacterStat HpRegen;
        public CharacterStat MaxMana;
        public CharacterStat ManaRegen;
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

        private StatModifier MaxHealthMod       = new StatModifier(0f, StatModType.Flat);
        private StatModifier MaxManaMod         = new StatModifier(0f, StatModType.Flat);
        private StatModifier ArmorMod           = new StatModifier(0f, StatModType.Flat);
        private StatModifier MagicArmorMod      = new StatModifier(0f, StatModType.Flat);
        private StatModifier AttackMod          = new StatModifier(0f, StatModType.Flat);
        private StatModifier AttackSpeedMod     = new StatModifier(0f, StatModType.Flat);
        private StatModifier MovementSpeedMod   = new StatModifier(0f, StatModType.Flat);

        public void InitStats()
        {
            MaxHealth.BaseValue     = baseStats.MaxHealth;
            HpRegen.BaseValue       = baseStats.HpRegen;
            MaxMana.BaseValue       = baseStats.MaxMana;
            ManaRegen.BaseValue     = baseStats.ManaRegen;
            Armor.BaseValue         = baseStats.Armor;
            MagicArmor.BaseValue    = baseStats.MagicArmor;
            Attack.BaseValue        = baseStats.Attack;
            AttackSpeed.BaseValue   = baseStats.AttackSpeed;
            Str.BaseValue           = baseStats.Str;
            Agi.BaseValue           = baseStats.Agi;
            Int.BaseValue           = baseStats.Int;
            Will.BaseValue          = baseStats.Will;
            MovementSpeed.BaseValue = baseStats.MovementSpeed;

            MaxHealth.AddModifier(MaxHealthMod);
            MaxMana.AddModifier(MaxManaMod);
            Armor.AddModifier(ArmorMod);
            MagicArmor.AddModifier(MagicArmorMod);
            Attack.AddModifier(AttackMod);
            AttackSpeed.AddModifier(AttackSpeedMod);
            MovementSpeed.AddModifier(MovementSpeedMod);
        }

        public void UpdateLevel(int level)
        {
            if (level != this.level)
            {
                Str.BaseValue = baseStats.Str + level * StrGain;
                Agi.BaseValue = baseStats.Agi + level * AgiGain;
                Int.BaseValue = baseStats.Int + level * IntGain;
                Will.BaseValue = baseStats.Will + level * WillGain;

                //UpdateDerivedStats();
            }
        }

        public void UpdateDerivedStats()
        {
            MaxHealthMod.Value      = 10f * Str.Value;
            MaxManaMod.Value        = 2f * Int.Value + Will.Value;
            ArmorMod.Value          = Agi.Value / 5f;
            MagicArmorMod.Value     = Will.Value;
            AttackMod.Value         = Str.Value;
            AttackSpeedMod.Value    = Agi.Value;
            MovementSpeedMod.Value  = Agi.Value / 20f;
        }

        private void AddStatModifier(StatType stat, float value, StatModType modType, object source)
        {
            CharacterStat characterStat = GetCharacterStat(stat);
            characterStat.AddModifier(new StatModifier(value, modType, (int)modType, source));
        }

        public void AddStatModifier(StatBonus statBonus, object source)
        {
            CharacterStat characterStat = GetCharacterStat(statBonus.stat);
            characterStat.AddModifier(new StatModifier(statBonus, this, source));
        }

        public void RemoveStatModifier(StatType stat, object source)
        {
            CharacterStat characterStat = GetCharacterStat(stat);
            characterStat.RemoveAllModifiersFromSource(source);
        }

        public void AddStatModifierList(List<StatBonus> modifiers, object source)
        {
            foreach (StatBonus statBonus in modifiers)
            {
                AddStatModifier(statBonus, source);
            }
        }

        public void RemoveStatModifierList(List<StatBonus> modifiers, object source)
        {
            foreach (StatBonus statBonus in modifiers)
            {
                RemoveStatModifier(statBonus.stat, source);
            }
        }

        public CharacterStat GetCharacterStat(StatType stat)
        {
            switch (stat)
            {
                case StatType.MaxHealth:    return MaxHealth;
                case StatType.HpRegen:      return HpRegen;
                case StatType.MaxMana:      return MaxMana;
                case StatType.ManaRegen:    return ManaRegen;
                case StatType.Armor:        return Armor;
                case StatType.MagicArmor:   return MagicArmor;
                case StatType.Attack:       return Attack;
                case StatType.AttackSpeed:  return AttackSpeed;
                case StatType.Str:          return Str;
                case StatType.Agi:          return Agi;
                case StatType.Int:          return Int;
                case StatType.Will:         return Will;
                case StatType.MovementSpeed:return MovementSpeed;
                default: return null;
            }
        }
    }

    public enum StatType
    {
        MaxHealth,
        HpRegen,
        MaxMana,
        ManaRegen,
        Armor,
        MagicArmor,
        Attack,
        AttackSpeed,
        Str,
        Agi,
        Int,
        Will,
        MovementSpeed
    }

    [System.Serializable]
    public struct BaseStats
    {
        public float MaxHealth;
        public float HpRegen;
        public float MaxMana;
        public float ManaRegen;
        public float Armor;
        public float MagicArmor;
        public float Attack;
        public float AttackSpeed;
        public float Str;
        public float Agi;
        public float Int;
        public float Will;
        public float MovementSpeed;

        public static BaseStats operator+ (BaseStats stats1, BaseStats stats2)
        {
            BaseStats result;
        
            result.MaxHealth        = stats1.MaxHealth       + stats2.MaxHealth;
            result.HpRegen          = stats1.HpRegen         + stats2.HpRegen;
            result.MaxMana          = stats1.MaxMana         + stats2.MaxMana;
            result.ManaRegen        = stats1.ManaRegen       + stats2.ManaRegen;
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
            result.HpRegen          = mult * stats.HpRegen;
            result.MaxMana          = mult * stats.MaxMana;
            result.ManaRegen        = mult * stats.ManaRegen;
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
}
