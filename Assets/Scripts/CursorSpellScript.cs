using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
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
    public LayerMask hitLayers2;

    public MouseMovements mouseMovements;

    public Transform spaceForFullItems;


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
           rigidbody2DItem.mass /= 1.75f;
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


 


   }
}