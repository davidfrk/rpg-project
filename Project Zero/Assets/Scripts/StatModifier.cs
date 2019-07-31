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
	}
}
