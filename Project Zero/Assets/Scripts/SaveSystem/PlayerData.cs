using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int gold;

    public void GetData(PlayerController player)
    {
        gold = player.gold;
    }

    public void Apply(PlayerController player)
    {
        player.gold = gold;
    }
}
