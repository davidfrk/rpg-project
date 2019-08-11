using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Items;

namespace Rpg.UI
{
    public class ItemPickUpUI : MonoBehaviour
    {
        public ItemPickUpTooltip itemPickUpTooltip;

        void OnPickUp(Item item)
        {
            ItemPickUpTooltip tooltip = Instantiate<ItemPickUpTooltip>(itemPickUpTooltip, transform);
            tooltip.UpdateUI(item);
        }

        void Start()
        {
            PlayerController.localPlayer.OnItemPickUpCallback += OnPickUp;
        }
    }
}
