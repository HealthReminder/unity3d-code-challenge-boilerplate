using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public string ItemName;
    //Pickable Items can have as many variables as the designers want
    //To enable a new variable the code to handle it needs to be added 
    //to the OnTriggerPortion of the Player script
    public int MoneyGain;
    public int HealthGain;


    public void Collect(out int moneyGain, out int healthGain)
    {
        //The different parameters on this class could be expanded
        //To support not only money gain but also health, respect, points
        moneyGain = MoneyGain;
        healthGain = HealthGain;
        Destroy(gameObject);
    }
}