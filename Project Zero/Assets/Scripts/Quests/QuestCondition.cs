using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestCondition
{
    public bool completed;
    //public delegate void OnCompleteEvent();
    //public event OnCompleteEvent OnCompleteCallback;

    public ConditionType conditionType;
    public Item item;
    public Unit targetUnit;
    public Goal goal;
    public int amount;
    public int currentAmount;

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
        if (this.item == null)
        {
            return true;
        }

        EquipmentManager equipmentManager = owner.GetComponent<EquipmentManager>();

        if (equipmentManager != null)
        {
            currentAmount = 0;
            
            foreach (EquipmentSlot equipmentSlot in equipmentManager.equipmentSlots)
            {
                Item item = equipmentSlot.equipment?.item;
                if (item != null && item.id == this.item.id)
                {
                    currentAmount++;
                }
            }
            
            foreach (Item item in equipmentManager.inventory.Items)
            {
                if (item != null && item.id == this.item.id)
                {
                    currentAmount++;
                }
            }
        }

        return currentAmount >= amount;
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
            if (prey.id == targetUnit.id)
            {
                currentAmount = Mathf.Min(currentAmount + 1, amount) ;

                if (currentAmount >= amount)
                {
                    completed = true;
                }
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
