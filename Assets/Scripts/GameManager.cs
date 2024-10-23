using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, IDataPersistence
{

    public static GameManager instance {get; private set;}

    public bool isInventoryOpen;

    [Header("For Description")]

    public string currentTextName;
    public string currentTextDescription;

    public bool needImageDark;


    [Header("Inventory")]
    public bool forceToCloseDescription;

    public bool animationInventoryIsDone;
    public bool canAccessToInventory;



    [Header("Level management")]
    public bool isCountDownOn;

    public bool isWon;

    public int currentLevel;

    public bool isGameOver;

    public bool isNeedToStuckUI;

    public bool[] levelsState;

    public bool goNextFloor;

    // Faut check
    [Header("Other")]
    public bool FirstLaunch;


    [Header("Checks for win (NEED REWORK !!)")]
    public bool waitUntilFirstObject;

    public bool isDelivered2;
    
    [Header("Strange Potion")]
    public bool activeStrangePotion;

    [Header("Clone Potion")]
    public bool isInCloningProcess;

    [Header("Worlds")]
    public readonly int MONDE1 = 0; // 0
    public readonly int MONDE2 = 11; // 12
    public readonly int MONDE3 = 23; // 24
    public readonly int MONDE4 = 35; // 36 
    public readonly int MONDE5 = 43; // 44
    public readonly int MONDE6 = 51; // 52
    public readonly int MONDE7 = 59; // 60  

    // Compter tout les True de complete pour unlock les niveaux 4, 5 et 6 puis 7 quand tout fini

    [Header("Bestiary")]

    public bool[] AlreadyUsedItem;

    [Header("Dialogues")]

    public bool isInDialogueWithMonster;



    public MMF_Player feedbacksOpen; // Anim du son
    public MMF_Player feedbacksClose;

    [Header("Preferences")]

    public int controlsPreference = 2; // 0->Space 1->Click 2->Mouse Over (Default)
    public bool isTransparencyNeeded;
    public bool modeHard;

    public delegate void OnSoftlockStrangePotion();
    public static event OnSoftlockStrangePotion LaunchDialogue; // Potion full de strange potion et pas possible de les placer


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        goNextFloor = false;
        currentLevel = 1;
    }


    public void InitGame()
    {
        Debug.Log("Demarrage du jeu...");
    }

    // Si je vois un double de moi j'explose sinon je vie
    private void Awake() 
    {
        if(instance != null && instance != this)
        { 
            Destroy(gameObject); 
        }
        else
        {
            instance = this;
        }
    }



    // ----------------- SAVE DATA -----------------

    public void LoadData(GameData data)
    {
        AlreadyUsedItem = data.itemDiscovered;
        levelsState = data.levelsDone;
        
    }

    public void SaveData(GameData data)
    {
        data.itemDiscovered = AlreadyUsedItem;
        data.levelsDone = levelsState;
    } 


    public void TriggerEvent()
    {
        // Si quelque chose est abonné à l'événement, on le déclenche
        if(LaunchDialogue != null)
        {
            LaunchDialogue.Invoke();
        }
    }


    void TriggerOnSoftlockStrangePotion()
    {}
}
