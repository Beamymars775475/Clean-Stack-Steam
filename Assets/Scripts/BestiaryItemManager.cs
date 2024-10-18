using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestiaryItemManager : MonoBehaviour
{
    public bool deleteItemsOnGround;

    public RectTransform hidderPos;

    public int limitItemsAmount = 0;

    public TextMeshProUGUI limitItemUI;

    [Header("Killing all items ")] // Potions only

    public Transform stockItemsThatCanBePotion;

    public Transform stockItems;
    void Start()
    {
        if(deleteItemsOnGround == false)
        {
            hidderPos.position = new Vector3(hidderPos.position.x+120f, hidderPos.position.y, hidderPos.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDeathItemsOnGround()
    {
        if(deleteItemsOnGround == false)
        {
            hidderPos.position = new Vector3(hidderPos.position.x-120f, hidderPos.position.y, hidderPos.position.z);
        }

        else
        {
            hidderPos.position = new Vector3(hidderPos.position.x+120f, hidderPos.position.y, hidderPos.position.z);
        }

        deleteItemsOnGround = !deleteItemsOnGround;
    }

    public void UpdateLimit()
    {
        limitItemUI.text = "Limit: " + limitItemsAmount.ToString() + "/25";
    }

    public void KillAllItems()
    {
        foreach(Transform child in stockItemsThatCanBePotion)
        {
            Destroy(child.gameObject);
            limitItemsAmount--;
            UpdateLimit();
        }

        foreach(Transform child in stockItems)
        {
            Destroy(child.gameObject);
            limitItemsAmount--;
            UpdateLimit();
        }
    }
}
