using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills;

namespace Rpg.ProgressionSystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Talent", order = 1)]
    public class Talent : ScriptableObject
    {
        public float tierScale;
        public List<StatBonus> statBonus;
        public List<SkillTag> skillTags;
        public Sprite icon;

        private int tier;
        public int Tier
        {
            get
            {
                return tier;
            }
        }

        public int Cost
        {
            get
            {
                return 1;// tier;
            }
        }

        public bool Acquired { get; private set; } = false;

        public void Apply(Unit target, object source)
        {
            Acquired = true;

            foreach(StatBonus statBonus in statBonus)
            {
                target.stats.AddStatModifier(statBonus, source);
            }
        }

        public StatBonus GetFinalBonus(StatBonus statBonus, int tier)
        {
            StatBonus finalBonus = statBonus;
            finalBonus.value *= Random.Range(0.9f, 1.1f) * tierScale * tier;
            return finalBonus;
        }

        public void UpdateTier(int tier)
        {
            this.tier = tier;

            for(int i = 0; i < statBonus.Count; i++)
            {
                statBonus[i] = GetFinalBonus(statBonus[i], tier);
            }
        }
    }
}
