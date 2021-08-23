using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
internal static class PlayerInfo
{
    static internal int money;
    static internal int health;

    public static Dictionary<ItemType, int> itemToCount;
    static public void LogInfo()
    {
        string s = $"Player stored persistent data:\n";
        s += $"Money {money}\n";
        s += $"Health {health}\n";
        ItemType[] items = itemToCount.Keys.ToArray();
        int[] itemCount = itemToCount.Values.ToArray();
        for (int i = 0; i < items.Length; i++)
        {
            string itemName = items[i].ToString();
            s += $"{itemName} {itemCount[i]}\n";
        }
        Debug.Log(s);
    }

}

public static class PersistentData
{
    public static string GetItemResourcePath(ItemType item)
    {
        //Utility Function
        //Returns the path to a gameObject prefab by its ItemType
        //Usually I have a Helper class that has these
        return "Items/" +item.ToString();
    }
    //Save to the "cloud"
    public static void SavePlayerInfo(int coins, int health, Inventory inventory)
    {
        PlayerInfo.money = coins;
        PlayerInfo.health = health;
        if(inventory!= null)
            PlayerInfo.itemToCount = inventory.itemToCount;

    }
    public static void SaveItemBought(int coins, Inventory inventory)
    {
        PlayerInfo.money = coins;
        PlayerInfo.itemToCount = inventory.itemToCount;

    }
 
    //Retrieve from the "cloud"
    public static void GetPlayerInfo(out int coins, out int health, out Dictionary<ItemType, int> ItemToCount)
    {
        coins = PlayerInfo.money;
        health = PlayerInfo.health;
        ItemToCount = PlayerInfo.itemToCount;

    }
    public static Inventory GetPlayerInventory()
    {
        return new Inventory(PlayerInfo.itemToCount);
    }
    public static int GetPlayerCoins()
    {
        return PlayerInfo.money;
    }
    public static void Debug()
    {
        PlayerInfo.LogInfo();
    }
}
