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
using UnityEngine.InputSystem;

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
    public bool isMicro;

    [Header("Personal State")]
    public bool isTable;
    public bool isClickable;

    public bool isClicked;
    public bool isFalling;
    public bool isReady;

    public bool isReady2;

    [Header("Particules")]

    public GameObject particuleImplosion; // Implosion 


    public GameObject particuleExplosion; // Implosion 



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
        if(isStrange && GameManager.instance.isPhase1Done && !isExplodedFromStrange)
        {
            StartCoroutine(CooldownStrangeAfterPhase1Done(1.5f));
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
                if(!bestiaryItemManager.spareTablesFromDeath)
                {
                    Destroy(gameObject); // Meilleur effet ?
                    bestiaryItemManager.limitItemsAmount--;
                    bestiaryItemManager.UpdateLimit();
                }
                else if(bestiaryItemManager.spareTablesFromDeath && !isTable) // Epargner les tables
                {
                    Destroy(gameObject); // Meilleur effet ?
                    bestiaryItemManager.limitItemsAmount--;
                    bestiaryItemManager.UpdateLimit();
                }

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


        if(!isClickable)
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

    IEnumerator CooldownStrangeAfterPhase1Done(float cooldown)
    {
        isExplodedFromStrange = true;
        yield return new WaitForSeconds(cooldown);

        Rigidbody2D gameObjectRb = gameObject.GetComponent<Rigidbody2D>();

        gameObjectRb.mass = gameObjectRb.mass *= 3f; // Par 3 car de base il est a /2 et comme c'est la potion étrange il passe de super petit à super gros
        explosionFeedbacks.PlayFeedbacks();
        explosionFlickerFeedbacks.StopFeedbacks();
        // CHANGE STATE

    }

    public void Pulsions(Collider2D[] colliderItems)
    {
        SpriteRenderer gbShader = gameObject.GetComponent<SpriteRenderer>();
        gbShader.material.EnableKeyword("HITEFFECT_ON"); 
        gbShader.material.SetFloat("_HitEffectBlend", 0);
        for (int i = 0; i < 21; i++) // TURN WHITE
        {
            StartCoroutine(TurnWhite((float)(0.025*i)));
        }
        StartCoroutine(ParticuleInAnimationExplosion(0.5f, gameObject.transform.position));
        StartCoroutine(Pulsion(0.5f, colliderItems, gameObject));
        for (float j = 0; j < 6; j++) // TURN BASIC
        {
            StartCoroutine(TurnBasic((float)(0.5+(0.05*j))));
        }
    }

    private IEnumerator TurnWhite(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        SpriteRenderer gbShader = gameObject.GetComponent<SpriteRenderer>();
        gbShader.material.SetFloat("_HitEffectBlend", waitTime);
    }
    private IEnumerator TurnBasic(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        SpriteRenderer gbShader = gameObject.GetComponent<SpriteRenderer>();
        if(2-waitTime > 0) // 2 CAR 1 DE OFFSET
        {
            gbShader.material.SetFloat("_HitEffectBlend", 1.5f-(waitTime+0.75f)); // SA CHANGE ENTRE 0 ET 0.25      
        }
        else
        {
            gbShader.material.DisableKeyword("HITEFFECT_ON"); 
        }
    
    
    }

    private IEnumerator Pulsion(float waitTime, Collider2D[] gbItems, GameObject mainItem)
    {
        Debug.Log("Wahhh");
        yield return new WaitForSeconds(waitTime);

        Collider2D colliderItem = mainItem.GetComponent<Collider2D>();

           foreach(Collider2D collider in gbItems)
           {
                if(collider != null)
                {
                    Debug.Log("WAAAAAAAAAAAAAAAAAAAA");
                    Rigidbody2D rbItem = collider.gameObject.GetComponent<Rigidbody2D>();
                    if(rbItem != null)
                    {
                        if(collider != colliderItem)
                        {
                            rbItem.AddForce(new Vector2(mainItem.transform.position.x - collider.transform.position.x, mainItem.transform.position.y - collider.transform.position.y).normalized*-20, ForceMode2D.Impulse);
                        }
                    }
                }
           }
    }

    IEnumerator ParticuleInAnimationExplosion(float cooldown, Vector3 itemTargetPos)
    {
        yield return new WaitForSeconds(cooldown);
        GameObject particules = Instantiate(particuleExplosion, itemTargetPos, Quaternion.identity);

        
        particules.transform.SetParent(gameObject.transform);
        particules.transform.localScale = gameObject.transform.lossyScale;

        yield return new WaitForSeconds(cooldown);
        Destroy(particules);
    }

    public void Implosions(Collider2D[] colliderItems, Vector3 itemTargetPos)
    {
        StartCoroutine(ParticuleInAnimationImplosion(0.8f, itemTargetPos)); // Particules
        float delay = 0f;

        for (int i = 0; i < 20; i++)
        {
            StartCoroutine(Implosion(delay, colliderItems, gameObject));
            delay += 0.05f;
        }
    }

    private IEnumerator Implosion(float waitTime, Collider2D[] gbItems, GameObject mainItem)
    {
        yield return new WaitForSeconds(waitTime);

        Rigidbody2D rigidbody2DItem = mainItem.GetComponent<Rigidbody2D>();
        Collider2D colliderItem = mainItem.GetComponent<Collider2D>();

           foreach(Collider2D collider in gbItems)
           {
                if(collider != null)
                {
                    Debug.Log("WAAAAAAAAAAAAAAAAAAAA");
                    Rigidbody2D rbItem = collider.gameObject.GetComponent<Rigidbody2D>();
                    if(rbItem != null)
                    {
                        if(collider != colliderItem)
                        {
                            rbItem.AddForce(new Vector2(mainItem.transform.position.x - collider.transform.position.x, mainItem.transform.position.y - collider.transform.position.y).normalized*7.5f*(waitTime+0.7f), ForceMode2D.Impulse);
                        }
                    }
                }
           }
    }


    IEnumerator ParticuleInAnimationImplosion(float cooldown, Vector3 itemTargetPos)
    {
        GameObject particules = Instantiate(particuleImplosion, itemTargetPos, Quaternion.identity);
        
        particules.transform.SetParent(gameObject.transform);
        particules.transform.localScale = gameObject.transform.lossyScale;

        yield return new WaitForSeconds(cooldown);
        Destroy(particules);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Vector2 areaToCheck = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        SpriteRenderer gb = gameObject.GetComponent<SpriteRenderer>();
        Gizmos.DrawWireSphere(areaToCheck, (float)(0.1*((gb.sprite.texture.width + gb.sprite.texture.height)/2))*gameObject.transform.lossyScale.x);
    }
}
