using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestsManager : MonoBehaviour
{
    public Unit questOwner;
    public Quest quest;

    public UnityEvent OnGameStart;

    public void Start()
    {
        OnGameStart.Invoke();
    }

    public void Update()
    {
        quest?.UpdateState(questOwner);
    }

    public void AddQuest(Quest quest)
    {
        this.quest = quest;
    }
}
