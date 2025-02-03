using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverDescription : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text Description;
    public TMP_Text Price;
    public GameObject descBack;

    private Color color;
    private void Start()
    {
    /*    itemName.enabled = false;
        Description.enabled = false;
        Price.enabled = false;
        descBack.SetActive(false);*/
    }

    public void ShowDesc(string name, int price, string description)
    {
        itemName.enabled = true;
        itemName.text = name; 

        Description.enabled = true;
        Description.text = $"{description}";

        Price.enabled = true;
        Price.text = price.ToString();

        descBack.SetActive(true);
    }

    public void HideDesc() {
     /*   itemName.enabled = false;
        Description.enabled = false;
        Price.enabled = false;
        descBack.SetActive(false);*/
    }
}
