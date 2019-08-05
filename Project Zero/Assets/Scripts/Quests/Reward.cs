using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Quest
{
    [System.Serializable]
    public struct Reward
    {
        public float exp;
        public int gold;

        public void RewardPlayer(PlayerController player)
        {
            player.gold += gold;
            player.selectedUnit?.experienceManager.AddExperience(exp);//ToDo: main unit
        }
    }
}
