using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MouseMovements : MonoBehaviour
{

    public Transform moveThis;
    //the layers the ray can hit

    public Rigidbody2D moveThisRigidbody; // Peut tomber ou pas

    public Collider2D moveThisCollider; 
    public LayerMask hitLayers;

    private float timeOffSet = 0.05f;
    private Vector3 velocity;

    public Transform itemsThrowed;

    float rayLength = 0.5f;
        
	Vector3 dir;
	Ray ray;
    RaycastHit hit_ray;
    public LayerMask hitLayers2;

    [Header("Glowing")]

    public itemScript itemScriptLastObjectSaved;

    [Header("Other")]
    
    public MMF_Player feedbacksInventory;

    public GameObject inventory;

    public Transform tables; // Pour les tables

    public Transform spaceForFullItems; // Pour les clones




    void Start()
    {
        feedbacksInventory.PlayFeedbacks();
    }


    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPointmouse = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hitMouse;


        
        if (gameObject.transform.childCount != 0 && gameObject.transform.GetChild(0).tag == "item")
        {
            moveThis = gameObject.transform.GetChild(0);
            moveThisRigidbody = moveThis.GetComponent<Rigidbody2D>();
            moveThisCollider = moveThis.GetComponent<Collider2D>();

            if(moveThisRigidbody != null) // NORMAL
            {
            moveThisRigidbody.gravityScale = 0;
            }
            if(moveThisCollider != null) // NORMAL
            {
                moveThisCollider.isTrigger = true;
            }
            itemScript itemItemScript = moveThis.GetComponent<itemScript>();
            if(itemItemScript.isSTUCKOnWall)
            {
                moveThisRigidbody.bodyType = RigidbodyType2D.Static; // POUR LA CORDE
            }


            foreach(Transform child in moveThis) // POUR LES GOSSES
            {
                Rigidbody2D moveThisRigidbodyChild = child.GetComponent<Rigidbody2D>();
                Collider2D moveThisColliderChild = child.GetComponent<Collider2D>();
                if(moveThisRigidbodyChild != null)
                {
                    moveThisRigidbodyChild.gravityScale = 0;
                    moveThisRigidbodyChild.bodyType = RigidbodyType2D.Static; // POUR LA CORDE
                }
                if(moveThisColliderChild != null)
                {
                    moveThisColliderChild.isTrigger = true;
                }
            }

            GameManager.instance.canAccessToInventory = false;
            GameManager.instance.isCountDownOn = false; // Ici


            itemScript itemChilditemScript = gameObject.transform.GetChild(0).GetComponent<itemScript>(); // Item in hand
            if (Physics.Raycast(castPointmouse, out hitMouse, Mathf.Infinity, hitLayers)) // && moveThisRigidbody.gravityScale != 1
            {
                Vector3 targetPos = hitMouse.point;

                dir = transform.TransformDirection(new Vector3(moveThis.transform.position.x - targetPos.x, 0, 0));


                ray = new Ray(moveThis.transform.position, dir * rayLength);

                if (!Physics.Raycast(ray, out hit_ray, Mathf.Infinity, hitLayers2) && !itemChilditemScript.isFollowingMouse)
                {
                    Vector3 ForceToGoToMouse = Vector3.SmoothDamp(moveThis.position, new Vector3(targetPos.x, 3.75f, 0), ref velocity, timeOffSet); 

                    moveThis.transform.position = ForceToGoToMouse; 
                }
                else if (!Physics.Raycast(ray, out hit_ray, Mathf.Infinity, hitLayers2) && itemChilditemScript.isFollowingMouse)
                {
                    Vector3 ForceToGoToMouse = Vector3.SmoothDamp(moveThis.position, new Vector3(targetPos.x, targetPos.y, 0), ref velocity, timeOffSet); 

                    moveThis.transform.position = ForceToGoToMouse; 
                }


            }   



            if(!itemChilditemScript.isTable)
            {
                moveThis.Rotate(new Vector3(0, 0, Input.mouseScrollDelta.y*Time.deltaTime*800));
            }
        

            if (Input.GetMouseButtonDown(0))
            {
                itemScript itemDropitemScript = moveThis.GetComponent<itemScript>();
                if(moveThisRigidbody != null && !itemDropitemScript.isSTUCKOnWall)
                {
                    moveThisRigidbody.gravityScale = 1;
                }
                if(moveThisCollider != null) // NORMAL
                {
                    moveThisCollider.isTrigger = false;
                }

            foreach(Transform child in moveThis) // POUR LES GOSSES
            {
                Rigidbody2D moveThisRigidbodyChild = child.GetComponent<Rigidbody2D>();
                Collider2D moveThisColliderChild = child.GetComponent<Collider2D>();
                if(moveThisRigidbodyChild != null)
                {
                    moveThisRigidbodyChild.gravityScale = 1;
                    moveThisRigidbodyChild.bodyType = RigidbodyType2D.Dynamic; // POUR LA CORDE
                }
                if(moveThisColliderChild != null)
                {
                    moveThisColliderChild.isTrigger = false;
                }
            }


                moveThis.SetParent(itemsThrowed);
                GameManager.instance.canAccessToInventory = true;

                


                if(itemDropitemScript.isTable)
                {
                    moveThis.SetParent(tables);
                }
                else if(itemDropitemScript.isStrange)
                {
                    moveThis.SetParent(spaceForFullItems);
                }

                // COMMENCER TIMER
                itemDropitemScript.isFalling = true;
            }
        }

        // CLICKABLE SYSTEM
        if (gameObject.transform.childCount == 0)
        {
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

                // Si on touche bien une box
                if(itemHitItemScript.isClickable && !itemHitItemScript.isClicked)
                {

                    itemScript itemScriptObjectOnMouse = hitSomething.collider.gameObject.GetComponent<itemScript>();

                    if(itemScriptLastObjectSaved != itemScriptObjectOnMouse)
                    {
                        itemScriptLastObjectSaved = itemScriptObjectOnMouse;
                    }
                    if(itemScriptLastObjectSaved != null)
                    {
                        itemScriptLastObjectSaved.isGlowing = true;
                        Color defaultColor = new Color(1f, 1f, 1f, 1f);
                        itemScriptLastObjectSaved.SwitcherGlowing(defaultColor); 
                    }
                    
                }
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

        if(Input.GetKeyDown("space") && GameManager.instance.controlsPreference == 0) // Pour space
        {
            if(GameManager.instance.isInventoryOpen == false && GameManager.instance.canAccessToInventory && inventory.transform.childCount > 3 && GameManager.instance.isWon == false && GameManager.instance.isGameOver == false && GameManager.instance.animationInventoryIsDone == true)
            {
                GameManager.instance.isInventoryOpen = true;
                feedbacksInventory.PlayFeedbacks();
            }
        }
  
    }


    public void OnceAnimationFinishYouCanOpenInventory()
    {
        GameManager.instance.animationInventoryIsDone = true;
    }

    public void OnClick() // When preference click on Inventory
   {
       if(GameManager.instance.controlsPreference == 1)
       {
            if(GameManager.instance.isInventoryOpen == false && GameManager.instance.canAccessToInventory && inventory.transform.childCount > 3 && GameManager.instance.isWon == false && GameManager.instance.isGameOver == false && GameManager.instance.animationInventoryIsDone == true)
            {
                GameManager.instance.isInventoryOpen = true;
                feedbacksInventory.PlayFeedbacks();
            }
       }
   }

   public void OnEnterInventory()
   {
        if(GameManager.instance.controlsPreference == 2)
        {
            if(GameManager.instance.isInventoryOpen == false && GameManager.instance.canAccessToInventory && inventory.transform.childCount > 3 && GameManager.instance.isWon == false && GameManager.instance.isGameOver == false && GameManager.instance.animationInventoryIsDone == true)
            {
                GameManager.instance.isInventoryOpen = true;
                feedbacksInventory.PlayFeedbacks();
            }
        }    
   }
}
