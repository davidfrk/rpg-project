using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        public CharacterStat Dex;
        public CharacterStat Int;
        public CharacterStat Will;
        public CharacterStat MovementSpeed;

        public CharacterStat DamageTakenMultiplier;

        [Space(10)]
        public BaseStats baseStats;

        [Space(10)]
        public float StrGain;
        public float DexGain;
        public float IntGain;
        public float WillGain;

        private int level = 0;

        public int StrLevel = 0;
        public int DexLevel = 0;
        public int IntLevel = 0;
        public int WillLevel = 0;
        private float statGain = 3f;

        private StatModifier MaxHealthMod       = new StatModifier(0f, StatModType.Flat);
        private StatModifier MaxManaMod         = new StatModifier(0f, StatModType.Flat);
        private StatModifier ArmorMod           = new StatModifier(0f, StatModType.Flat);
        private StatModifier MagicArmorMod      = new StatModifier(0f, StatModType.Flat);
        private StatModifier AttackMod          = new StatModifier(0f, StatModType.Flat);
        private StatModifier AttackSpeedMod     = new StatModifier(0f, StatModType.Flat);
        private StatModifier MovementSpeedMod   = new StatModifier(0f, StatModType.Flat);
        private StatModifier HpRegenMod         = new StatModifier(0f, StatModType.Flat);
        private StatModifier ManaRegenMod       = new StatModifier(0f, StatModType.Flat);

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
            Dex.BaseValue           = baseStats.Dex;
            Int.BaseValue           = baseStats.Int;
            Will.BaseValue          = baseStats.Will;
            MovementSpeed.BaseValue = baseStats.MovementSpeed;
            DamageTakenMultiplier.BaseValue = 1f;

            MaxHealth.AddModifier(MaxHealthMod);
            MaxMana.AddModifier(MaxManaMod);
            Armor.AddModifier(ArmorMod);
            MagicArmor.AddModifier(MagicArmorMod);
            Attack.AddModifier(AttackMod);
            AttackSpeed.AddModifier(AttackSpeedMod);
            MovementSpeed.AddModifier(MovementSpeedMod);
            HpRegen.AddModifier(HpRegenMod);
            ManaRegen.AddModifier(ManaRegenMod);
        }

        public void LevelUp(StatType type)
        {
            switch (type)
            {
                case StatType.Str: StrLevel += 1; break;
                case StatType.Dex: DexLevel += 1; break;
                case StatType.Int: IntLevel += 1; break;
                case StatType.Will: WillLevel += 1; break;
                default: return;
            }

            UpdateBaseStats();
        }

        public bool HasStatsPointsAvailable()
        {
            return StatsPointsAvailable() > 0;
        }

        private int StatsPointsAvailable()
        {
            return level - (StrLevel + DexLevel + IntLevel + WillLevel);
        }

        public void UpdateLevel(int level)
        {
            if (level != this.level)
            {
                this.level = level;
                UpdateBaseStats();
                //UpdateDerivedStats();
            }
        }

        public void UpdateBaseStats()
        {
            Str.BaseValue = baseStats.Str + level * StrGain + statGain * StrLevel;
            Dex.BaseValue = baseStats.Dex + level * DexGain + statGain * DexLevel;
            Int.BaseValue = baseStats.Int + level * IntGain + statGain * IntLevel;
            Will.BaseValue = baseStats.Will + level * WillGain + statGain * WillLevel;
        }

        public void UpdateDerivedStats()
        {
            MaxHealthMod.Value      = 10f * Str.Value;
            MaxManaMod.Value        = 2f * Int.Value + Will.Value;
            ArmorMod.Value          = Dex.Value / 4f;
            MagicArmorMod.Value     = Will.Value;
            AttackMod.Value         = Str.Value;
            AttackSpeedMod.Value    = Dex.Value;
            MovementSpeedMod.Value  = Dex.Value / 20f;
            HpRegenMod.Value        = Will.Value / 40f;
            ManaRegenMod.Value      = Int.Value / 60f + Will.Value / 40f;
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

        public void RemoveAllStatModifiersFrom(object source)
        {
            foreach(StatType stat in Enum.GetValues(typeof(StatType)))
            {
                RemoveStatModifier(stat, source);
            }
        }

        public CharacterStat GetCharacterStat(StatType stat)
        {
            switch (stat)
            {
                case StatType.MaxHealth:                return MaxHealth;
                case StatType.HpRegen:                  return HpRegen;
                case StatType.MaxMana:                  return MaxMana;
                case StatType.ManaRegen:                return ManaRegen;
                case StatType.Armor:                    return Armor;
                case StatType.MagicArmor:               return MagicArmor;
                case StatType.Attack:                   return Attack;
                case StatType.AttackSpeed:              return AttackSpeed;
                case StatType.Str:                      return Str;
                case StatType.Dex:                      return Dex;
                case StatType.Int:                      return Int;
                case StatType.Will:                     return Will;
                case StatType.MovementSpeed:            return MovementSpeed;
                case StatType.DamageTakenMultiplier:    return DamageTakenMultiplier;
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
        Dex,
        Int,
        Will,
        MovementSpeed,
        DamageTakenMultiplier,
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
        public float Dex;
        public float Int;
        public float Will;
        public float MovementSpeed;
        public CritBonus crit;
    }
}
