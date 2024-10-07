using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class SimpleMouseMovements : MonoBehaviour
{

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

    public MMF_Player feedbacksInventory;

    public GameObject inventory;


    void Start()
    {
        
    }


    void Update()
    {
        Debug.Log(gameObject);

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
                Vector3 ForceToGoToMouse = Vector3.SmoothDamp(moveThis.position, new Vector3(targetPos.x, 3.75f, 0), ref velocity, timeOffSet); 

                moveThis.transform.position = ForceToGoToMouse; 
            }
            if(mouse.y<-3.5f && GameManager.instance.isInventoryOpen == false && inventory.transform.childCount > 1) // Pour ouvrir l'inventaire
            {
                GameManager.instance.isInventoryOpen = true;
                feedbacksInventory.PlayFeedbacks();
            }

        }   

    }
}
