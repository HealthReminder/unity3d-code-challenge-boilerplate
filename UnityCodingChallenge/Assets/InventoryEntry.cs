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
    public Button Button;
    public TextMeshProUGUI textButton;
    public void SetInfo(string name, string description, string price)
    {
        textName.text = name;
        textDescription.text = description;
        if (textButton)
            textButton.text = "$"+price;
    }
    public void SetMerchandise(bool isMerchandise)
    {
        if (isMerchandise)
            if (Button)
                Button.gameObject.SetActive(true);
        else
            if (Button)
                Button.gameObject.SetActive(false);
    }
}
