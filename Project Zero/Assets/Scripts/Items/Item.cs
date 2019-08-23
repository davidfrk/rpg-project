using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg.Items
{
    public class Item : MonoBehaviour
    {
        public int id;
        public string name;
        public int price;
        public Sprite sprite;
        public ItemType itemType = ItemType.Item;
        public GameObject model;
        internal Item item;
        public ItemState State { get; private set; } = ItemState.InWorld;

        void Awake()
        {
            item = this;
        }

        public void SetState(ItemState state)
        {
            if (state == ItemState.InWorld)
            {
                model?.SetActive(true);
                State = ItemState.InWorld;

            }
            else
            {
                model?.SetActive(false);
                State = ItemState.InInventory;
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
