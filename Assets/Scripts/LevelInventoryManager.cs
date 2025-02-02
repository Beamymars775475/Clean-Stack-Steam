using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


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

    [Header("---Type of boxes---")]
    public GameObject classicBox;
    public GameObject blueBox;
    public GameObject hardBox;
    public GameObject classicPotion;
    public GameObject[] listOfItemsOfTheLevel;
    public GameObject[] listOfPotionOfTheLevel;

    public GameObject inventoryParent;

    [Header("-----Hard mode : Textures-----")]

    public Sprite hardModTextureBox;
    public Sprite hardModTexturePotion;

    [Header("Bestiary Scene (NOT NEEDED)")]

    public BestiaryItemManager bestiaryItemManager;

    [Header("Cursor spell script")] // Potions only

    public Transform placeToPutFullItems;

    public int indexSpawningItems;

    [Header("Preferences")]

    public Image inv; // To change Transparency

    [Header("Game End")]
    public  MMF_Player closeInvGameEnd;





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
        foreach (GameObject gbItemPrefab in listOfItemsOfTheLevel) // BOITE
        {

            Debug.Log(gbItemPrefab.name + " created"); // Affichage dans la console
            itemScript gbItemPrefabItemScript = gbItemPrefab.GetComponent<itemScript>();

            GameObject newPrefab = null;
            if(GameManager.instance.modeHard && SceneManager.GetActiveScene().name != "BestiaryScene")
            {
                newPrefab = Instantiate(hardBox);
            }
            else if(gbItemPrefabItemScript.isTable)
            {
                newPrefab = Instantiate(blueBox);
            }
            else
            {
                newPrefab = Instantiate(classicBox);
            }
                
                itemScript newItemPrefabContentItemScript = gbItemPrefab.GetComponent<itemScript>(); // Le script du contenu de la boîte
                if(SceneManager.GetActiveScene().name != "BestiaryScene" || (SceneManager.GetActiveScene().name == "BestiaryScene" && GameManager.instance.AlreadyUsedItem[newItemPrefabContentItemScript.itemID] == true))
                {

                newPrefab.transform.SetParent(inventoryParent.transform);
                newPrefab.transform.SetAsFirstSibling(); // Pour mettre la description devant les items

                // Seulement pour les items
                // Feedbacks when Description (Showing/Hiding content)
                GameObject gbShowingContent = newPrefab.transform.GetChild(0).gameObject; // Chopper la preview
                Image gbShowingContentTexture = gbShowingContent.GetComponent<Image>(); // Prendre l'ancienne texture

                SpriteRenderer newTexture = gbItemPrefab.GetComponent<SpriteRenderer>(); // Prendre la gueule de la preview
                gbShowingContentTexture.sprite = newTexture.sprite; // Appliquer



                // Changer la texture uniquement si hardmode
                Image oldTexture = newPrefab.GetComponent<Image>();
                if(GameManager.instance.modeHard && SceneManager.GetActiveScene().name != "BestiaryScene")
                {
                    oldTexture.sprite = hardModTextureBox;
                }


                // Prefab infos
                ButtonSpawnItem buttonSpawnItemPrefab = newPrefab.GetComponent<ButtonSpawnItem>();
                buttonSpawnItemPrefab.itemPrefab = gbItemPrefab;
                buttonSpawnItemPrefab.feedbacksInventory = feedbacksInventory;

                // Bestiary Scene
                if(bestiaryItemManager != null)
                {
                    buttonSpawnItemPrefab.bestiaryItemManager = bestiaryItemManager; 

                }


                // Feedbacks when opening (closing)
                RectTransform rectTransformPrefab = newPrefab.GetComponentInChildren<RectTransform>();


                // Position
                if(SceneManager.GetActiveScene().name != "BestiaryScene")
                {
                    if(indexSpawningItems % 2 == 0)
                    {
                        rectTransformPrefab.anchoredPosition = new Vector2((-675f+((975/listOfItemsOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-100f, 55f));
                    }
                    else
                    {
                        rectTransformPrefab.anchoredPosition = new Vector2((-675f+((975/listOfItemsOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-200f, -50f));
                    }
                    // Formule pour calculer la position : co de base + (longueur de la ligne pour spawn/le nombre d'objet) * l'index
                }
                else
                {
                    if(indexSpawningItems % 2 == 0)
                    {
                        rectTransformPrefab.anchoredPosition = new Vector2((-775f+((1325/listOfItemsOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-100f, 55f));
                    }
                    else
                    {
                        rectTransformPrefab.anchoredPosition = new Vector2((-775f+((1325/listOfItemsOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-200f, -50f));
                    }
                    // Formule pour calculer la position : co de base + (longueur de la ligne pour spawn/le nombre d'objet) * l'index
                }



                EventTrigger evTrig = newPrefab.GetComponent<EventTrigger>();
                EventTrigger.Entry enterEvent = new EventTrigger.Entry() // Animattion preview entrée
                {
                    eventID = EventTriggerType.PointerEnter
                };
                enterEvent.callback.AddListener((eventData) => descriptionOverButtonScript.WhenOnUI((BaseEventData)eventData, rectTransformPrefab));
                evTrig.triggers.Add(enterEvent);
                


                EventTrigger.Entry exitEvent = new EventTrigger.Entry() // Animattion preview sortie
                {
                    eventID = EventTriggerType.PointerExit
                };
                exitEvent.callback.AddListener(descriptionOverButtonScript.WhenExitingUI);
                evTrig.triggers.Add(exitEvent);
                



                // buttonSpawnItemPrefab Setup avant la création de la boîte
                buttonSpawnItemPrefab.itemscript = newItemPrefabContentItemScript;


                // Other
                buttonSpawnItemPrefab.cursor = cursor;
                buttonSpawnItemPrefab.itemThrowed = itemThrowed;
                buttonSpawnItemPrefab.feedbacksBox = feedbacksBox;
                buttonSpawnItemPrefab.animator = newPrefab.GetComponent<Animator>();


                indexSpawningItems++; // Pour le calcul de la position on incrémente de 1
            }
            else
            {
                Destroy(newPrefab);
            }

        }
        
        indexSpawningItems = 0;
        foreach (GameObject gbPotionPrefab in listOfPotionOfTheLevel) // ------------------------------------------- POTION -------------------------------------------
        {
            Debug.Log(gbPotionPrefab.name + " created"); // Affichage dans la console
            GameObject newPrefab = Instantiate(classicPotion);

            newPrefab.transform.SetParent(inventoryParent.transform);
            newPrefab.transform.SetAsFirstSibling(); // Pour mettre la description devant les items

            itemScript newItemPrefabContentItemScript = gbPotionPrefab.GetComponent<itemScript>(); // Le script du contenu de la boîte
            if(SceneManager.GetActiveScene().name != "BestiaryScene" || (SceneManager.GetActiveScene().name == "BestiaryScene" && GameManager.instance.AlreadyUsedItem[newItemPrefabContentItemScript.itemID] == true))
            {

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
                if(GameManager.instance.modeHard && SceneManager.GetActiveScene().name != "BestiaryScene")
                {
                    oldTexture.sprite = hardModTexturePotion;
                }
                else
                {
                    oldTexture.sprite = newTexture.sprite;
                }




                if(SceneManager.GetActiveScene().name != "BestiaryScene")
                {
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
                }
                else
                {
                    // Position
                    if(indexSpawningItems % 2 == 0)
                    {
                        rectTransformPrefab.anchoredPosition = new Vector2((550+((300/listOfPotionOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-100f, 55f));
                    }
                    else
                    {
                        rectTransformPrefab.anchoredPosition = new Vector2((550f+((300/listOfPotionOfTheLevel.Length) * indexSpawningItems))+Random.Range(-15f, 15f), Random.Range(-200f, -50f));
                    }
                    // Formule pour calculer la position : co de base + (longueur de la ligne pour spawn/le nombre d'objet) * l'index
                }



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
                



                // buttonSpawnItemPrefab Setup avant la création de la boîte
                buttonSpawnItemPrefab.itemscript = newItemPrefabContentItemScript;



                // Other
                buttonSpawnItemPrefab.cursor = cursor;
                buttonSpawnItemPrefab.itemThrowed = itemThrowed;
                buttonSpawnItemPrefab.feedbacksBox = feedbacksBox;
                buttonSpawnItemPrefab.animator = newPrefab.GetComponent<Animator>(); // Il a pas d'animator, à check

                indexSpawningItems++; // Pour le calcul de la position on incrémente de 1


                // Spell script
                CursorSpellScript cursorSpellScript = gbPotionPrefab.GetComponentInChildren<CursorSpellScript>();
                if(cursorSpellScript != null)
                {
                    cursorSpellScript.spaceForFullItems = placeToPutFullItems;
                }
                


            }
            else
            {
                Destroy(newPrefab);
            }
        }





        // EVENT INV TUTO GENERAL : LIGHTS -------------------------------------------------------------
        if(SceneManager.GetActiveScene().name == "Level0") // SETUP DIALOGUE
        {
            foreach(Transform item in inventoryParent.transform)
            {


                ButtonSpawnItem buttonSpawnItem = item.GetComponent<ButtonSpawnItem>();
                if(buttonSpawnItem != null) // SI ITEM OU POTION
                {
                    GameManager.instance.posToLightUp.Add(item.gameObject); // OU METTRE LA LUMIERE

                    if(buttonSpawnItem.itemPrefab.tag == "item")
                    {
                        GameManager.instance.indexToLightUp.Add(1);// ORDRE DE PASSAGE DEPUIS DIALOGUE (ITEMS)
                    }
                    else
                    {
                        GameManager.instance.indexToLightUp.Add(2); // ORDRE DE PASSAGE DEPUIS DIALOGUE (POTIONS)
                    }
                }

                
            }
            GameManager.instance.indexDialogues = 9991;
            GameManager.instance.TriggerEvent();

        }
        // EVENT INV TUTO GENERAL : LIGHTS -------------------------------------------------------------
        if(SceneManager.GetActiveScene().name == "Level7") // SETUP DIALOGUE
        {
            foreach(Transform item in inventoryParent.transform)
            {


                ButtonSpawnItem buttonSpawnItem = item.GetComponent<ButtonSpawnItem>();
                if(buttonSpawnItem != null) // SI ITEM OU POTION
                {
                    itemScript itemScriptItem = buttonSpawnItem.itemPrefab.GetComponent<itemScript>();
                    if(itemScriptItem != null)
                    {
                        if(itemScriptItem.isTable)
                        {
                            RectTransform itemRTTransform = item.GetComponent<RectTransform>();
                            GameManager.instance.posToLightUp.Add(item.gameObject); // OU METTRE LA LUMIERE
                            GameManager.instance.indexToLightUp.Add(4);// ORDRE DE PASSAGE DEPUIS DIALOGUE (ITEMS)
                        }
                    }

                }

                
            }
            GameManager.instance.indexDialogues = 9992;
            GameManager.instance.TriggerEvent();

        }


        // EVENT INV TUTO POTIONS -------------------------------------------------------------
        if(!GameManager.instance.AlreadyUsedItem[30])
        {
            bool isTutoRedPotionOnce = false; // CHECK SI DEJA ENVOYER UNE FOIS
            foreach(Transform item in inventoryParent.transform)
            {
                ButtonSpawnItem buttonSpawnItem = item.GetComponent<ButtonSpawnItem>();
                if(buttonSpawnItem != null)
                {
                    if(buttonSpawnItem.itemPrefab.tag == "BiggerPotion")
                    {
                        RectTransform itemRTTransform = item.GetComponent<RectTransform>();
                        GameManager.instance.posToLightUp.Add(item.gameObject); // OU METTRE LA LUMIERE
                        GameManager.instance.indexToLightUp.Add(3); // ORDRE DE PASSAGE DEPUIS DIALOGUE (RED POTIONS)
                    }

                    if(buttonSpawnItem.itemscript != null)
                    {
                        if(GameManager.instance.AlreadyUsedItem[buttonSpawnItem.itemscript.itemID] == false && buttonSpawnItem.itemscript.itemID == 30 && !isTutoRedPotionOnce && SceneManager.GetActiveScene().name != "BestiaryScene") // POTION ROUGE
                        {
                            isTutoRedPotionOnce = true;
                            GameManager.instance.indexDialogues = 6661;
                            GameManager.instance.TriggerEvent();
                        }
                    }



                }


            }
        }

        // LAUNCH DIALOGUE !!!
        GameManager.instance.indexDialogues = 122774647;
        GameManager.instance.TriggerEvent();

    }


    void Update()
    {
        if((GameManager.instance.isWon || GameManager.instance.isGameOver) && GameManager.instance.isInventoryOpen)
        {
            closeInvGameEnd.PlayFeedbacks();
            GameManager.instance.isInventoryOpen = false;
        }
    }

    public void OnceIsFullyOpen()
    {
        if(GameManager.instance.isGameOver)
        {
            closeInvGameEnd.PlayFeedbacks();
        }
    }
}
