using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rpg.Items;

namespace Rpg.UI
{
    public class ItemPickUpTooltip : MonoBehaviour
    {
        public Image itemImage;
        public float duration;

        public void UpdateUI(Item item)
        {
            itemImage.sprite = item.sprite;
            Invoke("Hide", duration);
        }

        public void Hide()
        {
            Destroy(gameObject);
        }
    }
}
