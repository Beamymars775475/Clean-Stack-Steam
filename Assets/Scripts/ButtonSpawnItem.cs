using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSpawnItem : MonoBehaviour
{

    [Header("Prefab infos")]

    public GameObject itemPrefab;
    public itemScript itemscript;

    [Header("Feedbacks when opening (closing)")]

    public MMF_Player feedbacksInventory;

    [Header("Feedbacks when Description (Showing/Hiding content) | ONLY FOR ITEMS -> NOT POTIONS")]

    public MMF_Player feedbacksContentShow;
    public MMF_Player feedbacksContentHide;

    public GameObject gbForShowingContent;

    public RectTransform imageContentSize;




    public Sprite saveOfContentTexture;
    public Sprite whenNewItemReplaceTextureWithThat;


    [Header("Other")]
    public Transform cursor;
    public GameObject itemThrowed;

    public MMF_Player feedbacksBox;

    public Animator animator;

    public bool isLoadingAnimation;
    public bool canBeDestroy;

    public bool isDelivered;


    public int typeOfBox; // 0 : Basic --- 1 : Table --- 2 : Hard | NOT FOR POTIONS

    [Header("Bestiary Scene")]
    public BestiaryItemManager bestiaryItemManager;

    [Header("To clone strange potion")]

    public GameObject placeWhereItemsStocked;


    void Start()
    {

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        // Utilisez l'aspectRatio pour ajuster la taille de l'item
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Sprite spriteGb = gameObject.GetComponent<Sprite>();
        rectTransform.localScale = Vector3.one;


 
        // Mettre à jour la taille de la preview et save l'image de base
        if((itemPrefab.tag != "BiggerPotion" || itemPrefab.tag != "ShrinkPotion" || itemPrefab.tag != "StrangePotion" || itemPrefab.tag != "ClonePotion" || itemPrefab.tag != "MicroPotion" || itemPrefab.tag != "PulsePotion") && gameObject.transform.childCount != 0)
        {
            Image itemPrefabActualImage = gbForShowingContent.GetComponent<Image>();
            saveOfContentTexture = itemPrefabActualImage.sprite;

            imageContentSize.sizeDelta = new Vector2(itemPrefabActualImage.sprite.texture.width*1.35f, itemPrefabActualImage.sprite.texture.height*1.35f); // *1.35 pour aggrandi un peu
        }
    }


    void Update()
    {
        if(itemPrefab.tag == "BiggerPotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#DD3737><wave>Red Potion</wave></color>";
            itemscript.txtDescription = "You can't use this potion at the moment.";
        }

        else if(itemPrefab.tag == "ShrinkPotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#0097FF><wave>Blue Potion</wave></color>";
            itemscript.txtDescription = "You can't use this potion at the moment.";
        }

        else if(itemPrefab.tag == "StrangePotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#1b9115><wave>Green Potion</wave></color>";
            itemscript.txtDescription = "You can't use this potion at the moment.";
        }

        else if(itemPrefab.tag == "ClonePotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#8100FF><wave>Purple Potion</wave></color>";
            itemscript.txtDescription = "You can't use this potion at the moment.";
        }
        else if(itemPrefab.tag == "MicroPotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#BF4591><wave>Pink Potion</wave></color>";
            itemscript.txtDescription = "You can't use this potion at the moment.";
        }
        else if(itemPrefab.tag == "PulsePotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#D58025><wave>Orange Potion</wave></color>";
            itemscript.txtDescription = "You can't use this potion at the moment.";
        }
        else if(itemPrefab.tag == "ExpulsePotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#D58025><wave>Orange Potion</wave></color>";
            itemscript.txtDescription = "You can't use this potion at the moment.";
        }
        else if((itemPrefab.tag == "BiggerPotion" || itemPrefab.tag == "ShrinkPotion" || itemPrefab.tag == "StrangePotion" || itemPrefab.tag == "ClonePotion" || itemPrefab.tag == "MicroPotion" || itemPrefab.tag == "PulsePotion" || itemPrefab.tag == "ExpulsePotion") && itemThrowed.transform.childCount > 0)
        {
            itemscript.txtDescription = "You can use this potion on a object but <shake a=2>DON'T</shake> drink it.";
        }

        if((canBeDestroy && isLoadingAnimation == false))
        {
            Destroy(gameObject);
        }


    }

    public void CatchAObject()
    {
        if(GameManager.instance.isGameOver == false)
        {
            bool isGoodForStrangePotionoOrNo = false; // POUR POTION STRANGE    
            bool isGoodForBiggerAndShrinkPotionoOrNo = false; // POUR POTION PETIT ET GROS
            foreach (Transform child in itemThrowed.transform)
            {
                itemScript childItemScript = child.GetComponent<itemScript>();
                if(childItemScript.isClear) // Oui ya un item sur lequel appliqué
                {
                    isGoodForStrangePotionoOrNo = true;
                }

                if(childItemScript.isClear || childItemScript.isBiggerOnce || childItemScript.isShrinkOnce) // Oui ya un item sur lequel appliqué
                {
                    isGoodForBiggerAndShrinkPotionoOrNo = true;
                }
            }

            itemScript itemPrefabScript = itemPrefab.GetComponent<itemScript>();

            if(itemPrefab.tag == "item" && GameManager.instance.isInventoryOpen == true && isDelivered == false && (SceneManager.GetActiveScene().name != "BestiaryScene" || bestiaryItemManager.limitItemsAmount < 25))
            {
                GameObject prefab = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                Transform prefabTransform = prefab.GetComponent<Transform>();
                prefabTransform.SetParent(cursor);

                
                if(!itemPrefabScript.isTable)
                {
                    MMF_Player feedbackClone = prefab.transform.GetChild(10).GetComponent<MMF_Player>(); // Need to fill destination
                    MMF_DestinationTransform feedbackCloneDestionationFeedback = feedbackClone.GetFeedbackOfType<MMF_DestinationTransform>();
                    feedbackCloneDestionationFeedback.Destination = cursor;
                }

                
                feedbacksInventory.PlayFeedbacks();
                GameManager.instance.animationInventoryIsDone = false; // L'animation commence
                GameManager.instance.isInventoryOpen = false;
                GameManager.instance.forceToCloseDescription = true;
                
                if(SceneManager.GetActiveScene().name != "BestiaryScene")
                {
                    isDelivered = true; // IsDelivered signifie "La boîte a été ouverte"
                }

                itemScript prefabItemScript = prefab.GetComponent<itemScript>();
                prefabItemScript.cursor = cursor; // Pour le clonning
                prefabItemScript.isClear = true; // Pour le setup

                if(bestiaryItemManager != null)
                {
                    prefabItemScript.bestiaryItemManager = bestiaryItemManager;
                }

                GameManager.instance.canAccessToInventory = false;

                

                feedbacksBox.PlayFeedbacks();
                
                StartCoroutine(WaitAnimation(0.80f));
                
            }

            // Permet de sortir les potions de l'inventaire
            else if(((((itemPrefab.tag == "BiggerPotion" || itemPrefab.tag == "ShrinkPotion" || itemPrefab.tag == "MicroPotion") && isGoodForBiggerAndShrinkPotionoOrNo) || itemPrefab.tag == "ClonePotion" || (itemPrefab.tag == "StrangePotion" && isGoodForStrangePotionoOrNo)) && itemThrowed.transform.childCount > 0) || itemPrefab.tag == "PulsePotion" || itemPrefab.tag == "ExpulsePotion" && GameManager.instance.isInventoryOpen == true && isDelivered == false) 
            {

                GameObject prefab = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                Transform prefabTransform = prefab.GetComponent<Transform>();
                prefabTransform.SetParent(cursor);
                feedbacksInventory.PlayFeedbacks();
                GameManager.instance.animationInventoryIsDone = false; // L'animation commence
                GameManager.instance.isInventoryOpen = false;
                GameManager.instance.forceToCloseDescription = true;

                if(SceneManager.GetActiveScene().name != "BestiaryScene")
                {
                    isDelivered = true; // IsDelivered signifie "La boîte a été ouverte"
                }


                
                if(bestiaryItemManager != null)
                {
                    itemScript prefabItemScript = prefab.GetComponent<itemScript>();
                    prefabItemScript.bestiaryItemManager = bestiaryItemManager;
                }

                GameManager.instance.canAccessToInventory = false;

                if(SceneManager.GetActiveScene().name != "BestiaryScene")
                {
                    Destroy(gameObject);
                }



            }
            int countNumberOfStrangePotion = 0;
            foreach(Transform child in gameObject.transform.parent)
            {
                ButtonSpawnItem childItemScript = child.GetComponent<ButtonSpawnItem>();
                if(childItemScript != null)
                {
                    if(childItemScript.itemPrefab.name == "StrangePotion")
                    {
                        countNumberOfStrangePotion += 1;
                    }
                }

            }

            if(isGoodForStrangePotionoOrNo == false && gameObject.transform.parent.childCount == countNumberOfStrangePotion+2) // 2 feedbacks et countNumberOfStrangePotion pour le nombre de potion verte
            {
                GameManager.instance.TriggerEvent();
            }


        }


    }


    public void SendingDescriptionInformations()
    {
        GameManager.instance.currentTextName = itemscript.txtName;
        GameManager.instance.currentTextDescription = itemscript.txtDescription;

        itemScript itemPrefabScript = itemPrefab.GetComponent<itemScript>();

        if(!itemPrefabScript.isTable)
        {
            GameManager.instance.needImageDark = false;
        }
        else if(itemPrefabScript.isTable)
        {
            GameManager.instance.needImageDark = true;
        }
    }

    IEnumerator WaitAnimation(float cooldown)
    {
        if(isLoadingAnimation == false)
        {
            animator.StopPlayback();
            isLoadingAnimation = true;

            animator.SetTrigger("StartAnimBox");

            yield return new WaitForSeconds(cooldown);

            isLoadingAnimation = false;

            if(SceneManager.GetActiveScene().name != "BestiaryScene")
            {
                canBeDestroy = true;
            }
            else
            {
                animator.SetTrigger("StartBackIdleState");
            }
        }
    }


    public void ShowingContentIfPossible() // Ajouter condition de quand ç'est possible
    {

        if(feedbacksContentHide.IsPlaying)
        {
            feedbacksContentHide.StopFeedbacks();
        }


        Image itemPrefabActualTexture = gbForShowingContent.GetComponent<Image>();
        if(GameManager.instance.AlreadyUsedItem[itemscript.itemID] == true && GameManager.instance.modeHard == false)
        {
            itemPrefabActualTexture.sprite = saveOfContentTexture;
            imageContentSize.sizeDelta = new Vector2(itemPrefabActualTexture.sprite.texture.width*1.35f, itemPrefabActualTexture.sprite.texture.height*1.35f);
        }
        else
        {
            itemPrefabActualTexture.sprite = whenNewItemReplaceTextureWithThat;
            imageContentSize.sizeDelta = new Vector2(itemPrefabActualTexture.sprite.texture.width*1.35f, itemPrefabActualTexture.sprite.texture.height*1.35f);
        }

        feedbacksContentShow.PlayFeedbacks();

    }

    public void HidingContentIfPossible() // Ajouter condition de quand ç'est possible
    {
        if(feedbacksContentShow.IsPlaying)
        {
            feedbacksContentShow.StopFeedbacks();
        }

        feedbacksContentHide.PlayFeedbacks();
    }

}
