using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PersistentDataTests
{
    
    [UnityTest]
    public IEnumerator TransferData_InventoryFull_ChangeScene()
    {
        Player playerObject = new GameObject().AddComponent<Player>();
        Dictionary<ItemType, int> expectedInventoryItemToQuantity = new Dictionary<ItemType, int>()
        {
        {ItemType.Flowers, 1},
        {ItemType.Grass, 10},
        {ItemType.Rake, 100},
        {ItemType.Shrubs, 1000}
        };

        //playerObject.Inventory = new Inventory()

        yield return null;
    }
}
