using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class InventoryEntry : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textButton;
    public void Set(string name, string description, string price)
    {
        textName.text = name;
        textDescription.text = description;
        if(textButton)
            textButton.text = price;
    }
}
