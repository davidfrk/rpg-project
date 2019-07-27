using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestConditionUI : MonoBehaviour
{
    public Text text;
    
    public void Awake()
    {
        //text = GetComponent<Text>();
    }

    public void UpdateUI(QuestCondition condition)
    {
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
