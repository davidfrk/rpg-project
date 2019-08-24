using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Quest;

public class QuestUI : MonoBehaviour
{
    public Quest quest;
    public Text questName;
    public Transform conditionsTransform;
    public QuestConditionUI conditionUIPrefab;

    List<QuestConditionUI> conditionUIList = new List<QuestConditionUI>();

    public void InitiateUI(Quest quest)
    {
        this.quest = quest;
        questName.text = quest.gameObject.name;

        foreach(QuestConditionUI condition in conditionUIList)
        {
            Destroy(condition.gameObject);
        }
        conditionUIList.Clear();

        for (int i = 0; i < quest.questConditions.Count; i++)
        {
            QuestConditionUI newConditionUI = Instantiate<QuestConditionUI>(conditionUIPrefab, conditionsTransform);
            newConditionUI.InitiateUI(quest.questConditions[i]);
            conditionUIList.Add(newConditionUI);
        }
    }
}
