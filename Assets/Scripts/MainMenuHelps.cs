using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class MainMenuHelps : MonoBehaviour
{
    public GameObject otherHelp;

    public MMF_Player help1Feedbacks;
    public MMF_Player help2Feedbacks;

    public MainMenuSpawners mainMenuSpawners;



    void Start()
    {
        GameManager.instance.FirstLaunch = false;
        mainMenuSpawners.needToRemoveHelps = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainMenuSpawners.needToRemoveHelps && GameManager.instance.FirstLaunch == false)
        {
            help1Feedbacks.PlayFeedbacks();
            help2Feedbacks.PlayFeedbacks();
            GameManager.instance.FirstLaunch = true;
        }
    }

}
