using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Items.CraftSystem
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item/RecipeList", order = 2)]
    public class RecipeList : ScriptableObject
    {
        public List<Recipe> recipes;
    }
}