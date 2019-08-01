using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg.Items
{
    public class Item : MonoBehaviour
    {
        public int id;
        public Sprite sprite;
        public ItemType itemType = ItemType.Item;
        public GameObject model;
        internal Item item;

        void Awake()
        {
            item = this;
        }

        public void SetState(ItemState state)
        {
            if (state == ItemState.InWorld)
            {
                model?.SetActive(true);
            }
            else
            {
                model?.SetActive(false);
            }
        }

        public enum ItemType
        {
            Item,
            Key,
            Equipment
        }

        public enum ItemState
        {
            InWorld,
            InInventory
        }
    }
}
