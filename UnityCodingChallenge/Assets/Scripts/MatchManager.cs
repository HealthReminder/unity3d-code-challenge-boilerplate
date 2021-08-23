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
    public LlamaManager LlamaPool;
    public Player Player;
    public bool isMatch = false;
    private void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        isMatch = true;
        StartCoroutine(WorkMatch());
    }
    IEnumerator WorkMatch()
    {
        Debug.Log("A new match is being set up.");
        for (int i = 0; i < initialLlamaQuantity; i++)
        {
            LlamaPool.SpawnLlamaRandom();
        }
        Debug.Log("Spawned Llamas.");

        Player.hasInput = true;
        Debug.Log("Enabled Player.");


        Debug.Log("A new match started.");

        while (isMatch)
        {
            //Winning conditions could be here
            yield return null;
        }
        yield break;
    }


}
