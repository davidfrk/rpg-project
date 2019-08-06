using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Items;

namespace Rpg.UI {
    public class ShopUI : MonoBehaviour , IShopManager
    {
        public ShopItemTooltip shopItemTooltipPrefab;
        public Transform contentTransform;

        private Shop shop;
        private List<ShopItemTooltip> tooltips = new List<ShopItemTooltip>();

        public void Open(Shop shop)
        {
            this.shop = shop;
            UpdateUI();
            gameObject.SetActive(true);
            SoundManager.instance.PlaySound(SoundManager.UISound.OpenShop);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            SoundManager.instance.PlaySound(SoundManager.UISound.CloseShop);
        }

        public void UpdateUI()
        {
            foreach(ShopItemTooltip tooltip in tooltips)
            {
                Destroy(tooltip.gameObject);
            }
            tooltips.Clear();

            foreach(Item item in shop.items)
            {
                AddItemTooltip(item);
            }
        }

        private void AddItemTooltip(Item item)
        {
            ShopItemTooltip tooltip = Instantiate<ShopItemTooltip>(shopItemTooltipPrefab, contentTransform);
            tooltip.UpdateUI(item);
            tooltips.Add(tooltip);
        }

        public void OnMouseRightClickDown(ShopItemTooltip shopItemTooltip)
        {
            shop.Sell(shopItemTooltip.Item, PlayerController.localPlayer);
            UIController.instance.lastUIClick = Time.time;
        }
    }
}
