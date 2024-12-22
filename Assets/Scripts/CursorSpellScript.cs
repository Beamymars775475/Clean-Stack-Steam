using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class CursorSpellScript : MonoBehaviour
{
    private Camera _mainCamera;


    public Transform moveThis;
    //the layers the ray can hit

    public LayerMask hitLayers;

    public float timeOffSet;
    private Vector3 velocity;

    float rayLength = 0.5f;
        
	Vector3 dir;
	Ray ray;
    RaycastHit hit_ray;

    Vector3 targetPos;
    public LayerMask hitLayers2;


    public Transform spaceForFullItems;


    [Header("Clone Potion")]

    public Transform cursor; // Mettre le clone dans les mains

    [Header("Glowing")]

    public itemScript itemScriptLastObjectSaved;

    void Start()
    {
        
    }

    private void Awake ()
    {
        _mainCamera = Camera.main; 
    }

    void Update()
    {
            Vector3 mouse = Input.mousePosition;
            Ray castPointmouse = Camera.main.ScreenPointToRay(mouse);
            RaycastHit hitMouse;
            
            if (Physics.Raycast(castPointmouse, out hitMouse, Mathf.Infinity, hitLayers))
            {
                Vector3 targetPos = hitMouse.point;

                dir = transform.TransformDirection(new Vector3(moveThis.transform.position.x - targetPos.x, 0, 0));


                ray = new Ray(moveThis.transform.position, dir * rayLength);

                Debug.DrawRay(moveThis.transform.position, ray.direction, Color.green);

                if (!Physics.Raycast(ray, out hit_ray, Mathf.Infinity, hitLayers2))
                {
                    Vector3 ForceToGoToMouse = Vector3.SmoothDamp(moveThis.position, new Vector3(targetPos.x, targetPos.y, 0), ref velocity, timeOffSet); 

                    moveThis.transform.position = ForceToGoToMouse; 
                }

            }   

        // Glowing Effect
        RaycastHit2D hitSomething = Physics2D.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction);
        if(hitSomething)
        {
            if(hitSomething.collider.gameObject != null && itemScriptLastObjectSaved != null)
            {
                if(hitSomething.collider.gameObject != itemScriptLastObjectSaved.gameObject)
                {
                    itemScriptLastObjectSaved.isGlowing = false;
                    Color defaultColor = new Color(1f, 1f, 1f, 1f);
                    itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
                }
            }

            itemScript itemHitItemScript = hitSomething.collider.gameObject.GetComponent<itemScript>();
            if(itemHitItemScript == null) return; // SI C PO UN ITEM !!

            // Si la potion est compatible avec l'item touché
            if((itemHitItemScript.isClear && (gameObject.tag == "BiggerPotion" || gameObject.tag == "ShrinkPotion" || gameObject.tag == "StrangePotion"))
            || (itemHitItemScript.isBiggerOnce || itemHitItemScript.isShrinkOnce) && (gameObject.tag == "BiggerPotion" || gameObject.tag == "ShrinkPotion")
            || (!itemHitItemScript.isCloned && !itemHitItemScript.isFullOfPotions && gameObject.tag == "ClonePotion"))
            {

                itemScript itemScriptObjectOnMouse = hitSomething.collider.gameObject.GetComponent<itemScript>();

                if(itemScriptLastObjectSaved != itemScriptObjectOnMouse)
                {
                    itemScriptLastObjectSaved = itemScriptObjectOnMouse;
                }
                if(itemScriptLastObjectSaved != null)
                {
                    itemScriptLastObjectSaved.isGlowing = true;

                    Color glowingColorOfItem = new Color(1f, 1f, 1f, 1f); // Default
                    if(gameObject.tag == "BiggerPotion")
                    {
                        ColorUtility.TryParseHtmlString( "#A53030", out glowingColorOfItem);
                    }
                    else if(gameObject.tag == "ShrinkPotion")
                    {
                        ColorUtility.TryParseHtmlString( "#4F8FBA", out glowingColorOfItem);
                    }
                    else if(gameObject.tag == "StrangePotion")
                    {
                        ColorUtility.TryParseHtmlString( "#23BC6B", out glowingColorOfItem);
                    }
                    else if(gameObject.tag == "ClonePotion")
                    {
                        ColorUtility.TryParseHtmlString( "#8339B2", out glowingColorOfItem);
                    }
                    itemScriptObjectOnMouse.SwitcherGlowing(glowingColorOfItem);
                }
                
            }
            // Si la potion n'est pas compatible avec un item
            else
            {
                if(itemScriptLastObjectSaved != null)
                {
                    itemScriptLastObjectSaved.isGlowing = false;
                    Color defaultColor = new Color(1f, 1f, 1f, 1f);
                    itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
                }
                    
            }
        }
        // Si la souris ne pointe plus l'object faut le mettre à jour
        else
        {
            if(itemScriptLastObjectSaved != null)
            {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
            }
                
        }


    }


   public void OnClick(InputAction.CallbackContext context)
   {
       if (!context.started) return;

       var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

       if (!rayHit.collider) return;

        

       itemScript itemHitItemScript = rayHit.collider.gameObject.GetComponent<itemScript>();

       if (itemHitItemScript == null || itemHitItemScript.isTable == true) return; // les items qui peuvent pas être potionés

       if (itemHitItemScript.isClear && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(0).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 2.5f;

           // MODIFY STATE
           itemHitItemScript.isBiggerOnce = true;
           itemHitItemScript.isClear = false;
           itemHitItemScript.isReady = false;
           itemHitItemScript.isFalling = true;
           
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }

            

           Destroy(gameObject);

       }
       else if (itemHitItemScript.isClear && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(1).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 2.5f;

           // MODIFY STATE
           itemHitItemScript.isShrinkOnce = true;
           itemHitItemScript.isClear = false;
           itemHitItemScript.isReady = false;
           itemHitItemScript.isFalling = true;
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }
           Destroy(gameObject);

       }

        else if (itemHitItemScript.isClear && gameObject.tag == "StrangePotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(6).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           MMF_Player feedbacksItemFlicker = rayHit.collider.gameObject.transform.GetChild(8).GetComponent<MMF_Player>(); // Flicker
           feedbacksItemFlicker.PlayFeedbacks();
           rigidbody2DItem.mass /= 2f; // Pour lui c'est par 2 et non 2.5 ou 1.75 car c'est la potion étrange

           // MODIFY STATE
           itemHitItemScript.isStrange = true;
           itemHitItemScript.isClear = false;
           itemHitItemScript.isReady = false;
           itemHitItemScript.isFalling = true;

           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }
           Destroy(gameObject);

       }  

        else if (itemHitItemScript.isBiggerOnce && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(2).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 1.75f;
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isBiggerOnce = false;
           itemHitItemScript.isReady = false;
           itemHitItemScript.isFalling = true;


           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       }   



        else if (itemHitItemScript.isShrinkOnce && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(3).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 1.75f;
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isShrinkOnce = false;
           itemHitItemScript.isReady = false;
           itemHitItemScript.isFalling = true;

           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }
           Destroy(gameObject);

       }   


        else if (itemHitItemScript.isBiggerOnce  && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(4).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 1.75f;
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isBiggerOnce = false;
           itemHitItemScript.isReady = false;
           itemHitItemScript.isFalling = true;

           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }
           Destroy(gameObject);

       } 


        else if (itemHitItemScript.isShrinkOnce && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(5).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 1.75f;
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isShrinkOnce = false;
           itemHitItemScript.isReady = false;
           itemHitItemScript.isFalling = true;
           
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }
           Destroy(gameObject);

       }   


        else if (!itemHitItemScript.isCloned && !itemHitItemScript.isFullOfPotions && gameObject.tag == "ClonePotion")
       {
           // Stop glowing effect (early for Clone potion because of the prefab)
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                Color defaultColor = new Color(1f, 1f, 1f, 1f);
                itemScriptLastObjectSaved.SwitcherGlowing(defaultColor);
           }
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(9).GetComponent<MMF_Player>(); // Devient Violet
           feedbacksItem.PlayFeedbacks();
        
           SpriteRenderer prefabMaterial = rayHit.collider.gameObject.transform.GetComponent<SpriteRenderer>(); // Prévention de si le flicker change la couleur
           prefabMaterial.material.SetColor("_Color", Color.white);
           SpawnClonedItem(rayHit.collider.gameObject);
           // GameManager.instance.canAccessToInventory = true;
           
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isCloned = true;

           Destroy(gameObject);
       }  
 

   }


    // For spawned items
    public void SpawnClonedItem(GameObject prefabClone) // == rayHit.collider.gameObject
    {

        GameObject prefab = Instantiate(prefabClone, new Vector3(prefabClone.transform.position.x, prefabClone.transform.position.y, prefabClone.transform.position.z), Quaternion.identity);
        Transform prefabTransform = prefab.GetComponent<Transform>();

        Rigidbody2D prefabRigid = prefab.GetComponent<Rigidbody2D>();

        Collider2D prefabCollider = prefab.GetComponent<Collider2D>();


        prefabCollider.isTrigger = true; // Désactiver les collisions pendant le vol

        itemScript prefabCloneItemScript = prefabClone.GetComponent<itemScript>();
        itemScript prefabItemScript = prefab.GetComponent<itemScript>();

        prefabItemScript.isClear = prefabCloneItemScript.isClear;
        prefabItemScript.isShrinkOnce = prefabCloneItemScript.isShrinkOnce;
        prefabItemScript.isBiggerOnce = prefabCloneItemScript.isBiggerOnce;
        prefabItemScript.isFullOfPotions = prefabCloneItemScript.isFullOfPotions;
        prefabItemScript.isCloned = prefabCloneItemScript.isCloned;
        prefabItemScript.isStrange = prefabCloneItemScript.isStrange;
        prefabItemScript.isExplodedFromStrange = prefabCloneItemScript.isExplodedFromStrange;

        if(prefabCloneItemScript.isStrange == true) // Si flicker de l'enfant actif
        {
            MMF_Player feedbacksItemFlicker = prefabTransform.GetChild(8).GetComponent<MMF_Player>(); // Animation item qui flotte
            feedbacksItemFlicker.PlayFeedbacks();
        }

        GameManager.instance.isInventoryOpen = false;
        GameManager.instance.forceToCloseDescription = true;

        GameManager.instance.canAccessToInventory = false;

        // Animation
        prefabItemScript.CloningAnimation(); // Quand animation terminé
        //GameObject[] cursorForNewItem = GameObject.FindGameObjectsWithTag("Cursor"); // -> MAINTENANT ON FAIT DANS ITEMSCRIPT UNE FOIS FEEDBACK FINIT
        //prefab.transform.SetParent(cursorForNewItem[0].transform); // Mettre dans cursor une fois l'animation finit

    }


}