using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg.Quest
{
    [System.Serializable]
    public class Quest : MonoBehaviour
    {
        public bool started = false;
        public bool completed = false;
        public List<QuestCondition> questConditions;
        public Reward reward;

        public UnityEvent OnComplete;
        private QuestsManager questsManager;
        private PlayerController owner;

        void Awake()
        {
            questsManager = GetComponentInParent<QuestsManager>();
        }

        public void StartQuest()
        {
            started = true;
            //Debug.Log("Iniciou quest " + gameObject.name);
            questsManager.AddQuest(this);
        }

        public bool UpdateState(Unit ownerUnit)
        {
            if (!started) return false;

            bool state = true;
            foreach (QuestCondition condition in questConditions)
            {
                condition.Update(ownerUnit);
                if (condition.completed == false)
                {
                    state = false;
                }
            }

            if (completed == false && state == true)
            {
                Complete();
            }

            return completed;
        }

        public void OnSlay(Unit prey)
        {
            foreach (QuestCondition condition in questConditions)
            {
                condition.OnSlay(prey);
            }
        }

        public void Complete()
        {
            completed = true;
            Debug.Log("Completou quest " + gameObject.name);
            reward.RewardPlayer(PlayerController.localPlayer);
            OnComplete.Invoke();
        }
    }
}
