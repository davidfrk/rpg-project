using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Items;

namespace Rpg.UI {
    public class ShopUI : MonoBehaviour , IShopManager
    {
        public ShopItemTooltip shopItemTooltipPrefab;
        public Text goldText;
        public Transform contentTransform;

        private Shop shop;
        private List<ShopItemTooltip> tooltips = new List<ShopItemTooltip>();

        public void Open(Shop shop)
        {
            if (shop != null)
            {
                this.shop = shop;
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
            gameObject.SetActive(false);
            AudioManager.instance.PlaySound(AudioManager.UISound.CloseShop);
        }

        public void Update()
        {
            if (shop != null)
            {
                if ((PlayerController.localPlayer.MainUnit.transform.position - shop.transform.position).magnitude > (Shop.range + 0.5f))
                {
                    Close();
                }
            }
        }

        public void UpdateUI()
        {
            goldText.text = shop.gold.ToString();

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
            UIController.UIClick();
            shop.Sell(shopItemTooltip.Item, PlayerController.localPlayer);
            goldText.text = shop.gold.ToString();
        }
    }
}
