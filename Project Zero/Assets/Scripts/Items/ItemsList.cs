using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Items;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item/ItemsList", order = 3)]
public class ItemsList : ScriptableObject
{
    public List<Item> items;
}
