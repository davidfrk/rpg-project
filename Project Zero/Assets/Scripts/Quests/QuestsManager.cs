using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg.Quest
{
    public class QuestsManager : MonoBehaviour
    {
        static public QuestsManager questsManager;
        public PlayerController questOwner;
        public List<Quest> activeQuests;

        public UnityEvent OnGameStart;

        public delegate void QuestUpdateEvent();
        public event QuestUpdateEvent OnQuestUpdateCallback;

        void Awake()
        {
            questsManager = this;
        }

        public void Start()
        {
            questOwner = PlayerController.localPlayer;
            questOwner.OnKillCallback += OnSlay;
            OnGameStart.Invoke();
        }

        public void Update()
        {
            Unit selectedUnit = questOwner.selectedUnit;
            if (selectedUnit != null)
            {
                bool dirty = false;
                List<Quest> questsCopy = new List<Quest>(activeQuests);

                foreach (Quest quest in questsCopy)
                {
                    if (quest.UpdateState(selectedUnit))
                    {
                        activeQuests.Remove(quest);
                        dirty = true;
                    }
                }
                questsCopy.Clear();

                if (dirty)
                {
                    OnQuestUpdateCallback?.Invoke();
                }
            }
        }

        public void AddQuest(Quest quest)
        {
            activeQuests.Add(quest);
            OnQuestUpdateCallback?.Invoke();
        }

        public void OnSlay(Unit prey)
        {
            foreach (Quest quest in activeQuests)
            {
                quest.OnSlay(prey);
            }
        }
    }
}
