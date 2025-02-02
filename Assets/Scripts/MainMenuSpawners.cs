using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MainMenuSpawners : MonoBehaviour
{

    private Camera _mainCamera;
    public GameObject[] prefabs;

    public bool canSpawn;

    
    void Start()
    {
        canSpawn = true;
        _mainCamera = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void OnClick(InputAction.CallbackContext context)
   {


       if (!context.started) return;

       var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

       if (!rayHit.collider) return;

       if(rayHit.collider.gameObject.tag == "spawner" && canSpawn && !GameManager.instance.isInDialogueWithMonster)
       {
           StartCoroutine(CooldownToSpawn(0.2f));
           Instantiate(prefabs[Random.Range(0, prefabs.Length)], new Vector3(gameObject.transform.position.x-1f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
       }
   }


    IEnumerator CooldownToSpawn(float cooldown)
    {
        canSpawn = false;
        yield return new WaitForSeconds(cooldown);

        canSpawn = true;
    }
}
