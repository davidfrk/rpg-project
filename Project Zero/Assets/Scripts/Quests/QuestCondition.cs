using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuestCondition
{
    public bool completed;
    //public delegate void OnCompleteEvent();
    //public event OnCompleteEvent OnCompleteCallback;

    public ConditionType conditionType;
    public Item item;
    public Unit targetUnit;
    public Goal goal;

    public bool Update(Unit owner)
    {
        switch (conditionType)
        {
            case ConditionType.Item:
                {
                    completed = CheckItemCondition(owner);
                    break;
                }
            case ConditionType.Goal:
                {
                    completed = CheckGoalCondition(owner);
                    break;
                }
            default:
                {
                    break;
                }
        }
        return completed;
    }

    private bool CheckItemCondition(Unit owner)
    {
        Inventory inventory = owner.GetComponent<Inventory>();

        if (inventory != null)
        {
            foreach (Item item in inventory.Items)
            {
                if (item != null && this.item && item.id == this.item.id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckGoalCondition(Unit owner)
    {
        if (goal != null)
        {
            return (goal.transform.position - owner.transform.position).magnitude < goal.radius;
        }
        else
        {
            return false;
        }
    }

    public void OnSlay(Unit prey)
    {
        if (conditionType == ConditionType.Slay)
        {
            if (prey == targetUnit)
            {
                completed = true;
            }
        }
    }
}

public enum ConditionType
{
    Item,
    Slay,
    Goal
}
