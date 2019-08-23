using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/ResetCameraSkillEffect")]
    public class ResetCameraSkillEffect : SkillEffect
    {
        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            CameraController.mainCamera.CenterOnUnit(skill.Owner);
        }
    }
}