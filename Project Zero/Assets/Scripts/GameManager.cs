using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemsList itemsList;

    void Awake()
    {
        instance = this;
    }
}
