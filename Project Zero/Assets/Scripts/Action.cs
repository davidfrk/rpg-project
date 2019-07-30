using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Action
{
    public ActionType actionType;
    public Unit targetUnit;
    public Item item;
    public Skill skill;
    //public Vector3 targetPosition;

    public enum ActionType
    {
        Attack,
        PickItem,
        Cast
    }
}
