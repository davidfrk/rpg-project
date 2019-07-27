using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestsManager : MonoBehaviour
{
    public PlayerController questOwner;
    public Quest quest;

    public UnityEvent OnGameStart;

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
    }

    public void OnSlay(Unit prey)
    {
        quest?.OnSlay(prey);
    }
}
