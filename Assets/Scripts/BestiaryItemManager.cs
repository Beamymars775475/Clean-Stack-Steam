using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
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

    [Header("Furniture Skins")] // Potions only

    public GameObject[] whiteFurnitures;
    public GameObject[] blackFurnitures;
    public GameObject inventory;

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

    public void KillTable()
    {
        foreach(Transform items in stockItems.transform)
        {
            itemScript itemItemScript = items.GetComponent<itemScript>();
            if(itemItemScript != null)
            {
                if(itemItemScript.isTable)
                {
                    Destroy(items.gameObject);
                }
            }

        }
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

    public void SetFurnitureSkin(int skinIndex)
    {
        for (int i = 0; i < inventory.transform.childCount-3; i++) // -3 A CAUSE DES TRUCS DANS INV
        {
            ButtonSpawnItem buttonSpawnItem = inventory.transform.GetChild(i).GetComponent<ButtonSpawnItem>();
            itemScript itemScriptItem = buttonSpawnItem.itemPrefab.GetComponent<itemScript>();
            
            if(itemScriptItem.isTable)
            {
                if(skinIndex == 0) // WHITE
                {
                    if(itemScriptItem.itemID == 20) // TABLE
                    {
                        buttonSpawnItem.itemPrefab = whiteFurnitures[0];
                    }
                    else if(itemScriptItem.itemID == 21) // TABLE
                    {
                        buttonSpawnItem.itemPrefab = whiteFurnitures[1];
                    }
                    else if(itemScriptItem.itemID == 22) // TABLE
                    {
                        buttonSpawnItem.itemPrefab = whiteFurnitures[2];
                    }
                }

                else if(skinIndex == 1) // BLACK
                {
                    if(itemScriptItem.itemID == 20) // TABLE
                    {
                        buttonSpawnItem.itemPrefab = blackFurnitures[0];
                    }
                    else if(itemScriptItem.itemID == 21) // TABLE
                    {
                        buttonSpawnItem.itemPrefab = blackFurnitures[1];
                    }
                    else if(itemScriptItem.itemID == 22) // TABLE
                    {
                        buttonSpawnItem.itemPrefab = blackFurnitures[2];
                    }
                }


                if(buttonSpawnItem.transform.childCount > 0)
                {
                    SpriteRenderer prefabTexture = buttonSpawnItem.itemPrefab.GetComponent<SpriteRenderer>();
                    buttonSpawnItem.saveOfContentTexture = prefabTexture.sprite;
                }
            }
        }
    }
}
