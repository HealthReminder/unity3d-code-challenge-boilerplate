using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public abstract class MatchProperties: MonoBehaviour
{
    internal int initialLlamaQuantity = 7; 
}
public class MatchManager : MatchProperties
{
    public LlamaPool LlamaPool;
    public bool isMatch = false;
    private void Start()
    {
        isMatch = true;
        StartCoroutine(WorkMatch());
    }
    IEnumerator WorkMatch()
    {
        Debug.Log("A new match was triggered. Setting up.");
        for (int i = 0; i < initialLlamaQuantity; i++)
        {
            LlamaPool.SpawnLlamaRandom();
        }
        Debug.Log("Spawned Llamas.");

        Debug.Log("A new match started.");

        while (isMatch)
        {

            yield return null;
        }
        yield break;
    }


}
