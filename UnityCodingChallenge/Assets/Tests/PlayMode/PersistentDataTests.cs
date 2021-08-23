using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.TestTools;

public class PersistentDataTests
{

    [UnityTest]
    public IEnumerator TransferData_PlayerInventory_EmptySceneToShopScene()
    {
        Dictionary<ItemType, int> expectedInventoryItemToQuantity = new Dictionary<ItemType, int>()
        {
        {ItemType.Flowers, 1},
        {ItemType.Grass, 10},
        {ItemType.Rake, 100},
        {ItemType.Shrubs, 1000}
        };
        Inventory expectedInventory = new Inventory(expectedInventoryItemToQuantity);

        yield return null;

        PersistentData.SavePlayerInfo(0, 0, expectedInventory);

        AsyncOperation loadShopScene = SceneManager.LoadSceneAsync(1);
        loadShopScene.allowSceneActivation = true;
        while (!loadShopScene.isDone)
        {
            yield return null;
        }

        Inventory outputInventory = PersistentData.GetPlayerInventory();

        Assert.AreEqual(expected: expectedInventory.itemToCount, actual: outputInventory.itemToCount);

        yield return null;
    }
    [UnityTest]
    public IEnumerator TransferData_PlayerMoney_EmptySceneToShopScene()
    {
        
        int expectedCoins = 10;
        yield return null;

        PersistentData.SavePlayerInfo(expectedCoins, 0, null);

        AsyncOperation loadShopScene = SceneManager.LoadSceneAsync(1);
        while (!loadShopScene.isDone)
        {
            yield return null;
        }

        int outputCoins = PersistentData.GetPlayerCoins();
        PersistentData.GetPlayerInfo(out outputCoins, out int outputHealth, out Dictionary<ItemType, int> countList);

        Assert.AreEqual(expected: expectedCoins, actual: outputCoins);

        yield return null;
    }
}
