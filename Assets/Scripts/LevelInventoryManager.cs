using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.EventSystems;


public class LevelInventoryManager : MonoBehaviour
{
    [Header("-----Item var infos-----")]

    [Header("Feedbacks when opening (closing)")]

    public MMF_Player feedbacksInventory;

    [Header("Other")]
    public Transform cursor;
    public GameObject itemThrowed;

    public MMF_Player feedbacksBox;

    public Animator animator;
    [Header("---Event---")]
    public DescriptionOverButtonScript descriptionOverButtonScript;


    [Header("-----Level Inventory Manager-----")]
    public GameObject classicBox;
    public GameObject classicPotion;
    public GameObject[] listOfItemsOfTheLevel;
    public GameObject[] listOfPotionOfTheLevel;

    public GameObject inventoryParent;


    public int indexSpawningItems;


    void InvItemsShuffle(GameObject[] gbs)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < gbs.Length; t++ )
        {
            GameObject tmp = gbs[t];
            int r = Random.Range(t, gbs.Length);
            gbs[t] = gbs[r];
            gbs[r] = tmp;
        }
    }

    void Start()
    {
        InvItemsShuffle(listOfItemsOfTheLevel);
        InvItemsShuffle(listOfPotionOfTheLevel);
        
        indexSpawningItems = 0;
        foreach (GameObject gbItemPrefab in listOfItemsOfTheLevel)
        {

            Debug.Log("Bou!!!");
            GameObject newPrefab = Instantiate(classicBox);

            newPrefab.transform.SetParent(inventoryParent.transform);
            newPrefab.transform.SetAsFirstSibling(); // Pour mettre la description devant les items

            // Seulement pour les items
            // Feedbacks when Description (Showing/Hiding content)
            GameObject gbShowingContent = newPrefab.transform.GetChild(0).gameObject; // Chopper la preview
            Image gbShowingContentTexture = gbShowingContent.GetComponent<Image>(); // Prendre l'ancienne texture

            SpriteRenderer newTexture = gbItemPrefab.GetComponent<SpriteRenderer>(); // Prendre la gueule de la preview
            gbShowingContentTexture.sprite = newTexture.sprite; // Appliquer


            // Prefab infos
            ButtonSpawnItem buttonSpawnItemPrefab = newPrefab.GetComponent<ButtonSpawnItem>();
            buttonSpawnItemPrefab.itemPrefab = gbItemPrefab;
            buttonSpawnItemPrefab.feedbacksInventory = feedbacksInventory;

            // Feedbacks when opening (closing)
            RectTransform rectTransformPrefab = newPrefab.GetComponentInChildren<RectTransform>();


            // Position
            if(indexSpawningItems % 2 == 0)
            {
                rectTransformPrefab.anchoredPosition = new Vector2((-675f+((975/listOfItemsOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-100f, 55f));
            }
            else
            {
                rectTransformPrefab.anchoredPosition = new Vector2((-675f+((975/listOfItemsOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-200f, -50f));
            }
            // Formule pour calculer la position : co de base + (longueur de la ligne pour spawn/le nombre d'objet) * l'index


            EventTrigger evTrig = newPrefab.GetComponent<EventTrigger>();
            EventTrigger.Entry enterEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter
            };
            enterEvent.callback.AddListener((eventData) => descriptionOverButtonScript.WhenOnUI((BaseEventData)eventData, rectTransformPrefab));
            evTrig.triggers.Add(enterEvent);
            


            EventTrigger.Entry exitEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerExit
            };
            exitEvent.callback.AddListener(descriptionOverButtonScript.WhenExitingUI);
            evTrig.triggers.Add(exitEvent);
            



            itemScript newItemPrefabContentItemScript = gbItemPrefab.GetComponent<itemScript>(); // Le script du contenu de la boîte
            buttonSpawnItemPrefab.itemscript = newItemPrefabContentItemScript;


            // Other
            buttonSpawnItemPrefab.cursor = cursor;
            buttonSpawnItemPrefab.itemThrowed = itemThrowed;
            buttonSpawnItemPrefab.feedbacksBox = feedbacksBox;
            buttonSpawnItemPrefab.animator = newPrefab.GetComponent<Animator>(); // Il a pas d'animator, à check


            indexSpawningItems++; // Pour le calcul de la position on incrémente de 1
        }
        
        indexSpawningItems = 0;
        foreach (GameObject gbPotionPrefab in listOfPotionOfTheLevel)
        {
            Debug.Log("Bou!!!");
            GameObject newPrefab = Instantiate(classicPotion);

            newPrefab.transform.SetParent(inventoryParent.transform);
            newPrefab.transform.SetAsFirstSibling(); // Pour mettre la description devant les items



            // Prefab infos
            ButtonSpawnItem buttonSpawnItemPrefab = newPrefab.GetComponent<ButtonSpawnItem>();
            buttonSpawnItemPrefab.itemPrefab = gbPotionPrefab;
            buttonSpawnItemPrefab.feedbacksInventory = feedbacksInventory;

            // Feedbacks when opening (closing)
            RectTransform rectTransformPrefab = newPrefab.GetComponentInChildren<RectTransform>();


            // Seulement pour les potions
            Image oldTexture = newPrefab.GetComponent<Image>();
            SpriteRenderer newTexture = gbPotionPrefab.GetComponent<SpriteRenderer>(); // Prendre la gueule de la preview
            oldTexture.color = Color.white;
            oldTexture.sprite = newTexture.sprite; // Appliquer



            
            // Position
            if(indexSpawningItems % 2 == 0)
            {
                rectTransformPrefab.anchoredPosition = new Vector2((300f+((440/listOfPotionOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-100f, 55f));
            }
            else
            {
                rectTransformPrefab.anchoredPosition = new Vector2((300f+((440/listOfPotionOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-200f, -50f));
            }
            // Formule pour calculer la position : co de base + (longueur de la ligne pour spawn/le nombre d'objet) * l'index



            EventTrigger evTrig = newPrefab.GetComponent<EventTrigger>();
            EventTrigger.Entry enterEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter
            };
            enterEvent.callback.AddListener((eventData) => descriptionOverButtonScript.WhenOnUI((BaseEventData)eventData, rectTransformPrefab));
            evTrig.triggers.Add(enterEvent);
            


            EventTrigger.Entry exitEvent = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerExit
            };
            exitEvent.callback.AddListener(descriptionOverButtonScript.WhenExitingUI);
            evTrig.triggers.Add(exitEvent);
            



            itemScript newItemPrefabContentItemScript = gbPotionPrefab.GetComponent<itemScript>(); // Le script du contenu de la boîte
            buttonSpawnItemPrefab.itemscript = newItemPrefabContentItemScript;


            // Other
            buttonSpawnItemPrefab.cursor = cursor;
            buttonSpawnItemPrefab.itemThrowed = itemThrowed;
            buttonSpawnItemPrefab.feedbacksBox = feedbacksBox;
            buttonSpawnItemPrefab.animator = newPrefab.GetComponent<Animator>(); // Il a pas d'animator, à check

            indexSpawningItems++; // Pour le calcul de la position on incrémente de 1
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
