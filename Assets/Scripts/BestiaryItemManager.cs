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

    public Transform[] differentPos;

    [Header("Detonation System")] // Potions only

    public bool isAbleToToggleDetonation;





    void Start()
    {
        isAbleToToggleDetonation = true;
        if(deleteItemsOnGround == false)
        {
            hidderPos.position = differentPos[1].position;
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
            hidderPos.position = differentPos[0].position;
        }

        else
        {
            hidderPos.position = differentPos[1].position;
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

    public void DetonateForGreenItems()
    {
        StartCoroutine(cooldownForDetonation(0.2f));
    }

    IEnumerator cooldownForDetonation(float cooldown)
    {
        if(isAbleToToggleDetonation == true)
        {
            isAbleToToggleDetonation = false;
            GameManager.instance.activeStrangePotion = true;
            yield return new WaitForSeconds(cooldown);
            GameManager.instance.activeStrangePotion = false;
            isAbleToToggleDetonation = true;
        }
    }


}
