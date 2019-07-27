using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public Quest quest;
    public QuestConditionUI conditionUIPrefab;
    public Transform questTransform;

    List<QuestConditionUI> conditionUIList = new List<QuestConditionUI>();

    void Start()
    {
        QuestManager.questManager.OnQuestUpdateCallback += InitiateUI;
        Quest newQuest = QuestManager.questManager.quest;
        if (newQuest != null)
        {
            InitiateUI(newQuest);
        }
    }

    void Update()
    {
        UpdateUI();
    }

    public void InitiateUI(Quest quest)
    {
        this.quest = quest;

        foreach(QuestConditionUI condition in conditionUIList)
        {
            Destroy(condition.gameObject);
        }
        conditionUIList.Clear();

        for (int i = 0; i < quest.questConditions.Count; i++)
        {
            QuestConditionUI newConditionUI = Instantiate<QuestConditionUI>(conditionUIPrefab, questTransform);
            conditionUIList.Add(newConditionUI);
        }
    }

    public void UpdateUI()
    {
        if (quest != null && quest.questConditions.Count == conditionUIList.Count)
        {
            for (int i = 0; i < conditionUIList.Count; i++)
            {
                conditionUIList[i].UpdateUI(quest.questConditions[i]);
            }
        }
    }
}
