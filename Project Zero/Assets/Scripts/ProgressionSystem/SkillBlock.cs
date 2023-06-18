using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills;

namespace Rpg.ProgressionSystem {
    public class SkillBlock
    {
        public int tier;
        public List<Talent> talents = new List<Talent>();

        private bool isLocked = true;
        public bool IsLocked
        {
            get
            {
                return isLocked;
            }
            set
            {
                isLocked = value;
            }
        }

        public void Generate(SkillTag skillTag, int tier)
        {
            this.tier = tier;
            List<Talent> newTalents = TalentBook.instance.GetSkills(skillTag);

            if (newTalents == null || newTalents.Count == 0) return;

            talents.Clear();

            newTalents.Shuffle();
            int amount = Random.Range(GameManager.instance.talentTreeMinimumNumberOfTalents, GameManager.instance.talentTreeMaximumNumberOfTalents + 1);

            for (int i = 0; i < amount; i++)
            {
                int index = i % newTalents.Count;
                Talent newTalent = GameObject.Instantiate(newTalents[index]);
                newTalent.UpdateTier(tier);
                talents.Add(newTalent);
            }
        }
    }
}
