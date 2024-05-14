using System.Collections;
using UnityEngine;

public class ItemPUS : MonoBehaviour
{
    [field: SerializeField]
    public Item InventoryItem { get; set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;


    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
        if (InventoryItem != null)
            GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemIcon;

        // Set the sorting layer name
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingLayerName = "Player";
    }

    public void SetIcon(Item item)
    {
        if (item == null)
            return;
        InventoryItem = item;
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemIcon;
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());

    }

    private IEnumerator AnimateItemPickup()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}