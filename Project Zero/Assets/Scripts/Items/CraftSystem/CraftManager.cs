using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Items.CraftSystem
{
    public class CraftManager : MonoBehaviour
    {
        public RecipeList recipeList;

        public bool Craft(Unit playerUnit, Recipe recipe)
        {
            if (playerUnit != null && recipe != null)
            {
                EquipmentManager equipmentManager = playerUnit.unitController.equipmentManager;
                if (equipmentManager != null && equipmentManager.HasItemList(recipe.ingredients))
                {
                    equipmentManager.RemoveItemList(recipe.ingredients);
                    Item item = Instantiate<Item>(recipe.item);
                    equipmentManager.PickItem(item);
                    return true;
                }
            }
            return false;
        }
    }
}
