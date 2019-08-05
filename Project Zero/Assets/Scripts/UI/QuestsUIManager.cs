using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Quest;

public class QuestsUIManager : MonoBehaviour
{
    public Transform questsTransform;
    public QuestUI questUIPrefab;

    QuestsManager questsManager;
    List<QuestUI> questUIList = new List<QuestUI>();
    private bool dirty = true;

    void Start()
    {
        questsManager = QuestsManager.questsManager;
        questsManager.OnQuestUpdateCallback += SetDirty;
    }

    void InitiateUI()
    {
        foreach (QuestUI questUI in questUIList)
        {
            Destroy(questUI.gameObject);
        }
        questUIList.Clear();

        for (int i = 0; i < questsManager.activeQuests.Count; i++)
        {
            QuestUI newQuestUI = Instantiate<QuestUI>(questUIPrefab, questsTransform);
            newQuestUI.InitiateUI(questsManager.activeQuests[i]);
            questUIList.Add(newQuestUI);
        }
    }
    
    void Update()
    {
        if (dirty)
        {
            InitiateUI();
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (questUIList.Count == questsManager.activeQuests.Count)
        {
            for (int i = 0; i < questUIList.Count; i++)
            {
                questUIList[i].UpdateUI();
            }
        }
    }

    public void SetDirty()
    {
        dirty = true;
    }
}
