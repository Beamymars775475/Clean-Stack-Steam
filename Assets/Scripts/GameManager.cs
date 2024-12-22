using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.Audio;
using System.Linq;
using UnityEngine.SceneManagement;

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

    public int[] levelsState; // 0 - Pas fait | 1 - Fait | 2 - Hardcore

    public bool goNextFloor;

    // Faut check
    [Header("Other")]
    public bool FirstLaunch;


    [Header("Checks for win (NEED REWORK !!)")]

    public bool isPhase1Done;

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

    [Header("Sounds")]
    public AudioMixer scrollBars;

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

        if(scrollBars != null)
        {
            scrollBars.SetFloat("MasterVolume", data.volumeMain); 
            scrollBars.SetFloat("MusicVolume", data.volumeMusic); 
            scrollBars.SetFloat("SfxVolume", data.volumeSound); 
        }

        Resolution[] resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
                
        List<string> options = new List<string>();
        List<int> resolutionsGoodRatioIndexList = new List<int>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if((float)resolutions[i].width/resolutions[i].height > 1.7f && (float)resolutions[i].width/resolutions[i].height < 1.8f)
            {
                resolutionsGoodRatioIndexList.Add(i);
            }
        } 

        if(data.resolutionToUse == -1)
        {
            Resolution resolution = resolutions[resolutionsGoodRatioIndexList[resolutionsGoodRatioIndexList.Count-1]]; // resolutionsGoodRatioIndexList donne le vrai Index
            Screen.SetResolution(resolution.width, resolution.height, data.isFullScreen);  
        }
        else
        {
            Resolution resolution = resolutions[resolutionsGoodRatioIndexList[data.resolutionToUse]]; // resolutionsGoodRatioIndexList donne le vrai Index
            Screen.SetResolution(resolution.width, resolution.height, data.isFullScreen);  
        }

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

}
