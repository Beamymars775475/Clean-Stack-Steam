using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorScript : MonoBehaviour
{



    public MMF_Player feedbacksScreen0; // Left
    public MMF_Player feedbacksScreen1; // Right
    public MMF_Player feedbacksScreen2; // Deep

   public MMF_Player feedbacksScreen2Up; // Deep

   public MMF_Player feedbacksScreenUpper; // Si on clique sur le bouton du monde vert depuis le monde Hardcore





    private int stateOfLevelSelector; 

    void Start()
    {

    }

    void Update()
    {
        //if(GameManager.instance.levelsState[11] == true)
        //{
        //    monde4.SetActive(true);
        //}

        //if(GameManager.instance.levelsState[23] == true)
        //{
        //    monde5.SetActive(true);
        //}

        //if(GameManager.instance.levelsState[35] == true)
        //{
        //    monde6.SetActive(true);
        //}

        //if(GameManager.instance.levelsState[11] == true && GameManager.instance.levelsState[23] == true && GameManager.instance.levelsState[35] == true && GameManager.instance.levelsState[43] == true && GameManager.instance.levelsState[52] == true)
        //{
        //    monde7.SetActive(true);
        //}
    }


    public void GoRight()
    {
        if(stateOfLevelSelector == 0)
        {
            feedbacksScreen1.PlayFeedbacks();
            stateOfLevelSelector = 1;
        }

    }
    
    public void GoLeft()
    {
        if(stateOfLevelSelector == 1)
        {
            feedbacksScreen0.PlayFeedbacks();
            stateOfLevelSelector = 0;
        }
    }

    public void GoDownOrUp()
    {
        if(stateOfLevelSelector == 1)
        {
            feedbacksScreen2.PlayFeedbacks();
            stateOfLevelSelector = 2;
        }
        else if(stateOfLevelSelector == 2)
        {
            feedbacksScreen2Up.PlayFeedbacks();
            stateOfLevelSelector = 1;
        }


        else if(stateOfLevelSelector == 3)
        {
            feedbacksScreenUpper.PlayFeedbacks();
            stateOfLevelSelector = 1;
        }
    }

    public void GoDownOrUpDeeper()
    {
        if(stateOfLevelSelector == 2)
        {
            feedbacksScreen2.PlayFeedbacks();
            stateOfLevelSelector = 3;
        }
        else if(stateOfLevelSelector == 3)
        {
            feedbacksScreen2Up.PlayFeedbacks();
            stateOfLevelSelector = 2;
        }
    }
}
