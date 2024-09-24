using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class GameManager : MonoBehaviour
{

    public static GameManager instance {get; private set;}

    public bool isInventoryOpen;

    public string currentTextName;
    public string currentTextDescription;

    public string currentColor;
    
    public bool goNextFloor;

    public bool forceToCloseDescription;

    public bool canAccessToInventory;

    public bool needImageDark;
    
    public bool isCountDownOn;

    public bool isWon;

    public int currentLevel;

    public bool isGameOver;

    public bool isNeedToStuckUI;

    public bool[] levelsState;

    // Faut check

    public bool FirstLaunch;

    public bool waitUntilFirstObject;

    public bool isDelivered2;

    public bool activeStrangePotion;


    public readonly int MONDE1 = 0; // 0
    public readonly int MONDE2 = 11; // 12
    public readonly int MONDE3 = 23; // 24
    public readonly int MONDE4 = 35; // 36 
    public readonly int MONDE5 = 43; // 44
    public readonly int MONDE6 = 51; // 52
    public readonly int MONDE7 = 59; // 60  

// Compter tout les True de complete pour unlock les niveaux 4, 5 et 6 puis 7 quand tout fini

    public MMF_Player feedbacksOpen; // Anim du son
    public MMF_Player feedbacksClose;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        goNextFloor = false;
        currentLevel = 1;

        levelsState = new bool[65];

        
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

    
}
