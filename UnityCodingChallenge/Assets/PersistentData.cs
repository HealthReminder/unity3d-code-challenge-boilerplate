using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
internal static class PlayerInfo
{
    static internal int coins;
    static internal int health;
    static internal Inventory inventory;
    static public void LogInfo()
    {
        string s = $"Player stored persistent data:\n";
        s += $"Coins {coins}\n";
        s += $"Health {health}\n";
        if (inventory)
        {
            Item[] items = inventory.itemToQuantity.Keys.ToArray();
            int[] itemCount = inventory.itemToQuantity.Values.ToArray();
            for (int i = 0; i < items.Length; i++)
            {
                string itemName = items[i].ToString();
                s += $"{itemName} {itemCount[i]}\n";
            }
        }
        Debug.Log(s);
    }
}

public static class PersistentData
{
    public static void UpdatePlayer(int coins, int health, Inventory inventory)
    {
        PlayerInfo.coins = coins;
        PlayerInfo.health = health;
        PlayerInfo.inventory = inventory;
    }
    public static void Debug()
    {
        PlayerInfo.LogInfo();
    }
}
