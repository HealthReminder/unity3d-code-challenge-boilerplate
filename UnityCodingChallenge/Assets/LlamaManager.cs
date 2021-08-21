using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PoolingData : MonoBehaviour
{
    public GameObject LlamaPrefab;
    public int poolSize;
    internal GameObject[] instances;                    //List all instances being pooled
    internal List<int> freeIds = new List<int>();       //Keeps track of the objects not in use (free to allocate)

}
public abstract class PoolingController : PoolingData
{
    internal void SetupPooling()
    {
        instances = new GameObject[poolSize];
        freeIds = new List<int>();
        for (int i = 0; i < poolSize; i++)
        {
            instances[i] = Instantiate(LlamaPrefab.gameObject, transform);
            instances[i].name = $"Llama 0{i}";
            DeactivateInstance(i);
        }
    }
    internal int PoolNext()
    {
        if(freeIds.Count <= 0)
            Debug.LogError("Tried to fetch a pooled object but all are in use. Consider increasing the pool size.");

        int randomIndex = Random.Range(0, freeIds.Count - 1);
        int Id = freeIds[randomIndex];
        freeIds.RemoveAt(randomIndex);
        return Id;
    }
    internal void DeactivateInstance(int id)
    {
        instances[id].SetActive(false);
        freeIds.Add(id);
    }
}
public class LlamaManager : PoolingController
{
    //This scripts is responsible for the results of the interactions between player and llamas
    //This script could be generic but it seems that Llamas are an integral part of this project

    public Vector2 boundariesMinMax = new Vector2(-18, 18); //Coordinates that set a square boundary for spawning Llamas
    public GameObject currencyPrefab;                       //The object for the coins that will drop
    List<float> nextLlamaCountdown;      //A list of timestamps to schedule new llama spawns
    private void Awake()
    {
        nextLlamaCountdown = new List<float>();
        SetupPooling();
    }
    private void Update()
    {
        if (nextLlamaCountdown.Count > 0)
        {
            for (int i = nextLlamaCountdown.Count - 1; i >= 0; i--)
            {
                nextLlamaCountdown[i] -= Time.deltaTime;
                if (nextLlamaCountdown[i] <= 0)
                {
                    SpawnLlamaRandom();
                    nextLlamaCountdown.RemoveAt(i);
                }
            }
        }
    }
    public void SpawnLlamaRandom()
    {
        Debug.Log("Here");
        int newId = PoolNext();
        Transform current = instances[newId].transform;
        Llama currentLlama = current.GetComponent<Llama>();
        currentLlama.PoolId = newId;
        currentLlama.OnCaptured.AddListener(delegate { OnLlamaCaptured(currentLlama.PoolId);});
        current.position = new Vector3(Random.Range(boundariesMinMax.x, boundariesMinMax.y), 1.0f, Random.Range(boundariesMinMax.x, boundariesMinMax.y));
        current.gameObject.SetActive(true);
    }

    public void OnLlamaCaptured (int poolId)
    {
        Llama capturedLlama = instances[poolId].GetComponent<Llama>();
        DropCurrency(capturedLlama.DropQuantity, capturedLlama.transform.position); //Drop coins
        DeactivateInstance(poolId);                                                 //Disappear
        nextLlamaCountdown.Add(Random.Range(4.0f, 6.0f));                           //Schedule next llama spawn
    }
    public void DropCurrency(int qtd, Vector3 pos)
    {
        for (int i = 0; i < qtd; i++)
        {
            Vector3 _randomPosition = transform.position + new Vector3(Random.Range(0.1f, 0.2f), 0, Random.Range(0.1f, 0.2f));
            Vector3 _randomRotation = new Vector3(0, Random.Range(0.0f, 180.0f), 0);
            Transform t = Instantiate(currencyPrefab, pos + _randomPosition, Quaternion.Euler(_randomRotation), null).transform;
            t.position = new Vector3(t.position.x, 1.05f, t.position.z);
        }
    }
}
