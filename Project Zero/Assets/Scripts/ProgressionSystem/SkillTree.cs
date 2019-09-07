using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills;

namespace Rpg.ProgressionSystem
{
    public class SkillTree : MonoBehaviour
    {
        public SkillTag skillTag;
        public List<SkillBlock> skillBlocks = new List<SkillBlock>();
        public int SkillPoints { get; private set; } = 0;
        public int TotalPoints { get; set; } = 0;

        private Unit unit;
        private ExperienceManager experienceManager;

        public delegate void GainSkillPointEvent();
        public event GainSkillPointEvent OnGainSkillPointCallback;

        void Awake()
        {
            unit = GetComponent<Unit>();
            experienceManager = GetComponent<ExperienceManager>();
        }

        void Start()
        {
            Generate();
            experienceManager.OnLevelUpCallback += OnLevelUp;
        }

        public void Generate()
        {
            for (int i = 0; i < 10; i++)
            {
                SkillBlock skillBlock = new SkillBlock();
                skillBlock.Generate(skillTag, i + 1);
                skillBlocks.Add(skillBlock);
            }

            skillBlocks[0].IsLocked = false;
        }

        public void Reset()
        {
            SkillPoints = TotalPoints;
            unit.stats.RemoveAllStatModifiersFrom(this);
            skillBlocks.Clear();

            Generate();
        }

        public Talent Get(int block, int talent)
        {
            return skillBlocks[block].talents[talent];
        }

        private void Apply(Talent talent)
        {
            talent.Apply(unit, this);
        }

        public void Buy(Talent talent)
        {
            if (SkillPoints >= talent.Cost && talent.Acquired == false && !skillBlocks[talent.Tier - 1].IsLocked)
            {
                SkillPoints -= talent.Cost;
                Apply(talent);

                //Lock current block
                skillBlocks[talent.Tier - 1].IsLocked = true;

                //Unlock next block
                if (talent.Tier < skillBlocks.Count)
                {
                    skillBlocks[talent.Tier].IsLocked = false;
                }
                
                AudioManager.instance.PlaySound(AudioManager.UISound.TalentUpgrade);
            }
            else
            {
                AudioManager.instance.PlaySound(AudioManager.UISound.CantDo);
            }
        }

        public void OnLevelUp()
        {
            if (experienceManager.Level % 3 == 0)
            {
                GainSkillPoints(1);// experienceManager.Level - 1;
            }
        }

        public void GainSkillPoints(int points)
        {
            OnGainSkillPointCallback?.Invoke();
            SkillPoints += points;
            TotalPoints += points;
        }
    }
}
