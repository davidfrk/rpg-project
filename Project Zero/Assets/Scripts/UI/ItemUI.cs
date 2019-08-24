using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Items;
using UnityEngine.EventSystems;

namespace Rpg.UI
{
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image image;
        public Text itemName;
        public Text cost;
        private Item item;

        public void UpdateUI(Item item)
        {
            this.item = item;
            image.sprite = item.sprite;
            itemName.text = item.name;
            cost.text = item.price.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UIController.ShowItemTooltip(item, eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIController.HideItemTooltip();
        }
    }
}