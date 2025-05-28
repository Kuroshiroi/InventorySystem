using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Game.Inventory.UI
{
    public class UI_InventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private TMP_Text titleTxt;

        [SerializeField]
        private TMP_Text descriptionTxt;

        public void Awake()
        {
            ResetDescription();
        }

        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            itemImage.sprite = null;
            titleTxt.SetText("");
            descriptionTxt.SetText("");
        }

        public void SetDescription(Sprite image, string name, string description)
        {
            itemImage.sprite = image;
            titleTxt.SetText(name);
            descriptionTxt.SetText(description);
            itemImage.gameObject.SetActive(true);
        }
    }
}