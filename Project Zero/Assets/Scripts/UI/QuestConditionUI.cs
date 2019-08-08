using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestConditionUI : MonoBehaviour
{
    public Text text;
    private QuestCondition condition;
    
    public void Awake()
    {
        //text = GetComponent<Text>();
    }

    public void InitiateUI(QuestCondition condition)
    {
        this.condition = condition;
        condition.OnChangeCallback += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (text == null) return;
        switch (condition.conditionType)
        {
            case ConditionType.Goal:
                {
                    text.text = "Go to " + condition.goal.gameObject.name + ".";
                    break;
                }
            case ConditionType.Slay:
                {
                    text.text = "Slay " + condition.targetUnit.gameObject.name + " : " + condition.currentAmount + "/" + condition.amount;
                    break;
                }
            case ConditionType.Item:
                {
                    text.text = "Get " + condition.item.gameObject.name + " : " + condition.currentAmount + "/" + condition.amount;
                    break;
                }
        }
    }
}
