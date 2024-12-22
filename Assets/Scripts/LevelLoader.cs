using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;


public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.9f;
    // Update is called once per frame

    public AudioClip musicMain;
    public AudioClip musicLevels;


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
            StartCoroutine(LoadLevel("LevelSelectorScene"));
        }
        else if(SceneManager.GetActiveScene().name == "LevelSelectorScene")
        {
            string nameOfButton = EventSystem.current.currentSelectedGameObject.name;
            int sceneOfButton = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + nameOfButton + ".unity");
            Debug.Log((sceneOfButton-2) - GameManager.instance.MONDE1);
            if((sceneOfButton-2) - GameManager.instance.MONDE1 == 0)
            {
                StartCoroutine(LoadLevel("RedLevel1")); 
            }
            else if((sceneOfButton-2) - GameManager.instance.MONDE2 == 0)
            {
                StartCoroutine(LoadLevel("BlueLevel1")); 
            }
            else if((sceneOfButton-2) - GameManager.instance.MONDE3 == 0)
            {
                StartCoroutine(LoadLevel("YellowLevel1")); 
            }
        
            else if(GameManager.instance.levelsState[sceneOfButton-3] != 0) // -3 car on cherche celui d'avant
            {
                StartCoroutine(LoadLevel(nameOfButton));
            }
        } 

        else if(SceneManager.GetActiveScene().name != "LevelSelectorScene")
        {
            StartCoroutine(LoadLevel("RedLevel" + SceneManager.GetActiveScene().buildIndex));  // FAUT CHANGER SA PROCHAIN TRUC
        }

        else
        {
            Debug.LogError("Tu veux aller dans les backrooms ?");
        }

    }


    public void LoadMenu()
    {
        StartCoroutine(LoadLevel("Mainscene"));
        GameManager.instance.isInventoryOpen = true; // ?
    }

    public void LoadSameScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
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


            GameManager.instance.activeStrangePotion = false;        
        }
    }

    public void LoadThisSceneWithAnimation(string nameOfScene)
    {
        StartCoroutine(LoadLevel(nameOfScene));
    }

    IEnumerator LoadLevel(string levelIndex) // levelIndex c'est le niveau qu'on va lancer
    {
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

        GameManager.instance.activeStrangePotion = false;

        GameManager.instance.isWon = false;
        GameManager.instance.isCountDownOn = false;
    }


    public void GoBackToMainsceneFromBestiaryOrSettings()
    {
        StartCoroutine(LoadLevel("Mainscene"));
    }

}
