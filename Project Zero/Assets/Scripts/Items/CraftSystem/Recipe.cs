using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Items.CraftSystem {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item/Recipe", order = 1)]
    public class Recipe : ScriptableObject
    {
        public Item item;
        public List<ItemQuantity> ingredients;
    }
}
