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
    public static Dictionary<ItemType, string> itemToPath;
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
        return "Items/"+item.ToString();
    }
    public static void UpdateLocalPlayer(out int coins, out int health, out Dictionary<ItemType, int> ItemToCount)
    {
        coins = PlayerInfo.money;
        health = PlayerInfo.health;
        ItemToCount = PlayerInfo.itemToCount;

    }
    public static void UpdateItemBought(int coins, Inventory inventory)
    {
        PlayerInfo.money = coins;
        PlayerInfo.itemToCount = inventory.itemToCount;

    }
    public static void UpdatePersistentPlayer(int coins, int health, Inventory inventory)
    {
        PlayerInfo.money = coins;
        PlayerInfo.health = health;
        PlayerInfo.itemToCount = inventory.itemToCount;

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
