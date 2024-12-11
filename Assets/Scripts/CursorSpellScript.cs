using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEditor;
using UnityEngine;
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
                    itemScriptLastObjectSaved.SwitcherGlowing();
                }
            }


            // Si la potion est compatible avec l'item touché
            if((hitSomething.collider.gameObject.tag == "item" && (gameObject.tag == "BiggerPotion" || gameObject.tag == "ShrinkPotion" || gameObject.tag == "StrangePotion"))
            || (hitSomething.collider.gameObject.tag == "BiggerItem" || hitSomething.collider.gameObject.tag == "ShrinkItem") && (gameObject.tag == "BiggerPotion" || gameObject.tag == "ShrinkPotion")
            || (hitSomething.collider.gameObject.tag != "Cloned(Not More Usable)" && hitSomething.collider.gameObject.tag != "CantAcceptPotions" && gameObject.tag == "ClonePotion"))
            {

                itemScript itemScriptObjectOnMouse = hitSomething.collider.gameObject.GetComponent<itemScript>();

                if(itemScriptLastObjectSaved != itemScriptObjectOnMouse)
                {
                    itemScriptLastObjectSaved = itemScriptObjectOnMouse;
                }
                if(itemScriptLastObjectSaved != null)
                {
                    itemScriptLastObjectSaved.isGlowing = true;
                    itemScriptObjectOnMouse.SwitcherGlowing();
                }
                
            }
            // Si la potion n'est pas compatible avec un item
            else
            {
                if(itemScriptLastObjectSaved != null)
                {
                    itemScriptLastObjectSaved.isGlowing = false;
                    itemScriptLastObjectSaved.SwitcherGlowing();
                }
                    
            }
        }
        // Si la souris ne pointe plus l'object faut le mettre à jour
        else
        {
            if(itemScriptLastObjectSaved != null)
            {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
            }
                
        }


    }


   public void OnClick(InputAction.CallbackContext context)
   {
       if (!context.started) return;

       var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

       if (!rayHit.collider) return;

        

       itemScript itemHitItemScript = rayHit.collider.gameObject.GetComponent<itemScript>();

       if (itemHitItemScript == null || rayHit.collider.gameObject.tag == "itemTable") return; // les items qui peuvent pas être potionés

       if (itemHitItemScript.isClear && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(0).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 2.5f;

           // MODIFY STATE
           itemHitItemScript.isBiggerOnce = true;
           itemHitItemScript.isClear = false;
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
           }
           Destroy(gameObject);

       }
       else if (itemHitItemScript.isClear && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(1).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 2.5f;
           rayHit.collider.gameObject.tag = "ShrinkItem";

           // MODIFY STATE
           itemHitItemScript.isShrinkOnce = true;
           itemHitItemScript.isClear = false;
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
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
           rayHit.collider.gameObject.tag = "ReadyToExplode";

           // MODIFY STATE
           itemHitItemScript.isStrange = true;
           itemHitItemScript.isClear = false;
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
           }
           Destroy(gameObject);

       }  

        else if (itemHitItemScript.isBiggerOnce && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(2).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 1.75f;
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isBiggerOnce = false;


           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
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
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isShrinkOnce = false;
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
           }
           Destroy(gameObject);

       }   


        else if (itemHitItemScript.isBiggerOnce  && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(4).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 1.75f;
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isBiggerOnce = false;
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
           }
           Destroy(gameObject);

       } 


        else if (itemHitItemScript.isShrinkOnce && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(5).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 1.75f;
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

           // MODIFY STATE
           itemHitItemScript.isFullOfPotions = true;
           itemHitItemScript.isShrinkOnce = false;
           
           GameManager.instance.canAccessToInventory = true;
           // Stop glowing effect
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
           }
           Destroy(gameObject);

       }   


        else if (rayHit.collider.gameObject.tag != "Cloned(Not More Usable)" && rayHit.collider.gameObject.tag != "CantAcceptPotions" && gameObject.tag == "ClonePotion")
       {
           // Stop glowing effect (early for Clone potion because of the prefab)
           if(itemScriptLastObjectSaved != null)
           {
                itemScriptLastObjectSaved.isGlowing = false;
                itemScriptLastObjectSaved.SwitcherGlowing();
           }
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(9).GetComponent<MMF_Player>(); // Devient Violet
           feedbacksItem.PlayFeedbacks();
        
           SpriteRenderer prefabMaterial = rayHit.collider.gameObject.transform.GetComponent<SpriteRenderer>(); // Prévention de si le flicker change la couleur
           prefabMaterial.material.SetColor("_Color", Color.white);
           SpawnClonedItem(rayHit.collider.gameObject);
           // GameManager.instance.canAccessToInventory = true;

           rayHit.collider.gameObject.tag = "Cloned(Not More Usable)"; // Quand le clone est fini
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

        prefab.tag = prefabClone.tag; // Remettre l'item par défaut

        if(prefabClone.tag == "ReadyToExplode") // Si flicker de l'enfant actif
        {
            prefab.tag = "ReadyToExplode";
            MMF_Player feedbacksItemFlicker = prefabTransform.GetChild(8).GetComponent<MMF_Player>(); // Animation item qui flotte
            feedbacksItemFlicker.PlayFeedbacks();

        }

        GameManager.instance.isInventoryOpen = false;
        GameManager.instance.forceToCloseDescription = true;

        GameManager.instance.canAccessToInventory = false;

        // Animation
        prefabRigid.gravityScale = 0;
        Vector3 ForceToGoToMouse = Vector3.SmoothDamp(moveThis.position, new Vector3(targetPos.x, 3.75f, 0), ref velocity, timeOffSet); 

        MMF_Player feedbacksItem = prefabTransform.GetChild(10).GetComponent<MMF_Player>(); // Animation item qui flotte
        feedbacksItem.PlayFeedbacks();

        itemScript prefabItemScript = prefab.GetComponentInChildren<itemScript>();

        prefabItemScript.WhenCloningDoneGoCursorParent();
        //GameObject[] cursorForNewItem = GameObject.FindGameObjectsWithTag("Cursor"); // -> MAINTENANT ON FAIT DANS ITEMSCRIPT UNE FOIS FEEDBACK FINIT
        //prefab.transform.SetParent(cursorForNewItem[0].transform); // Mettre dans cursor une fois l'animation finit

    }


}