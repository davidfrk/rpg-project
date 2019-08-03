using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Skills;
using Rpg.Items;

[System.Serializable]
public struct Action
{
    public ActionType actionType;
    public Unit targetUnit;
    public Item item;
    public Skill skill;
    public Vector3 targetPosition;

    public enum ActionType
    {
        None,
        Attack,
        PickItem,
        Cast,
        Move,
    }
}
