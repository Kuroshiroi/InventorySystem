using UnityEngine;

namespace Unity.Game.Inventory.UI
{
    public class UI_PlaceHolderFollower : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private UI_InventoryItem item;

        public void Awake()
        {
            canvas = transform.root.GetComponent<Canvas>();
            item = GetComponentInChildren<UI_InventoryItem>();
        }

        public void SetData(Sprite image, int quantity)
        {
            item.SetData(image, quantity);
        }

        private void Update()
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)canvas.transform,
                Input.mousePosition,
                canvas.worldCamera,
                out position
                );
            transform.position = canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool value)
        {
            gameObject.SetActive(value);
        }

    }
}