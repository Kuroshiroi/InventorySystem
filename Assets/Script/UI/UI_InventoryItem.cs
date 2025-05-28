using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity.Game.Inventory.UI
{
    public class UI_InventoryItem : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private Image borderImage;

        [SerializeField]
        private TMP_Text quantityTxt;

        [SerializeField]
        private Image equipedIcon;
 
        public event Action<UI_InventoryItem> OnItemClicked,
            OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;

        private bool emptyState = true;

        public void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            emptyState = true;
            equipedIcon.gameObject.SetActive(false);
        }

        public void Deselect()
        {
            borderImage.enabled = false;
        }

        public void SetData(Sprite image, int quantity, bool equiped = false)
        {
            itemImage.sprite = image;
            quantityTxt.SetText(quantity.ToString());
            emptyState = false;
            itemImage.gameObject.SetActive(true);
            equipedIcon.gameObject.SetActive(equiped);
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (emptyState)
                return;
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (emptyState)
                return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}