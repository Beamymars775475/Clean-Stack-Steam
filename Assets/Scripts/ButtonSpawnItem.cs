using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    

    void Start()
    {
        Image itemPrefabActualImage = gbForShowingContent.GetComponent<Image>();
        saveOfContentTexture = itemPrefabActualImage.sprite;


        // Mettre à jour la taille de la preview
        if((itemPrefab.tag != "BiggerPotion" || itemPrefab.tag != "ShrinkPotion" || itemPrefab.tag != "StrangePotion" || itemPrefab.tag != "ClonePotion") && gameObject.transform.childCount != 0)
        {
            imageContentSize.sizeDelta = new Vector2(itemPrefabActualImage.sprite.texture.width*1.35f, itemPrefabActualImage.sprite.texture.height*1.35f); // *1.35 pour aggrandi un peu
        }
    }


    void Update()
    {
        if(itemPrefab.tag == "BiggerPotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#DD3737><wave>Red Potion</wave></color>";
            itemscript.txtDescription = "You can use this potion on a object to trigger <wave>weird</wave> things ! ";
        }

        else if(itemPrefab.tag == "ShrinkPotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#0097FF><wave>Blue Potion</wave></color>";
            itemscript.txtDescription = "You can use this potion on a object to trigger <wave>weird</wave> things ! ";
        }

        else if(itemPrefab.tag == "StrangePotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#1b9115><wave>Green Potion</wave></color>";
            itemscript.txtDescription = "You can use this potion on a object to trigger <wave>weird</wave> things ! ";
        }

        else if(itemPrefab.tag == "ClonePotion" && itemThrowed.transform.childCount <= 0)
        {
            itemscript.txtName = "<color=#8100FF><wave>Purple Potion</wave></color>";
            itemscript.txtDescription = "You can use this potion on a object to trigger <wave>weird</wave> things ! ";
        }

        if(canBeDestroy && isLoadingAnimation == false)
        {
            Destroy(gameObject);
        }

    }

    public void CatchAObject()
    {
        if((itemPrefab.tag == "item" || itemPrefab.tag == "itemTable" ) && GameManager.instance.isInventoryOpen == true && isDelivered == false)
        {
            Debug.Log("a");
            GameObject prefab = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Transform prefabTransform = prefab.GetComponent<Transform>();
            prefabTransform.SetParent(cursor);
            feedbacksInventory.PlayFeedbacks();
            GameManager.instance.animationInventoryIsDone = false; // L'animation commence
            GameManager.instance.isInventoryOpen = false;
            GameManager.instance.forceToCloseDescription = true;
            isDelivered = true; // IsDelivered signifie "La boîte a été ouverte"

            GameManager.instance.canAccessToInventory = false;

            

            feedbacksBox.PlayFeedbacks();
            
            StartCoroutine(WaitAnimation(0.80f));
            
        }

        // Permet de sortir les potions de l'inventaire
        else if((itemPrefab.tag == "BiggerPotion" || itemPrefab.tag == "ShrinkPotion" || itemPrefab.tag == "StrangePotion" || itemPrefab.tag == "ClonePotion") && itemThrowed.transform.childCount > 0)
        {
            GameObject prefab = Instantiate(itemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Transform prefabTransform = prefab.GetComponent<Transform>();
            prefabTransform.SetParent(cursor);
            feedbacksInventory.PlayFeedbacks();
            GameManager.instance.animationInventoryIsDone = false; // L'animation commence
            GameManager.instance.isInventoryOpen = false;
            GameManager.instance.forceToCloseDescription = true;

            isDelivered = true;

            GameManager.instance.canAccessToInventory = false;

            Destroy(gameObject);


        }



    }


    public void SendingDescriptionInformations()
    {
        GameManager.instance.currentTextName = itemscript.txtName;
        GameManager.instance.currentTextDescription = itemscript.txtDescription;

        if(itemPrefab.tag != "itemTable")
        {
            GameManager.instance.needImageDark = false;
        }
        else if(itemPrefab.tag == "itemTable")
        {
            GameManager.instance.needImageDark = true;
        }
    }

    IEnumerator WaitAnimation(float cooldown)
    {
        if(isLoadingAnimation == false)
        {
            isLoadingAnimation = true;
            animator.SetTrigger("StartAnimBox");
            yield return new WaitForSeconds(cooldown);

            isLoadingAnimation = false;
            canBeDestroy = true;
        }
    }


    public void ShowingContentIfPossible() // Ajouter condition de quand ç'est possible
    {

        if(feedbacksContentHide.IsPlaying)
        {
            feedbacksContentHide.StopFeedbacks();
        }


        Image itemPrefabActualTexture = gbForShowingContent.GetComponent<Image>();
        if(GameManager.instance.AlreadyUsedItem[itemscript.itemID] == true)
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
