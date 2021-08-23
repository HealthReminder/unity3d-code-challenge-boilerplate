using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] public ItemType itemType;
    //Pickable Items can have as many variables as the designers want
    //To enable a new variable the code to handle it needs to be added 
    //to the OnTriggerPortion of the Player script
    public bool isConsumable = false;
    [Header("Pickup gains")]
    public int Price;
    public int MoneyGain;
    public int HealthGain;


    public void Collect(out int moneyGain, out int healthGain, out string prefabPath)
    {
        //The different parameters on this class could be expanded
        //To support not only money gain but also health, respect, points
        moneyGain = MoneyGain;
        healthGain = HealthGain;
        prefabPath = $"Items/{itemType}";
        Destroy(gameObject);
    }
}
