using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool enableStatsUpgrade = true;
    public int talentTreeMinimumNumberOfTalents = 3;
    public int talentTreeMaximumNumberOfTalents = 4;
    public ItemsList itemsList;

    void Awake()
    {
        instance = this;
    }
}
