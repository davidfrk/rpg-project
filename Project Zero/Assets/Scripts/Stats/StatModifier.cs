using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Stats
{
	public enum StatModType
	{
		Flat = 100,
		PercentAdd = 200,
		PercentMult = 300,
	}

	public class StatModifier
	{
		private float value;
        public float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                if (owner != null)
                {
                    owner.SetDirty();
                }
            }
        }
		public readonly StatModType Type;
		public readonly int Order;
		public readonly object Source;

        private Stats ownerStats;
        private List<Dependency> dependencies;

		public StatModifier(float value, StatModType type, int order, object source)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
		}

		public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

		public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

		public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }

        public StatModifier(StatBonus statBonus, Stats ownerStats, object source)
        {
            Value = statBonus.value;
            Type = statBonus.statModType;
            Order = (int)Type;
            Source = source;
            this.ownerStats = ownerStats;
            dependencies = statBonus.dependencies;

            RegisterCallbacks();
            UpdateValue();
        }

        private CharacterStat owner;
        public CharacterStat Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }

        private void RegisterCallbacks()
        {
            foreach(Dependency dependency in dependencies)
            {
                ownerStats.GetCharacterStat(dependency.StatType).OnChangeCallback += UpdateValue;
            }
        }

        private void UnregisterCallbacks()
        {
            foreach (Dependency dependency in dependencies)
            {
                ownerStats.GetCharacterStat(dependency.StatType).OnChangeCallback -= UpdateValue;
            }
        }

        private void UpdateValue()
        {
            if (dependencies.Count == 0f) return;

            float newValue = 0f;
            foreach (Dependency dependency in dependencies)
            {
                newValue += dependency.Value * ownerStats.GetCharacterStat(dependency.StatType).Value;
            }
            Value = newValue;
        }

        public void Remove()
        {
            UnregisterCallbacks();
            owner = null;
            ownerStats = null;
        }

        [System.Serializable]
        public struct Dependency : IStat
        {
            public StatType StatType;
            public float Value;

            public string GetName()
            {
                return StatType.ToString() + " grade";
            }

            public string ValueToString()
            {
                return GetGrade(Value);
            }

            private string GetGrade(float value)
            {
                int grade = Mathf.FloorToInt(value / 0.5f);

                if (grade <= 0) return "F";
                switch (grade)
                {
                    case 0: return "F";
                    case 1: return "E";
                    case 2: return "D";
                    case 3: return "C";
                    case 4: return "B";
                    case 5: return "A";
                    case 6: return "S";
                    default: return "S+";
                }
            }
        }
    }
}
