using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Quest : MonoBehaviour
{
    public bool started = false;
    public bool completed = false;
    public List<QuestCondition> questConditions;

    public UnityEvent OnComplete;
    private QuestsManager questsManager;

    void Awake()
    {
        questsManager = GetComponentInParent<QuestsManager>();
    }

    public void StartQuest()
    {
        started = true;
        Debug.Log("Iniciou quest " + gameObject.name);
        questsManager.AddQuest(this);
    }

    public bool UpdateState(Unit owner)
    {
        if (!started) return false;

        bool state = true;
        foreach(QuestCondition condition in questConditions)
        {
            condition.Update(owner);
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

    public void Complete()
    {
        completed = true;
        Debug.Log("Completou quest " + gameObject.name);
        OnComplete.Invoke();
    }
}
