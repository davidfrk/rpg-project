using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rpg.Items;
using Rpg.Stats;
using Rpg.Items.CraftSystem;

namespace Rpg.UI
{
    public class RecipeTooltip : MonoBehaviour, IPointerDownHandler
    {
        public Transform ingredientsTransform;
        public ItemUI itemUIPrefab;
        public ItemUI mainItemUI;
        public ICraftManager craftManager;
        internal Recipe recipe;

        void Awake()
        {
            craftManager = GetComponentInParent<ICraftManager>();
        }

        public void UpdateUI(Recipe recipe)
        {
            this.recipe = recipe;
            mainItemUI.UpdateUI(recipe.item);

            foreach (ItemQuantity itemQuantity in recipe.ingredients)
            {
                for (int i = 0; i < itemQuantity.amount; i++)
                {
                    AddItemUI(itemQuantity.item);
                }
            }
        }

        private void AddItemUI(Item item)
        {
            ItemUI itemUI = Instantiate<ItemUI>(itemUIPrefab, ingredientsTransform, false);
            itemUI.UpdateUI(item);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                craftManager.OnMouseRightClickDown(this);
            }
        }
    }
}
