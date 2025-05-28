using Unity.Game.Inventory.Model;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickUpItem : MonoBehaviour
{

    [field: SerializeField]
    public ItemSO item { get; private set; }

    [field:SerializeField]
    public int quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
       
    }

    public void SetPickup(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    internal void DestroyItem()
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 StartScale = transform.localScale;
        float time = 0;
        while (time<duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(StartScale, Vector3.zero, time / duration) ;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
