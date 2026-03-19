using System;
using UnityEngine;
using System.Collections.Generic;
using Item;

namespace Character
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private CharacterConfig _config;
        private List<ItemID> _inventory = new();
        private event Action<ItemID> OnItemAdded;
        private event Action<ItemID> OnItemRemoved;
        private event Action<ItemID> OnInventoryFull;

        private void Awake()
        {
            for (int i = 0; i < _config.MaxSlots; i++)
            {
                _inventory.Add(ItemID.None);
            }
        }


        public void Add(ItemID item)
        {
            for (int i = 0; i < _config.MaxSlots; i++)
            {
                if (_inventory[i] == ItemID.None)
                {
                    _inventory[i] = item;
                    OnItemAdded?.Invoke(item);
                    return;
                }
                
            }
            OnInventoryFull?.Invoke(item);
        }

        public void Remove(ItemID item)
        {
            for (int i = 0; i < _config.MaxSlots; i++)
            {
                if (_inventory[i] == item)
                {
                    _inventory[i] = ItemID.None;
                    OnItemRemoved?.Invoke(item);
                    return;
                }
            }
        }

        public void Remove(int slot)
        {
            if (slot < 0 || slot >= _inventory.Count) return;
            ItemID item = _inventory[slot];
            _inventory[slot] = ItemID.None;
            OnItemRemoved?.Invoke(item);
        }
    }
}