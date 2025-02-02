using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;
using UnityEngine.AI;
using TMPro;


public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.9f;
    // Update is called once per frame

    public AudioClip musicMain;
    public AudioClip musicLevels;
    
    private bool isDoingLoading;


    public void Start()
    {

    }

    void Update()
    {
        if(GameManager.instance.goNextFloor == true) // A voir
        {
            LoadNextLevel();
            GameManager.instance.goNextFloor = false;

        }
    }
    
    public void LoadNextLevel()
    {

        if(SceneManager.GetActiveScene().name == "Mainscene")
        {
            GameManager.instance.firstTimeInMainMenuThisLaunch = false;
            StartCoroutine(LoadLevel("LevelSelectorScene"));
        }
        else if(SceneManager.GetActiveScene().name == "LevelSelectorScene")
        {
            LevelSelectorBoxScript boxLevelSelectorBoxScript = EventSystem.current.currentSelectedGameObject.GetComponent<LevelSelectorBoxScript>();

            // SI C LE PREMIER LEVEL
            if(boxLevelSelectorBoxScript.indexSceneOfButton == 0) // + 1 POUR LE PROCHAIN
            { 
                StartCoroutine(LoadLevel("Level" + (GameManager.instance.MONDES[boxLevelSelectorBoxScript.worldIndex] + boxLevelSelectorBoxScript.indexSceneOfButton)));
            }
            // SI LE LEVEL D'AVANT EST BON ET CELUI D'APRES AUSSI
            else if(GameManager.instance.levelsState[GameManager.instance.MONDES[boxLevelSelectorBoxScript.worldIndex] + boxLevelSelectorBoxScript.indexSceneOfButton - 1] == 1)
            { 
                StartCoroutine(LoadLevel("Level" + (GameManager.instance.MONDES[boxLevelSelectorBoxScript.worldIndex] + boxLevelSelectorBoxScript.indexSceneOfButton)));
            }
        } 

        else if(SceneManager.GetActiveScene().name != "LevelSelectorScene")
        {
            StartCoroutine(LoadLevel("Level" + (SceneManager.GetActiveScene().buildIndex-1)));  // -1 pour celui d'apres (2 de offset je rappelle)
        }

        else
        {
            Debug.LogError("Tu veux aller dans les backrooms ?");
        }

    }


    public void LoadMenu()
    {
        if(!isDoingLoading)
        {
            StartCoroutine(LoadLevel("LevelSelectorScene"));
            // GameManager.instance.isInventoryOpen = true; // ?
        }
    }

    public void LoadSameScene()
    {
        if(!isDoingLoading)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
        }
    }

    public void NextScene() // il fait tout le taff
    {
        if(GameManager.instance.isWon) // Reset 
        {
            GameManager.instance.currentLevel++;
            GameManager.instance.goNextFloor = true;
            GameManager.instance.isWon = false;
            GameManager.instance.isInventoryOpen = false;
            GameManager.instance.isCountDownOn = false;

            GameManager.instance.isPhase1Done = false; 

            GameManager.instance.activeStrangePotion = false;
        }
        else
        {
            GameManager.instance.isWon = false;
            GameManager.instance.isInventoryOpen = false;
            GameManager.instance.isCountDownOn = false;

            GameManager.instance.isPhase1Done = false; 
            GameManager.instance.activeStrangePotion = false;        
        }
        
        Debug.Log("KAZOOKIE !");
        GameManager.instance.indexDialogues = 0;
        GameManager.instance.TriggerEvent();
    }

    public void LoadThisSceneWithAnimation(string nameOfScene)
    {
        StartCoroutine(LoadLevel(nameOfScene));
    }

    IEnumerator LoadLevel(string levelIndex) // levelIndex c'est le niveau qu'on va lancer
    {
        isDoingLoading = true;


        transition.SetTrigger("Start");
        
        GameManager.instance.feedbacksClose.PlayFeedbacks();

        yield return new WaitForSeconds (transitionTime);

        AudioSource audio = GameManager.instance.GetComponent<AudioSource>();
        if(levelIndex == "Mainscene" || levelIndex == "LevelSelectorScene" || levelIndex == "BestiaryScene" || levelIndex == "SettingsScene") // Pas de changement de son entre ces 2 scènes
        {
            if(levelIndex == "BestiaryScene")
            {
                GameManager.instance.isInventoryOpen = true;
                GameManager.instance.canAccessToInventory = true; // Setup l'inv au début pour qu'il s'ouvre correctement                  
            }
            if(audio.clip != musicMain)
            {
                audio.clip = musicMain;
                audio.Play();
            }
        }
        else if(SceneManager.GetActiveScene().name == "LevelSelectorScene" && levelIndex != "Mainscene")
        {
            if(audio.clip != musicLevels)
            {
                audio.clip = musicLevels;
                audio.Play();

                GameManager.instance.isInventoryOpen = true;
                GameManager.instance.canAccessToInventory = true; // Setup l'inv au début pour qu'il s'ouvre correctement
            }
        }

        else if(levelIndex != "Mainscene" || levelIndex != "LevelSelectorScene" || levelIndex != "SettingsScene" )
        {
            GameManager.instance.isInventoryOpen = true;
            GameManager.instance.canAccessToInventory = true; // Setup l'inv au début pour qu'il s'ouvre correctement
        }





        GameManager.instance.feedbacksOpen.PlayFeedbacks();

        SceneManager.LoadScene(levelIndex);
        GameManager.instance.isGameOver = false;
        GameManager.instance.isNeedToStuckUI = false;

        GameManager.instance.isPhase1Done = false; 
        GameManager.instance.activeStrangePotion = false;

        GameManager.instance.isWon = false;
        GameManager.instance.isCountDownOn = false;


        GameManager.instance.indexDialogues = 0;
        GameManager.instance.TriggerEvent();


        isDoingLoading = false;
    }


    public void GoBackToMainsceneFromBestiaryOrSettings()
    {
        if(!isDoingLoading)
        {
            StartCoroutine(LoadLevel("Mainscene"));
        }
    }

}
