using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Unity.Game.Inventory.Model
{
    [CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        List<InventoryItem> inventoryItems;

        [field: SerializeField]
        public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnChangedEvent;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }
        public bool IsInventoryFull()
        => !inventoryItems.Where(item => item.emptyState).Any();


        public bool UpdateItem(ItemSO item, int quantity, List<EquipParameter> equipState = null)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].emptyState)
                {
                    continue;
                }
                if (inventoryItems[i].item.ID == item.ID)
                {
                    inventoryItems[i] = InventoryItem.GetItem(item, quantity, equipState);
                    InformAboutChanges();
                    return true;
                }
            }
            return false;
        }
        public int AddItem(ItemSO item, int quantity, List<EquipParameter> equipState= null)
        {
            if (item.IsStackable == false)
            {
                //for (int i = 0; i < inventoryItems.Count; i++)
                //{
                    bool full = IsInventoryFull();
                    while (quantity > 0 && !full)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1, equipState);
                    }
                    InformAboutChanges();
                    return quantity;
                //}
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChanges();

            return quantity;

        }
        public int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<EquipParameter> equipState= null)
        {
            InventoryItem tmp = InventoryItem.GetItem(item, quantity, equipState);
                /*new InventoryItem 
            { 
                item = item, 
                quantity = quantity, 
            };*/

            for (int i=0; i<inventoryItems.Count; i++)
            {
                if (inventoryItems[i].emptyState)
                {
                    inventoryItems[i] = tmp;
                    return quantity;
                }
            }
            return 0;
        }

        //Non mi piace affatto 
        // Domanda : Io voglio che un oggettto che superi la MAxSize occupi un altro slot? :/ 
        public int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].emptyState)
                {
                    continue;
                }
                if(inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;
                    if (quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);
                        quantity = quantity - amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(quantity+inventoryItems[i].quantity);
                        quantity = 0;
                    }
                }
            }
            while(quantity >0 && IsInventoryFull() == false)
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }



        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].emptyState)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int index)
        {
            return inventoryItems[index];
        }

        public void SwapItems(int index1, int index2)
        {
            InventoryItem tmp = inventoryItems[index1];
            inventoryItems[index1] = inventoryItems[index2];
            inventoryItems[index2] = tmp;
            InformAboutChanges();
        }

        private void InformAboutChanges()
        {
            OnChangedEvent?.Invoke(GetCurrentInventoryState());
        }

        public void RemoveItem(int index, int amount )
        {
            if (inventoryItems.Count > index)
            {
                if (inventoryItems[index].emptyState)
                    return;
                int reminder = inventoryItems[index].quantity - amount;
                if (reminder <= 0)
                {
                    inventoryItems[index] = InventoryItem.GetEmptyItem();
                }
                else
                {
                    inventoryItems[index] = inventoryItems[index].ChangeQuantity(reminder);
                }
                InformAboutChanges();
            }
        }
    }



    [Serializable]
    public struct InventoryItem
    {
        public int quantity;
        public ItemSO item;
        public List<EquipParameter> equipState;

        public bool emptyState => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                quantity = newQuantity,
                item = this.item,
                equipState = new List<EquipParameter>(),
            };
        }

        public InventoryItem ChangeEquiped(List<EquipParameter> newEquipState)
        {
            return new InventoryItem
            {
                quantity = this.quantity,
                item = this.item,
                equipState = newEquipState,
            };
        }

        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            item = null,
            quantity = 0,
            equipState = new List<EquipParameter>(),
        };

        public static InventoryItem GetItem(ItemSO itemInput, int quantityInput, List<EquipParameter> equipInput= null)
            => new InventoryItem
            {
                item = itemInput,
                quantity = quantityInput,
                equipState = new List<EquipParameter>(equipInput ==null? itemInput.DefaultParametersList : equipInput),
            };
    }
}
