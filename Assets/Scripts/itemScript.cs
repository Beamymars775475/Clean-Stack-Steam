using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using AllIn1SpriteShader;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class itemScript : MonoBehaviour
{
    
    [Header("ID")]

    public int itemID;

    [Header("Other")]

    public string txtName;
    public string txtDescription;

    public MMF_Player explosionFeedbacks;
    public MMF_Player explosionFlickerFeedbacks;


    [Header("Bestiary Scene")]
    public BestiaryItemManager bestiaryItemManager;

    private bool needToDie;

    public bool touchedGroundOnce; // Son pour avoir toucher le sol !!!

    [Header("If animation cloning")]

    public Transform cursor;

    [Header("Glowing")]

    public bool isGlowing;

    [Header("State")]
    public bool isClear;
    public bool isShrinkOnce;
    public bool isBiggerOnce;
    public bool isFullOfPotions;
    public bool isCloned;
    public bool isStrange;
    public bool isExplodedFromStrange;

    [Header("Personal State")]
    public bool isTable;
    public bool isBox;


    public bool isFalling;
    public bool isReady;

    public bool isReady2;


    // Start is called before the first frame update
    void Start()
    {
        isClear = true; 
        
        if(bestiaryItemManager != null)
        {
            bestiaryItemManager.limitItemsAmount++;
            bestiaryItemManager.UpdateLimit();;
        }



        if(SceneManager.GetActiveScene().name != "Mainscene" && SceneManager.GetActiveScene().name != "BestiaryScene" && SceneManager.GetActiveScene().name != "LevelSelectorScene")
        {
            GameManager.instance.AlreadyUsedItem[itemID] = true;
        }

        txtName = "Banana :";
        txtDescription = "<rot> Banana. </rot>";
    }

    // Update is called once per frame
    void Update()
    {
        if(isStrange && GameManager.instance.activeStrangePotion && !isExplodedFromStrange)
        {
            Rigidbody2D gameObjectRb = gameObject.GetComponent<Rigidbody2D>();

            gameObjectRb.mass = gameObjectRb.mass *= 3f; // Par 3 car de base il est a /2 et comme c'est la potion étrange il passe de super petit à super gros
            explosionFeedbacks.PlayFeedbacks();
            explosionFlickerFeedbacks.StopFeedbacks();
            // CHANGE STATE
            isExplodedFromStrange = true;
        }
        if(explosionFlickerFeedbacks != null) // Si il existe
        {
            if(explosionFlickerFeedbacks.IsPlaying && GameManager.instance.activeStrangePotion)
            {
                // CHANGE STATE
                isStrange = true;
            }
        }
        if(bestiaryItemManager != null)
        {
            if(needToDie && bestiaryItemManager.deleteItemsOnGround == true)
            {
                Destroy(gameObject); // Meilleur effet ?
                bestiaryItemManager.limitItemsAmount--;
                bestiaryItemManager.UpdateLimit();
            }
        }

        Rigidbody2D itemRb = gameObject.GetComponentInParent<Rigidbody2D>();


        // Si il tombe on attend 2.5 sec pour le mettre prêt
        if(isFalling && !isReady)
        {
            StartCoroutine(CountDownUntilReady(2.5f));
        }
        // Si il bouge un peu on attend aussi
        if(isReady && (itemRb.velocity.x > 1f || itemRb.velocity.x < -1f || itemRb.velocity.y > 1f || itemRb.velocity.y < -1f))
        {
            isFalling = true;
            isReady = false;
        }


        if(!isBox)
        {
            isReady2 = true;
        }
    }

    void OnCollisionEnter2D(Collision2D _col)
    {
        // All variations of states of item
        if(_col.gameObject.tag == "deadArea" && gameObject.tag == "item") 
        {
            if(bestiaryItemManager == null && !isTable) // Pour les niveaux normaux
            {
                GameManager.instance.isGameOver = true;
            }
            else if(bestiaryItemManager != null)
            {
                needToDie = true; // Il mourra la prochaine fois que le bouton mort est activé
            }
            
        }

        if(_col.gameObject.tag == "nextArea" && isClear)
        {
            GameManager.instance.goNextFloor = true;
        }
    }

    public void CloningAnimation()
    {   
        Rigidbody2D rbItem = gameObject.GetComponent<Rigidbody2D>();
        rbItem.gravityScale = 0;

        MMF_Player feedbacksItem = gameObject.transform.GetChild(10).GetComponent<MMF_Player>(); // Animation item qui flotte
        MMF_DestinationTransform feedbacksItemMMMF_DestinationTransform = feedbacksItem.GetFeedbackOfType<MMF_DestinationTransform>();
        feedbacksItemMMMF_DestinationTransform.Destination = cursor.transform;
        feedbacksItem.PlayFeedbacks();

        StartCoroutine(CooldownCloning(1f));
    }

    IEnumerator CooldownCloning(float cooldown)
    {
        GameManager.instance.isInCloningProcess = true;
        yield return new WaitForSeconds(cooldown);
        gameObject.transform.SetParent(cursor); // Mettre dans cursor une fois l'animation finit
        GameManager.instance.isInCloningProcess = false;
    }

    public void SwitcherGlowing(Color newShaderColor) // Need to be switched outside of the method
    {
        SpriteRenderer gbShader = gameObject.GetComponent<SpriteRenderer>();
        if(isGlowing == true)
        {
            gbShader.material.SetColor("_OutlineColor", newShaderColor);
            gbShader.material.EnableKeyword("OUTBASE_ON");           
        }
        else
        {
            gbShader.material.DisableKeyword("OUTBASE_ON");
        }
        
    }

    IEnumerator CountDownUntilReady(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        Rigidbody2D itemRb = gameObject.GetComponent<Rigidbody2D>();
        if(itemRb.velocity.x < 1f && itemRb.velocity.x > -1f && itemRb.velocity.y < 1f && itemRb.velocity.y > -1f && isFalling == true)
        {
            isReady = true;
            isFalling = false;
        }
        
    }

}
