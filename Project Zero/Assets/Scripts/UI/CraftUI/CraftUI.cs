using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Items;
using Rpg.Items.CraftSystem;

namespace Rpg.UI {
    public class CraftUI : MonoBehaviour , ICraftManager
    {
        public RecipeTooltip craftItemTooltipPrefab;
        public Transform contentTransform;

        private CraftManager craftManager;
        private List<RecipeTooltip> tooltips = new List<RecipeTooltip>();

        public void Open(CraftManager craftManager)
        {
            if (craftManager != null)
            {
                this.craftManager = craftManager;
                UpdateUI();
                gameObject.SetActive(true);
                AudioManager.instance.PlaySound(AudioManager.UISound.OpenShop);
            }
            else
            {
                Close();
            }
        }

        public void Close()
        {
            UIController.HideItemTooltip();
            gameObject.SetActive(false);
            AudioManager.instance.PlaySound(AudioManager.UISound.CloseShop);
        }

        public void Toggle(CraftManager craftManager)
        {
            if (isActiveAndEnabled)
            {
                Close();
            }
            else
            {
                Open(craftManager);
            }
        }

        public void Update()
        {
            if (craftManager != null)
            {
                if ((PlayerController.localPlayer.MainUnit.transform.position - craftManager.transform.position).magnitude > (Shop.range + 0.5f))
                {
                    //Close();
                }
            }
        }

        public void UpdateUI()
        {
            foreach(RecipeTooltip tooltip in tooltips)
            {
                Destroy(tooltip.gameObject);
            }
            tooltips.Clear();

            foreach(Recipe recipe in craftManager.recipeList.recipes)
            {
                AddRecipeTooltip(recipe);
            }
        }

        private void AddRecipeTooltip(Recipe recipe)
        {
            RecipeTooltip tooltip = Instantiate<RecipeTooltip>(craftItemTooltipPrefab, contentTransform, false);
            tooltip.UpdateUI(recipe);
            tooltips.Add(tooltip);
        }

        public void OnMouseRightClickDown(RecipeTooltip recipeTooltip)
        {
            UIController.UIClick();
            craftManager.Craft(PlayerController.localPlayer.MainUnit, recipeTooltip.recipe);
        }
    }
}
