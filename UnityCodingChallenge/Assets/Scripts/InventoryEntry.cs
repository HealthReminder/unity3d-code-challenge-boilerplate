using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[System.Serializable] public class ItemEvent : UnityEvent<ItemType>
{
}
[System.Serializable] public class InventoryEntry : MonoBehaviour
{
    public ItemType Item;
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextDescription;
    public Button Button;
    public TextMeshProUGUI TextButton;
    [SerializeField] public ItemEvent OnAction;
    public void OnButtonPressed()
    {
        OnAction.Invoke(Item);
    }
    public void SetInfo(ItemType item ,string name, string description, string price)
    {
        Item = item;
        TextName.text = name;
        TextDescription.text = description;
        if (TextButton)
            TextButton.text = "$"+price;
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
