using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable] public class ShopOption
{
    public ItemType item;
    public int quantity;
}
public class Shop : InventoryView
{
    public TextMeshProUGUI coinsText;
    public int playerCoins = 0;
    public ShopOption[] options;
    public Inventory shopInventory;
    public Inventory playerInventory;
    private void OnEnable()
    {
        playerCoins = PersistentData.GetPlayerCoins();
        playerInventory = PersistentData.GetPlayerInventory();
        coinsText.text = "$"+playerCoins.ToString();
        DisplayShopInventory();
    }
  
    [ContextMenu("Display Shop Inventory")] public void DisplayShopInventory()
    {
        Dictionary<ItemType, int> itemToCount = new Dictionary<ItemType, int>();
        for (int i = 0; i < options.Length; i++)
        {
            ItemType item = options[i].item;
            itemToCount.Add(item, options[i].quantity);
        }
        shopInventory = new Inventory(itemToCount);

        DisplayInventory(itemToCount);

    }

    internal void SetMerchandise()
    {
        if(entries != null)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].SetMerchandise(true);
            }
        }
    }
    public void BuyItem(ItemType Item)
    {
        Debug.Log($"Player is trying to buy item {Item}");
        PickableItem product = Instantiate(Resources.Load(PersistentData.GetItemResourcePath(Item)) as GameObject).GetComponent<PickableItem>();
        bool canBuy = true;
        if (playerCoins - product.Price < 0)
            canBuy = false;
        if (canBuy)
        {
            playerCoins -= product.Price;
            playerInventory.AddItem(product, ($"Items/{product.itemType}"));
            PersistentData.SaveItemBought(playerCoins, playerInventory);
            coinsText.text = "$" + playerCoins.ToString();

        }
        Destroy(product.gameObject);



    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
