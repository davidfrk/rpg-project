using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Stats;
using Rpg.Skills.SkillEvent;

namespace Rpg.Skills.Effects
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillEffect/TeleportSkillEffect")]
    public class TeleportSkillEffect : SkillEffect
    {
        public TeleportType teleportType;

        public override void Cast(OnSkillEvent skillEvent, Skill skill)
        {
            switch (teleportType)
            {
                case TeleportType.RestPoint:
                    {
                        skill.Owner.unitController.Teleport(skill.Owner.GetComponent<UnitRespawn>().SpawnPosition, skill.Owner.transform.rotation);
                        break;
                    }
                case TeleportType.Owner:
                    {
                        skill.Owner.unitController.Teleport(skill.Owner.transform.position, skill.Owner.transform.rotation);
                        break;
                    }
            }
        }

        public enum TeleportType
        {
            RestPoint,
            Owner,
        }
    }
}