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
public abstract class PoolingController : PoolingData
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
        iterator++;
        if (iterator >= poolSize)
            iterator = 0;
        GameObject current = instances[iterator];
        
        return current;
    }
    internal void DeactivateInstance(int id)
    {
        instances[id].SetActive(false);
    }
}
public class LlamaManager : PoolingController
{
    public Vector2 boundariesMinMax = new Vector2(-18, 18);

    public GameObject currencyPrefab;
    private void Awake()
    {
        SetupPooling();
    }
    public void SpawnLlamaRandom()
    {
        Transform current = PoolNext().transform;
        current.position = new Vector3(Random.Range(boundariesMinMax.x, boundariesMinMax.y), 1.0f, Random.Range(boundariesMinMax.x, boundariesMinMax.y));
        Llama currentLlama = current.GetComponent<Llama>();
        currentLlama.Id = iterator;
        currentLlama.OnCaptured.AddListener(delegate { OnLlamaCaptured(currentLlama.Id);});
        current.gameObject.SetActive(true);
    }
    public void OnLlamaCaptured (int llamaId)
    {
        Llama capturedLlama = instances[llamaId].GetComponent<Llama>();
        DropCurrency(capturedLlama.DropQuantity, capturedLlama.transform.position);
        DeactivateInstance(llamaId);
    }
    public void DropCurrency(int qtd, Vector3 pos)
    {
        for (int i = 0; i < qtd; i++)
        {
            Vector3 _randomPosition = transform.position + new Vector3(Random.Range(0.05f, 0.1f), 0, Random.Range(0.05f, 0.1f));
            Vector3 _randomRotation = new Vector3(0, Random.Range(0.0f, 180.0f), 0);
            Transform t = Instantiate(currencyPrefab, pos + _randomPosition, Quaternion.Euler(_randomRotation), null).transform;
            t.position = new Vector3(t.position.x, 1.05f, t.position.z);
        }
    }
}
