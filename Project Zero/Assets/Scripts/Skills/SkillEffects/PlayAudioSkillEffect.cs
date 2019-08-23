using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/PlayAudioSkillEffect", order = 1)]
    public class PlayAudioSkillEffect : SkillEffect
    {
        public AudioClip audioClip;

        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            if (audioClip == null)
            {
                skill.AudioSource.PlayOneShot(skill.AudioSource.clip);
            }
            else
            {
                skill.AudioSource.PlayOneShot(audioClip);
            }
        }
    }
}