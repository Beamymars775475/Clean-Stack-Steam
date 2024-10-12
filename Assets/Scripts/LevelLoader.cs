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
        
            else if(GameManager.instance.levelsState[sceneOfButton-3] == true) // -3 car on cherche celui d'avant
            {
                StartCoroutine(LoadLevel(nameOfButton));
            }
            Debug.Log(sceneOfButton-2);
        } 

        else
        {
            Debug.Log("Euhh Bebou tu es fou ou quoi ??");
        }

    }


    public void LoadMenu()
    {
        StartCoroutine(LoadLevel("Mainscene"));
        GameManager.instance.isInventoryOpen = true; // ?
    }

    public void LoadSameScene()
    {
        Debug.Log("A");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public void NextScene()
    {
        if(GameManager.instance.isWon)
        {
            GameManager.instance.currentLevel++;
            GameManager.instance.goNextFloor = true;
            GameManager.instance.isWon = false;
            GameManager.instance.isNeedToStuckUI = false;
            GameManager.instance.isInventoryOpen = false;
            GameManager.instance.waitUntilFirstObject = true;
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
        if(SceneManager.GetActiveScene().name != "Mainscene" && SceneManager.GetActiveScene().name != "LevelSelectorScene" && (levelIndex == "Mainscene" || levelIndex == "LevelSelectorScene") || levelIndex == "BestiaryScene") // Pas d'anim entre ces 2 scènes
        {
            if(audio.clip != musicMain)
            {
                audio.clip = musicMain;
                audio.Play();
            }
        }
        else if(SceneManager.GetActiveScene().name == "LevelSelectorScene" && (levelIndex != "Mainscene" || levelIndex != "LevelSelectorScene"))
        {
            if(audio.clip != musicLevels)
            {
                Debug.Log("Veinti UNOOOO");
                audio.clip = musicLevels;
                audio.Play();

                GameManager.instance.isInventoryOpen = true;
                GameManager.instance.canAccessToInventory = true; // Setup l'inv au début pour qu'il s'ouvre correctement
            }
        }

        else if(SceneManager.GetActiveScene().name != "LevelSelectorScene" && SceneManager.GetActiveScene().name != "Mainscene" && (levelIndex != "Mainscene" || levelIndex != "LevelSelectorScene"))
        {
            GameManager.instance.isInventoryOpen = true;
            GameManager.instance.canAccessToInventory = true; // Setup l'inv au début pour qu'il s'ouvre correctement
        }

        GameManager.instance.feedbacksOpen.PlayFeedbacks();

        SceneManager.LoadScene(levelIndex);
        GameManager.instance.isGameOver = false;
        GameManager.instance.isNeedToStuckUI = false;
        GameManager.instance.waitUntilFirstObject = true;
        GameManager.instance.isDelivered2 = false;

        GameManager.instance.activeStrangePotion = false;

    }


}
