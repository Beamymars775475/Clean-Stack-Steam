using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuLever : MonoBehaviour
{

    private Camera _mainCamera;

    public bool leverBool;

    public GameObject shelfForButtonsMenu;

    public bool needToSpawnButtons;

    public SmallAnimationMainMenuScript mainMenuSpawners;

    public LevelLoader levelLoader;

    
    void Start()
    {
        shelfForButtonsMenu.SetActive(false);

        _mainCamera = Camera.main; 

        leverBool = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainMenuSpawners.placeToPutFlyingItems.childCount == 0)
        {
            shelfForButtonsMenu.SetActive(true);

            if(needToSpawnButtons)
            {
                mainMenuSpawners.SpawnMenuButtons();
                needToSpawnButtons = false;
            }
        }
        else
        {
            shelfForButtonsMenu.SetActive(false);
        }
    }

   public void OnClick(InputAction.CallbackContext context)
   {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider) return;
       
        if(rayHit.collider.gameObject.tag == "Lever")
        {
            if(leverBool)
            {
                leverBool = false;
                mainMenuSpawners.isAbleToStartFlood = false;
                needToSpawnButtons = true;
            }
            else
            {
                leverBool = true;
                mainMenuSpawners.isAbleToStartFlood = true;
            }
        }

        if(rayHit.collider.gameObject.tag == "ButtonQuit")
        {
            Debug.Log("Quit");
        }

        if(rayHit.collider.gameObject.tag == "ButtonSettings")
        {
            levelLoader.LoadThisSceneWithAnimation("SettingsScene");
        }

        if(rayHit.collider.gameObject.tag == "ButtonFreeSpace")
        {
            levelLoader.LoadThisSceneWithAnimation("BestiaryScene");
        }



   }


}

