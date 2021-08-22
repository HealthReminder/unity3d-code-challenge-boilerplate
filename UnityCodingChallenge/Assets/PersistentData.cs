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
    public static void UpdatePlayer(int coins, int health, Inventory inventory)
    {
        PlayerInfo.money = coins;
        PlayerInfo.health = health;
        PlayerInfo.itemToCount = inventory.itemToCount;
        PlayerInfo.itemToPath = inventory.itemToPath;

    }
    public static void GetPlayerInventory(out Dictionary<ItemType, int> ItemToCount, out Dictionary<ItemType, string> ItemToPath)
    {
        ItemToCount = PlayerInfo.itemToCount;
        ItemToPath = PlayerInfo.itemToPath;
    }
    public static void Debug()
    {
        PlayerInfo.LogInfo();
    }
}
