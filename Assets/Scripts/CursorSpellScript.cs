using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
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


    }


   public void OnClick(InputAction.CallbackContext context)
   {
       if (!context.started) return;

       var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

       if (!rayHit.collider) return;


       if (rayHit.collider.gameObject.tag == "item" && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(0).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 2.5f;
           rayHit.collider.gameObject.tag = "BiggerItem";
           
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       }
       else if (rayHit.collider.gameObject.tag == "item" && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(1).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 2.5f;
           rayHit.collider.gameObject.tag = "ShrinkItem";
           
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       }

        else if (rayHit.collider.gameObject.tag == "item" && gameObject.tag == "StrangePotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(6).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           MMF_Player feedbacksItemFlicker = rayHit.collider.gameObject.transform.GetChild(8).GetComponent<MMF_Player>(); // Flicker
           feedbacksItemFlicker.PlayFeedbacks();
           rigidbody2DItem.mass /= 2f; // Pour lui c'est par 2 et non 2.5 ou 1.75 car c'est la potion étrange
           rayHit.collider.gameObject.tag = "ReadyToExplode";
           
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       }  

        else if (rayHit.collider.gameObject.tag == "BiggerItem" && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(2).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 1.75f;
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);
           
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       }   



        else if (rayHit.collider.gameObject.tag == "ShrinkItem" && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(3).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 1.75f;
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);
           
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       }   


        else if (rayHit.collider.gameObject.tag == "BiggerItem" && gameObject.tag == "BiggerPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(4).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass *= 1.75f;
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);
           
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       } 


        else if (rayHit.collider.gameObject.tag == "ShrinkItem" && gameObject.tag == "ShrinkPotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(5).GetComponent<MMF_Player>();
           Rigidbody2D rigidbody2DItem = rayHit.collider.gameObject.GetComponentInChildren<Rigidbody2D>();
           feedbacksItem.PlayFeedbacks();
           rigidbody2DItem.mass /= 1.75f;
           rayHit.collider.gameObject.tag = "CantAcceptPotions";
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);
           
           GameManager.instance.canAccessToInventory = true;
           Destroy(gameObject);

       }   


        else if (rayHit.collider.gameObject.tag != "Cloned(Not More Usable)" && gameObject.tag == "ClonePotion")
       {
           MMF_Player feedbacksItem = rayHit.collider.gameObject.transform.GetChild(9).GetComponent<MMF_Player>(); // Devient Violet
           feedbacksItem.PlayFeedbacks();
        
           SpriteRenderer prefabMaterial = rayHit.collider.gameObject.transform.GetComponent<SpriteRenderer>(); // Prévention de si le flicker change la couleur
           prefabMaterial.material.SetColor("_Color", Color.white);
           SpawnClonedItem(rayHit.collider.gameObject);
           // GameManager.instance.canAccessToInventory = true;

           rayHit.collider.gameObject.tag = "Cloned(Not More Usable)"; // Quand le clone est fini
           rayHit.collider.gameObject.transform.SetParent(spaceForFullItems);

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

        prefab.tag = "item"; // Remettre l'item par défaut

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

        Debug.Log(GameObject.FindGameObjectsWithTag("Cursor")[0]);
        GameObject[] cursorForNewItem = GameObject.FindGameObjectsWithTag("Cursor");
        prefab.transform.SetParent(cursorForNewItem[0].transform); // Mettre dans cursor une fois l'animation finit

    }

    
}