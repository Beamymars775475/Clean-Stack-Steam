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
    public GameObject darkness;

    public bool needToSpawnButtons;

    public SmallAnimationMainMenuScript mainMenuSpawners;

    public LevelLoader levelLoader;

    public Sprite buttonUp;
    public Sprite buttonDown;

    public bool alreadyTriedToQuit = false; // POUR SCENE

    [Header("For title easter egg")]
    public GameObject title;
    
    void Start()
    {
        shelfForButtonsMenu.SetActive(false);
        darkness.SetActive(false);

        _mainCamera = Camera.main; 

        leverBool = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainMenuSpawners.isAbleToStartFlood )
        {
            darkness.SetActive(false);
        }
        else
        {
            darkness.SetActive(true);
        }

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
       
        if(rayHit.collider.gameObject.tag == "Lever" && !GameManager.instance.isInDialogueWithMonster)
        {
            SpriteRenderer gameobjectSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if(leverBool)
            {
                leverBool = false;
                mainMenuSpawners.isAbleToStartFlood = false;
                needToSpawnButtons = true;
                
                gameobjectSpriteRenderer.sprite = buttonDown;
            }
            else
            {
                leverBool = true;
                mainMenuSpawners.isAbleToStartFlood = true;

                gameobjectSpriteRenderer.sprite = buttonUp;
            }
        }

        if(rayHit.collider.gameObject.tag == "ButtonQuit" && !GameManager.instance.isInDialogueWithMonster)
        {
            if(GameManager.instance.levelsState[GameManager.instance.MONDES[0]] != 0 || GameManager.instance.levelsState[GameManager.instance.MONDES[1]] != 0 || GameManager.instance.levelsState[GameManager.instance.MONDES[2]] != 0)
            {
                Debug.Log("I Quit !");
                Application.Quit();
            }
            else
            {
                GameManager.instance.indexDialogues = 0; // RESET
                GameManager.instance.TriggerEvent();
                if(!alreadyTriedToQuit)
                {
                    GameManager.instance.indexDialogues = 10001;
                    alreadyTriedToQuit = true;
                }
                else
                {
                    GameManager.instance.indexDialogues = 10002;
                }
                GameManager.instance.TriggerEvent();

                GameManager.instance.indexDialogues = 122774647; // LAUNCH
                GameManager.instance.TriggerEvent();
            }
        }

        if(rayHit.collider.gameObject.tag == "ButtonSettings" && !GameManager.instance.isInDialogueWithMonster)
        {
            levelLoader.LoadThisSceneWithAnimation("SettingsScene");
        }

        if(rayHit.collider.gameObject.tag == "ButtonFreeSpace" && !GameManager.instance.isInDialogueWithMonster)
        {
            levelLoader.LoadThisSceneWithAnimation("BestiaryScene");
        }

        if(rayHit.collider.gameObject.name == "Screw")
        {
            Destroy(rayHit.collider.transform.parent.gameObject);
        }

        if(rayHit.collider.gameObject.name == "Aliz")
        {
            SteamIntegration steamIntegrationGameManager = GameManager.instance.gameObject.GetComponent<SteamIntegration>();
            steamIntegrationGameManager.UnlockAchievement("ACH_02");
        }

   }
}

