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

    public float timeOffSet;
    private Vector3 velocity;

    public Transform itemsThrowed;

    float rayLength = 0.5f;
        
	Vector3 dir;
	Ray ray;
    RaycastHit hit_ray;
    public LayerMask hitLayers2;

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
        
        if (gameObject.transform.childCount != 0 && (gameObject.transform.GetChild(0).tag == "item" || gameObject.transform.GetChild(0).tag == "itemTable" || gameObject.transform.GetChild(0).tag == "ReadyToExplode" || gameObject.transform.GetChild(0).tag == "BiggerItem" || gameObject.transform.GetChild(0).tag == "ShrinkItem")) // ReadyToExplode for clones who have Strange potion effect 
        {
            moveThis = gameObject.transform.GetChild(0);
            moveThisRigidbody = moveThis.GetComponent<Rigidbody2D>();
            moveThisCollider = moveThis.GetComponent<Collider2D>();
            moveThisCollider.isTrigger = true;
            moveThisRigidbody.gravityScale = 0;
            GameManager.instance.canAccessToInventory = false;
            GameManager.instance.isCountDownOn = false; // Ici


            GameManager.instance.waitUntilFirstObject = false;

            if (Physics.Raycast(castPointmouse, out hitMouse, Mathf.Infinity, hitLayers) && moveThisRigidbody.gravityScale != 1)
            {
                Vector3 targetPos = hitMouse.point;

                dir = transform.TransformDirection(new Vector3(moveThis.transform.position.x - targetPos.x, 0, 0));


                ray = new Ray(moveThis.transform.position, dir * rayLength);

                if (!Physics.Raycast(ray, out hit_ray, Mathf.Infinity, hitLayers2))
                {
                    Vector3 ForceToGoToMouse = Vector3.SmoothDamp(moveThis.position, new Vector3(targetPos.x, 3.75f, 0), ref velocity, timeOffSet); 

                    moveThis.transform.position = ForceToGoToMouse; 
                }


            }   

            if(gameObject.transform.GetChild(0).tag != "itemTable")
            {
                moveThis.Rotate(new Vector3(0, 0, Input.mouseScrollDelta.y*Time.deltaTime*800));
            }
        

            if (Input.GetMouseButtonDown(0))
            {
                moveThisRigidbody.gravityScale = 1;
                moveThisCollider.isTrigger = false;
                moveThis.SetParent(itemsThrowed);
                GameManager.instance.canAccessToInventory = true;
                if(moveThis.tag == "itemTable")
                {
                    moveThis.SetParent(tables);
                }
                else if(moveThis.tag == "ReadyToExplode")
                {
                    moveThis.SetParent(spaceForFullItems);
                }
            }
        }



        if(inventory.transform.childCount <= 2 && GameManager.instance.isCountDownOn == false && gameObject.transform.childCount == 0 && GameManager.instance.isInCloningProcess == false) // Condition de Win
        {
            StartCoroutine(CountDownUntilWin(5f));
        }

        if(Input.GetKeyDown("space") && GameManager.instance.controlsPreference == 0) // Pour space
        {
            if(GameManager.instance.isInventoryOpen == false && GameManager.instance.canAccessToInventory && inventory.transform.childCount > 2 && GameManager.instance.isWon == false && GameManager.instance.isGameOver == false && GameManager.instance.animationInventoryIsDone == true)
            {
                GameManager.instance.isInventoryOpen = true;
                feedbacksInventory.PlayFeedbacks();
            }
        }
  
    }

    IEnumerator CountDownUntilWin(float cooldown)
    {
        if(GameManager.instance.isDelivered2 == false) // Sus a check
        {
            GameManager.instance.isDelivered2 = true;
            GameManager.instance.isCountDownOn = true;

            yield return new WaitForSeconds(0.4f);
            // ça c'est safe
            GameManager.instance.activeStrangePotion = true;
            Debug.Log("aaaa");
            yield return new WaitForSeconds(cooldown-0.4f); // Anim Strange

            GameManager.instance.isCountDownOn = false;
            GameManager.instance.isWon = true;


            GameManager.instance.levelsState[SceneManager.GetActiveScene().buildIndex-2] = true; // Update de la valeur dans le Level Selector // -2 Car MainScene et Level Selector


        } // Mettre ça dans un autre script pour faire plus propre

    }


    public void OnceAnimationFinishYouCanOpenInventory()
    {
        GameManager.instance.animationInventoryIsDone = true;
    }
    

    public void OnClick() // When preference click on Inventory
   {
       if(GameManager.instance.controlsPreference == 1)
       {
            if(GameManager.instance.isInventoryOpen == false && GameManager.instance.canAccessToInventory && inventory.transform.childCount > 2 && GameManager.instance.isWon == false && GameManager.instance.isGameOver == false && GameManager.instance.animationInventoryIsDone == true)
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
            if(GameManager.instance.isInventoryOpen == false && GameManager.instance.canAccessToInventory && inventory.transform.childCount > 2 && GameManager.instance.isWon == false && GameManager.instance.isGameOver == false && GameManager.instance.animationInventoryIsDone == true)
            {
                GameManager.instance.isInventoryOpen = true;
                feedbacksInventory.PlayFeedbacks();
            }
        }    
   }
}
