using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    static public QuestManager questManager;
    public PlayerController questOwner;
    public Quest quest;

    public UnityEvent OnGameStart;

    public delegate void QuestUpdateEvent(Quest quest);
    public event QuestUpdateEvent OnQuestUpdateCallback;

    void Awake()
    {
        questManager = this;
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
            quest?.UpdateState(selectedUnit);
        }
    }

    public void AddQuest(Quest quest)
    {
        this.quest = quest;
        OnQuestUpdateCallback?.Invoke(quest);
    }

    public void OnSlay(Unit prey)
    {
        quest?.OnSlay(prey);
    }
}
