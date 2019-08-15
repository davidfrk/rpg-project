using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.ProgressionSystem
{
    public class SkillTree : MonoBehaviour
    {
        public List<SkillBlock> skillBlocks = new List<SkillBlock>();

        private Unit unit;
        public int SkillPoints { get; private set; } = 10;

        void Awake()
        {
            unit = GetComponent<Unit>();
        }

        void Start()
        {
            Generate();
        }

        public void Generate()
        {
            for (int i = 0; i < 10; i++)
            {
                SkillBlock skillBlock = new SkillBlock();
                skillBlock.Generate(Skills.SkillTag.Attack, i + 1);
                skillBlocks.Add(skillBlock);
            }
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
            if (SkillPoints >= talent.Cost && talent.Acquired == false)
            {
                SkillPoints -= talent.Cost;
                Apply(talent);
            }
        }
    }
}
