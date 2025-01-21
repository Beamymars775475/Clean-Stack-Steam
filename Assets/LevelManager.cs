using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject mouse;

    public GameObject inventory;

    [Header("Win conditions")]
    public Transform placeToPutItems; 
    public Transform placeToPutFullItems;


    void Update()
    {
        if(inventory.transform.childCount == 2 && mouse.transform.childCount == 0&& !GameManager.instance.isInCloningProcess)
        {
            GameManager.instance.isPhase1Done = true;
        }

        if(GameManager.instance.isPhase1Done)
        {
            isPlayerElligibleToWin();
        }  
    }


    void isPlayerElligibleToWin()
    {
        int countItemsReady = 0;

        // CHECK ITEMS AU SOL IF READY
        foreach(Transform child in placeToPutItems)
        {
            itemScript childItemScript = child.GetComponent<itemScript>();
            if(!childItemScript.isReady || !childItemScript.isReady2)
            {
                countItemsReady++;
            }
        }
        // CHECK ITEMS FULL POTIONNER AU SOL IF READY
        foreach(Transform child in placeToPutFullItems.transform)
        {
            itemScript childItemScript = child.GetComponent<itemScript>();
            if(!childItemScript.isReady || !childItemScript.isReady2)
            {
                countItemsReady++;
            }
        }    

        if(countItemsReady == 0) // Mettre à jour l'état du niveau + win
        {
            GameManager.instance.isWon = true;
            if(GameManager.instance.modeHard)
            {
                GameManager.instance.levelsState[SceneManager.GetActiveScene().buildIndex-2] = 2;
            }
            else
            {
                GameManager.instance.levelsState[SceneManager.GetActiveScene().buildIndex-2] = 1;
            }
            
        }
    }

}
