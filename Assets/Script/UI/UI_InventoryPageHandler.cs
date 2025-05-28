using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Unity.Game.Inventory.UI
{
    public class UI_InventoryPageHandler : MonoBehaviour
    {
        [SerializeField]
        private UI_InventoryItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private UI_InventoryDescription inventoryDescription;

        [SerializeField]
        private UI_PlaceHolderFollower mouseFollower;
        [SerializeField]
        List<UI_InventoryItem> items = new List<UI_InventoryItem>();

        private int draggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
                OnItemActionsRequested, OnStartDragging;

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private UI_ItemActionPanel actionPanel;

        private void Awake()
        {
            Hide();

        }

        public void InitilizeInventoryUI(int size)
        {
            for (int i = 0; i < size; i++)
            {
                UI_InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(contentPanel);
                items.Add(item);
                item.OnItemClicked += HandleItemSelected;
                item.OnItemBeginDrag += HandleItemBeginDrag;
                item.OnItemEndDrag += HandleItemEndDrag;
                item.OnItemDroppedOn += HandleItemDrop;
                item.OnRightMouseBtnClick += HandleRightClick;

            }

        }

        public void ResetAllItems()
        {
            foreach(UI_InventoryItem item in items)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void UpdateData(int itemIndex, Sprite image, int quantity,bool equiped =false)
        {
            if (items.Count > itemIndex)
            {
                items[itemIndex].SetData(image, quantity,equiped);
            }
        }

        public void UpdateDescription(int index, Sprite image, string name, string description)
        {
            inventoryDescription.SetDescription(image, name, description);
            DeselectAllItems();
            items[index].Select();
        }

        private void DeselectAllItems()
        {
            foreach (UI_InventoryItem item in items)
            {
                item.Deselect();
            }
            actionPanel.Toogle(false);
        }

        public void ResetSelection()
        {
            inventoryDescription.ResetDescription();
            DeselectAllItems();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetSelection();
            ResetDraggedItem();
        }

        public void CreateDraggedItem(Sprite image, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(image, quantity);
        }

        public void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            draggedItemIndex = -1;
        }
        private void HandleItemSelected(UI_InventoryItem item)
        {
            int index = items.IndexOf(item);
            if (index < 0)
                return;
            OnDescriptionRequested?.Invoke(index);
        }
        private void HandleItemBeginDrag(UI_InventoryItem item)
        {
            int index = items.IndexOf(item);
            if (index < 0)
                return;
            draggedItemIndex = index;
            HandleItemSelected(item);
            OnStartDragging?.Invoke(index);

        }
        private void HandleItemEndDrag(UI_InventoryItem item)
        {
            Debug.Log("EDrag -> " + item.name);
            ResetDraggedItem();

        }
        private void HandleItemDrop(UI_InventoryItem item)
        {
            int index = items.IndexOf(item);
            if (index < 0)
            {
                return;
            }
            OnSwapItems?.Invoke(draggedItemIndex, index);
            HandleItemSelected(item);
        }

        private void HandleRightClick(UI_InventoryItem item)
        {
            Debug.Log("Right Clicked -> " + item.name);
            int index = items.IndexOf(item);
            if (index < 0)
                return;
            OnItemActionsRequested?.Invoke(index);
        }

        public void AddAction(string actionName, Action performAction)
        {
            if (actionPanel.isActiveAndEnabled)
            {
                actionPanel.Toogle(false);
            }
            actionPanel.AddButton(actionName, performAction);
        }

        public void ShowItemAction(int index)
        {
            actionPanel.Toogle(true);
            actionPanel.transform.position = items[index].transform.position;
        }
    }
}