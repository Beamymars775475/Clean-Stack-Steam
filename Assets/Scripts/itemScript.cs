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

    // Start is called before the first frame update
    void Start()
    {
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
        if(gameObject.tag == "ReadyToExplode" && GameManager.instance.activeStrangePotion)
        {
            Rigidbody2D gameObjectRb = gameObject.GetComponent<Rigidbody2D>();

            gameObjectRb.mass = gameObjectRb.mass *= 3f; // Par 3 car de base il est a /2 et comme c'est la potion étrange il passe de super petit à super gros
            explosionFeedbacks.PlayFeedbacks();
            explosionFlickerFeedbacks.StopFeedbacks();
            gameObject.tag = "Exploded";
        }
        if(explosionFlickerFeedbacks != null) // Si il existe
        {
            if(explosionFlickerFeedbacks.IsPlaying && GameManager.instance.activeStrangePotion)
            {
                gameObject.tag = "ReadyToExplode";
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


    }

    void OnCollisionEnter2D(Collision2D _col)
    {
        // All variations of states of item
        if(_col.gameObject.tag == "deadArea" && (gameObject.tag == "item" || gameObject.tag == "itemTable" || gameObject.tag == "ShrinkItem" || gameObject.tag == "BiggerItem" || gameObject.tag == "ReadyToExplode" || gameObject.tag == "Exploded" || gameObject.tag == "Cloned(Not More Usable)" || gameObject.tag == "CantAcceptPotions")) 
        {
            if(bestiaryItemManager == null && gameObject.tag != "itemTable") // Pour les niveaux normaux
            {
                GameManager.instance.isGameOver = true;
            }
            else if(bestiaryItemManager != null)
            {
                needToDie = true; // Il mourra la prochaine fois que le bouton mort est activé
            }
            
        }

        if(_col.gameObject.tag == "nextArea" && (gameObject.tag == "item" || gameObject.tag == "CantAcceptPotions"))
        {
            GameManager.instance.goNextFloor = true;
        }
    }

    public void WhenCloningDoneGoCursorParent()
    {
        StartCoroutine(CooldownCloning(1f));
    }

    IEnumerator CooldownCloning(float cooldown)
    {
        GameManager.instance.isInCloningProcess = true;
        yield return new WaitForSeconds(cooldown);
        gameObject.transform.SetParent(cursor); // Mettre dans cursor une fois l'animation finit
        GameManager.instance.isInCloningProcess = false;
    }

    public void SwitcherGlowing() // Need to be switched outside of the method
    {
        SpriteRenderer gbShader = gameObject.GetComponent<SpriteRenderer>();
        if(isGlowing == true)
        {
            gbShader.material.EnableKeyword("OUTBASE_ON");
        }
        else
        {
            gbShader.material.DisableKeyword("OUTBASE_ON");
        }
        
    }

}
