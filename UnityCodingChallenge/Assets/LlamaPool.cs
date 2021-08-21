using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PoolingData : MonoBehaviour
{
    public GameObject LlamaPrefab;
    public int poolSize;
    internal int iterator = 0;
    internal GameObject[] instances;

}
public abstract class PoolController : PoolingData
{
    internal void SetupPooling()
    {
        instances = new GameObject[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            instances[i] = Instantiate(LlamaPrefab.gameObject, transform);
            instances[i].name = $"Llama 0{i}";
            instances[i].SetActive(false);
        }
    }
    internal GameObject PoolNext()
    {
        GameObject current = instances[iterator];
        iterator++;
        return current;
    }
}
public class LlamaPool : PoolController
{
    public Vector2 boundariesMinMax = new Vector2(-18, 18);
    private void Awake()
    {
        SetupPooling();
    }
    public void SpawnLlamaRandom()
    {
        Transform current = PoolNext().transform;
        current.position = new Vector3(Random.Range(boundariesMinMax.x, boundariesMinMax.y), 1.0f, Random.Range(boundariesMinMax.x, boundariesMinMax.y));
        current.gameObject.SetActive(true);
    }

}
