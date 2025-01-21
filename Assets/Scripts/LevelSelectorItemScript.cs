using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelectorItemScript : MonoBehaviour
{
    private int state;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void OnClickItem()
   {
        MMF_Player itemFeedbacksReveal = gameObject.transform.GetChild(0).GetComponent<MMF_Player>();
        MMF_Player itemFeedbacksHide = gameObject.transform.GetChild(1).GetComponent<MMF_Player>();
        if((!itemFeedbacksReveal.IsPlaying && !itemFeedbacksHide.IsPlaying) && state == 0)
        {
            itemFeedbacksReveal.PlayFeedbacks();
            state = 1;
        }
        else if((!itemFeedbacksReveal.IsPlaying && !itemFeedbacksHide.IsPlaying) && state == 1)
        {
            itemFeedbacksHide.PlayFeedbacks();
            state = 0;
        }
   }
}
