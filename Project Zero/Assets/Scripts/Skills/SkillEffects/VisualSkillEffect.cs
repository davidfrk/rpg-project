using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/VisualSkillEffect", order = 1)]
    public class VisualSkillEffect : SkillEffect
    {
        public float duration = 10f;
        public GameObject visualEffectPrefab;

        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            GameObject visualEffect = Instantiate(visualEffectPrefab, skill.TargetUnit.transform, false);

            skill.StartCoroutine(Stop(visualEffect, duration));
        }

        IEnumerator Stop(GameObject visualEffect, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            Destroy(visualEffect);
        }
    }
}