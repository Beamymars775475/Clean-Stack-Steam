using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{
    public MMF_Player feedBacksRetryButton;
    public MMF_Player feedBacksNextButton;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager.instance.isWon && GameManager.instance.isNeedToStuckUI == false && GameManager.instance.waitUntilFirstObject == false)
        {
            feedBacksNextButton.PlayFeedbacks();
            GameManager.instance.isNeedToStuckUI = true;
        }
        else if(GameManager.instance.isGameOver && GameManager.instance.isNeedToStuckUI == false && GameManager.instance.waitUntilFirstObject == false)
        {
            feedBacksRetryButton.PlayFeedbacks();
            GameManager.instance.isNeedToStuckUI = true;
        }
        
    }

}
