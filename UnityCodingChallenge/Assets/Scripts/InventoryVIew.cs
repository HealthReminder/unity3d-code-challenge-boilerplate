using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InventoryView : MonoBehaviour
{
    [SerializeField] public GameObject templateEntry;
    public Transform ItemsContainer;
    [HideInInspector] public List<InventoryEntry> entries;
    private void Awake()
    {
        entries = new List<InventoryEntry>();
    }
    public void DisplayInventory(Dictionary<ItemType, int> itemToCount)
    {
        if (itemToCount == null)
            Debug.LogError("Tried to displayed a null reference inventory.");

        ClearDisplay();
        ItemType[] items = itemToCount.Keys.ToArray();
        for (int i = 0; i < items.Length; i++)
        {
            //Debug.Log(paths[i]);

            GameObject prefab = Instantiate(Resources.Load(PersistentData.GetItemResourcePath(items[i])) as GameObject);
            prefab.SetActive(false);

            PickableItem item = prefab.GetComponent<PickableItem>();
            InventoryEntry entry = Instantiate(templateEntry, transform.position, Quaternion.identity, ItemsContainer).GetComponent<InventoryEntry>();
            prefab.transform.SetParent(entry.transform);
            string itemName = item.itemType.ToString();
            string quantityText = itemToCount[item.itemType].ToString();
            string priceText = item.Price.ToString();
            entry.SetInfo(item.itemType,itemName, quantityText, priceText);
            entry.gameObject.SetActive(true);
        }

    }
    public void ClearDisplay()
    {
        if (entries != null)
        {
            if(entries.Count > 0)
            {
                for (int i = entries.Count-1; i >= 0; i--)
                {
                    Destroy(entries[i].gameObject);
                }
            }
        }
        entries = new List<InventoryEntry>();
    }
}
